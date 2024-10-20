using AJTarefasDomain.Interfaces;
using AJTarefasDomain.Interfaces.Negocio.Usuario;
using AJTarefasDomain.Relatorios;
using AJTarefasDomain.Tarefa;
using iText.Html2pdf;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJTarefasNegocio.Relatorio
{
    public class Relatorio : IRelatorio
    {
        private readonly IUsuarioService _usuarioService;

        public Relatorio (IUsuarioService usuarioService)
        {
           _usuarioService = usuarioService;
        }

        public async Task<List<TarefasConcluidasPorUsuario>> RelatorioTarefasConcluidasUsuarioMesAsync(int UsuarioId)
        {
            var usuario = await _usuarioService.RecuperarUsuarioAsync(UsuarioId);

            if (usuario.UsuarioId == 0)
            {
                throw new Exception("Apenas gerentes podem acessar esse relatório");
            }

            if ((int)usuario.Papel.UsuarioPapelCode > 1)
            {
                throw new Exception("Apenas gerentes podem acessar esse relatório");
            }

            var massa = AJTarefasRecursos.GeradorDados.MassaDados.GerarProjetosComTarefas(500, 15);

            var tarefasConcluidas = massa.Select(p => p.Tarefas.Where(t => t.Status.StatusCode == StatusTarefa.Concluida));

            var resultado = tarefasConcluidas.SelectMany(t => t)
                                             .GroupBy(t => new { t.Usuario, t.DataTermino.Value.Year, t.DataTermino.Value.Month })
                                             .Select(g => new TarefasConcluidasPorUsuario()
                                             {
                                                 Usuario = g.Key.Usuario,
                                                 MesAno = g.Key.Year.ToString() + g.Key.Month.ToString(),
                                                 Quantidade = g.Count()
                                             }).ToList();

            return resultado;
        }

        public async Task<List<MediaTarfasConcluidasPorMesPorUsuario>> RelatorioMediasTarefasConcluidasUsuarioMesAsync(int UsuarioId)
        {
            var tarefas = await RelatorioTarefasConcluidasUsuarioMesAsync(UsuarioId);

            var resultado = tarefas.GroupBy(g => g.Usuario)
                                    .Select(s => new MediaTarfasConcluidasPorMesPorUsuario()
                                    {
                                        Usuario = s.Key,
                                        TarefasMediasMes = Math.Round(s.Average(a => a.Quantidade), 2)
                                    }).ToList();



            return resultado;
        }

        public async Task<byte[]> ExcelRelatorioTarefasConcluidasUsuarioMesAsync(int UsuarioId)
        {

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var resultado = await RelatorioTarefasConcluidasUsuarioMesAsync(UsuarioId);

            using(var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Tarefas do Usuários Por Mês");

                worksheet.Cells[1, 1].Value = "UsuárioId";
                worksheet.Cells[1, 2].Value = "Usuário";
                worksheet.Cells[1, 3].Value = "Papel";
                worksheet.Cells[1, 4].Value = "Mês / Ano";
                worksheet.Cells[1, 5].Value = "Quantidade";

                int linha = 2;

                foreach(var item in resultado)
                {
                    worksheet.Cells[linha, 1].Value = item.Usuario.UsuarioId.ToString();
                    worksheet.Cells[linha, 2].Value = item.Usuario.Nome;
                    worksheet.Cells[linha, 3].Value = item.Usuario.Papel.Papel;
                    worksheet.Cells[linha, 4].Value = item.MesAno.ToString();
                    worksheet.Cells[linha, 5].Value = item.Quantidade;
                    linha++;
                }

                return package.GetAsByteArray();
            }
        }

        public async Task<byte[]> ExcelRelatorioMediasTarefasConcluidasUsuarioMesAsync(int UsuarioId)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var resultado = await RelatorioMediasTarefasConcluidasUsuarioMesAsync(UsuarioId);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Média de Tarefas do Usuários Por Mês");

                worksheet.Cells[1, 1].Value = "UsuárioId";
                worksheet.Cells[1, 2].Value = "Usuário";
                worksheet.Cells[1, 3].Value = "Papel";
                worksheet.Cells[1, 5].Value = "Média";

                int linha = 2;

                foreach (var item in resultado)
                {
                    worksheet.Cells[linha, 1].Value = item.Usuario.UsuarioId.ToString();
                    worksheet.Cells[linha, 2].Value = item.Usuario.Nome;
                    worksheet.Cells[linha, 3].Value = item.Usuario.Papel.Papel;
                    worksheet.Cells[linha, 5].Value = item.TarefasMediasMes;
                    linha++;
                }

                return package.GetAsByteArray();
            }
        }

        public async Task<byte[]> PfdRelatorioTarefasConcluidasUsuarioMesAsync(int UsuarioId)
        {
            var resultado = await RelatorioTarefasConcluidasUsuarioMesAsync(UsuarioId);

            string htmlTemplate = File.ReadAllText("RelatorioModelo\\ModeloRelatorio.html");

            var tabelaHtmlHeader = new StringBuilder();
            var tabelaHtml = new StringBuilder();

            string htmlFinal = htmlTemplate.Replace("{{TITULO}}", "Usuários e tarefas concluídas por mês");

            tabelaHtmlHeader.Append($@"
                <tr>
                    <th>Id</td>
                    <th>Usuário</td>
                    <th>Papel</td>
                    <th>Mês / Ano</td>
                    <th>Quantidade</td>
                </tr>");

            htmlFinal = htmlFinal.Replace("{{TABLEHEADER}}", tabelaHtmlHeader.ToString());

            foreach (var item in resultado)
            {
                tabelaHtml.Append($@"
                <tr>
                    <td>{item.Usuario.UsuarioId}</td>
                    <td>{item.Usuario.Nome}</td>
                    <td>{item.Usuario.Papel}</td>
                    <td>{item.MesAno}</td>
                    <td>{item.Quantidade}</td>
                </tr>");
            }

            htmlFinal = htmlFinal.Replace("{{TAREFAS}}", tabelaHtml.ToString());

            using (var memoryStream = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(htmlFinal, memoryStream);
                return memoryStream.ToArray(); 
            }
        }

        public async Task<byte[]> PdfRelatorioMediasTarefasConcluidasUsuarioMesAsync(int UsuarioId)
        {
            var resultado = await RelatorioMediasTarefasConcluidasUsuarioMesAsync(UsuarioId);

            string htmlTemplate = File.ReadAllText("RelatorioModelo\\ModeloRelatorio.html");

            var tabelaHtmlHeader = new StringBuilder();
            var tabelaHtml = new StringBuilder();

            string htmlFinal = htmlTemplate.Replace("{{TITULO}}", "Usuários e tarefas concluídas por mês");

            tabelaHtmlHeader.Append($@"
                <tr>
                    <th>Id</td>
                    <th>Usuário</td>
                    <th>Papel</td>
                    <th>Média por mês</td>
                </tr>");

            htmlFinal = htmlFinal.Replace("{{TABLEHEADER}}", tabelaHtmlHeader.ToString());

            foreach (var item in resultado)
            {
                tabelaHtml.Append($@"
                <tr>
                    <td>{item.Usuario.UsuarioId}</td>
                    <td>{item.Usuario.Nome}</td>
                    <td>{item.Usuario.Papel}</td>
                    <td>{item.TarefasMediasMes}</td>
                </tr>");
            }

            htmlFinal = htmlFinal.Replace("{{TAREFAS}}", tabelaHtml.ToString());

            using (var memoryStream = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(htmlFinal, memoryStream);
                return memoryStream.ToArray();
            }
        }

    }
}

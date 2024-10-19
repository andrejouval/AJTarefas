using AJTarefasDomain.Relatorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces
{
    public interface IRelatorio
    {
        Task<List<TarefasConcluidasPorUsuario>> RelatorioTarefasConcluidasUsuarioMesAsync();

        Task<List<MediaTarfasConcluidasPorMesPorUsuario>> RelatorioMediasTarefasConcluidasUsuarioMesAsync();

        Task<byte[]> ExcelRelatorioTarefasConcluidasUsuarioMesAsync();

        Task<byte[]> ExcelRelatorioMediasTarefasConcluidasUsuarioMesAsync();

        Task<byte[]> PfdRelatorioTarefasConcluidasUsuarioMesAsync();

        Task<byte[]> PdfRelatorioMediasTarefasConcluidasUsuarioMesAsync();

    }
}

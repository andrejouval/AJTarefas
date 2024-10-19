using AJTarefasDomain.Relatorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces
{
    public interface IRelatorio
    {
        Task<List<TarefasConcluidasPorUsuario>> RelatorioTarefasConcluidasUsuarioMesAsync(int UsuarioId);

        Task<List<MediaTarfasConcluidasPorMesPorUsuario>> RelatorioMediasTarefasConcluidasUsuarioMesAsync(int UsuarioId);

        Task<byte[]> ExcelRelatorioTarefasConcluidasUsuarioMesAsync(int UsuarioId);
        
        Task<byte[]> ExcelRelatorioMediasTarefasConcluidasUsuarioMesAsync(int UsuarioId);

        Task<byte[]> PfdRelatorioTarefasConcluidasUsuarioMesAsync(int UsuarioId);

        Task<byte[]> PdfRelatorioMediasTarefasConcluidasUsuarioMesAsync(int UsuarioId);

    }
}

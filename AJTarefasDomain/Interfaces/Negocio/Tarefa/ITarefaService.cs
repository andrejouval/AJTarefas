using AJTarefasDomain.Tarefa;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces.Negocio.Tarefa
{
    public interface ITarefaService
    {
        Task<TarefaDto> PostTarefaAsync(PostTarefaRequest Tarefa);

        Task<TarefaDto> PatchTarefaAsync(TarefaDto Tarefa);

        Task<TarefaDto> GetTarefaAsync(int ProjetoId, int Id);

        Task DeleteTarefaAsync(int ProjetoId, int Id);

    }
}

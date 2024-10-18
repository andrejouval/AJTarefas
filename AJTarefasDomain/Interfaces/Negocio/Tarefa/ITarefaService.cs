using AJTarefasDomain.Tarefa;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces.Negocio.Tarefa
{
    public interface ITarefaService
    {
        Task<TarefaDto> PostTarefaAsync(PostTarefaRequest Tarefa);

        Task<TarefaDto> PatchTarefaAsync(TarefaDto Tarefa);

    }
}

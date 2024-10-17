using AJTarefasDomain.Tarefa;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces.Negocio.Tarefa
{
    public interface ITarefaService
    {
        Task<int> PostTarefaAsync(PostTarefaRequest Tarefa);

        Task PatchTarefaAsync(TarefaDto Tarefa);
    }
}

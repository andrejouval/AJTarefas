using AJTarefasDomain.Tarefa;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces.Repositorio.Projeto
{
    public interface ITarefaRepositorio
    {
        Task<int> PostTarefaAsync(PostTarefaRequest Tarefa);

        Task PatchTarefaAsync(TarefaDto Tarefa);
    }
}

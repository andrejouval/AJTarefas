using AJTarefasDomain.Projeto.Post;
using AJTarefasDomain.Tarefa;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces.Negocio.Tarefa
{
    public interface ITarefaService
    {
        Task<int> PostTarefaAsync(PostTarefaRequest Tarefa);
    }
}

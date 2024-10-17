
using AJTarefasDomain.Projeto.Post;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces.Negocio.Projeto
{
    public interface IProjetoService
    {
        Task<int> PostProjetoAsync(PostProjetoRequest Projeto);
    }
}

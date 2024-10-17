using AJTarefasDomain.Projeto.Post;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces.Repositorio.Projeto
{
    public interface IProjetoRepositorio
    {
        Task<int> PostProjeto(PostProjetoRequest Projeto);
    }
}

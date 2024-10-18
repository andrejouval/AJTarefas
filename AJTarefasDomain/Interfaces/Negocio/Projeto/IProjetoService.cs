
using AJTarefasDomain.Projeto;
using AJTarefasDomain.Projeto.Post;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces.Negocio.Projeto
{
    public interface IProjetoService
    {
        Task<ProjetoDto> PostProjetoAsync(PostProjetoRequest Projeto);

        Task PatchProjetoAsync(int ProjetoId);
    }
}

using AJTarefasDomain.Projeto;
using AJTarefasDomain.Projeto.Post;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces.Repositorio.Projeto
{
    public interface IProjetoRepositorio
    {
        Task<int> PostProjetoAsync(PostProjetoRequest Projeto);

        Task<bool> ProjetoExisteAsync(int ProjetoId);

        Task<ProjetoDto> RecuperarProjetoAsync(int ProjetoId);

        Task PatchProjetoAsync(int ProjetoId);

        Task DeleteProjetoAsync(int ProjetoId);

        Task<int> RecuperarQuantidadeTarefasAsync(int ProjetoId);

        Task<IEnumerable<ProjetoDto>> RecuperarProjetosAsync();
    }
}

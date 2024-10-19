
using AJTarefasDomain.Projeto;
using AJTarefasDomain.Projeto.Post;
using AJTarefasDomain.Relatorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces.Negocio.Projeto
{
    public interface IProjetoService
    {
        Task<ProjetoDto> PostProjetoAsync(PostProjetoRequest Projeto);

        Task PatchProjetoAsync(int ProjetoId);

        Task DeleteProjetoAsync(int ProjetoId);

        Task<IEnumerable<ProjetoDto>> RecuperarProjetosAsync(int? ProjetoId = null, int? UsuarioId = null);

    }
}

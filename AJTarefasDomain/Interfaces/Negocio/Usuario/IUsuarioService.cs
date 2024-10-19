using AJTarefasDomain.Base;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces.Negocio.Usuario
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> RecuperarUsuarioAsync(int UsuarioId);
    }
}

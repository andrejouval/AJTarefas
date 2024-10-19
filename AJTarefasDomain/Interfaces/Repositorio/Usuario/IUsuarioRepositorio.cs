using AJTarefasDomain.Base;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces.Repositorio.Usuario
{
    public interface IUsuarioRepositorio
    {
        Task<UsuarioDto> RecuperarUsuarioAsync(int UsuarioId);
    }
}

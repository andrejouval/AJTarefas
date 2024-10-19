using AJTarefasDomain.Base;
using AJTarefasDomain.Interfaces.Negocio.Usuario;
using AJTarefasDomain.Interfaces.Repositorio.Usuario;
using System.Threading.Tasks;

namespace AJTarefasNegocio.Usuario
{
    public class UsuarioService : IUsuarioService
    {
        private IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioService(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<UsuarioDto> RecuperarUsuarioAsync(int UsuarioId)
        {
            return await _usuarioRepositorio.RecuperarUsuarioAsync(UsuarioId);
        }
    }
}

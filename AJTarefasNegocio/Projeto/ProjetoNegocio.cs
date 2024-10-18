using AJTarefasDomain.Interfaces.Negocio.Projeto;
using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasDomain.Projeto;
using AJTarefasDomain.Projeto.Post;
using System.Threading.Tasks;

namespace AJTarefasNegocio.Projeto
{
    public class ProjetoNegocio : IProjetoService
    {
        private readonly IProjetoRepositorio _projetoRepositorio;

        public ProjetoNegocio(IProjetoRepositorio projetoRepo)
        {
            _projetoRepositorio = projetoRepo;
        }
        public async Task<ProjetoDto> PostProjetoAsync(PostProjetoRequest Projeto)
        {
            if (string.IsNullOrWhiteSpace(Projeto.NomeProjeto))
            {
                throw new System.Exception("O nome do projeto é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(Projeto.DescricaoProjeto))
            {
                throw new System.Exception("A descrição do projeto é obrigarório");
            }

            if (Projeto.UsuarioId == 0)
            {
                throw new System.Exception("Usuário é obrigatório");
            }

            var id = await _projetoRepositorio.PostProjetoAsync(Projeto);

            var projeto = await _projetoRepositorio.RecuperarProjetoAsync(id);

            return projeto;
        }
    }
}

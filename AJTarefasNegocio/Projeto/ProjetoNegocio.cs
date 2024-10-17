using AJTarefasDomain.Interfaces.Negocio.Projeto;
using AJTarefasDomain.Interfaces.Repositorio.Projeto;
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
        public async Task<int> PostProjetoAsync(PostProjetoRequest Projeto)
        {
            return await _projetoRepositorio.PostProjeto(Projeto);
        }
    }
}

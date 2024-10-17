using AJTarefasDomain.Interfaces.Negocio.Projeto;
using AJTarefasDomain.Projeto.Post;
using System.Threading.Tasks;

namespace AJTarefasNegocio.Projeto
{
    public class ProjetoNegocio : IProjetoService
    {
        public async Task<int> PostProjetoAsync(PostProjetoRequest Projeto)
        {
            return 0;
        }
    }
}

using AJTarefasDomain.Interfaces.Negocio.Projeto;
using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasDomain.Projeto;
using AJTarefasDomain.Projeto.Post;
using System.Collections.Generic;
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
                throw new System.Exception("O nome do projeto é obrigatório ou é inexistente.");
            }

            if (string.IsNullOrWhiteSpace(Projeto.DescricaoProjeto))
            {
                throw new System.Exception("A descrição do projeto é obrigarório");
            }

            if (Projeto.UsuarioId == 0)
            {
                throw new System.Exception("Usuário é obrigatório ou é inexistente.");
            }

            var id = await _projetoRepositorio.PostProjetoAsync(Projeto);

            var projeto = await _projetoRepositorio.RecuperarProjetoAsync(id);

            return projeto;
        }

        public async Task PatchProjetoAsync(int ProjetoId)
        {
            await _projetoRepositorio.PatchProjetoAsync(ProjetoId);
        }

        public async Task DeleteProjetoAsync(int ProjetoId)
        {
            var quantidadeTerefas = await _projetoRepositorio.RecuperarQuantidadeTarefasAsync(ProjetoId);

            if(quantidadeTerefas > 0)
            {
                throw new System.Exception("Não é possível remover o projeto com tarefas associadas");
            }

            await _projetoRepositorio.DeleteProjetoAsync(ProjetoId);
        }

        public async Task<IEnumerable<ProjetoDto>> RecuperarProjetosAsync(int? ProjetoId = null, int? UsuarioId = null)
        {
            return await _projetoRepositorio.RecuperarProjetosAsync(ProjetoId, UsuarioId);
        }

    }
}

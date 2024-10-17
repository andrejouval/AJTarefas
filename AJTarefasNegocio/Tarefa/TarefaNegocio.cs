using AJTarefasDomain.Interfaces.Negocio.Tarefa;
using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasDomain.Tarefa;
using System.Threading.Tasks;

namespace AJTarefasNegocio.Projeto
{
    public class TarefaNegocio : ITarefaService
    {
        private readonly ITarefaRepositorio _tarefaRepositorio;

        public TarefaNegocio(ITarefaRepositorio tarefaRepo)
        {
            _tarefaRepositorio = tarefaRepo;
        }
        public async Task<int> PostTarefaAsync(PostTarefaRequest Tarefa)
        {
            if (Tarefa.ProjetoId == 0)
            {
                throw new System.Exception("O projeto da tarefa deve ser informado.");
            }

            if (string.IsNullOrEmpty(Tarefa.Titulo))
            {
                throw new System.Exception("O a tarefa deve ter um título");
            }

            if (string.IsNullOrEmpty(Tarefa.Descricao))
            {
                throw new System.Exception("O a tarefa deve ter um descrição");
            }

            return await _tarefaRepositorio.PostTarefa(Tarefa);
        }
    }
}

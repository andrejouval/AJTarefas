using AJTarefasDomain.Base;
using AJTarefasDomain.Interfaces.Negocio.Tarefa;
using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasDomain.Tarefa;
using System.Threading;
using System.Threading.Tasks;

namespace AJTarefasNegocio.Projeto
{
    public class TarefaNegocio : ITarefaService
    {
        private readonly ITarefaRepositorio _tarefaRepositorio;
        private readonly IProjetoRepositorio _projetoRepositorio;

        public TarefaNegocio(ITarefaRepositorio tarefaRepo, IProjetoRepositorio projetoRepo)
        {
            _tarefaRepositorio = tarefaRepo;
            _projetoRepositorio = projetoRepo;
        }
        public async Task<int> PostTarefaAsync(PostTarefaRequest Tarefa)
        {
            var tarefa = new TarefaDto()
            {
                ProjetoId = Tarefa.ProjetoId,
                Descricao = Tarefa.Descricao,
                PrioridadeTarefa = new TarefaPrioridadeDto()
                {
                    PrioridadeCode = (PrioridadeTarefa)Tarefa.PrioridadeTarefa,
                    Prioridade = Tarefa.PrioridadeTarefa.GetEnumTextos()
                },
                Titulo = Tarefa.Titulo
            };

            var eValido = await EValido(tarefa);

            if (eValido == null)
            {
                throw new System.Exception("Erro interno. Facor entrar em contato com a TI");
            }

            if (!eValido.EValido)
            {
                throw new System.Exception(eValido.MensagemError);
            }

            return await _tarefaRepositorio.PostTarefaAsync(Tarefa);
        }

        public async Task PatchTarefaAsync(TarefaDto Tarefa)
        {
            var eValido = await EValido(Tarefa);

            if (eValido == null)
            {
                throw new System.Exception("Erro interno. Facor entrar em contato com a TI");
            }

            if (!eValido.EValido)
            {
                throw new System.Exception(eValido.MensagemError);
            }


            await _tarefaRepositorio.PatchTarefaAsync(Tarefa);
        }

        private async Task<ErroComum> EValido(TarefaDto Tarefa)
        {
            var erroComum = new ErroComum()
            {
                EValido = true
            };

            if (Tarefa.ProjetoId == 0)
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "O projeto da tarefa deve ser informado.";
            }

            if (string.IsNullOrEmpty(Tarefa.Titulo))
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "O a tarefa deve ter um título";
            }

            if (string.IsNullOrEmpty(Tarefa.Descricao))
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "O a tarefa deve ter um descrição";
            }

            if (Tarefa.PrioridadeTarefa != null)
            {
                if (!Tarefa.PrioridadeTarefa.PrioridadeCode.IsValid())
                {
                    erroComum.EValido = false;
                    erroComum.MensagemError = "As opções de prioridade são 1 - Baixa, 2 - Média e 3 - Alta";
                }
            }

            var projetoExiste = await _projetoRepositorio.ProjetoExisteAsync(Tarefa.ProjetoId);

            if (!projetoExiste)
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "Favor entrar um código de projeto existente";
            }

            return erroComum;
        }


    }
}

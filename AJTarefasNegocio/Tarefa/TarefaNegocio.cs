using AJTarefasDomain.Base;
using AJTarefasDomain.Interfaces.Negocio.Tarefa;
using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasDomain.Tarefa;
using System;
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

        public async Task<TarefaDto> PatchTarefaAsync(TarefaDto Tarefa)
        {
            var tarefaAtual = await _tarefaRepositorio.RecuperarTarefaAsync(Tarefa.ProjetoId, Tarefa.Id);

            if (string.IsNullOrEmpty(Tarefa.Titulo))
            {
                Tarefa.Titulo = tarefaAtual.Titulo;
            }

            if (string.IsNullOrEmpty(Tarefa.Descricao))
            {
                Tarefa.Descricao = tarefaAtual.Descricao;
            }

            var eValido = await EValido(Tarefa);

            if (eValido == null)
            {
                throw new System.Exception("Erro interno. Facor entrar em contato com a TI");
            }

            if (!eValido.EValido)
            {
                throw new System.Exception(eValido.MensagemError);
            }

            var tarefaExiste = await _tarefaRepositorio.TarefaExisteAsync(Tarefa.ProjetoId, Tarefa.Id);

            if (!tarefaExiste)
            {
                throw new System.Exception("A tarefa " +  Tarefa.Id + "não pertence ao projeto ou não é válida");
            }

            if (!Tarefa.Status.StatusCode.IsValid())
            {
                throw new System.Exception("A tarefa só pode ter os status 1 - Pendente, 2 - Em execução, 3 - Bloqueada ou 4 - Concluída");
            }

            if (tarefaAtual.PrioridadeTarefa.PrioridadeCode != Tarefa.PrioridadeTarefa.PrioridadeCode)
            {
                throw new System.Exception("Não é permitido mudar a prioridade após a criação da tarefa");
            }

            if (Tarefa.Status.StatusCode != StatusTarefa.Concluida && tarefaAtual.DataTermino != null)
            {
                Tarefa.DataTermino = null;
            }

            if (Tarefa.Status.StatusCode != StatusTarefa.Pendente)
            {
                if (Tarefa.DataPrevistaTermino is null)
                {
                    throw new System.Exception("Para alterar o status da tarefa é necessário informar a data prevista de término");
                }

                if (!tarefaAtual.DataInicio.HasValue)
                {
                    Tarefa.DataInicio = DateTime.Now;
                }

                if (Tarefa.Status.StatusCode == StatusTarefa.Concluida && tarefaAtual.DataTermino is null)
                {
                    Tarefa.DataTermino = DateTime.Now;
                }

                await _tarefaRepositorio.PatchTarefaAsync(Tarefa);

            }
            else
            {
                await _tarefaRepositorio.PatchTarefaAsync(Tarefa);
            }

            var retorno = await _tarefaRepositorio.RecuperarTarefaAsync(Tarefa.ProjetoId, Tarefa.Id);

            return retorno;
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

            if (!Tarefa.PrioridadeTarefa.PrioridadeCode.IsValid())
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "As opções de prioridade são 1 - Baixa, 2 - Média e 3 - Alta";
            }

            var projetoExiste = await _projetoRepositorio.ProjetoExisteAsync(Tarefa.ProjetoId);

            if (!projetoExiste)
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "Favor entrar um código de projeto existente";
            }

            var numeroTerefas = await _tarefaRepositorio.RecuperarQuantidadeTarefasAsync(Tarefa.Id, Tarefa.ProjetoId);

            if (numeroTerefas > 20)
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "Número máximo de tarefas foi atingida para esse projeto.";
            }

            return erroComum;
        }


    }
}

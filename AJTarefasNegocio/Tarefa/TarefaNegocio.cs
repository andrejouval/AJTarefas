using AJTarefasDomain.Base;
using AJTarefasDomain.Interfaces.Negocio.Projeto;
using AJTarefasDomain.Interfaces.Negocio.Tarefa;
using AJTarefasDomain.Interfaces.Negocio.Usuario;
using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasDomain.Tarefa;
using System;
using System.Threading.Tasks;

namespace AJTarefasNegocio.Projeto
{
    public class TarefaNegocio : ITarefaService
    {
        private readonly ITarefaRepositorio _tarefaRepositorio;
        private readonly IProjetoRepositorio _projetoRepositorio;
        private readonly IProjetoService _projetoService;
        private readonly IUsuarioService _usuarioService;

        public TarefaNegocio(ITarefaRepositorio tarefaRepo, IProjetoRepositorio projetoRepo, IProjetoService projetoService
            , IUsuarioService usuarioService)
        {
            _tarefaRepositorio = tarefaRepo;
            _projetoRepositorio = projetoRepo;
            _projetoService = projetoService;
            _usuarioService = usuarioService;
        }
        public async Task<TarefaDto> PostTarefaAsync(PostTarefaRequest Tarefa)
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
                Titulo = Tarefa.Titulo,
                Usuario = new UsuarioDto()
                {
                    UsuarioId = Tarefa.UsuarioId
                }
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

            var id = await _tarefaRepositorio.PostTarefaAsync(Tarefa);

            return await _tarefaRepositorio.RecuperarTarefaAsync(tarefa.ProjetoId, id);
        }

        public async Task<TarefaDto> PatchTarefaAsync(TarefaDto Tarefa)
        {
            var tarefaAtual = await _tarefaRepositorio.RecuperarTarefaAsync(Tarefa.ProjetoId, Tarefa.Id);

            if (tarefaAtual.Id == 0)
            {
                throw new Exception("Tarefa inexistente.");
            }

            if (string.IsNullOrEmpty(Tarefa.Titulo))
            {
                Tarefa.Titulo = tarefaAtual.Titulo;
            }

            if (string.IsNullOrEmpty(Tarefa.Descricao))
            {
                Tarefa.Descricao = tarefaAtual.Descricao;
            }

            if (Tarefa.PrioridadeTarefa.PrioridadeCode == 0)
            {
                Tarefa.PrioridadeTarefa = tarefaAtual.PrioridadeTarefa;
            }

            var usuario = await _usuarioService.RecuperarUsuarioAsync(Tarefa.Usuario.UsuarioId);

            if (usuario.UsuarioId == 0)
            {
                var comentatio = Tarefa.Comentarios.FirstOrDefault();

                if (comentatio == null)
                {
                    throw new Exception("Deve ser informado o usuário da tarefa.");
                }

                usuario = await _usuarioService.RecuperarUsuarioAsync(comentatio.IdUsuario);

                Tarefa.Usuario = usuario;
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
                throw new System.Exception("A tarefa " +  Tarefa.Id + " não pertence ao projeto ou não é válida");
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
                else
                {
                    Tarefa.DataInicio = tarefaAtual.DataInicio;
                }

                if (Tarefa.Status.StatusCode == StatusTarefa.Concluida && tarefaAtual.DataTermino == null)
                {
                    Tarefa.DataTermino = DateTime.Now;
                }

                if (Tarefa.Status.StatusCode == StatusTarefa.Concluida && tarefaAtual.DataPrevistaTermino == null)
                {
                    Tarefa.DataPrevistaTermino = DateTime.Now;
                }

                await _tarefaRepositorio.PatchTarefaAsync(Tarefa);

                await _projetoService.PatchProjetoAsync(Tarefa.ProjetoId);

            }
            else
            {
                await _tarefaRepositorio.PatchTarefaAsync(Tarefa);

                await _projetoService.PatchProjetoAsync(Tarefa.ProjetoId);
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
                return erroComum;
            }

            if (string.IsNullOrEmpty(Tarefa.Titulo))
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "O a tarefa deve ter um título";
                return erroComum;
            }

            if (string.IsNullOrEmpty(Tarefa.Descricao))
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "O a tarefa deve ter um descrição";
                return erroComum;
            }

            if (Tarefa.PrioridadeTarefa == null)
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "As opções de prioridade são 1 - Baixa, 2 - Média e 3 - Alta";
                return erroComum;
            }

            if (!Tarefa.PrioridadeTarefa.PrioridadeCode.IsValid())
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "As opções de prioridade são 1 - Baixa, 2 - Média e 3 - Alta";
                return erroComum;
            }

            var projetoExiste = await _projetoRepositorio.ProjetoExisteAsync(Tarefa.ProjetoId);

            if (!projetoExiste)
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "Favor entrar um código de projeto existente";
                return erroComum;
            }

            var numeroTerefas = await _tarefaRepositorio.RecuperarQuantidadeTarefasAsync(Tarefa.ProjetoId);

            if (numeroTerefas > 20)
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "Número máximo de tarefas foi atingida para esse projeto.";
                return erroComum;
            }

            if (Tarefa.Usuario == null)
            {
                erroComum.EValido = false;
                erroComum.MensagemError = "Usuário é obrigatório.";
                return erroComum;
            }
            
            return erroComum;
        }

        public async Task<TarefaDto> RecuperarTarefaAsync(int ProjetoId, int Id)
        {
            return await _tarefaRepositorio.RecuperarTarefaAsync(ProjetoId, Id);
        }

        public async Task DeleteTarefaAsync(int ProjetoId, int Id)
        {
            await _tarefaRepositorio.DeleteTarefaAsync(ProjetoId, Id);
        }
    }
}

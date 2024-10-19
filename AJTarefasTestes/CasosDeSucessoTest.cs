using AJTarefasDomain.Interfaces.Negocio.Projeto;
using AJTarefasDomain.Interfaces.Negocio.Tarefa;
using AJTarefasDomain.Interfaces.Negocio.Usuario;
using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasDomain.Interfaces.Repositorio.Usuario;
using AJTarefasDomain.Projeto;
using AJTarefasDomain.Projeto.Post;
using AJTarefasDomain.Tarefa;
using AJTarefasNegocio.Projeto;
using AJTarefasNegocio.Usuario;
using AJTarefasRecursos.Repositorios.Projeto;
using AJTarefasRecursos.Repositorios.Usuario;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;

namespace AJTarefasTestes
{
    public class CasoDeSucessoTeste
    {
        private IConfiguration _config;
        private Fixture _fixture;

        private IProjetoService _projetoService;
        private ITarefaService _tarefaService;

        private IProjetoRepositorio _projetoRepositorio;
        private ITarefaRepositorio _tarefaRepositorio;

        private IUsuarioService _usuarioService;
        private IUsuarioRepositorio _usuarioRepositorio;


        private string _projetName = "Projeto_Teste_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

        [SetUp]
        public void CasosDeSucesso()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json", true, true).Build();
            _fixture = new Fixture();

            IServiceCollection builder = new ServiceCollection();

            builder.AddSingleton<IConfiguration>(_config);

            builder.AddScoped<IProjetoService, ProjetoNegocio>();

            builder.AddScoped<ITarefaService, TarefaNegocio>();

            builder.AddScoped<IProjetoRepositorio, ProjetoRepositorio>();

            builder.AddScoped<ITarefaRepositorio, TarefaRepositorio>();

            builder.AddScoped<IUsuarioService, UsuarioService>();

            builder.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

            IServiceProvider serviceProvider = builder.BuildServiceProvider();

            _projetoService = serviceProvider.GetService<IProjetoService>();
            _tarefaService = serviceProvider.GetService<ITarefaService>();
            _projetoRepositorio = serviceProvider.GetService<IProjetoRepositorio>();
            _tarefaRepositorio = serviceProvider.GetService<ITarefaRepositorio>();
            _usuarioService = serviceProvider.GetService<IUsuarioService>();
            _usuarioRepositorio = serviceProvider.GetService<IUsuarioRepositorio>();
        }

        [Test]
        public async Task t001_IncluirProjeto_CenarioDeSucesso()
        {
            var projetoTeste = new PostProjetoRequest()
            {
                NomeProjeto = _projetName,
                DescricaoProjeto = Guid.NewGuid().ToString(),
                UsuarioId = 2
            };

            var projeto = await _projetoService.PostProjetoAsync(projetoTeste);

            Assert.IsNotNull(projeto);

            var projetoVerificar = await _projetoService.RecuperarProjetosAsync(projeto.Id);

            Assert.IsNotNull(projetoVerificar);

            Assert.AreEqual(projetoVerificar.FirstOrDefault().Id, projeto.Id);
            Assert.AreEqual(projetoVerificar.FirstOrDefault().DataCriacao, projeto.DataCriacao);
            Assert.AreEqual(projetoVerificar.FirstOrDefault().Descricao, projeto.Descricao);
            Assert.AreEqual(projetoVerificar.FirstOrDefault().StatusProjeto.StatusProjetoCode, projeto.StatusProjeto.StatusProjetoCode);
            Assert.AreEqual(projetoVerificar.FirstOrDefault().Usuario.UsuarioId, projeto.Usuario.UsuarioId);

        }

        [Test]
        public async Task t002_IncluirTarefa_CenarioDeSucesso()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_IncluirProjeto_CenarioDeSucesso();
                projetoWork = await RecuperarProjetoTeste(_projetName);
            }

            var tarefa = new PostTarefaRequest()
            {
                Descricao = Guid.NewGuid().ToString(),
                Titulo = Guid.NewGuid().ToString(),
                PrioridadeTarefa = PrioridadeTarefa.Media,
                ProjetoId = projetoWork.Id,
                UsuarioId = 1
            };

            var tarefaCriada = await _tarefaService.PostTarefaAsync(tarefa);

            Assert.IsNotNull(tarefaCriada);

            var tarefaVerifica = await _tarefaService.RecuperarTarefaAsync(tarefaCriada.ProjetoId, tarefaCriada.Id);

            Assert.IsNotNull(tarefaVerifica);

            Assert.AreEqual(tarefaVerifica.Id, tarefaCriada.Id);
            Assert.AreEqual(tarefaVerifica.DataCriacao, tarefaCriada.DataCriacao);
            Assert.AreEqual(tarefaVerifica.Descricao, tarefaCriada.Descricao);
            Assert.AreEqual(tarefaVerifica.Status.StatusCode, tarefaCriada.Status.StatusCode);
            Assert.AreEqual(tarefaVerifica.Usuario.UsuarioId, tarefaCriada.Usuario.UsuarioId);
            Assert.AreEqual(tarefaVerifica.PrioridadeTarefa.PrioridadeCode, tarefaCriada.PrioridadeTarefa.PrioridadeCode);


        }

        [Test]
        public async Task t003_AtulizarTarefa_CenarioDeSucesso_EmExecucao()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_IncluirProjeto_CenarioDeSucesso();
                projetoWork = await RecuperarProjetoTeste(_projetName);
            }

            var tarefa = projetoWork.Tarefas.FirstOrDefault();

            if (tarefa == null)
            {
                await t002_IncluirTarefa_CenarioDeSucesso();
                projetoWork = await RecuperarProjetoTeste(_projetName);
                tarefa = projetoWork.Tarefas.FirstOrDefault();
            }

            tarefa.Comentarios = new List<TarefaComentariosDto>()
            {
                new TarefaComentariosDto { IdUsuario = 1, Comentario = "Comentário 1"},
                new TarefaComentariosDto { IdUsuario = 2, Comentario = "Comentário 2"}
            };

            tarefa.Status = new TarefaStatusDto()
            {
                StatusCode = StatusTarefa.EmExecucao
            };

            tarefa.DataPrevistaTermino = DateTime.Now.AddDays(2);

            var tarefaCriada = await _tarefaService.PatchTarefaAsync(tarefa);

            Assert.IsNotNull(tarefaCriada);

            var tarefaVerifica = await _tarefaService.RecuperarTarefaAsync(tarefaCriada.ProjetoId, tarefaCriada.Id);

            Assert.IsNotNull(tarefaVerifica);

            Assert.AreEqual(tarefaVerifica.Id, tarefaCriada.Id);
            Assert.AreEqual(tarefaVerifica.DataCriacao, tarefaCriada.DataCriacao);
            Assert.AreEqual(tarefaVerifica.Descricao, tarefaCriada.Descricao);
            Assert.AreEqual(tarefaVerifica.Status.StatusCode, tarefaCriada.Status.StatusCode);
            Assert.AreEqual(tarefaVerifica.Usuario.UsuarioId, tarefaCriada.Usuario.UsuarioId);
            Assert.AreEqual(tarefaVerifica.PrioridadeTarefa.PrioridadeCode, tarefaCriada.PrioridadeTarefa.PrioridadeCode);
            Assert.AreEqual(tarefaVerifica.DataInicio, tarefaCriada.DataInicio);
            Assert.IsNotNull(tarefaVerifica.DataInicio);
            Assert.AreEqual(tarefaVerifica.DataPrevistaTermino, tarefaCriada.DataPrevistaTermino);
            Assert.AreEqual(tarefaVerifica.DataTermino, tarefaCriada.DataTermino);

            var projetoVerificar = await _projetoService.RecuperarProjetosAsync(tarefaCriada.ProjetoId);

            Assert.AreEqual(projetoVerificar.FirstOrDefault().DataInicio,
                        projetoVerificar.First().Tarefas.Max(d => d.DataInicio));

            Assert.AreEqual((int)projetoVerificar.FirstOrDefault().StatusProjeto.StatusProjetoCode,
            (int)projetoVerificar.First().Tarefas.Max(d => d.Status.StatusCode));

            Assert.IsNotNull(projetoVerificar.FirstOrDefault().DataInicio);
            Assert.IsTrue((int)projetoVerificar.FirstOrDefault().StatusProjeto.StatusProjetoCode == 2);
            Assert.IsNull(projetoVerificar.FirstOrDefault().DataTermino);

            Assert.IsTrue(projetoVerificar.FirstOrDefault().DataPrevisaoTermino.Value.Date == DateTime.Now.AddDays(2).Date);

        }

        [Test]
        public async Task t004_AtulizarTarefa_CenarioDeSucesso_Conclusao()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_IncluirProjeto_CenarioDeSucesso();
                projetoWork = await RecuperarProjetoTeste(_projetName);
            }

            var tarefa = projetoWork.Tarefas.FirstOrDefault();

            if (tarefa == null)
            {
                await t002_IncluirTarefa_CenarioDeSucesso();
                projetoWork = await RecuperarProjetoTeste(_projetName);
                tarefa = projetoWork.Tarefas.FirstOrDefault();
            }

            tarefa.Comentarios = new List<TarefaComentariosDto>()
            {
                new TarefaComentariosDto { IdUsuario = 1, Comentario = "Comentário 1"},
                new TarefaComentariosDto { IdUsuario = 2, Comentario = "Comentário 2"}
            };

            tarefa.Status = new TarefaStatusDto()
            {
                StatusCode = StatusTarefa.Concluida
            };

            tarefa.DataPrevistaTermino = DateTime.Now.AddDays(2);

            var tarefaCriada = await _tarefaService.PatchTarefaAsync(tarefa);

            Assert.IsNotNull(tarefaCriada);

            var tarefaVerifica = await _tarefaService.RecuperarTarefaAsync(tarefaCriada.ProjetoId, tarefaCriada.Id);

            Assert.IsNotNull(tarefaVerifica);

            Assert.AreEqual(tarefaVerifica.Id, tarefaCriada.Id);
            Assert.AreEqual(tarefaVerifica.DataCriacao, tarefaCriada.DataCriacao);
            Assert.AreEqual(tarefaVerifica.Descricao, tarefaCriada.Descricao);
            Assert.AreEqual(tarefaVerifica.Status.StatusCode, tarefaCriada.Status.StatusCode);
            Assert.AreEqual(tarefaVerifica.Usuario.UsuarioId, tarefaCriada.Usuario.UsuarioId);
            Assert.AreEqual(tarefaVerifica.PrioridadeTarefa.PrioridadeCode, tarefaCriada.PrioridadeTarefa.PrioridadeCode);
            Assert.AreEqual(tarefaVerifica.DataInicio, tarefaCriada.DataInicio);
            Assert.AreEqual(tarefaVerifica.DataPrevistaTermino, tarefaCriada.DataPrevistaTermino);
            Assert.AreEqual(tarefaVerifica.DataTermino, tarefaCriada.DataTermino);

            var projetoVerificar = await _projetoService.RecuperarProjetosAsync(tarefaCriada.ProjetoId);

            Assert.AreEqual(projetoVerificar.FirstOrDefault().DataInicio, 
                        projetoVerificar.First().Tarefas.Max(d => d.DataInicio));

            Assert.AreEqual((int)projetoVerificar.FirstOrDefault().StatusProjeto.StatusProjetoCode,
            (int)projetoVerificar.First().Tarefas.Max(d => d.Status.StatusCode));

            Assert.AreEqual(projetoVerificar.FirstOrDefault().DataTermino,
            projetoVerificar.First().Tarefas.Max(d => d.DataTermino));

            Assert.IsNotNull(projetoVerificar.FirstOrDefault().DataInicio);
            Assert.IsTrue((int)projetoVerificar.FirstOrDefault().StatusProjeto.StatusProjetoCode == 4);
            Assert.IsNotNull(projetoVerificar.FirstOrDefault().DataTermino);

            Assert.IsTrue(projetoVerificar.FirstOrDefault().DataPrevisaoTermino.Value.Date == DateTime.Now.AddDays(2).Date);

        }

        [Test]
        public async Task t005_AtulizarTarefa_CenarioDeSucesso_Reaberto()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_IncluirProjeto_CenarioDeSucesso();
                projetoWork = await RecuperarProjetoTeste(_projetName);
            }

            var tarefa = projetoWork.Tarefas.FirstOrDefault();

            if (tarefa == null)
            {
                await t002_IncluirTarefa_CenarioDeSucesso();
                projetoWork = await RecuperarProjetoTeste(_projetName);
                tarefa = projetoWork.Tarefas.FirstOrDefault();
            }

            tarefa.Comentarios = new List<TarefaComentariosDto>()
            {
                new TarefaComentariosDto { IdUsuario = 1, Comentario = "Comentário 1"},
                new TarefaComentariosDto { IdUsuario = 2, Comentario = "Comentário 2"}
            };

            tarefa.Status = new TarefaStatusDto()
            {
                StatusCode = StatusTarefa.EmExecucao
            };

            tarefa.DataPrevistaTermino = DateTime.Now.AddDays(2);

            var tarefaCriada = await _tarefaService.PatchTarefaAsync(tarefa);

            Assert.IsNotNull(tarefaCriada);

            var tarefaVerifica = await _tarefaService.RecuperarTarefaAsync(tarefaCriada.ProjetoId, tarefaCriada.Id);

            Assert.IsNotNull(tarefaVerifica);

            Assert.AreEqual(tarefaVerifica.Id, tarefaCriada.Id);
            Assert.AreEqual(tarefaVerifica.DataCriacao, tarefaCriada.DataCriacao);
            Assert.AreEqual(tarefaVerifica.Descricao, tarefaCriada.Descricao);
            Assert.AreEqual(tarefaVerifica.Status.StatusCode, tarefaCriada.Status.StatusCode);
            Assert.AreEqual(tarefaVerifica.Usuario.UsuarioId, tarefaCriada.Usuario.UsuarioId);
            Assert.AreEqual(tarefaVerifica.PrioridadeTarefa.PrioridadeCode, tarefaCriada.PrioridadeTarefa.PrioridadeCode);
            Assert.AreEqual(tarefaVerifica.DataInicio, tarefaCriada.DataInicio);
            Assert.AreEqual(tarefaVerifica.DataPrevistaTermino, tarefaCriada.DataPrevistaTermino);
            Assert.AreEqual(tarefaVerifica.DataTermino, tarefaCriada.DataTermino);

            var projetoVerificar = await _projetoService.RecuperarProjetosAsync(tarefaCriada.ProjetoId);

            Assert.AreEqual(projetoVerificar.FirstOrDefault().DataInicio,
                        projetoVerificar.First().Tarefas.Max(d => d.DataInicio));

            Assert.AreEqual((int)projetoVerificar.FirstOrDefault().StatusProjeto.StatusProjetoCode,
            (int)projetoVerificar.First().Tarefas.Max(d => d.Status.StatusCode));

            Assert.AreEqual(projetoVerificar.FirstOrDefault().DataTermino,
            projetoVerificar.First().Tarefas.Max(d => d.DataTermino));

            Assert.IsNotNull(projetoVerificar.FirstOrDefault().DataInicio);
            Assert.IsTrue((int)projetoVerificar.FirstOrDefault().StatusProjeto.StatusProjetoCode == 2);
            Assert.IsNull(projetoVerificar.FirstOrDefault().DataTermino);

            Assert.IsTrue(projetoVerificar.FirstOrDefault().DataPrevisaoTermino.Value.Date == DateTime.Now.AddDays(2).Date);

        }

        [Test]
        public async Task t005_AtulizarTarefa_CenarioDeSucesso_VoltaPendente()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_IncluirProjeto_CenarioDeSucesso();
                projetoWork = await RecuperarProjetoTeste(_projetName);
            }

            var tarefa = projetoWork.Tarefas.FirstOrDefault();

            if (tarefa == null)
            {
                await t002_IncluirTarefa_CenarioDeSucesso();
                projetoWork = await RecuperarProjetoTeste(_projetName);
                tarefa = projetoWork.Tarefas.FirstOrDefault();
            }

            tarefa.Comentarios = new List<TarefaComentariosDto>()
            {
                new TarefaComentariosDto { IdUsuario = 1, Comentario = "Comentário 1"},
                new TarefaComentariosDto { IdUsuario = 2, Comentario = "Comentário 2"}
            };

            tarefa.Status = new TarefaStatusDto()
            {
                StatusCode = StatusTarefa.Pendente
            };

            tarefa.DataPrevistaTermino = DateTime.Now.AddDays(2);

            var tarefaCriada = await _tarefaService.PatchTarefaAsync(tarefa);

            Assert.IsNotNull(tarefaCriada);

            var tarefaVerifica = await _tarefaService.RecuperarTarefaAsync(tarefaCriada.ProjetoId, tarefaCriada.Id);

            Assert.IsNotNull(tarefaVerifica);

            Assert.AreEqual(tarefaVerifica.Id, tarefaCriada.Id);
            Assert.AreEqual(tarefaVerifica.DataCriacao, tarefaCriada.DataCriacao);
            Assert.AreEqual(tarefaVerifica.Descricao, tarefaCriada.Descricao);
            Assert.AreEqual(tarefaVerifica.Status.StatusCode, tarefaCriada.Status.StatusCode);
            Assert.AreEqual(tarefaVerifica.Usuario.UsuarioId, tarefaCriada.Usuario.UsuarioId);
            Assert.AreEqual(tarefaVerifica.PrioridadeTarefa.PrioridadeCode, tarefaCriada.PrioridadeTarefa.PrioridadeCode);
            Assert.AreEqual(tarefaVerifica.DataInicio, tarefaCriada.DataInicio);
            Assert.AreEqual(tarefaVerifica.DataPrevistaTermino, tarefaCriada.DataPrevistaTermino);
            Assert.AreEqual(tarefaVerifica.DataTermino, tarefaCriada.DataTermino);

            var projetoVerificar = await _projetoService.RecuperarProjetosAsync(tarefaCriada.ProjetoId);

            Assert.AreEqual(projetoVerificar.FirstOrDefault().DataInicio,
                        projetoVerificar.First().Tarefas.Max(d => d.DataInicio));

            Assert.AreEqual((int)projetoVerificar.FirstOrDefault().StatusProjeto.StatusProjetoCode,
            (int)projetoVerificar.First().Tarefas.Max(d => d.Status.StatusCode));

            Assert.AreEqual(projetoVerificar.FirstOrDefault().DataTermino,
            projetoVerificar.First().Tarefas.Max(d => d.DataTermino));

        }

        [Test]
        public async Task t006_CenarioDeSucesso_HistoricoSendoGerado()
        {
            //Cria projeto
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_IncluirProjeto_CenarioDeSucesso();
                projetoWork = await RecuperarProjetoTeste(_projetName);
            }

            var tarefa = projetoWork.Tarefas.FirstOrDefault();

            if (tarefa == null)
            {
                await t002_IncluirTarefa_CenarioDeSucesso();
                projetoWork = await RecuperarProjetoTeste(_projetName);
                tarefa = projetoWork.Tarefas.FirstOrDefault();
            }

            tarefa.Comentarios = new List<TarefaComentariosDto>()
            {
                new TarefaComentariosDto { IdUsuario = 1, Comentario = "Comentário 1"},
                new TarefaComentariosDto { IdUsuario = 2, Comentario = "Comentário 2"}
            };

            tarefa.Status = new TarefaStatusDto()
            {
                StatusCode = StatusTarefa.EmExecucao
            };

            tarefa.DataPrevistaTermino = DateTime.Now.AddDays(2);

            var tarefaCriada = await _tarefaService.PatchTarefaAsync(tarefa);

            Assert.IsNotNull(tarefaCriada);

            tarefa.Status = new TarefaStatusDto()
            {
                StatusCode = StatusTarefa.Bloqueada
            };

            tarefaCriada = await _tarefaService.PatchTarefaAsync(tarefa);

            tarefa.Status = new TarefaStatusDto()
            {
                StatusCode = StatusTarefa.Concluida
            };

            tarefaCriada = await _tarefaService.PatchTarefaAsync(tarefa);

            var con = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var cmd = new SqlCommand("select count(1) from historicoTarefas where ProjetoId = " + tarefaCriada.ProjetoId, con);

            cmd.CommandType = System.Data.CommandType.Text;


            try
            {
                con.Open();

                var quantidade = Convert.ToInt32(cmd.ExecuteScalar());

                con.Close();

                Assert.IsTrue(quantidade > 0);
            }
            catch (Exception)
            {
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        [Test]
        public async Task t007_CenarioDeSucesso_UsuarioEGernteParaPegarRelatiios()
        {
            var usuario = await _usuarioRepositorio.RecuperarUsuarioAsync(1);

            if ((int)usuario.Papel.UsuarioPapelCode == 1) Assert.Pass();
            else Assert.Fail();
        }

        private async Task<ProjetoDto> RecuperarProjetoTeste(string NomeProjeto)
        {
            var projetos = await _projetoService.RecuperarProjetosAsync();

            var projetoWork = projetos.Where(p => p.Nome == NomeProjeto).FirstOrDefault();

            return projetoWork;
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_IncluirProjeto_CenarioDeSucesso();
                projetoWork = await RecuperarProjetoTeste(_projetName);
            }

            foreach(var tarefa in projetoWork.Tarefas)
            {
                await _tarefaService.DeleteTarefaAsync(tarefa.ProjetoId, tarefa.Id);
            }

            await _projetoService.DeleteProjetoAsync(projetoWork.Id);

        }
    }
}
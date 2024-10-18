using AJTarefasDomain.Interfaces.Negocio.Projeto;
using AJTarefasDomain.Interfaces.Negocio.Tarefa;
using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasDomain.Projeto;
using AJTarefasDomain.Projeto.Post;
using AJTarefasDomain.Tarefa;
using AJTarefasNegocio.Projeto;
using AJTarefasRecursos.Repositorios.Projeto;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace AJTarefasTestes
{
    public class CasosDeErroTest
    {
        private IConfiguration _config;
        private Fixture _fixture;

        private IProjetoService _projetoService;
        private ITarefaService _tarefaService;

        private IProjetoRepositorio _projetoRepositorio;
        private ITarefaRepositorio _tarefaRepositorio;

        private string _projetName = "Projeto_Teste_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");

        [SetUp]
        public void CasosDeErro()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json", true, true).Build();
            _fixture = new Fixture();

            IServiceCollection builder = new ServiceCollection();

            builder.AddSingleton<IConfiguration>(_config);

            builder.AddScoped<IProjetoService, ProjetoNegocio>();

            builder.AddScoped<ITarefaService, TarefaNegocio>();

            builder.AddScoped<IProjetoRepositorio, ProjetoRepositorio>();

            builder.AddScoped<ITarefaRepositorio, TarefaRepositorio>();

            IServiceProvider serviceProvider = builder.BuildServiceProvider();

            _projetoService = serviceProvider.GetService<IProjetoService>();
            _tarefaService = serviceProvider.GetService<ITarefaService>();
            _projetoRepositorio = serviceProvider.GetService<IProjetoRepositorio>();
            _tarefaRepositorio = serviceProvider.GetService<ITarefaRepositorio>();
        }

        private async Task<ProjetoDto> RecuperarProjetoTeste(string NomeProjeto)
        {
            var projetos = await _projetoService.RecuperarProjetosAsync();

            var projetoWork = projetos.Where(p => p.Nome == NomeProjeto).FirstOrDefault();

            return projetoWork;
        }

        [Test]
        public async Task t001_Criacao_Projeto_Teste()
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
        public async Task t002_CasoDeErro_MudancaDePrioridadeAposCriacao()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_Criacao_Projeto_Teste();
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

            tarefaCriada.PrioridadeTarefa = new TarefaPrioridadeDto()
            {
                PrioridadeCode = PrioridadeTarefa.Alta
            };

            try
            {
                await _tarefaService.PatchTarefaAsync(tarefaCriada);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Não é permitido mudar a prioridade após a criação da tarefa")
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }

        }

        [Test]
        public async Task t003_CasoDeErro_RemocaoProjetoComAtividade()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_Criacao_Projeto_Teste();
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

            tarefaCriada.Status.StatusCode = StatusTarefa.EmExecucao;

            tarefaCriada.DataPrevistaTermino = DateTime.Now.AddDays(2);

            await _tarefaService.PatchTarefaAsync(tarefaCriada);

            try
            {
                await _projetoService.DeleteProjetoAsync(tarefaCriada.ProjetoId);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Não é possível remover o projeto com tarefas associadas")
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }

        }

        [Test]
        public async Task t004_Criacao_Projeto_Sem_Nome()
        {
            var projetoTeste = new PostProjetoRequest()
            {
                NomeProjeto = "",
                DescricaoProjeto = Guid.NewGuid().ToString(),
                UsuarioId = 2
            };
            
            try
            {
                var projeto = await _projetoService.PostProjetoAsync(projetoTeste);
            }
            catch (Exception ex)
            {
                if (ex.Message == "O nome do projeto é obrigatório ou é inexistente.")
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }

        }

        [Test]
        public async Task t005_Criacao_Projeto_Sem_Descricao()
        {
            var projetoTeste = new PostProjetoRequest()
            {
                NomeProjeto = Guid.NewGuid().ToString(),
                DescricaoProjeto = "",
                UsuarioId = 2
            };

            try
            {
                var projeto = await _projetoService.PostProjetoAsync(projetoTeste);
            }
            catch (Exception ex)
            {
                if (ex.Message == "A descrição do projeto é obrigarório")
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }

        }

        [Test]
        public async Task t006_Criacao_Tarefa_Sem_Descricao()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_Criacao_Projeto_Teste();
                projetoWork = await RecuperarProjetoTeste(_projetName);
            }

            var tarefa = new PostTarefaRequest()
            {
                Descricao = "",
                Titulo = Guid.NewGuid().ToString(),
                PrioridadeTarefa = PrioridadeTarefa.Media,
                ProjetoId = projetoWork.Id,
                UsuarioId = 1
            };

            try
            {
                var tarefaCriada = await _tarefaService.PostTarefaAsync(tarefa);
            }
            catch (Exception ex)
            {
                if (ex.Message == "O a tarefa deve ter um descrição")
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }

        }

        [Test]
        public async Task t007_Criacao_Tarefa_Sem_Titulo()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_Criacao_Projeto_Teste();
                projetoWork = await RecuperarProjetoTeste(_projetName);
            }

            var tarefa = new PostTarefaRequest()
            {
                Descricao = Guid.NewGuid().ToString(),
                Titulo = "",
                PrioridadeTarefa = PrioridadeTarefa.Media,
                ProjetoId = projetoWork.Id,
                UsuarioId = 1
            };

            try
            {
                var tarefaCriada = await _tarefaService.PostTarefaAsync(tarefa);
            }
            catch (Exception ex)
            {
                if (ex.Message == "O a tarefa deve ter um título")
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }

        }

        [Test]
        public async Task t008_Criacao_Tarefa_ProjetoInexistente()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_Criacao_Projeto_Teste();
                projetoWork = await RecuperarProjetoTeste(_projetName);
            }

            var tarefa = new PostTarefaRequest()
            {
                Descricao = Guid.NewGuid().ToString(),
                Titulo = Guid.NewGuid().ToString(),
                PrioridadeTarefa = PrioridadeTarefa.Media,
                ProjetoId = 0,
                UsuarioId = 1
            };

            try
            {
                var tarefaCriada = await _tarefaService.PostTarefaAsync(tarefa);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Favor entrar um código de projeto existente")
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }

        }

        [Test]
        public async Task t011_Criacao_Tarefa_QuantidadeMaxTerfasAtingida()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_Criacao_Projeto_Teste();
                projetoWork = await RecuperarProjetoTeste(_projetName);
            }

            var ate = (21 - projetoWork.Tarefas.Count());

            for(int i = 0; i < ate; i++)
            {
                await _tarefaService.PostTarefaAsync(new PostTarefaRequest()
                {
                    Descricao = Guid.NewGuid().ToString(),
                    Titulo = Guid.NewGuid().ToString(),
                    PrioridadeTarefa = PrioridadeTarefa.Media,
                    ProjetoId = projetoWork.Id,
                    UsuarioId = 1
               });
            }

            try
            {
                await _tarefaService.PostTarefaAsync(new PostTarefaRequest()
                {
                    Descricao = Guid.NewGuid().ToString(),
                    Titulo = Guid.NewGuid().ToString(),
                    PrioridadeTarefa = PrioridadeTarefa.Media,
                    ProjetoId = projetoWork.Id,
                    UsuarioId = 1
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Número máximo de tarefas foi atingida para esse projeto.")
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }

        }

        [Test]
        public async Task t010_Criacao_Projeto_Sem_Usuario()
        {
            var projetoTeste = new PostProjetoRequest()
            {
                NomeProjeto = Guid.NewGuid().ToString(),
                DescricaoProjeto = Guid.NewGuid().ToString(),
                UsuarioId = 0
            };

            try
            {
                var projeto = await _projetoService.PostProjetoAsync(projetoTeste);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Usuário é obrigatório ou é inexistente.")
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }

        }

        [Test]
        public async Task t009_Atualizacao_Tarefa_DeOutroProjeto()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_Criacao_Projeto_Teste();
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

            var projetos = await _projetoService.RecuperarProjetosAsync();

            tarefaCriada.DataPrevistaTermino = DateTime.Now.AddDays(2);
            tarefaCriada.ProjetoId = projetos.FirstOrDefault().Id;

            try
            {
                await _tarefaService.PatchTarefaAsync(tarefaCriada);
            }
            catch (Exception ex)
            {
                if (ex.Message == "A tarefa " + tarefaCriada.Id + " não pertence ao projeto ou não é válida")
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }

        }



        [OneTimeTearDown]
        public async Task TearDown()
        {
            var projetoWork = await RecuperarProjetoTeste(_projetName);

            if (projetoWork == null)
            {
                await t001_Criacao_Projeto_Teste();
                projetoWork = await RecuperarProjetoTeste(_projetName);
            }

            foreach (var tarefa in projetoWork.Tarefas)
            {
                await _tarefaService.DeleteTarefaAsync(tarefa.ProjetoId, tarefa.Id);
            }

            await _projetoService.DeleteProjetoAsync(projetoWork.Id);

        }

    }
}

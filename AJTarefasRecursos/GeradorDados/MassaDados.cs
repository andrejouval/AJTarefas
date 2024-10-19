using AJTarefasDomain.Base;
using AJTarefasDomain.Projeto;
using AJTarefasDomain.Tarefa;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace AJTarefasRecursos.GeradorDados
{
    public static class MassaDados
    {
        private static List<UsuarioDto> GerarUsuarios(int quantidade)
        {
            var fakerUsuario = new Faker<UsuarioDto>()
                .RuleFor(u => u.UsuarioId, f => f.IndexFaker + 1)
                .RuleFor(u => u.Nome, f => f.Name.FullName())
                .RuleFor(u => u.Papel, f => f.PickRandom(new List<UsuarioPapelDto>()
                {
                    new UsuarioPapelDto { UsuarioPapelCode = UsuariosPapel.Gerente, Papel = "Gerente"  },
                    new UsuarioPapelDto { UsuarioPapelCode = UsuariosPapel.Usuario, Papel = "Analista 1"},
                    new UsuarioPapelDto { UsuarioPapelCode = UsuariosPapel.Usuario, Papel = "Analista 2"},
                    new UsuarioPapelDto { UsuarioPapelCode = UsuariosPapel.Usuario, Papel = "Analista 3"},
                    new UsuarioPapelDto { UsuarioPapelCode = UsuariosPapel.Usuario, Papel = "Analista 4"}
                }));

            return fakerUsuario.Generate(quantidade);
        }

        public static List<ProjetoDto> GerarProjetosComTarefas(int quantidadeProjetos, int quantidadeUsuarios)
        {
            // Gerar uma lista de usuários
            var usuarios = GerarUsuarios(quantidadeUsuarios);

            var fakerProjeto = new Faker<ProjetoDto>()
                .RuleFor(p => p.Id, f => f.IndexFaker + 1)
                .RuleFor(p => p.Nome, f => f.Company.CompanyName())
                .RuleFor(p => p.Tarefas, (f, p) => GerarTarefasAleatorias(f.Random.Int(3, 20), usuarios))
                .RuleFor(p => p.Descricao, f => f.Lorem.Sentence(15))
                .RuleFor(p => p.Usuario, f => f.PickRandom(usuarios));

            var projetos = fakerProjeto.Generate(quantidadeProjetos);

            foreach (var projeto in projetos)
            {
                foreach (var tarefa in projeto.Tarefas)
                {
                    tarefa.ProjetoId = projeto.Id;
                };

                projeto.DataCriacao = projeto.Tarefas.Min(d => d.DataCriacao);

                var maxStatusCode = projeto.Tarefas.Max(s => (int)s.Status.StatusCode);

                switch (maxStatusCode)
                {
                    case 1:
                        {
                            projeto.StatusProjeto = new ProjetoStatusDto()
                            {
                                StatusProjetoCode = StatusProjeto.Pendente,
                                Status = StatusProjeto.Pendente.GetEnumTextos()
                            };

                            break;
                        }
                    case 2:
                        {
                            projeto.StatusProjeto = new ProjetoStatusDto()
                            {
                                StatusProjetoCode = StatusProjeto.EmExecucao,
                                Status = StatusProjeto.EmExecucao.GetEnumTextos()
                            };

                            break;
                        }
                    case 3:
                        {
                            projeto.StatusProjeto = new ProjetoStatusDto()
                            {
                                StatusProjetoCode = StatusProjeto.Suspenso,
                                Status = StatusProjeto.Suspenso.GetEnumTextos()
                            };

                            break;
                        }
                    case 4:
                        {
                            projeto.StatusProjeto = new ProjetoStatusDto()
                            {
                                StatusProjetoCode = StatusProjeto.Concluido,
                                Status = StatusProjeto.Concluido.GetEnumTextos()
                            };

                            break;
                        }
                    case 5:
                        {
                            projeto.StatusProjeto = new ProjetoStatusDto()
                            {
                                StatusProjetoCode = StatusProjeto.Cancelado,
                                Status = StatusProjeto.Cancelado.GetEnumTextos()
                            };

                            break;
                        }
                }

                projeto.DataPrevisaoTermino = projeto.Tarefas.Max(d => d.DataPrevistaTermino);

                projeto.DataTermino = maxStatusCode == 4 ? projeto.Tarefas.Max(d => d.DataTermino) : null;
            }

            return projetos;
        }


        private static List<TarefaDto> GerarTarefasAleatorias(int quantidadeTarefas, List<UsuarioDto> usuarios)
        {
            var fakerTarefa = new Faker<TarefaDto>()
                .RuleFor(t => t.Id, f => f.IndexFaker + 1)
                .RuleFor(t => t.Titulo, f => f.Lorem.Sentence(3))
                .RuleFor(t => t.Descricao, f => f.Lorem.Sentence(10))
                .RuleFor(t => t.Status, f => f.PickRandom(new List<TarefaStatusDto>()
                {
                    new TarefaStatusDto { StatusCode = StatusTarefa.Pendente, Status = StatusTarefa.Pendente.GetEnumTextos() },
                    new TarefaStatusDto { StatusCode = StatusTarefa.Bloqueada , Status = StatusTarefa.Bloqueada.GetEnumTextos() },
                    new TarefaStatusDto { StatusCode = StatusTarefa.EmExecucao, Status = StatusTarefa.EmExecucao.GetEnumTextos() },
                    new TarefaStatusDto { StatusCode = StatusTarefa.Concluida, Status = StatusTarefa.Concluida.GetEnumTextos() },
                }))
                .RuleFor(t => t.DataCriacao, f => DateTime.Now.AddDays(f.Random.Double(-365)))
                .RuleFor(t => t.DataInicio, (f, t) => DataAbertura(t.DataCriacao, f.Random.Double(0, 15), (int)t.Status.StatusCode))
                .RuleFor(t => t.DataTermino, (f, t) => DataTermino(t.DataInicio, (int)t.Status.StatusCode, f.Random.Double(365)))
                .RuleFor(t => t.DataPrevistaTermino, (f, t) => DataPrevistaTermino(t.DataInicio, (int)t.Status.StatusCode, f.Random.Double(15)))
                .RuleFor(t => t.PrioridadeTarefa, f => f.PickRandom(new List<TarefaPrioridadeDto>()
                {
                    new TarefaPrioridadeDto { PrioridadeCode = PrioridadeTarefa.Baixa, Prioridade = PrioridadeTarefa.Baixa.GetEnumTextos() },
                    new TarefaPrioridadeDto { PrioridadeCode = PrioridadeTarefa.Media, Prioridade = PrioridadeTarefa.Media.GetEnumTextos() },
                    new TarefaPrioridadeDto { PrioridadeCode = PrioridadeTarefa.Alta, Prioridade = PrioridadeTarefa.Alta.GetEnumTextos() },
                }))
                .RuleFor(t => t.Usuario, f => f.PickRandom(usuarios));

            return fakerTarefa.Generate(quantidadeTarefas);
        }

        private static DateTime? DataAbertura(DateTime Date, double Item, int StatusCode)
        {
            if ((Item == 0) || (StatusCode == 1))
            {
                return null;
            }
            else
            {
                return Date.AddDays(Item);
            }
        }

        private static DateTime? DataTermino(DateTime? Date, int StatusCode, double Days)
        {
            if (StatusCode == 4)
            {
                return Date.Value.AddDays(Days);
            }
            else
            {
                return null;
            }
        }

        private static DateTime? DataPrevistaTermino(DateTime? Date, int StatusCode, double Days)
        {
            if (StatusCode == 1)
            {
                return null;
            }
            else
            {
                return Date.Value.AddDays(Days);
            }
        }
    }
}

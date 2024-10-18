using AJTarefasDomain.Base;
using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasDomain.Projeto;
using AJTarefasDomain.Projeto.Post;
using AJTarefasDomain.Tarefa;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AJTarefasRecursos.Repositorios.Projeto
{
    public class ProjetoRepositorio : IProjetoRepositorio
    {
        private SqlConnection _con = new SqlConnection();
        private readonly ITarefaRepositorio _tarefaRepositorio;
        public ProjetoRepositorio(IConfiguration configuration, ITarefaRepositorio tarefaRepositorio) 
        {
            _con.ConnectionString = configuration["ConnectionStrings:DefaultConnection"];
            _tarefaRepositorio = tarefaRepositorio;
        }

        public async Task<int> PostProjetoAsync(PostProjetoRequest Projeto)
        {
            var cmd = new SqlCommand(@"insert into Projetos(
                                        NomeProjeto
                                        , DescricaoProjeto
                                        , DataCriacao
                                        , StatusProjeto
                                        , UsuarioId
                                        )
                                        output inserted.Id
                                        values
                                        (
                                        @nomeProjeto,
                                        @descricaoProjeto,
                                        getdate(),
                                        1,
                                        @usuarioId
                                        )", _con);

            cmd.CommandType = System.Data.CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@nomeProjeto", System.Data.SqlDbType.VarChar, 300)).Value = Projeto.NomeProjeto;
            cmd.Parameters.Add(new SqlParameter("@descricaoProjeto", System.Data.SqlDbType.VarChar, 8000)).Value = Projeto.DescricaoProjeto;
            cmd.Parameters.Add(new SqlParameter("@usuarioId", System.Data.SqlDbType.Int)).Value = Projeto.UsuarioId;

            try
            {
                _con.Open();

                var id = await cmd.ExecuteScalarAsync();

                _con.Close();

                return Convert.ToInt32(id);
            }
            catch (System.Exception)
            {
                if (_con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
                throw;
            }

        }

        public async Task<bool> ProjetoExisteAsync(int ProjetoId)
        {
            var cmd = new SqlCommand(@"select count(1) from Projetos where Id = " + ProjetoId, _con);

            cmd.CommandType = System.Data.CommandType.Text;

            try
            {
                _con.Open();

                var existe = await cmd.ExecuteScalarAsync();

                _con.Close();

                return Convert.ToBoolean(existe);
            }
            catch (System.Exception)
            {
                if (_con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
                throw;
            }

        }

        public async Task<ProjetoDto> RecuperarProjetoAsync(int ProjetoId)
        {
            var projetos = await RecuperarProjetosAsync(ProjetoId, null);

            var projeto = projetos.Where(p => p.Id == ProjetoId).FirstOrDefault();

            return projeto;
        }

        public async Task PatchProjetoAsync(int ProjetoId)
        {
            var cmd = new SqlCommand(@"sp_AtualizarProjeto", _con);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@projetoId", System.Data.SqlDbType.Int)).Value = ProjetoId;

            try
            {
                _con.Open();

                await cmd.ExecuteNonQueryAsync();

                _con.Close();

            }
            catch (System.Exception)
            {
                if (_con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
                throw;
            }

        }

        public async Task<int> RecuperarQuantidadeTarefasAsync(int ProjetoId)
        {
            var cmd = new SqlCommand(@"select count(1) from Projetos p inner join Tarefas t 
                                        on t.ProjetoId = p.Id where p.Id = " + ProjetoId, _con);

            cmd.CommandType = System.Data.CommandType.Text;

            try
            {
                _con.Open();

                var quantidade = await cmd.ExecuteScalarAsync();

                _con.Close();

                return Convert.ToInt32(quantidade);
            }
            catch (System.Exception)
            {
                if (_con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
                throw;
            }

        }

        public async Task DeleteProjetoAsync(int ProjetoId)
        {
            var cmd = new SqlCommand(@"delete Projetos where Id = " + ProjetoId, _con);

            cmd.CommandType = System.Data.CommandType.Text;

            try
            {
                _con.Open();

                await cmd.ExecuteNonQueryAsync();

                _con.Close();

            }
            catch (System.Exception)
            {
                if (_con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
                throw;
            }

        }

        public async Task<IEnumerable<ProjetoDto>> RecuperarProjetosAsync(int? ProjetoId, int? UsuarioId)
        {
            var projetos = new List<ProjetoDto>();

            ProjetoDto projeto = null;

            List<TarefaDto> tarefas = null;

            var query = @"select p.Id, p.NomeProjeto, p.DescricaoProjeto, p.DataCriacao, p.DataInicio, 
                                        p.DataTermino, p.DataPrevisaoTermino, p.StatusProjeto, p.UsuarioId,
                                        u.Nome, u.Papel, t.Id as 'TarefaId'
                                        from Projetos p
                                        inner join Usuarios u
                                        on p.UsuarioId = u.Id 
		                                left join Tarefas t
		                                on t.ProjetoId = p.Id where 1=1 ";

            if (ProjetoId != null)
            {
                query += " and p.Id = " + ProjetoId;
            }

            if (UsuarioId != null)
            {
                query += " and p.UsuarioId = " + UsuarioId;
            }

            var cmd = new SqlCommand(query, _con);

            cmd.CommandType = System.Data.CommandType.Text;

            var projetoAtual = 0;

            try
            {
                _con.Open();

                var reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    if (projetoAtual != Convert.ToInt32(reader["Id"]))
                    {
                        if (projeto != null)
                        {
                            projetos.Add(projeto);
                        }

                        projetoAtual = Convert.ToInt32(reader["Id"]);

                        tarefas = new List<TarefaDto>();

                        projeto = new ProjetoDto()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Descricao = reader["DescricaoProjeto"].ToString(),
                            Nome = reader["NomeProjeto"].ToString(),
                            DataCriacao = Convert.ToDateTime(reader["DataCriacao"]),
                            StatusProjeto = new ProjetoStatusDto()
                            {
                                StatusProjetoCode = (StatusProjeto)Convert.ToInt32(reader["StatusProjeto"]),
                                Status = ((StatusProjeto)Convert.ToInt32(reader["StatusProjeto"])).GetEnumTextos()
                            },
                            Usuario = new UsuarioDto()
                            {
                                Nome = reader["Nome"].ToString(),
                                UsuarioId = Convert.ToInt32(reader["UsuarioId"]),
                                Papel = new UsuarioPapelDto()
                                {
                                    UsuarioPapelCode = (UsuariosPapel)Convert.ToInt32(reader["Papel"]),
                                    Papel = ((UsuariosPapel)Convert.ToInt32(reader["Papel"])).GetEnumTextos()
                                }
                            }
                        };

                        if (reader["DataInicio"] != DBNull.Value)
                        {
                            projeto.DataInicio = Convert.ToDateTime(reader["DataInicio"]);
                        }

                        if (reader["DataPrevisaoTermino"] != DBNull.Value)
                        {
                            projeto.DataPrevisaoTermino = Convert.ToDateTime(reader["DataPrevisaoTermino"]);
                        }

                        if (reader["DataTermino"] != DBNull.Value)
                        {
                            projeto.DataTermino = Convert.ToDateTime(reader["DataTermino"]);
                        }

                        projeto.Tarefas = tarefas;
                    }

                    if (reader["TarefaId"] != DBNull.Value)
                    {
                        var tarefa = await _tarefaRepositorio.RecuperarTarefaAsync(projetoAtual, Convert.ToInt32(reader["TarefaId"]));

                        tarefas.Add(tarefa);
                    }
                }

                projetos.Add(projeto);

                _con.Close();

                return projetos;
            }
            catch (System.Exception)
            {
                if (_con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
                throw;
            }

        }


    }
}

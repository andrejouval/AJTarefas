using AJTarefasDomain.Base;
using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasDomain.Tarefa;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace AJTarefasRecursos.Repositorios.Projeto
{
    public class TarefaRepositorio : ITarefaRepositorio
    {
        private SqlConnection _con = new SqlConnection();
        public TarefaRepositorio(IConfiguration configuration) 
        {
            _con.ConnectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public async Task<int> PostTarefaAsync(PostTarefaRequest Tarefa)
        {
            var cmd = new SqlCommand(@"insert into Tarefas
                                        (
                                        ProjetoId
                                        , Titulo
                                        , Descricao
                                        , DataCriacao
                                        , PrioridadeTarefa
                                        , StatusTarefa
                                        )
                                        output inserted.id
                                        values
                                        (
                                        @projetoId,
                                        @titulo,
                                        @descricao,
                                        getdate(),
                                        @prioridade,
                                        1
                                        )", _con);

            cmd.CommandType = System.Data.CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@projetoId", System.Data.SqlDbType.Int)).Value = Tarefa.ProjetoId;
            cmd.Parameters.Add(new SqlParameter("@titulo", System.Data.SqlDbType.VarChar, 30)).Value = Tarefa.Titulo;
            cmd.Parameters.Add(new SqlParameter("@descricao", System.Data.SqlDbType.VarChar, 8000)).Value = Tarefa.Descricao;
            
            if (Tarefa.PrioridadeTarefa != null)
            {
                cmd.Parameters.Add(new SqlParameter("@prioridade", System.Data.SqlDbType.Int)).Value = Tarefa.PrioridadeTarefa;
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@prioridade", System.Data.SqlDbType.Int)).Value = DBNull.Value;
            }

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

        public async Task PatchTarefaAsync(TarefaDto Tarefa)
        {
            var cmd = new SqlCommand(@"update Tarefas 
                                        set Titulo = @titulo
                                        , DataInicio = @dataInicio
                                        , DataPrevistaTermino = @dataPrevistaTermino
                                        , DataTermino = @dataTermino
                                        , StatusTarefa = @statusTarefa
                                        , Descricao = @descricao
                                        where ProjetoId = @projetoId
                                        and Id = @Id
                                        ", _con);

            cmd.CommandType = System.Data.CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@projetoId", SqlDbType.Int)).Value = Tarefa.ProjetoId;
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Tarefa.Id;
            cmd.Parameters.Add(new SqlParameter("@titulo", SqlDbType.VarChar, 30)).Value = Tarefa.Titulo;
            cmd.Parameters.Add(new SqlParameter("@descricao", SqlDbType.VarChar, 8000)).Value = Tarefa.Descricao;
            cmd.Parameters.Add(new SqlParameter("@statusTarefa", SqlDbType.Int)).Value = Tarefa.Status.StatusCode;

            if (Tarefa.DataInicio != null)
            {
                cmd.Parameters.Add(new SqlParameter("@dataInicio", SqlDbType.DateTime)).Value = Tarefa.DataInicio;
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@dataInicio", SqlDbType.DateTime)).Value = DBNull.Value;
            }

            if (Tarefa.DataTermino != null)
            {
                cmd.Parameters.Add(new SqlParameter("@dataTermino", SqlDbType.DateTime)).Value = Tarefa.DataTermino;
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@dataTermino", SqlDbType.DateTime)).Value = DBNull.Value;
            }

            if (Tarefa.DataPrevistaTermino != null)
            {
                cmd.Parameters.Add(new SqlParameter("@dataPrevistaTermino", SqlDbType.DateTime)).Value = Tarefa.DataPrevistaTermino;
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@dataPrevistaTermino", SqlDbType.DateTime)).Value = DBNull.Value;
            }

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

        public async Task<bool> TarefaExisteAsync(int ProjetoId, int Id)
        {
            var cmd = new SqlCommand(@"select count(1) from Tarefas where Id = " + Id + " and ProjetoId = " + ProjetoId, _con);

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

        public async Task<TarefaDto> RecuperarTarefaAsync(int ProjetoId, int Id)
        {
            var cmd = new SqlCommand(@"select Id
                                        , ProjetoId
                                        , Titulo
                                        , Descricao
                                        , DataCriacao
                                        , DataInicio
                                        , DataPrevistaTermino
                                        , DataTermino
                                        , PrioridadeTarefa
                                        , StatusTarefa from Tarefas where Id = " + Id + " and ProjetoId = " + ProjetoId, _con);

            cmd.CommandType = System.Data.CommandType.Text;

            try
            {
                var tarefa = new TarefaDto();

                _con.Open();

                var reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    tarefa.PrioridadeTarefa = new TarefaPrioridadeDto()
                    {
                        PrioridadeCode = (PrioridadeTarefa)Convert.ToInt32(reader["PrioridadeTarefa"]),
                        Prioridade = ((PrioridadeTarefa)Convert.ToInt32(reader["PrioridadeTarefa"])).GetEnumTextos()
                    };

                    if (reader["DataTermino"] != DBNull.Value)
                    {
                        tarefa.DataTermino = Convert.ToDateTime(reader["DataTermino"]);
                    }

                    tarefa.Status = new TarefaStatusDto()
                    {
                        StatusCode = (StatusTarefa)Convert.ToInt32(reader["StatusTarefa"]),
                        Status = ((StatusTarefa)Convert.ToInt32(reader["StatusTarefa"])).GetEnumTextos()
                    };

                    if (reader["DataPrevistaTermino"] != DBNull.Value)
                    {
                        tarefa.DataPrevistaTermino = Convert.ToDateTime(reader["DataPrevistaTermino"]);
                    };

                    if (reader["DataCriacao"] != DBNull.Value)
                    {
                        tarefa.DataCriacao = Convert.ToDateTime(reader["DataCriacao"]);
                    };

                    if (reader["DataInicio"] != DBNull.Value)
                    {
                        tarefa.DataInicio = Convert.ToDateTime(reader["DataInicio"]);
                    };

                    tarefa.Descricao = reader["Descricao"].ToString();

                    tarefa.Titulo = reader["Titulo"].ToString();

                    tarefa.Id = Id;

                    tarefa.ProjetoId = ProjetoId;

                }
                _con.Close();

                return tarefa;
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

        public async Task<int> RecuperarQuantidadeTarefasAsync(int ProjetoId, int Id)
        {
            var cmd = new SqlCommand(@"select count(1) from Tarefas where Id = " + Id + " and ProjetoId = " + ProjetoId, _con);

            cmd.CommandType = System.Data.CommandType.Text;

            try
            {
                _con.Open();

                var prioridade = await cmd.ExecuteScalarAsync();

                _con.Close();

                return Convert.ToInt32(prioridade);
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

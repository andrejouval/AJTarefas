using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasDomain.Tarefa;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
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

        public async Task<int> PostTarefa(PostTarefaRequest Tarefa)
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

    }
}

using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasDomain.Projeto.Post;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AJTarefasRecursos.Repositorios.Projeto
{
    public class ProjetoRepositorio : IProjetoRepositorio
    {
        private SqlConnection _con = new SqlConnection();
        public ProjetoRepositorio(IConfiguration configuration) 
        {
            _con.ConnectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public async Task<int> PostProjetoAsync(PostProjetoRequest Projeto)
        {
            var cmd = new SqlCommand(@"insert into Projetos(
                                        NomeProjeto
                                        , DescricaoProjeto
                                        , DataCriacao
                                        , StatusProjeto
                                        )
                                        output inserted.Id
                                        values
                                        (
                                        @NomeProjeto,
                                        @DescricaoProjeto,
                                        getdate(),
                                        1
                                        )", _con);

            cmd.CommandType = System.Data.CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@NomeProjeto", System.Data.SqlDbType.VarChar, 300)).Value = Projeto.NomeProjeto;
            cmd.Parameters.Add(new SqlParameter("@DescricaoProjeto", System.Data.SqlDbType.VarChar, 8000)).Value = Projeto.DescricaoProjeto;

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


    }
}

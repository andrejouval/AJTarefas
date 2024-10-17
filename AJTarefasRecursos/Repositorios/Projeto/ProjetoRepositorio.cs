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

        public async Task<int> PostProjeto(PostProjetoRequest Projeto)
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

    }
}

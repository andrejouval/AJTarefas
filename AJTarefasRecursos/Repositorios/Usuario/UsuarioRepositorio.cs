using AJTarefasDomain.Base;
using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasDomain.Interfaces.Repositorio.Usuario;
using AJTarefasDomain.Projeto;
using AJTarefasDomain.Tarefa;
using AJTarefasRecursos.Repositorios.Projeto;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AJTarefasRecursos.Repositorios.Usuario
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private SqlConnection _con = new SqlConnection();

        public UsuarioRepositorio(IConfiguration configuration, ITarefaRepositorio tarefaRepositorio)
        {
            _con.ConnectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public async Task<UsuarioDto> RecuperarUsuarioAsync(int UsuarioId)
        {
            var usuario = new UsuarioDto();

            var query = @"select id, nome, papel from Usuarios where id = " + UsuarioId;

            var cmd = new SqlCommand(query, _con);

            cmd.CommandType = System.Data.CommandType.Text;

            try
            {
                _con.Open();

                var reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    usuario.UsuarioId = UsuarioId;
                    usuario.Nome = reader["nome"].ToString();
                    var papel = Convert.ToInt32(reader["papel"]);
                    usuario.Papel = new UsuarioPapelDto()
                    {
                        UsuarioPapelCode = papel == 1 ? UsuariosPapel.Gerente : UsuariosPapel.Usuario,
                        Papel = papel == 1 ? UsuariosPapel.Gerente.GetEnumTextos() : UsuariosPapel.Usuario.GetEnumTextos(),
                    };
                }

                _con.Close();

                return usuario;
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

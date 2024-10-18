using AJTarefasDomain.Base;
using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Projeto.Base
{
    public class BaseUsuarioResponse
    {
        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("usuariosPapel")]
        public BaseUsuarioPapelResponse UsuariosPapel { get; set; }
    }
}

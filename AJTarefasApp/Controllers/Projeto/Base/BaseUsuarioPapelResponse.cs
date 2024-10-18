using AJTarefasDomain.Base;
using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Projeto.Base
{
    public class BaseUsuarioPapelResponse
    {
        [JsonPropertyName("usuariosPapelCode")]
        public UsuariosPapel UsuariosPapelCode { get; set; }

        [JsonPropertyName("papel")]
        public string Papel {  get; set; }
    }
}

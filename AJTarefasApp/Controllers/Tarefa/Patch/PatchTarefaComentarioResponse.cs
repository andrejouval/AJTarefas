using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Tarefa.Patch
{
    public class PatchTarefaComentarioResponse
    {
        [JsonPropertyName("idUsuario")]
        public int IdUsuario { get; set; }

        [JsonPropertyName("comentario")]
        public string Comentario { get; set; }
    }
}

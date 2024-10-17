using AJTarefasDomain.Tarefa;
using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Tarefa.Post
{
    public class PostTarefaStatusResponse
    {
        [JsonPropertyName("prioridadeCode")]
        public StatusTarefa StatusCode { get; set; }

        [JsonPropertyName("prioridade")]
        public string Status { get; set; }

    }
}

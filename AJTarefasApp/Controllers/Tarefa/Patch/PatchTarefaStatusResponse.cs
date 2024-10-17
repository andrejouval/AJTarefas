using AJTarefasDomain.Tarefa;
using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Tarefa.Patch
{
    public class PatchTarefaStatusResponse
    {
        [JsonPropertyName("prioridadeCode")]
        public StatusTarefa StatusCode { get; set; }

        [JsonPropertyName("prioridade")]
        public string Status { get; set; }

    }
}

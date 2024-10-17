using AJTarefasDomain.Tarefa;
using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Tarefa.Patch
{
    public class PatchTarefaPrioridadeResponse
    {
        [JsonPropertyName("prioridadeCode")]
        public PrioridadeTarefa PrioridadeCode { get; set; }

        [JsonPropertyName("prioridade")]
        public string Prioridade { get; set; }
    }
}

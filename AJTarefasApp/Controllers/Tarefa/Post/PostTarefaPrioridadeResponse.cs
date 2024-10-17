using AJTarefasDomain.Tarefa;
using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Tarefa.Post
{
    public class PostTarefaPrioridadeResponse
    {
        [JsonPropertyName("prioridadeCode")]
        public PrioridadeTarefa PrioridadeCode { get; set; }

        [JsonPropertyName("prioridade")]
        public string Prioridade { get; set; }
    }
}

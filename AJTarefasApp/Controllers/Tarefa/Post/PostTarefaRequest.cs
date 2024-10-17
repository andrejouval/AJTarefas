using AJTarefasDomain.Tarefa;
using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Tarefa.Post
{
    public class PostTarefaRequest
    {
        [JsonPropertyName("projetoId")]
        public int ProjetoId { get; set; }

        [JsonPropertyName("titulo")] 
        public string Titulo { get; set; }
        
        [JsonPropertyName("descricao")] 
        public string Descricao { get; set; }

        [JsonPropertyName("prioridadeTarefa")]
        public PrioridadeTarefa PrioridadeTarefa { get; set; }
    }
}

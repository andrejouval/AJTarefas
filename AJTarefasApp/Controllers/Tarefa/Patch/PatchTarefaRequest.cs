using AJTarefasDomain.Tarefa;
using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Tarefa.Patch
{
    public class PatchTarefaRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("projetoId")]
        public int ProjetoId { get; set; }

        [JsonPropertyName("titulo")] 
        public string Titulo { get; set; } = string.Empty;

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; } = String.Empty;

        [JsonPropertyName("prioridadeTarefa")]
        public PrioridadeTarefa PrioridadeTarefa { get; set; }

        [JsonPropertyName("statusTarefa")]
        public StatusTarefa StatusTarefa { get; set; }

        [JsonPropertyName("dataPrevistaTermino")]
        public DateTime DataPrevistaTermino { get; set; }

        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonPropertyName("comentarios")]
        public IEnumerable<PatchTarefaComentarioResponse> Comentarios { get; set; }
    }
}

using AJTarefasDomain.Tarefa;
using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Tarefa.Patch
{
    public class PatchTarefaResponse
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("projetoId")]
        public int ProjetoId { get; set; }

        [JsonPropertyName("titulo")]
        public string Titulo { get; set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("prioridadeTarefa")]
        public PatchTarefaPrioridadeResponse? PrioridadeTarefa { get; set; }

        [JsonPropertyName("dataCriacao")]
        public DateTime DataCriacao { get; set; }

        [JsonPropertyName("status")]
        public PatchTarefaStatusResponse Status {  get; set; }

        [JsonPropertyName("dataPrevistaTermino")]
        public DateTime? DataPrevistaTermino { get; set; }

        [JsonPropertyName("dataInico")]
        public DateTime? DataInico { get; set; }

        [JsonPropertyName("dataTermino")]
        public DateTime? DataTermino { get; set; }

        public IEnumerable<PatchTarefaComentarioResponse> Comentarios { get; set; }

    }

}

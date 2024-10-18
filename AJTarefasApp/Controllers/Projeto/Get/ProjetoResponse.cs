using AJTarefasApp.Controllers.Projeto.Base;
using AJTarefasApp.Controllers.Projeto.Post;
using AJTarefasApp.Controllers.Tarefa.Patch;
using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Projeto.Get
{
    public class ProjetoResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nomeProjeto")]
        public string NomeProjeto { get; set; }

        [JsonPropertyName("descricaoProjeto")]
        public string DescricaoProjeto { get; set; }

        [JsonPropertyName("dataCriacao")]
        public DateTime DataCriacao { get; set; }

        [JsonPropertyName("dataInicio")]
        public DateTime DataInicio { get; set; }

        [JsonPropertyName("dataTerminoPrevisto")]
        public DateTime DataTerminoPrevisto { get; set; }

        [JsonPropertyName("dataTermino")]
        public DateTime DataTermino { get; set; }

        [JsonPropertyName("statusProjeto")]
        public PostProjetoStatusResponse StatusProjeto { get; set; }

        [JsonPropertyName("usuarioId")]
        public BaseUsuarioResponse Usuario { get; set; }

        [JsonPropertyName("tarefas")]
        public IEnumerable<PatchTarefaResponse> Tarefas { get; set; }

    }
}

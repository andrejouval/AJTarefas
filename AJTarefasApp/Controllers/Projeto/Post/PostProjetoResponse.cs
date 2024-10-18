using AJTarefasApp.Controllers.Projeto.Base;
using AJTarefasDomain.Projeto;
using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Projeto.Post
{
    public class PostProjetoResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nomeProjeto")]
        public string NomeProjeto { get; set; }

        [JsonPropertyName("descricaoProjeto")]
        public string DescricaoProjeto { get; set; }

        [JsonPropertyName("dataCriacao")]
        public DateTime DataCriacao { get; set; }

        [JsonPropertyName("statusProjeto")]
        public PostProjetoStatusResponse StatusProjeto { get; set; }

        [JsonPropertyName("usuarioId")]
        public BaseUsuarioResponse Usuario { get; set; }


    }
}

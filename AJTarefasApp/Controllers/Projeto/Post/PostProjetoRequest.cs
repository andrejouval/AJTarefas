using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Projeto.Post
{
    public class PostProjetoRequest
    {
        [JsonPropertyName("nomeProjeto")]
        public string NomeProjeto { get; set; }
        [JsonPropertyName("DescricaoProjeto")]
        public string DescricaoProjeto { get; set; }
        
    }
}

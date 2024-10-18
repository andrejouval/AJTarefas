using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Projeto.Post
{
    public class PostProjetoRequest
    {
        [JsonPropertyName("nomeProjeto")]
        public string NomeProjeto { get; set; }

        [JsonPropertyName("descricaoProjeto")]
        public string DescricaoProjeto { get; set; }

        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }
        
    }
}

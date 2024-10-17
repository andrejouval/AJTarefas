using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Projeto.Post
{
    public class PostProjetoResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}

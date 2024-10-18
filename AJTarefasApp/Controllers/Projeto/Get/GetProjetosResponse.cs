using System.Text.Json.Serialization;

namespace AJTarefasApp.Controllers.Projeto.Get
{
    public class GetProjetosResponse
    {
        [JsonPropertyName("projetos")]
        public IEnumerable<ProjetoResponse> Projetos { get; set; }
    }
}

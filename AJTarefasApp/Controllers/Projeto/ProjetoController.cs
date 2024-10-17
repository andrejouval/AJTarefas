using AJTarefasApp.Controllers.Projeto.Post;
using AJTarefasDomain.Interfaces.Negocio.Projeto;
using Microsoft.AspNetCore.Mvc;

namespace AJTarefasApp.Controllers.Projeto
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetoController : ControllerBase
    {
        private readonly IProjetoService _projeto;

        public ProjetoController(IProjetoService projeto)
        {
            _projeto = projeto;
        }

        [HttpPost]
        public async Task<IActionResult> ProjetoAsync(PostProjetoRequest Projeto)
        {
            var Id = await _projeto.PostProjetoAsync(new AJTarefasDomain.Projeto.Post.PostProjetoRequest()
            {
                NomeProjeto = Projeto.NomeProjeto,
                DescricaoProjeto = Projeto.DescricaoProjeto
            });

            return Ok();
        }
    }
}

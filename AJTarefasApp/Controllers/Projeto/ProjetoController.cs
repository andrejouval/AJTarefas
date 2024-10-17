using AJTarefasApp.Controllers.Projeto.Post;
using AJTarefasDomain.Base;
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
            try
            {
                var id = await _projeto.PostProjetoAsync(new AJTarefasDomain.Projeto.Post.PostProjetoRequest()
                {
                    NomeProjeto = Projeto.NomeProjeto,
                    DescricaoProjeto = Projeto.DescricaoProjeto
                });

                var retorno = new PostProjetoResponse()
                {
                    Id = id
                };

                return Ok(BaseResponse<object>.SuccessResponse(retorno));
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                if (ex.InnerException != null)
                {
                    message = message + " - " + ex.InnerException.Message;
                }

                return BadRequest(BaseResponse<object>.ErrorResponse(message));
            }
        }
    }
}

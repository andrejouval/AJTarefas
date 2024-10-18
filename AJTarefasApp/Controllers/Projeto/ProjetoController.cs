using AJTarefasApp.Controllers.Projeto.Get;
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
                var projeto = await _projeto.PostProjetoAsync(new AJTarefasDomain.Projeto.Post.PostProjetoRequest()
                {
                    NomeProjeto = Projeto.NomeProjeto,
                    DescricaoProjeto = Projeto.DescricaoProjeto,
                    UsuarioId = Projeto.UsuarioId
                });

                var retorno = new PostProjetoResponse()
                {
                    Id = projeto.Id,
                    NomeProjeto = Projeto.NomeProjeto,
                    DescricaoProjeto = Projeto.DescricaoProjeto,
                    DataCriacao = DateTime.Now,
                    StatusProjeto = new PostProjetoStatusResponse()
                    {
                        StatusCode = AJTarefasDomain.Projeto.StatusProjeto.Pendente,
                        Status = AJTarefasDomain.Projeto.StatusProjeto.Pendente.GetEnumTextos()
                    },
                    Usuario = new Base.BaseUsuarioResponse()
                    {
                        Nome = projeto.Usuario.Nome,
                        UsuarioId = projeto.Usuario.UsuarioId,
                        UsuariosPapel = new Base.BaseUsuarioPapelResponse()
                        {
                            UsuariosPapelCode = projeto.Usuario.Papel.UsuarioPapelCode,
                            Papel = projeto.Usuario.Papel.Papel
                        }
                    }
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjetoAsync([FromRoute(Name = "id")] int Id)
        {
            try
            {
                await _projeto.DeleteProjetoAsync(Id);


                return Ok(BaseResponse<object>.SuccessResponse(null));
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

        [HttpGet]
        public async Task<IActionResult> GetProjetoAsync()
        {
            try
            {
                var projetos = await _projeto.RecuperarProjetosAsync();

                return Ok(BaseResponse<object>.SuccessResponse(projetos));
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

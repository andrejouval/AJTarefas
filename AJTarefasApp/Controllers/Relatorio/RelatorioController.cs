using AJTarefasApp.Controllers.Projeto.Post;
using AJTarefasDomain.Base;
using AJTarefasDomain.Interfaces.Negocio.Projeto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AJTarefasApp.Controllers.Relatorio
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioController : ControllerBase
    {
        private readonly IProjetoService _projeto;

        public RelatorioController(IProjetoService projeto)
        {
            _projeto = projeto;
        }

        [HttpGet("TarefasMediasConcluidasPorMesPorUsuario")]
        public async Task<IActionResult> RelatorioMediaConclusaoMes()
        {
            try
            {
                var dados = await _projeto.RelatorioMediasTarefasConcluidasUsuarioMesAsync();

                return Ok(BaseResponse<object>.SuccessResponse(dados));

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

        [HttpGet("TarefasConcluidasPorMesPorUsuario")]
        public async Task<IActionResult> RelatorioConclusaoMesPorUsuario()
        {
            try
            {
                var dados = await _projeto.RelatorioTarefasConcluidasUsuarioMesAsync();

                return Ok(BaseResponse<object>.SuccessResponse(dados));

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

using AJTarefasDomain.Base;
using AJTarefasDomain.Interfaces;
using AJTarefasDomain.Interfaces.Negocio.Projeto;
using Microsoft.AspNetCore.Mvc;

namespace AJTarefasApp.Controllers.Relatorio
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioController : ControllerBase
    {
        private readonly IRelatorio _relatorio;

        public RelatorioController(IProjetoService projeto, IRelatorio relatorio)
        {
            _relatorio = relatorio;
        }

        [HttpGet("Json/TarefasMediasConcluidasPorMesPorUsuario/{UsuarioId}")]
        public async Task<IActionResult> RelatorioMediaConclusaoMes([FromRoute(Name = "UsuarioId")] int UsuarioId)
        {
            try
            {
                var dados = await _relatorio.RelatorioMediasTarefasConcluidasUsuarioMesAsync(UsuarioId);

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

        [HttpGet("Json/TarefasConcluidasPorMesPorUsuario/{UsuarioId}")]
        public async Task<IActionResult> RelatorioConclusaoMesPorUsuario([FromRoute(Name = "UsuarioId")] int UsuarioId)
        {
            try
            {
                var dados = await _relatorio.RelatorioTarefasConcluidasUsuarioMesAsync(UsuarioId);

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

        [HttpGet("Excel/TarefasMediasConcluidasPorMesPorUsuario/{UsuarioId}")]
        public async Task<IActionResult> ExcelelatorioMediaConclusaoMes([FromRoute(Name = "UsuarioId")] int UsuarioId)
        {
            try
            {
                var dados = await _relatorio.ExcelRelatorioMediasTarefasConcluidasUsuarioMesAsync(UsuarioId);

                return File(dados, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RelatorioTarefasConcluidas.xlsx");

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

        [HttpGet("Excel/TarefasConcluidasPorMesPorUsuario/{UsuarioId}")]
        public async Task<IActionResult> ExcelRelatorioConclusaoMesPorUsuario([FromRoute(Name = "UsuarioId")] int UsuarioId)
        {
            try
            {
                var dados = await _relatorio.ExcelRelatorioTarefasConcluidasUsuarioMesAsync(UsuarioId);

                return File(dados, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RelatorioTarefasMediaConcluidas.xlsx");

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

        [HttpGet("Pdf/TarefasMediasConcluidasPorMesPorUsuario/{UsuarioId}")]
        public async Task<IActionResult> PdfRelatorioMediaConclusaoMes([FromRoute(Name = "UsuarioId")] int UsuarioId)
        {
            try
            {
                var dados = await _relatorio.PdfRelatorioMediasTarefasConcluidasUsuarioMesAsync(UsuarioId);

                return File(dados, "application/pdf", "RelatorioTarefasConcluídaPorUsuário.pdf");

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

        [HttpGet("Pdf/TarefasConcluidasPorMesPorUsuario/{UsuarioId}")]
        public async Task<IActionResult> PdfRelatorioConclusaoMesPorUsuario([FromRoute(Name = "UsuarioId")] int UsuarioId)
        {
            try
            {
                var dados = await _relatorio.PdfRelatorioMediasTarefasConcluidasUsuarioMesAsync(UsuarioId);

                return File(dados, "application/pdf", "RelatorioTarefasConcluídamédiasPorUsuário.pdf");

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

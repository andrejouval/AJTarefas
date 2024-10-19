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

        [HttpGet("Json/TarefasMediasConcluidasPorMesPorUsuario")]
        public async Task<IActionResult> RelatorioMediaConclusaoMes()
        {
            try
            {
                var dados = await _relatorio.RelatorioMediasTarefasConcluidasUsuarioMesAsync();

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

        [HttpGet("Json/TarefasConcluidasPorMesPorUsuario")]
        public async Task<IActionResult> RelatorioConclusaoMesPorUsuario()
        {
            try
            {
                var dados = await _relatorio.RelatorioTarefasConcluidasUsuarioMesAsync();

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

        [HttpGet("Excel/TarefasMediasConcluidasPorMesPorUsuario")]
        public async Task<IActionResult> ExcelelatorioMediaConclusaoMes()
        {
            try
            {
                var dados = await _relatorio.ExcelRelatorioMediasTarefasConcluidasUsuarioMesAsync();

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

        [HttpGet("Excel/TarefasConcluidasPorMesPorUsuario")]
        public async Task<IActionResult> ExcelRelatorioConclusaoMesPorUsuario()
        {
            try
            {
                var dados = await _relatorio.ExcelRelatorioTarefasConcluidasUsuarioMesAsync();

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

        [HttpGet("Pdf/TarefasMediasConcluidasPorMesPorUsuario")]
        public async Task<IActionResult> PdfRelatorioMediaConclusaoMes()
        {
            try
            {
                var dados = await _relatorio.PdfRelatorioMediasTarefasConcluidasUsuarioMesAsync();

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

        [HttpGet("Pdf/TarefasConcluidasPorMesPorUsuario")]
        public async Task<IActionResult> PdfRelatorioConclusaoMesPorUsuario()
        {
            try
            {
                var dados = await _relatorio.PdfRelatorioMediasTarefasConcluidasUsuarioMesAsync();

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

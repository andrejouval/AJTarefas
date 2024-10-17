using AJTarefasApp.Controllers.Tarefa.Post;
using AJTarefasDomain.Base;
using AJTarefasDomain.Interfaces.Negocio.Tarefa;
using Microsoft.AspNetCore.Mvc;

namespace AJTarefasApp.Controllers.Tarefa
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _tarefa;

        public TarefaController(ITarefaService tarefa)
        {
            _tarefa = tarefa;
        }

        [HttpPost]
        public async Task<IActionResult> ProjetoAsync(PostTarefaRequest Tarefa)
        {
            try
            {
                var id = await _tarefa.PostTarefaAsync(new AJTarefasDomain.Tarefa.PostTarefaRequest()
                {
                    ProjetoId = Tarefa.ProjetoId,
                    Descricao = Tarefa.Descricao,
                    PrioridadeTarefa = Tarefa.PrioridadeTarefa ?? null,
                    Titulo = Tarefa.Titulo
                });

                var retorno = new PostTarefaResponse()
                {
                    Id = id,
                    Descricao = Tarefa.Descricao,
                    Titulo = Tarefa.Titulo,
                    PrioridadeTarefa = Tarefa.PrioridadeTarefa != null ? new PostTarefaPrioridadeResponse()
                    {
                        PrioridadeCode = Tarefa.PrioridadeTarefa.Value,
                        Prioridade = Tarefa.PrioridadeTarefa.Value.GetEnumTextos()
                    } : null,
                    Status = new PostTarefaStatusResponse()
                    {
                        StatusCode = AJTarefasDomain.Tarefa.StatusTarefa.Pendente,
                        Status = AJTarefasDomain.Tarefa.StatusTarefa.Pendente.GetEnumTextos()
                    },
                    DataCriacao = DateTime.Now,
                    ProjetoId = Tarefa.ProjetoId
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

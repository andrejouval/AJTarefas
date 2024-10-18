using AJTarefasApp.Controllers.Tarefa.Patch;
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
        public async Task<IActionResult> PostProjetoAsync(PostTarefaRequest Tarefa)
        {
            try
            {
                var tarefa = await _tarefa.PostTarefaAsync(new AJTarefasDomain.Tarefa.PostTarefaRequest()
                {
                    ProjetoId = Tarefa.ProjetoId,
                    Descricao = Tarefa.Descricao,
                    PrioridadeTarefa = Tarefa.PrioridadeTarefa,
                    Titulo = Tarefa.Titulo,
                    UsuarioId = Tarefa.UsuarioId
                });

                var retorno = new PostTarefaResponse()
                {
                    Id = tarefa.Id,
                    Descricao = Tarefa.Descricao,
                    Titulo = Tarefa.Titulo,
                    PrioridadeTarefa = new PostTarefaPrioridadeResponse()
                    {
                        PrioridadeCode = (AJTarefasDomain.Tarefa.PrioridadeTarefa)Tarefa.PrioridadeTarefa,
                        Prioridade = Tarefa.PrioridadeTarefa.GetEnumTextos()
                    },
                    Status = new PostTarefaStatusResponse()
                    {
                        StatusCode = AJTarefasDomain.Tarefa.StatusTarefa.Pendente,
                        Status = AJTarefasDomain.Tarefa.StatusTarefa.Pendente.GetEnumTextos()
                    },
                    DataCriacao = DateTime.Now,
                    ProjetoId = Tarefa.ProjetoId,
                    Usuario = new Projeto.Base.BaseUsuarioResponse()
                    {
                        Nome = tarefa.Usuario.Nome,
                        UsuarioId = tarefa.Usuario.UsuarioId,
                        UsuariosPapel = new Projeto.Base.BaseUsuarioPapelResponse()
                        {
                            UsuariosPapelCode = (UsuariosPapel)tarefa.Usuario.Papel.UsuarioPapelCode,
                            Papel = tarefa.Usuario.Papel.Papel
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

        [HttpPatch]
        public async Task<IActionResult> PatchProjetoAsync(PatchTarefaRequest Tarefa)
        {
            try
            {

                var tarefa = new AJTarefasDomain.Tarefa.TarefaDto()
                {
                    Id = Tarefa.Id,
                    ProjetoId = Tarefa.ProjetoId,
                    Descricao = Tarefa.Descricao,
                    PrioridadeTarefa = new AJTarefasDomain.Tarefa.TarefaPrioridadeDto()
                    {
                        PrioridadeCode = Tarefa.PrioridadeTarefa,
                        Prioridade = Tarefa.PrioridadeTarefa.GetEnumTextos()
                    },
                    Titulo = Tarefa.Titulo,
                    DataPrevistaTermino = Tarefa.DataPrevistaTermino,
                    Status = new AJTarefasDomain.Tarefa.TarefaStatusDto()
                    {
                        StatusCode = Tarefa.StatusTarefa,
                        Status = Tarefa.StatusTarefa.GetEnumTextos()
                    },
                    Comentarios = Tarefa.Comentarios.Select(c => new AJTarefasDomain.Tarefa.TarefaComentariosDto()
                    {
                        IdUsuario = c.IdUsuario,
                        Comentario = c.Comentario
                    }),
                    Usuario = new UsuarioDto()
                    {
                        UsuarioId = Tarefa.UsuarioId
                    }
                };

                var retornoTarefa = await _tarefa.PatchTarefaAsync(tarefa);

                var retorno = new PatchTarefaResponse()
                {
                    Id = retornoTarefa.Id,
                    Descricao = retornoTarefa.Descricao,
                    Titulo = retornoTarefa.Titulo,
                    PrioridadeTarefa = new PatchTarefaPrioridadeResponse()
                    {
                        PrioridadeCode = retornoTarefa.PrioridadeTarefa.PrioridadeCode,
                        Prioridade = retornoTarefa.PrioridadeTarefa.Prioridade
                    },
                    Status = new PatchTarefaStatusResponse()
                    {
                        StatusCode = retornoTarefa.Status.StatusCode,
                        Status = retornoTarefa.Status.Status
                    },
                    DataCriacao = DateTime.Now,
                    ProjetoId = retornoTarefa.ProjetoId,
                    DataInico = retornoTarefa.DataInicio ?? null,
                    DataPrevistaTermino = retornoTarefa.DataPrevistaTermino ?? null,
                    DataTermino = retornoTarefa.DataTermino ?? null,
                    Usuario = new Projeto.Base.BaseUsuarioResponse()
                    {
                        Nome = retornoTarefa.Usuario.Nome,
                        UsuarioId = retornoTarefa.Usuario.UsuarioId,
                        UsuariosPapel = new Projeto.Base.BaseUsuarioPapelResponse()
                        {
                            UsuariosPapelCode = retornoTarefa.Usuario.Papel.UsuarioPapelCode,
                            Papel = retornoTarefa.Usuario.Papel.Papel
                        }
                    },
                    Comentarios = retornoTarefa.Comentarios.Select(c => new PatchTarefaComentarioResponse()
                    {
                        IdUsuario = c.IdUsuario,
                        Comentario = c.Comentario
                    })
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

        [HttpGet("{projetoId}/{id}")]
        public async Task<IActionResult> GetProjetoAsync([FromRoute(Name = "projetoId")] int ProjetoId,
                                                         [FromRoute(Name = "id")] int Id)
        {
            try
            {

                var retornoTarefa = await _tarefa.GetTarefaAsync(ProjetoId, Id);

                var retorno = new PatchTarefaResponse()
                {
                    Id = retornoTarefa.Id,
                    Descricao = retornoTarefa.Descricao,
                    Titulo = retornoTarefa.Titulo,
                    PrioridadeTarefa = new PatchTarefaPrioridadeResponse()
                    {
                        PrioridadeCode = retornoTarefa.PrioridadeTarefa.PrioridadeCode,
                        Prioridade = retornoTarefa.PrioridadeTarefa.Prioridade
                    },
                    Status = new PatchTarefaStatusResponse()
                    {
                        StatusCode = retornoTarefa.Status.StatusCode,
                        Status = retornoTarefa.Status.Status
                    },
                    DataCriacao = DateTime.Now,
                    ProjetoId = retornoTarefa.ProjetoId,
                    DataInico = retornoTarefa.DataInicio ?? null,
                    DataPrevistaTermino = retornoTarefa.DataPrevistaTermino ?? null,
                    DataTermino = retornoTarefa.DataTermino ?? null,
                    Usuario = new Projeto.Base.BaseUsuarioResponse()
                    {
                        Nome = retornoTarefa.Usuario.Nome,
                        UsuarioId = retornoTarefa.Usuario.UsuarioId,
                        UsuariosPapel = new Projeto.Base.BaseUsuarioPapelResponse()
                        {
                            UsuariosPapelCode = retornoTarefa.Usuario.Papel.UsuarioPapelCode,
                            Papel = retornoTarefa.Usuario.Papel.Papel
                        }
                    },
                    Comentarios = retornoTarefa.Comentarios.Select(c => new PatchTarefaComentarioResponse()
                    {
                        IdUsuario = c.IdUsuario,
                        Comentario = c.Comentario
                    })
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

        [HttpDelete("{projetoId}/{id}")]
        public async Task<IActionResult> DeleteProjetoAsync([FromRoute(Name = "projetoId")] int ProjetoId,
                                                         [FromRoute(Name = "id")] int Id)
        {
            try
            {

                await _tarefa.DeleteTarefaAsync(ProjetoId, Id);

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

    }
}

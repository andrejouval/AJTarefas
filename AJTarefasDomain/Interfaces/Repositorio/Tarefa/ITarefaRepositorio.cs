using AJTarefasDomain.Tarefa;
using System.Threading.Tasks;

namespace AJTarefasDomain.Interfaces.Repositorio.Projeto
{
    public interface ITarefaRepositorio
    {
        Task<int> PostTarefaAsync(PostTarefaRequest Tarefa);

        Task PatchTarefaAsync(TarefaDto Tarefa);

        Task<bool> TarefaExisteAsync(int ProjetoId, int Id);

        Task<TarefaDto> RecuperarTarefaAsync(int ProjetoId, int Id);

        Task<int> RecuperarQuantidadeTarefasAsync(int ProjetoId, int Id);

        Task IncluiComentarioAsync(int ProjetoId, int Id, TarefaComentariosDto Comentario);

        Task IncluiHistoricoAsync(TarefaDto Tarefa);
    }
}

namespace AJTarefasDomain.Tarefa
{
    public class PostTarefaRequest
    {
        public int ProjetoId { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public PrioridadeTarefa? PrioridadeTarefa { get; set; }

        public int UsuarioId { get; set; }

    }
}

using System.ComponentModel;

namespace AJTarefasDomain.Tarefa
{
    public enum PrioridadeTarefa
    {
        [Description("Prioridade baixa")]
        Baixa = 1,
        [Description("Prioridade média")]
        Media,
        [Description("Prioridade alta")]
        Alta
    }
}

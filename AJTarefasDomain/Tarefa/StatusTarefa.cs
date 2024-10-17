
using System.ComponentModel;

namespace AJTarefasDomain.Tarefa
{
    public enum StatusTarefa
    {

        [Description("Tarefa pendente")]
        Pendente = 1,
        [Description("Tarefa em execução")]
        EmExecucao,
        [Description("Tarefa bloqueada")]
        Bloqueada,
        [Description("Tarefa concluída")]
        Concluida
    }
}

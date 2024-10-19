using System.ComponentModel;

namespace AJTarefasDomain.Projeto
{
    public enum StatusProjeto
    {
        [Description("Projeto pendente")]
        Pendente = 1,
        [Description("Projeto em execução")]
        EmExecucao,
        [Description("Projeto suspenso")]
        Suspenso,
        [Description("Projeto concluído")]
        Concluido,
        [Description("Projeto cancelado")]
        Cancelado
    }
}

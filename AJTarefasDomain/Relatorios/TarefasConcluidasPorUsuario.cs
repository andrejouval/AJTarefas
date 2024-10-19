using AJTarefasDomain.Base;

namespace AJTarefasDomain.Relatorios
{
    public class TarefasConcluidasPorUsuario
    {
        public UsuarioDto Usuario { get; set; }
        public string MesAno { get; set; }
        public int Quantidade { get; set; }
    }
}

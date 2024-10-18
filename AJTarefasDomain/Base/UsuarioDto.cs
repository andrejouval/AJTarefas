namespace AJTarefasDomain.Base
{
    public class UsuarioDto
    {
        public int UsuarioId {  get; set; }

        public string Nome { get; set; }

        public UsuarioPapelDto Papel {  get; set; }
    }
}

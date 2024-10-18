using AJTarefasDomain.Base;
using AJTarefasDomain.Tarefa;
using System;
using System.Collections.Generic;

namespace AJTarefasDomain.Projeto
{
    public class ProjetoDto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public DateTime? DataCriacao { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataPrevisaoTermino { get; set; }

        public DateTime? DataTermino { get; set; }

        public ProjetoStatusDto StatusProjeto { get; set; }

        public UsuarioDto Usuario { get; set; }

        public IEnumerable<TarefaDto> Tarefas { get; set; }
    }
}

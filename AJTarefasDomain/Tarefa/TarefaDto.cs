using System;
using System.Collections.Generic;

namespace AJTarefasDomain.Tarefa
{
    public class TarefaDto
    {
        public int Id { get; set; }

        public int ProjetoId { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public TarefaPrioridadeDto PrioridadeTarefa { get; set; }

        public DateTime DataCriacao { get; set; }

        public TarefaStatusDto Status { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataPrevistaTermino { get; set; }

        public DateTime? DataTermino  { get; set; }

        public IEnumerable<TarefaComentariosDto> Comentarios { get; set; }

    }
}

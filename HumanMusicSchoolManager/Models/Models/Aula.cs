using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Aula
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Data da aula é obrigatório")]
        public DateTime Data { get; set; }

        public int ProfessorId { get; set; }
        public virtual Professor Professor { get; set; }

        public int CursoId { get; set; }
        public virtual Curso Curso { get; set; }

        public int SalaId { get; set; }
        public virtual Sala Sala { get; set; }

        public bool AulaDada { get; set; }

        [Display(Name = "Descrição de Atividades")]
        [MinLength(7, ErrorMessage = "A descrição deve ter no mínimo 7 caracteres")]
        public string DescAtividades { get; set; }

        public DateTime DataLimite { get; set; }

        public virtual List<Chamada> Chamadas { get; set; }

        public virtual List<Demostrativa> Demostrativas { get; set; }
    }
}

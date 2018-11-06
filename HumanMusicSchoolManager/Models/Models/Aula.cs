using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Aula
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Data da aula é obrigatório")]
        public DateTime Data { get; set; }

        public int ProfessorId { get; set; }
        public Professor Professor { get; set; }

        public int CursoId { get; set; }
        public Curso Curso { get; set; }

        public int SalaId { get; set; }
        public Sala Sala { get; set; }

        public bool AulaDada { get; set; }

        public string DescAtividades { get; set; }

        public int ChamadaId { get; set; }
        public List<Chamada> MyProperty { get; set; }
    }
}

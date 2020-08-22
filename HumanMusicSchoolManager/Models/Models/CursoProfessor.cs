using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class CursoProfessor
    {
        public int CursoId { get; set; }
        public int ProfessorId { get; set; }
        public virtual Professor Professor { get; set; }
        public virtual Curso Curso { get; set; }
    }
}

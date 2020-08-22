using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class Demostrativa
    {
        public int? Id { get; set; }

        public int CandidatoId { get; set; }
        public virtual Candidato Candidato { get; set; }

        public int? DispSalaId { get; set; }
        public virtual DispSala DispSala { get; set; }

        public virtual DateTime Data { get; set; }

        public Dia Dia { get; set; }

        public int Hora { get; set; }

        public int CursoId { get; set; }
        public virtual Curso Curso { get; set; }

        public int? AulaId { get; set; }
        public virtual Aula Aula { get; set; }

        [Display(Name = "Presença")]
        public bool? Presenca { get; set; }

        public Motivo Motivo { get; set; }

        public string Outros { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        public bool? Contratou { get; set; }

        public int? Estrelas { get; set; }

        public int PessoaId { get; set; }
        public virtual Pessoa Pessoa { get; set; }

        public int? ProfessorId { get; set; }
        public virtual Professor Professor { get; set; }

        public Confirmado Confirmado { get; set; }
    }

   

    public enum Confirmado
    {
        [Display(Name = "Não")]
        NAO,
        [Display(Name = "Aguardando")]
        AGUARDANDO,
        [Display(Name = "Sim")]
        SIM
    }
}

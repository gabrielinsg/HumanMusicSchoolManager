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
        public Candidato Candidato { get; set; }

        public int? DispSalaId { get; set; }
        public DispSala DispSala { get; set; }

        public int CursoId { get; set; }
        public Curso Curso { get; set; }

        public int? AulaId { get; set; }
        public Aula Aula { get; set; }

        [Display(Name = "Presença")]
        public bool? Presenca { get; set; }

        public Motivo Motivo { get; set; }

        public string Outros { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        public bool? Contratou { get; set; }

        public int? Estrelas { get; set; }

        public int PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }
    }

    public enum Motivo
    {
        [Display(Name = "Financeiro")]
        FINANCEIRO,
        [Display(Name = "Falta de tempo")]
        TEMPO,
        [Display(Name = "Não gostou do professor")]
        PROFESSOR,
        [Display(Name = "Não gostou da aula")]
        AULA,
        [Display(Name = "Não gostou da escola")]
        ESCOLA,
        [Display(Name = "Não retornou o contato")]
        RETORNO,
        [Display(Name = "Outros")]
        OUTROS
    }
}

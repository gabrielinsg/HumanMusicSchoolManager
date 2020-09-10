using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class RespFinanceiro : Pessoa
    {
        [Required(ErrorMessage = "O campo Nacionalidade é obrigatório")]
        public string Nacionalidade { get; set; }

        [Required(ErrorMessage = "O campo Naturidade é obrigatório")]
        public string Naturalidade { get; set; }

        [Required(ErrorMessage = "O campo Estado Civil é obrigatório")]
        [Display(Name = "Estado Civil")]
        public EstadoCivil EstadoCivil { get; set; }

        [Required(ErrorMessage = "O campo Profissão é obrigatório")]
        [Display(Name = "Profissão")]
        public string Profissao { get; set; }

        [Required(ErrorMessage = "o campo Órgão Expedidor é obrigatório")]
        [Display(Name = "Órgão Expedidor")]
        public string OrgaoExpedidor { get; set; }

        [JsonIgnore]
        public virtual List<Matricula> Matriculas { get; set; }
    }

    public enum EstadoCivil
    {
        [Display(Name = "Casado")]
        CASADO,
        [Display(Name = "Solteiro")]
        SOLTEIRO,
        [Display(Name = "Viúvo")]
        VIUVO,
        [Display(Name = "Divorciado")]
        DIVORCIADO

    }
}

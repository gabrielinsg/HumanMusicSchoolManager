using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public class AulaConfig
    {
        public int? Id { get; set; }
        [Display(Name = "Tempo limite para lançamento (Horas)")]
        public int TempoLimiteLancamento { get; set; }
        [Display(Name = "Permitir lançamento após vencimento")]
        public bool LancamentoAtrasado { get; set; }
        [Display(Name = "Mínimo caracteres Descrição de Atividades")]
        public int MinCaracteresDescAtividades { get; set; }
        [Display(Name = "Descrição de Atividades Obrigatória")]
        public bool DescAtividadesObrigatorio { get; set; }
    }
}

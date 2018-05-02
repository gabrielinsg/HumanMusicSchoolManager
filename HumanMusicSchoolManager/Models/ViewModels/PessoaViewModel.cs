using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class PessoaViewModel
    {
        public List<Pessoa> Pessoas { get; set; }

        public PessoaViewModel(List<Pessoa> pessoas)
        {
            this.Pessoas = pessoas;
        }
    }
}

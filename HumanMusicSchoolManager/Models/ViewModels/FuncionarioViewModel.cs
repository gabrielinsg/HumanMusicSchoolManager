using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class FuncionarioViewModel
    {
        public List<Funcionario> Funcionarios { get; set; }

        public FuncionarioViewModel(List<Funcionario> funcionarios)
        {
            this.Funcionarios = funcionarios;
        }
    }
}

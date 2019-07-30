using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IRelatorioService
    {
        List<Professor> Professores();
        List<Aluno> Alunos();
    }
}

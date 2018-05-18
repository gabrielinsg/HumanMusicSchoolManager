using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class MatriculaViewModel
    {

        public List<ListDiario> Matriculas { get; set; }


        public MatriculaViewModel(List<Matricula> matriculas, IDiarioClasseService diarioClasseService)
        {
            this.Matriculas = new List<ListDiario>();
            if(matriculas != null)
            {
                foreach (var m in matriculas)
                {
                    ListDiario matricula = new ListDiario()
                    {
                        Aluno = m.Aluno,
                        AlunoId = m.AlunoId,
                        Ativo = m.Ativo,
                        Curso = m.Curso,
                        CursoId = m.CursoId,
                        DataMatricula = m.DataMatricula,
                        Dia = m.Dia,
                        Hora = m.Hora,
                        Id = m.Id,
                        Professor = m.Professor,
                        ProfessorId = m.ProfessorId,
                        UltimaAula = diarioClasseService.UltimoRegistro((m.Id).Value)
                    };
                    this.Matriculas.Add(matricula);
                }
            }
        }
    }

    public class ListDiario : Matricula
    {
        public DateTime? UltimaAula { get; set; }
    }
}

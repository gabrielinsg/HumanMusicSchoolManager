using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public interface IDiarioClasseService
    {
        void Cadastrar(DiarioClasse diario);
        void Alterar(DiarioClasse diario);
        void Excluir(int diarioId);
        List<DiarioClasse> BuscarTodos();
        DiarioClasse BuscarPorId(int diarioId);
        List<DiarioClasse> BuscarPorAluno(Matricula matricula);
        List<DiarioClasse> BuscarAlguns(Matricula matricula);
        List<DiarioClasse> BuscarPorMatricula(int matriculaId);
        Matricula BuscarMatricula(int diarioId);
        DateTime? UltimoRegistro(int matriculaId);
    }
}

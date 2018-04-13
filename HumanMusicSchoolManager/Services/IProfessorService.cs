﻿using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public interface IProfessorService
    {
        void Cadastrar(Professor professor);
        void Alterar(Professor professor);
        List<Professor> BuscarTodos();
        Professor BuscarPorId(int id);
        void Excluir(Professor professor);
        void AdicionarCurso(int professorId, int cursoId);
        void RemoverCurso(int professorId, int cursoId);
        List<Professor> BuscarProfessorPorCurso(int cursoId);
    }
}

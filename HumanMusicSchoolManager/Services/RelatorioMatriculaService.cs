using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class RelatorioMatriculaService : IRelatorioMatriculaService
    {
        private readonly ApplicationDbContext _context;

        public RelatorioMatriculaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public List<RelatorioMatricula> BuscarPorMatricula(int matriculaId)
        {
            return _context.RelatorioMatriculas.Where(rm => rm.Matricula.Id == matriculaId).ToList();
        }

        public List<RelatorioMatricula> BuscarPorPessoa(int pessoaId)
        {
            return _context.RelatorioMatriculas.Where(rm => rm.Pessoa.Id == pessoaId).ToList();
        }

        public void Cadastrar(RelatorioMatricula relatorioMatricula)
        {
            _context.RelatorioMatriculas.Add(relatorioMatricula);
            _context.SaveChanges();
        }
    }
}

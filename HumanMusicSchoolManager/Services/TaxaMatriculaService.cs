using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class TaxaMatriculaService : ITaxaMatriculaService
    {
        private readonly ApplicationDbContext _context;

        public TaxaMatriculaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(TaxaMatricula taxaMatricula)
        {
            _context.TaxaMatriculas.Update(taxaMatricula);
            _context.SaveChanges();
        }

        public TaxaMatricula BuscarPorId(int taxaMatriculaId)
        {
            return _context.TaxaMatriculas.FirstOrDefault(tm => tm.Id == taxaMatriculaId);
        }

        public TaxaMatricula BuscarPorNome(string nome)
        {
            return _context.TaxaMatriculas.FirstOrDefault(tm => tm.Nome.Contains(nome));
        }

        public List<TaxaMatricula> BuscarTodos()
        {
            return _context.TaxaMatriculas.ToList();
        }

        public void Cadastrar(TaxaMatricula taxaMatricula)
        {
            _context.TaxaMatriculas.Add(taxaMatricula);
            _context.SaveChanges();
        }

        public void Excluir(int taxaMatriculaId)
        {
            var taxaMatricula = _context.TaxaMatriculas.FirstOrDefault(tm => tm.Id == taxaMatriculaId);
            if (taxaMatricula != null)
            {
                _context.TaxaMatriculas.Remove(taxaMatricula);
                _context.SaveChanges();
            }
        }
    }
}

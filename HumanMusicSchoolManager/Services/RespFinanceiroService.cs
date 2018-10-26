using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class RespFinanceiroService : IRespFinanceiroService
    {
        public ApplicationDbContext _context { get; }


        public RespFinanceiroService(ApplicationDbContext context)
        {
            this._context = context;
        }
        
        public RespFinanceiro Alterar(RespFinanceiro respFinanceiro)
        {
            _context.RespsFinanceiro.Update(respFinanceiro);
            _context.SaveChanges();
            return respFinanceiro;
        }

        public RespFinanceiro BuscarPorId(int respFinanceiroId)
        {
            return _context.RespsFinanceiro
                .Include(rf => rf.Endereco)
                .FirstOrDefault(rf => rf.Id == respFinanceiroId);
        }

        public List<RespFinanceiro> BuscarPorNome(string nome)
        {
            return _context.RespsFinanceiro.Where(rf => rf.Nome.Contains(nome)).OrderBy(rf => rf.Nome).ToList();
        }

        public List<RespFinanceiro> BuscarTodos()
        {
            return _context.RespsFinanceiro
                .Include(rf => rf.Endereco)
                .OrderBy(rf => rf.Nome).ToList();
        }

        public RespFinanceiro Cadastrar(RespFinanceiro respFinanceiro)
        {
            _context.Add(respFinanceiro);
            _context.SaveChanges();
            return respFinanceiro;
        }

        public void Excluir(int respFinanceiroId)
        {
            var respFinanceiro = _context.RespsFinanceiro.FirstOrDefault(rf => rf.Id == respFinanceiroId);
            if (respFinanceiro != null)
            {
                _context.RespsFinanceiro.Remove(respFinanceiro);
            }
        }

        public RespFinanceiro BuscarPorCPF(string cpf)
        {
            return _context.RespsFinanceiro.SingleOrDefault(rf => rf.CPF == cpf);
        }
    }
}

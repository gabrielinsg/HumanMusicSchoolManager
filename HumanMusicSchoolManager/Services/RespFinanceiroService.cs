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

            var resp = _context.RespsFinanceiro.Include(rf => rf.Endereco).FirstOrDefault(rf => rf.Id == respFinanceiro.Id);
            resp.Nome = respFinanceiro.Nome;
            resp.Ativo = respFinanceiro.Ativo;
            resp.Cel = respFinanceiro.Cel;
            resp.Tel = respFinanceiro.Tel;
            resp.Nacionalidade = respFinanceiro.Nacionalidade;
            resp.Naturalidade = respFinanceiro.Naturalidade;
            resp.RG = respFinanceiro.RG;
            resp.CPF = respFinanceiro.CPF;
            resp.DataNascimento = respFinanceiro.DataNascimento;
            resp.Email = respFinanceiro.Email;
            resp.OrgaoExpedidor = respFinanceiro.OrgaoExpedidor;
            resp.Profissao = respFinanceiro.Profissao;

            resp.Endereco.Bairro = respFinanceiro.Endereco.Bairro;
            resp.Endereco.CEP = respFinanceiro.Endereco.CEP;
            resp.Endereco.Cidade = respFinanceiro.Endereco.Cidade;
            resp.Endereco.Complemento = respFinanceiro.Endereco.Complemento;
            resp.Endereco.Logradouro = respFinanceiro.Endereco.Logradouro;
            resp.Endereco.Numero = respFinanceiro.Endereco.Numero;
            resp.Endereco.UF = respFinanceiro.Endereco.UF;

            _context.RespsFinanceiro.Update(resp);
            //var entry = _context.Entry<RespFinanceiro>(respFinanceiro);
            //if (entry.State == EntityState.Detached)
            //{
            //    var fin = _context.RespsFinanceiro.FirstOrDefault(f => f.Id == respFinanceiro.Id);
            //    fin.EnderecoId = endereco.Id;
            //    fin.Endereco.Id = endereco.Id;
            //    var ent = _context.Entry<RespFinanceiro>(fin);
            //    ent.State = EntityState.Detached;
            //    entry.State = EntityState.Modified;
            //}
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
                .Include(rf => rf.Matriculas)
                .ThenInclude(m => m.Aluno)
                .Include(rf => rf.Matriculas)
                .ThenInclude(m => m.Curso)
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
                _context.Enderecos.Remove(_context.Enderecos.FirstOrDefault(e => e.Id == respFinanceiro.EnderecoId));
                _context.RespsFinanceiro.Remove(respFinanceiro);
                _context.SaveChanges();
            }
        }

        public RespFinanceiro BuscarPorCPF(string cpf)
        {
            return _context.RespsFinanceiro
                .Include(rf => rf.Endereco)
                .SingleOrDefault(rf => rf.CPF == cpf);
        }
    }
}

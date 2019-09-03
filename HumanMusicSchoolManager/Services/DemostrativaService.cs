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
    public class DemostrativaService : IDemostrativaService
    {
        private readonly ApplicationDbContext _context;

        public DemostrativaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Demostrativa Demostrativa)
        {
            var entry = _context.Entry<Demostrativa>(Demostrativa);
            if (entry.State == EntityState.Detached)
            {
                var fin = _context.Demostrativas.FirstOrDefault(f => f.Id == Demostrativa.Id);
                var ent = _context.Entry<Demostrativa>(fin);
                ent.State = EntityState.Detached;
                entry.State = EntityState.Modified;
            }
            _context.SaveChanges();
        }

        public Demostrativa BuscarPorId(int DemostrativaId)
        {
           return _context.Demostrativas
                .Include(d => d.Aula)
                .Include(d => d.Pessoa)
                .FirstOrDefault(d => d.Id == DemostrativaId);
        }

        public List<Demostrativa> BuscarTodos()
        {
            return _context.Demostrativas.ToList();
        }

        public void Cadastrar(Demostrativa Demostrativa)
        {
            _context.Demostrativas.Add(Demostrativa);
            _context.SaveChanges();
        }

        public List<Demostrativa> DemostrativasAbertas()
        {
            return _context.Demostrativas
                .Where(d => d.DispSalaId != null)
                .Include(d => d.Curso)
                .Include(d => d.Candidato)
                .Include(d => d.DispSala)
                .ThenInclude(ds => ds.Professor)
                .Include(d => d.Aula)
                .Include(d => d.Pessoa)
                .OrderBy(d => d.Aula.Data)
                .ToList();
        }

        public void Excluir(int DemostrativaId)
        {
            var Demostrativa = _context.Demostrativas.FirstOrDefault(d => d.Id == DemostrativaId);
            if (Demostrativa != null)
            {
                _context.Demostrativas.Remove(Demostrativa);
                _context.SaveChanges();
            }
        }

        public List<Demostrativa> DemostrativasNaoContrataram(DateTime inicial, DateTime final)
        {
            inicial = inicial.AddHours(-inicial.Hour);
            inicial = inicial.AddMinutes(-inicial.Minute);
            inicial = inicial.AddMilliseconds(-inicial.Millisecond);
            final = final.AddHours(-final.Hour);
            final = final.AddMinutes(-final.Minute);
            final = final.AddMilliseconds(-final.Millisecond);
            final = final.AddHours(23);

            return _context.Demostrativas
                .Where(d => d.Contratou == false && d.Aula.Data >= inicial && d.Aula.Data <= final)
                .Include(d => d.Curso)
                .Include(d => d.Candidato)
                .Include(d => d.DispSala)
                .ThenInclude(ds => ds.Professor)
                .Include(d => d.Aula)
                .OrderBy(d => d.Aula.Data)
                .ToList();
        }

        public void AtualizarObservacao(int id, string conteudo)
        {
            var Demostrativa = _context.Demostrativas.FirstOrDefault(d => d.Id == id);
            if (Demostrativa != null)
            {
                Demostrativa.Observacao = conteudo;
                _context.Demostrativas.Update(Demostrativa);
                _context.SaveChanges();
            }
        }

        public void AtualizarConfirmado(int id, Confirmado confirmado)
        {
            var Demostrativa = _context.Demostrativas.FirstOrDefault(d => d.Id == id);
            if (Demostrativa != null)
            {
                Demostrativa.Confirmado = confirmado;
                _context.Demostrativas.Update(Demostrativa);
                _context.SaveChanges();
            }
        }
    }
}

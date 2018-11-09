using HumanMusicSchoolManager.Data;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public class EventoService : IEventoService
    {
        private readonly ApplicationDbContext _context;

        public EventoService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Alterar(Evento evento)
        {
            _context.Update(evento);
            _context.SaveChanges();
        }

        public List<Evento> BuscarEntreDatas(DateTime dataInicial, DateTime dataFinal)
        {
            return _context.Eventos.Where(f => f.DataInicial >= dataFinal && f.DataFinal <= dataFinal).ToList();
        }

        public Evento BuscarPorData(DateTime data)
        {
            return _context.Eventos.FirstOrDefault(f => f.DataInicial == data);
        }

        public Evento BuscarPorId(int eventoId)
        {
            return _context.Eventos.FirstOrDefault(f => f.Id == eventoId);
        }

        public List<Evento> BuscarTodos()
        {
            return _context.Eventos.ToList();
        }

        public void Cadastrar(Evento evento)
        {
            _context.Eventos.Add(evento);
            _context.SaveChanges();
        }

        public void Excluir(int eventoId)
        {
            var evento = _context.Eventos.FirstOrDefault(f => f.Id == eventoId);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
                _context.SaveChanges();
            }
        }
    }
}

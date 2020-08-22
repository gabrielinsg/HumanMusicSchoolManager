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
    public class DispSalaService : IDispSalaService
    {
        private readonly ApplicationDbContext _context;

        public DispSalaService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public List<DispSala> BuscarTodos()
        {
            var hr = _context.DispSalas                
                .ToList();



            return hr
                .OrderBy(h => h.Professor.Nome)
                .OrderBy(h => h.Hora)
                .OrderBy(h => h.Dia)
                .ToList();
        }

        public List<DispSala> BuscarTodosPorSala(int salaId)
        {
            var hr = _context.DispSalas
                .Where(ds => ds.Sala.Id == salaId)
                .ToList();



            return hr
                .OrderBy(h => h.Professor.Nome)
                .OrderBy(h => h.Hora)
                .OrderBy(h => h.Dia)
                .ToList();
        }

        public DispSala BuscarPorId(int dispSalaId)
        {
            return _context.DispSalas                
                .SingleOrDefault(ds => ds.Id == dispSalaId);
        }

        private Object RetornaObjeto(Object ob, string tipo)
        {
            if (ob == null)
            {

                switch (tipo)
                {
                    case "chamada": return new Chamada();
                    case "pacoteCompra": return new PacoteCompra();
                    default: return null;
                }
            }
            else
            {
                return ob;
            }
        }

        public List<DispSala> HorariosDisponiveis()
        {
            //var chamada = _context.Chamadas.FirstOrDefault(c => c.PacoteCompra.PacoteAula.TipoAula == TipoAula.INDIVIDUAL && c.Presenca == null);
            //var pacoteCompra = _context.PacoteCompras.FirstOrDefault(pc => pc.Chamadas.Contains((Chamada)RetornaObjeto(chamada, "chamada")));
            //var matricula = _context.Matriculas.FirstOrDefault(m => m.PacoteCompras.Contains((PacoteCompra)RetornaObjeto(pacoteCompra, "pacoteCompra")));


            var hr = _context.DispSalas
                .Where(ds => (ds.Sala.Capacidade > ds.Matriculas.Count + ds.Demostrativas.Count + ds.Reposicoes.Count) && ds.Ativo == true)                
                .ToList();



            return Organizar(hr);

        }

        public List<DispSala> HorariosDisponiveisPorSala(int salaId)
        {
            //var chamada = _context.Chamadas.FirstOrDefault(c => c.PacoteCompra.PacoteAula.TipoAula == TipoAula.INDIVIDUAL && c.Presenca == null);
            //var pacoteCompra = _context.PacoteCompras.FirstOrDefault(pc => pc.Chamadas.Contains((Chamada)RetornaObjeto(chamada, "chamada")));
            //var matricula = _context.Matriculas.FirstOrDefault(m => m.PacoteCompras.Contains((PacoteCompra)RetornaObjeto(pacoteCompra, "pacoteCompra")));


            var hr = _context.DispSalas
                .Where(ds => (ds.Sala.Capacidade > ds.Matriculas.Count + ds.Demostrativas.Count + ds.Reposicoes.Count) && ds.Ativo == true && ds.Sala.Id == salaId)                
                .ToList();



            return Organizar(hr);

        }

        private List<DispSala> Organizar(List<DispSala> hr)
        {
            List<DispSala> retirar = new List<DispSala>();
            if (hr != null)
            {
                foreach (var dispSala in hr)
                {
                    if (dispSala.Matriculas != null)
                    {
                        foreach (var matricula in dispSala.Matriculas)
                        {
                            if (matricula.PacoteCompras != null)
                            {
                                foreach (var pacoteCompra in matricula.PacoteCompras)
                                {
                                    if (pacoteCompra.Chamadas != null)
                                    {
                                        if (pacoteCompra.Chamadas.Any(c => c.Presenca == null))
                                        {
                                            if (pacoteCompra.PacoteAula.TipoAula == TipoAula.INDIVIDUAL)
                                            {
                                                retirar.Add(pacoteCompra.Matricula.DispSala);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (retirar != null)
            {
                foreach (var retira in retirar)
                {
                    hr.Remove(retira);
                }
            }

            retirar = new List<DispSala>();
            foreach (var dispSala in hr)
            {
                if (dispSala.Reposicoes != null)
                {
                    foreach (var reposicao in dispSala.Reposicoes)
                    {
                        if (reposicao.Chamada.PacoteCompra.Matricula.PacoteCompras != null)
                        {
                            foreach (var pacoteCompra in reposicao.Chamada.PacoteCompra.Matricula.PacoteCompras)
                            {
                                if (pacoteCompra.PacoteAula.TipoAula == TipoAula.INDIVIDUAL)
                                {
                                    retirar.Add(reposicao.DispSala);
                                }
                            }
                        }
                    }
                }
            }

            if (retirar != null)
            {
                foreach (var retira in retirar)
                {
                    hr.Remove(retira);
                }
            }

            return hr
                .OrderBy(h => h.Professor.Nome)
                .OrderBy(h => h.Hora)
                .OrderBy(h => h.Dia)
                .ToList();
        }
    }
}

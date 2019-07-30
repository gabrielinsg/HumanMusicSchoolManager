using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class RelatorioGeralProfessoresViewModel
    {
        public List<DispSala> DispSalas { get; set; }
        public int Potencial { get {
                return DispSalas.Select(ds => ds.Sala).Sum(s => s.Capacidade);
            }
        }

        public int PotencialAtual { get
            {
                var total = 0;
                foreach (var dispSala in DispSalas)
                {
                    if (dispSala.Matriculas.Count == 0)
                    {
                        total += dispSala.Sala.Capacidade;
                    }
                    else if (dispSala.Matriculas
                            .Any(m => m.PacoteCompras
                            .Any(pc => pc.PacoteAula.TipoAula == TipoAula.INDIVIDUAL && pc.Chamadas
                            .Any(c => c.Presenca == null))))
                    {
                        total += 1;
                    }
                    else
                    {
                        total += dispSala.Sala.Capacidade;
                    }
                }
                return total;
            }
        }

        public int HorariosDisponiveis { get
            {
                return DispSalas.Count();
            }
        }

        public int HorariosOcupados { get
            {
                return DispSalas.Where(ds => ds.Matriculas.Count > 0).ToList().Count();
            }
        }

        public List<DispSala> DispSalasOcupados { get
            {
                return DispSalas.Where(ds => ds.Matriculas.Count > 0).ToList();
            }
        }

        public int Matriculas { get
            {
                var total = 0;
                foreach (var dispSala in DispSalas)
                {
                    total += dispSala.Matriculas.Count();
                }
                return total;
            }
        }
    }
}

using HumanMusicSchoolManager.Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.ViewModels
{
    public class DispSalaViewModel
    {
        public Sala Sala { get; set; }
        public DispSala DispSala { get; set; }
        public List<SelectListItem> Professores { get; set; }

        public DispSalaViewModel()
        {

        }

        public DispSalaViewModel(Sala sala, List<Professor> professores)
        {
            this.Sala = sala;
            this.Professores = new List<SelectListItem>();
            foreach (var professor in professores)
            {
                var selectListItem = new SelectListItem
                {
                    Value = professor.Id.ToString(),
                    Text = professor.Nome
                };
                this.Professores.Add(selectListItem);
            }
        }

        public DispSalaViewModel(Sala sala, DispSala dispSala, List<Professor> professores)
        {
            this.Sala = sala;
            this.DispSala = dispSala;
            this.Professores = new List<SelectListItem>();
            foreach (var professor in professores)
            {
                var selectListItem = new SelectListItem
                {
                    Value = professor.Id.ToString(),
                    Text = professor.Nome
                };
                this.Professores.Add(selectListItem);
            }
        }
    }
}

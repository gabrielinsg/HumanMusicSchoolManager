using HumanMusicSchoolManager.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Components
{
    public class UsuarioMobile : ViewComponent
    {
        private readonly ApplicationDbContext _contex;

        public UsuarioMobile(ApplicationDbContext context)
        {
            this._contex = context;
        }

        public IViewComponentResult Invoke()
        {
            var user = _contex.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var pessoa = _contex.Pessoas.Find(user.PessoaId);
            var usuario = new Dictionary<string, string>
            {
                { "Nome", pessoa.Nome.Split()[0] },
                { "Url", pessoa.Foto }
            };

            return View(usuario);
        }
    }
}

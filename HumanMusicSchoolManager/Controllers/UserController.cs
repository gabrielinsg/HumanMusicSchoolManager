using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPessoaService _pessoaService;

        public UserController(
            UserManager<ApplicationUser> userManager,
            IPessoaService pessoaService)
        {
            _userManager = userManager;
            _pessoaService = pessoaService;
        }

        public IActionResult Index()
        {
            List<string> users = new List<string>();

            foreach (var user in _userManager.Users.ToList())
            {
                users.Add(user.UserName);
            }

            return View(users);
        }

        public IActionResult Pessoa()
        {
            return View(_pessoaService.BuscarTodos());
        }
    }
}
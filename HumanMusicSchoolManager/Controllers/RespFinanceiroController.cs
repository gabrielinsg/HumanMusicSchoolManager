using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize]
    public class RespFinanceiroController : Controller
    {
        private readonly IRespFinanceiroService _respFinanceiroService;
        private readonly IAlunoService _alunoService;
        private readonly IEnderecoService _enderecoService;
        private readonly IPessoaService _pessoaService;

        public RespFinanceiroController(IRespFinanceiroService respFinanceiroService,
            IAlunoService alunoService,
            IEnderecoService enderecoService,
            IPessoaService pessoaService)
        {
            this._respFinanceiroService = respFinanceiroService;
            this._alunoService = alunoService;
            this._enderecoService = enderecoService;
            this._pessoaService = pessoaService;
        }

        public IActionResult Index()
        {
            return View(_respFinanceiroService.BuscarTodos());
        }

        [HttpGet]
        public IActionResult Form(int? respFinanceiroId)
        {
            if (respFinanceiroId == null)
            {
                return View();
            }
            else
            {
                return View(_respFinanceiroService.BuscarPorId(respFinanceiroId.Value));
            }
        }

        [HttpPost]
        public IActionResult Form(RespFinanceiro respFinanceiro)
        {
            if (respFinanceiro.Id == null)
            {
                var cpfUnico = _respFinanceiroService.BuscarPorCPF(respFinanceiro.CPF);
                if (cpfUnico != null)
                {
                    ModelState.AddModelError("CPF", "CPF já cadastrado para outro Responsável Financeiro");
                }

                if (ModelState.IsValid)
                {

                    _respFinanceiroService.Cadastrar(respFinanceiro);

                    TempData["Success"] = "Responsável financeiro Cadastrado com sucesso!";
                    return RedirectToAction("Form");

                }
                else
                {
                    return View(respFinanceiro);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _respFinanceiroService.Alterar(respFinanceiro);

                    TempData["Success"] = "Responsável financeiro Alterado com sucesso!";

                    return RedirectToAction("Form");

                }
                else
                {
                    return View(respFinanceiro);
                }
            }
        }

        public IActionResult Excluir(int respFinanceiroId)
        {
            _respFinanceiroService.Excluir(respFinanceiroId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult BuscarPorNome(string nome)
        {
            var respFinanceiro = _respFinanceiroService.BuscarPorNome(nome);
            return Json(respFinanceiro);
        }

        public IActionResult ArrumaEnderecos()
        {
            var respFinanceiros = _respFinanceiroService.BuscarTodos();
            var alunos = _alunoService.BuscarTodos();

            var resFinaceirosEnd = new List<RespFinanceiro>();
            foreach (var resp in respFinanceiros)
            {
                if (alunos.Any(a => a.EnderecoId == resp.EnderecoId))
                {
                    resp.Endereco = new Endereco
                    {
                        Logradouro = resp.Endereco.Logradouro,
                        Bairro = resp.Endereco.Bairro,
                        CEP = resp.Endereco.CEP,
                        Cidade = resp.Endereco.Cidade,
                        Complemento = resp.Endereco.Complemento,
                        Numero = resp.Endereco.Numero,
                        UF = resp.Endereco.UF
                    };
                    _respFinanceiroService.Alterar(resp);
                }
            }

            var enderecos = _enderecoService.EnderecosSemPessoa();
            var pessoas = _pessoaService.BuscarTodos();
            foreach (var endereco in enderecos)
            {
                if (!pessoas.Any(p => p.EnderecoId == endereco.Id))
                {
                    _enderecoService.Excluir(endereco.Id);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
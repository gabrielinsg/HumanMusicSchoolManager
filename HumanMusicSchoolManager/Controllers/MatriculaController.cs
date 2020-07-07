using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Models.ViewModels;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize(Roles = "Admin, Atendimento")]
    public class MatriculaController : Controller
    {
        private readonly IMatriculaService _matriculaService;
        private readonly IAlunoService _alunoService;
        private readonly ISalaService _salaService;
        private readonly IDispSalaService _dispSalaService;
        private readonly ICursoService _cursoService;
        private readonly IRespFinanceiroService _respFinanceiroService;
        private readonly ITaxaMatriculaService _taxaMatriculaService;
        private readonly IPessoaService _pessoaService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFinanceiroService _financeiroService;
        private readonly IRelatorioMatriculaService _relatorioMatriculaService;
        private readonly IPacoteCompraService _pacoteCompraService;
        private readonly IAulaService _aulaService;
        private readonly IChamadaService _chamadaService;
        private readonly IFeriadoService _feriadoService;
        private readonly IAulaConfigService _aulaConfigService;

        public MatriculaController(IMatriculaService matriculaService,
            IAlunoService alunoService,
            ISalaService salaService,
            IDispSalaService dispSalaService,
            ICursoService cursoService,
            IRespFinanceiroService respFinanceiroService,
            ITaxaMatriculaService taxaMatriculaService,
            IPessoaService pessoaService,
            IFinanceiroService financeiroService,
            UserManager<ApplicationUser> userManager,
            IRelatorioMatriculaService relatorioMatriculaService,
            IPacoteCompraService pacoteCompraService,
            IAulaService aulaService,
            IChamadaService chamadaService,
            IFeriadoService feriadoService,
            IAulaConfigService aulaConfigService)
        {
            this._matriculaService = matriculaService;
            this._alunoService = alunoService;
            this._salaService = salaService;
            this._dispSalaService = dispSalaService;
            this._cursoService = cursoService;
            this._respFinanceiroService = respFinanceiroService;
            this._taxaMatriculaService = taxaMatriculaService;
            this._pessoaService = pessoaService;
            this._userManager = userManager;
            this._financeiroService = financeiroService;
            this._relatorioMatriculaService = relatorioMatriculaService;
            this._pacoteCompraService = pacoteCompraService;
            this._aulaService = aulaService;
            this._chamadaService = chamadaService;
            this._feriadoService = feriadoService;
            this._aulaConfigService = aulaConfigService;
        }

        [HttpGet]
        public IActionResult Form(int? matriculaId, int? alunoId, int? dispSalaId, int? cursoId, int? respFinanceiroId, string collapse)
        {
            if (matriculaId == null)
            {
                if (alunoId != null)
                {
                    var aluno = _alunoService.BuscarPorId(alunoId.Value);
                    var matricula = new MatriculaViewModel()
                    {
                        Aluno = aluno,
                        DispSalas = _dispSalaService.HorariosDisponiveis(),
                        Cursos = _cursoService.BuscarTodos(),
                        TaxasMatricula = _taxaMatriculaService.BuscarTodos()
                    };
                    if (dispSalaId != null)
                    {
                        matricula.DispSala = _dispSalaService.BuscarPorId(dispSalaId.Value);
                    }
                    if (cursoId != null)
                    {
                        matricula.Curso = _cursoService.BuscarPorId(cursoId.Value);
                    }
                    if (respFinanceiroId != null)
                    {
                        matricula.RespFinanceiro = _respFinanceiroService.BuscarPorId(respFinanceiroId.Value);
                    }

                    if (dispSalaId == null) ViewBag.Aba = 1;
                    else if (respFinanceiroId == null) ViewBag.Aba = 2;
                    else ViewBag.Aba = 3;

                    return View(matricula);
                }
                else
                {
                    return RedirectToAction(controllerName: "Aluno", actionName: "Index");
                }

            }
            else
            {
                var mat = _matriculaService.BuscarPorId(matriculaId.Value);
                var matricula = new MatriculaViewModel()
                {
                    Matricula = mat,
                    Aluno = mat.Aluno,
                    DispSala = mat.DispSala,
                    DispSalas = _dispSalaService.BuscarTodos(),
                    Cursos = _cursoService.BuscarTodos()
                };
                ViewBag.Aba = 1;
                return View(matricula);
            }
        }

        [HttpPost]
        public IActionResult Form(MatriculaViewModel matriculaViewModel)
        {
            foreach (var model in ModelState.Where(m => !m.Key.StartsWith("Financeiro") || m.Key.EndsWith("Pessoa") || m.Key.EndsWith("Aluno")).ToList())
            {
                ModelState.Remove(model.Key);
            }

            var pessoa = _pessoaService.BusacarPorUserName(User.Identity.Name);

            if (pessoa == null)
            {
                ModelState.AddModelError("Pessoa", "Não existe pessoa para a matricula");
            }
            else
            {
                if (matriculaViewModel.Financeiros != null)
                {
                    foreach (var financeiro in matriculaViewModel.Financeiros)
                    {
                        financeiro.Pessoa = pessoa;
                    }

                    foreach (var financeiro in matriculaViewModel.Financeiros)
                    {
                        financeiro.UltimaAlteracao = NowHorarioBrasilia.GetNow();
                    }
                }

            }



            if (matriculaViewModel.Aluno.Id == null)
            {
                ModelState.AddModelError("Aluno", "Aluno não selecionado!");
            }
            else
            {
                matriculaViewModel.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);
                matriculaViewModel.Matricula.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);

                if (matriculaViewModel.Financeiros != null)
                {
                    foreach (var financeiro in matriculaViewModel.Financeiros)
                    {
                        financeiro.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);
                    }
                }
            }

            if (matriculaViewModel.Curso.Id == null)
            {
                ModelState.AddModelError("Curso", "Curso deve ser selecionado!");
            }
            else
            {
                matriculaViewModel.Curso = _cursoService.BuscarPorId(matriculaViewModel.Curso.Id.Value);
                matriculaViewModel.Matricula.Curso = _cursoService.BuscarPorId(matriculaViewModel.Curso.Id.Value);
            }

            if (matriculaViewModel.DispSala.Id == null)
            {
                ModelState.AddModelError("Horário", "Um horário deve ser selecionado!");
            }
            else
            {
                matriculaViewModel.DispSala = _dispSalaService.BuscarPorId(matriculaViewModel.DispSala.Id.Value);
                matriculaViewModel.Matricula.DispSala = _dispSalaService.BuscarPorId(matriculaViewModel.DispSala.Id.Value);
            }

            if (matriculaViewModel.RespFinanceiro.Id == null)
            {
                ModelState.AddModelError("RespFinanceiro", "Um Responsável financeiro deve ser selecionado!");
            }
            else
            {
                matriculaViewModel.RespFinanceiro = _respFinanceiroService.BuscarPorId(matriculaViewModel.RespFinanceiro.Id.Value);
                matriculaViewModel.Matricula.RespFinanceiro = _respFinanceiroService.BuscarPorId(matriculaViewModel.RespFinanceiro.Id.Value);
            }

            matriculaViewModel.TaxasMatricula = _taxaMatriculaService.BuscarTodos();
            matriculaViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
            matriculaViewModel.Cursos = _cursoService.BuscarTodos();

            if (ModelState.IsValid)
            {
                matriculaViewModel.Matricula.Ativo = true;
                matriculaViewModel.Matricula.DataMatricula = NowHorarioBrasilia.GetNow();
                matriculaViewModel.Matricula.ProfessorId = matriculaViewModel.DispSala.Professor.Id.Value;
                _matriculaService.Cadastrar(matriculaViewModel.Matricula);
                var relatorioMatricula = new RelatorioMatricula
                {
                    PessoaId = _pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value,
                    MatriculaId = matriculaViewModel.Matricula.Id.Value,
                    Data = NowHorarioBrasilia.GetNow()
                };
                string descricao = "Matricula no curso de "
                    + matriculaViewModel.Matricula.Curso.Nome + " com " + matriculaViewModel.DispSala.Professor.Nome +
                    " na sala " + matriculaViewModel.DispSala.Sala.Nome + " " + matriculaViewModel.DispSala.Dia.GetType()
                        .GetMember(matriculaViewModel.DispSala.Dia.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .GetName() + " às " + matriculaViewModel.DispSala.Hora.ToString("00:'00'h");
                relatorioMatricula.Descricao = descricao;
                _relatorioMatriculaService.Cadastrar(relatorioMatricula);
                if (matriculaViewModel.Financeiros != null)
                {
                    foreach (var financeiro in matriculaViewModel.Financeiros)
                    {

                        decimal? desconto = 0;
                        if (financeiro.Desconto != null)
                        {
                            desconto = financeiro.Desconto;
                        }
                        var valor = financeiro.Valor - desconto;
                        if (financeiro.FormaPagamento == FormaPagamento.DEBITO || financeiro.FormaPagamento == FormaPagamento.CREDITO)
                        {
                            financeiro.ValorPago = valor;
                            financeiro.DataPagamento = NowHorarioBrasilia.GetNow();
                        }
                        _financeiroService.Cadastrar(financeiro);
                    }
                }

                return RedirectToAction("Aluno", "Aluno", new { alunoId = matriculaViewModel.Aluno.Id });
            }
            else
            {
                ViewBag.Aba = 1;
                return View(matriculaViewModel);
            }
        }

        [HttpPost]
        public JsonResult BuscarPorNome(string nome)
        {
            var respFinanceiro = _respFinanceiroService.BuscarPorNome(nome);
            return Json(respFinanceiro);
        }

        [HttpPost]
        public IActionResult CadastraRespFinanceiro(MatriculaViewModel matriculaViewModel)
        {

            if (matriculaViewModel.RespFinanceiro.Id == null)
            {
                foreach (var model in ModelState.Where(m => !m.Key.StartsWith("RespFinanceiro")).ToList())
                {
                    ModelState.Remove(model.Key);
                }

                var respPorCpf = _respFinanceiroService.BuscarPorCPF(matriculaViewModel.RespFinanceiro.CPF != null ? 
                    matriculaViewModel.RespFinanceiro.CPF : "");

                if (respPorCpf != null) ModelState.AddModelError("RespFinanceiro.CPF", "CPF já cadastrado");

                if (ModelState.IsValid)
                {
                    matriculaViewModel.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);
                    matriculaViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
                    matriculaViewModel.Cursos = _cursoService.BuscarTodos();
                    matriculaViewModel.TaxasMatricula = _taxaMatriculaService.BuscarTodos();
                    if (matriculaViewModel.DispSala.Id != null)
                    {
                        matriculaViewModel.DispSala = _dispSalaService.BuscarPorId(matriculaViewModel.DispSala.Id.Value);
                    }
                    if (matriculaViewModel.Curso.Id != null)
                    {
                        matriculaViewModel.Curso = _cursoService.BuscarPorId(matriculaViewModel.Curso.Id.Value);
                    }
                    _respFinanceiroService.Cadastrar(matriculaViewModel.RespFinanceiro);
                    ViewBag.Aba = 3;
                    return View("Form", matriculaViewModel);
                }
                else
                {
                    ViewBag.Collapse = "respFinanceiro";
                    matriculaViewModel.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);
                    matriculaViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
                    matriculaViewModel.Cursos = _cursoService.BuscarTodos();
                    matriculaViewModel.TaxasMatricula = _taxaMatriculaService.BuscarTodos();
                    if (matriculaViewModel.DispSala.Id != null)
                    {
                        matriculaViewModel.DispSala = _dispSalaService.BuscarPorId(matriculaViewModel.DispSala.Id.Value);
                    }
                    if (matriculaViewModel.Curso.Id != null)
                    {
                        matriculaViewModel.Curso = _cursoService.BuscarPorId(matriculaViewModel.Curso.Id.Value);
                    }
                    ViewBag.Aba = 2;
                    return View("Form", matriculaViewModel);
                }
            }
            else
            {
                foreach (var model in ModelState.Where(m => !m.Key.StartsWith("RespFinanceiro")).ToList())
                {
                    ModelState.Remove(model.Key);
                }
                if (ModelState.IsValid)
                {
                    matriculaViewModel.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);
                    matriculaViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
                    matriculaViewModel.Cursos = _cursoService.BuscarTodos();
                    matriculaViewModel.TaxasMatricula = _taxaMatriculaService.BuscarTodos();
                    if (matriculaViewModel.DispSala.Id != null)
                    {
                        matriculaViewModel.DispSala = _dispSalaService.BuscarPorId(matriculaViewModel.DispSala.Id.Value);
                    }
                    if (matriculaViewModel.Curso.Id != null)
                    {
                        matriculaViewModel.Curso = _cursoService.BuscarPorId(matriculaViewModel.Curso.Id.Value);
                    }
                    _respFinanceiroService.Alterar(matriculaViewModel.RespFinanceiro);
                    ViewBag.Aba = 3;
                    return View("Form", matriculaViewModel);
                }
                else
                {
                    ViewBag.Aba = 1;
                    matriculaViewModel.Aluno = _alunoService.BuscarPorId(matriculaViewModel.Aluno.Id.Value);
                    matriculaViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
                    matriculaViewModel.Cursos = _cursoService.BuscarTodos();
                    matriculaViewModel.TaxasMatricula = _taxaMatriculaService.BuscarTodos();
                    if (matriculaViewModel.DispSala.Id != null)
                    {
                        matriculaViewModel.DispSala = _dispSalaService.BuscarPorId(matriculaViewModel.DispSala.Id.Value);
                    }
                    if (matriculaViewModel.Curso.Id != null)
                    {
                        matriculaViewModel.Curso = _cursoService.BuscarPorId(matriculaViewModel.Curso.Id.Value);
                    }
                    ViewBag.Aba = 2;
                    return View("Form", matriculaViewModel);
                }
            }
        }

        [HttpPost]
        public JsonResult BuscarAlunoPorId(int alunoId)
        {
            var aluno = _alunoService.BuscarPorId(alunoId);
            return Json(alunoId);
        }

        [HttpPost]
        public JsonResult BuscarAlunoRespFinanceiro(int alunoId)
        {
            var aluno = _alunoService.BuscarPorId(alunoId);
            var respFinanceiro = _respFinanceiroService.BuscarPorCPF(aluno.CPF);
            if (respFinanceiro != null)
            {
                return Json(respFinanceiro);
            }
            else
            {
                aluno.Id = null;
                return Json(aluno);
            }
        }

        [HttpPost]
        public JsonResult BuscarRespFinanceiro(int respFinanceiroId)
        {
            return Json(_respFinanceiroService.BuscarPorId(respFinanceiroId));
        }

        [HttpGet]
        public IActionResult TrocaDispSala(int? matriculaId, int? dispSalaId, int? cursoId)
        {
            if (matriculaId != null)
            {

                var trocaDispSalaViewModel = new TrocaDispSalaViewModel
                {
                    DispSalas = _dispSalaService.HorariosDisponiveis(),
                    DiaAula = NowHorarioBrasilia.GetNow(),
                    Cursos = _cursoService.BuscarTodos()
                };

                trocaDispSalaViewModel.Matricula = _matriculaService.BuscarPorId(matriculaId.Value);

                if (trocaDispSalaViewModel.Matricula != null)
                {
                    if (dispSalaId != null)
                    {
                        trocaDispSalaViewModel.DispSala = _dispSalaService.BuscarPorId(dispSalaId.Value);
                        trocaDispSalaViewModel.Cursos = _cursoService.BuscarTodos();
                        trocaDispSalaViewModel.Curso = _cursoService.BuscarPorId(cursoId.Value);
                        ViewBag.Aba = 2;
                    }
                    else
                    {
                        ViewBag.Aba = 1;
                    }

                    return View(trocaDispSalaViewModel);
                }

            }

            TempData["Error"] = "Matrícula não encontrada!";
            return RedirectToAction("Index", "Aluno");
        }

        [HttpPost]
        public IActionResult TrocaDispSala(TrocaDispSalaViewModel trocaDispSalaViewModel)
        {
            trocaDispSalaViewModel.Matricula = _matriculaService.BuscarPorId(trocaDispSalaViewModel.Matricula.Id.Value);
            trocaDispSalaViewModel.DispSala = _dispSalaService.BuscarPorId(trocaDispSalaViewModel.DispSala.Id.Value);

            foreach (var model in ModelState)
            {
                ModelState.Remove(model.Key);
            }

            if (trocaDispSalaViewModel.DiaAula == null)
            {
                ModelState.AddModelError("DiaAula", "Primeira aula deve ser selecionada");
            }

            if (trocaDispSalaViewModel.DispSala.Id == null)
            {
                ModelState.AddModelError("DispSala", "Um horário deve ser selecionado");
            }
            else
            {
                trocaDispSalaViewModel.DispSala = _dispSalaService.BuscarPorId(trocaDispSalaViewModel.DispSala.Id.Value);
            }

            if (trocaDispSalaViewModel.DiaAula.DayOfWeek != (DayOfWeek)trocaDispSalaViewModel.DispSala.Dia)
            {
                var dia = trocaDispSalaViewModel.DispSala.Dia.GetType()
                        .GetMember(trocaDispSalaViewModel.DispSala.Dia.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .Name;
                ModelState.AddModelError("PrimeiraAula", "A primeira aula precisa ser " + dia);
            }

            if (ModelState.IsValid)
            {
                trocaDispSalaViewModel.Matricula.DispSala = trocaDispSalaViewModel.DispSala;
                trocaDispSalaViewModel.Matricula.ProfessorId = trocaDispSalaViewModel.DispSala.Professor.Id;
                trocaDispSalaViewModel.Matricula.CursoId = trocaDispSalaViewModel.Curso.Id.Value;
                trocaDispSalaViewModel.DiaAula = trocaDispSalaViewModel.DiaAula.AddHours(trocaDispSalaViewModel.DispSala.Hora);
                var aulaConfig = _aulaConfigService.Buscar();

                if (aulaConfig == null)
                {
                    aulaConfig.TempoLimiteLancamento = 72;
                }

                if (trocaDispSalaViewModel.Matricula.PacoteCompras != null)
                {
                    foreach (var pc in trocaDispSalaViewModel.Matricula.PacoteCompras)
                    {


                        var pacoteCompra = _pacoteCompraService.BuscarPorId(pc.Id.Value);

                        if (pacoteCompra.Chamadas.Any(c => c.Reposicao == null && c.Presenca == null))
                        {
                            foreach (var chamada in pacoteCompra.Chamadas.Where(c => c.Reposicao == null && c.Presenca == null).ToList())
                            {
                                var aulaAntiga = chamada.Aula;
                                chamada.AulaId = null;
                                _chamadaService.Alterar(chamada);
                                aulaAntiga = _aulaService.BuscarPorId(aulaAntiga.Id.Value);

                                if (aulaAntiga.Chamadas.Count == 0 && aulaAntiga.Demostrativas.Count == 0)
                                {
                                    _aulaService.Excluir(aulaAntiga.Id.Value);
                                }
                            }

                            pacoteCompra = _pacoteCompraService.BuscarPorId(pc.Id.Value);

                            foreach (var chamada in pacoteCompra.Chamadas.Where(c => c.Aula == null))
                            {
                                Feriado feriado = null;
                                do
                                {
                                    feriado = _feriadoService.BuscarPorData(trocaDispSalaViewModel.DiaAula);
                                    if (feriado != null)
                                    {
                                        trocaDispSalaViewModel.DiaAula = trocaDispSalaViewModel.DiaAula.AddDays(7);
                                    }
                                } while (feriado != null);


                                var aula = _aulaService.BuscarPorDiaHora(trocaDispSalaViewModel.DiaAula, trocaDispSalaViewModel.Matricula.DispSala);

                                if (aula == null)
                                {

                                    aula = new Aula()
                                    {
                                        CursoId = trocaDispSalaViewModel.Matricula.CursoId,
                                        ProfessorId = trocaDispSalaViewModel.Matricula.DispSala.Professor.Id.Value,
                                        SalaId = trocaDispSalaViewModel.Matricula.DispSala.Sala.Id.Value,
                                        Data = trocaDispSalaViewModel.DiaAula,
                                        DataLimite = trocaDispSalaViewModel.DiaAula.AddHours(aulaConfig.TempoLimiteLancamento)
                                    };
                                    _aulaService.Cadastrar(aula);
                                }

                                chamada.AulaId = aula.Id.Value;
                                _chamadaService.Alterar(chamada);
                                trocaDispSalaViewModel.DiaAula = trocaDispSalaViewModel.DiaAula.AddDays(7);
                            }
                        }
                    }
                }

                //Relatório Matrícula
                var relatorioMatricula = new RelatorioMatricula
                {
                    Data = NowHorarioBrasilia.GetNow(),
                    MatriculaId = trocaDispSalaViewModel.Matricula.Id.Value,
                    PessoaId = _pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value
                };

                var descricao = "Alterado matricula para " +
                 trocaDispSalaViewModel.Matricula.Curso.Nome + " com " + trocaDispSalaViewModel.Matricula.DispSala.Professor.Nome +
                 " na sala " + trocaDispSalaViewModel.Matricula.DispSala.Sala.Nome + " " + trocaDispSalaViewModel.Matricula.DispSala.Dia.GetType()
                .GetMember(trocaDispSalaViewModel.Matricula.DispSala.Dia.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName() + " às " + trocaDispSalaViewModel.Matricula.DispSala.Hora.ToString("00:'00'h");

                relatorioMatricula.Descricao = descricao;

                _relatorioMatriculaService.Cadastrar(relatorioMatricula);


                //Enviar Email
                //var corpo = trocaDispSalaViewModel.Matricula.Aluno.Nome + " mudou "


                TempData["Success"] = "Hórario alterado com sucesso. Verificar o calendário para novas aulas";
                return RedirectToAction("Aluno", "Aluno", new { alunoId = trocaDispSalaViewModel.Matricula.AlunoId });
            }
            else
            {
                trocaDispSalaViewModel.DispSalas = _dispSalaService.HorariosDisponiveis();
                trocaDispSalaViewModel.Cursos = _cursoService.BuscarTodos();
                ViewBag.Aba = 2;
                return View(trocaDispSalaViewModel);
            }
        }

        [HttpPost]
        public IActionResult CancelaMatricula(Matricula matriculaPost)
        {
            var matricula = _matriculaService.BuscarPorId(matriculaPost.Id.Value);
            if (matricula != null)
            {
                matricula.Motivo = matriculaPost.Motivo;
                matricula.Outros = matriculaPost.Outros;
                bool verifica = true;
                if (matricula.PacoteCompras != null)
                {
                    foreach (var pc in matricula.PacoteCompras)
                    {
                        var pacoteCompra = _pacoteCompraService.BuscarPorId(pc.Id.Value);
                        if (pacoteCompra.Chamadas.Any(c => c.Presenca == null))
                        {
                            verifica = false;
                            break;
                        }
                    }
                }

                if (verifica)
                {
                    matricula.DispSalaId = null;
                    matricula.EncerramentoMatricula = NowHorarioBrasilia.GetNow();
                    _matriculaService.Alterar(matricula);
                    TempData["Success"] = "Matricula Cancelada com sucesso!";
                    return RedirectToAction("Aluno", "Aluno", new { alunoId = matricula.AlunoId });
                }
                else
                {
                    TempData["Warning"] = "Não é possível fazer o cancelamento pois existem aulas em aberto para essa matrícula!";
                    return RedirectToAction("Aluno", "Aluno", new { alunoId = matricula.AlunoId });
                }
            }
            TempData["Error"] = "Matricula não encontrada!";
            return RedirectToAction("Index", "Aluno");
        }
    }
}
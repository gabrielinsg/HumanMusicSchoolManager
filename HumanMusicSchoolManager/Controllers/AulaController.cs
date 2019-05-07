using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.Services;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize(Roles = "Admin, Coordenacao, Professor")]
    public class AulaController : Controller
    {
        private readonly IAulaService _aulaService;
        private readonly IProfessorService _professorService;
        private readonly ICursoService _cursoService;
        private readonly ISalaService _salaService;
        private readonly IChamadaService _chamadaService;
        private readonly IMatriculaService _matriculaService;
        private readonly IReposicaoService _reposicaoService;
        private readonly IDemostrativaService _demostrativaService;
        private readonly IPessoaService _pessoaService;
        private readonly IAulaConfigService _aulaConfigService;

        public AulaController(IAulaService aulaService,
            IProfessorService professorService,
            ICursoService cursoService,
            ISalaService salaService,
            IChamadaService chamadaService,
            IMatriculaService matriculaService,
            IReposicaoService reposicaoService,
            IDemostrativaService demostrativaService,
            IPessoaService pessoaService,
            IAulaConfigService aulaConfigService)
        {
            this._aulaService = aulaService;
            this._professorService = professorService;
            this._cursoService = cursoService;
            this._salaService = salaService;
            this._chamadaService = chamadaService;
            this._matriculaService = matriculaService;
            this._reposicaoService = reposicaoService;
            this._demostrativaService = demostrativaService;
            this._pessoaService = pessoaService;
            this._aulaConfigService = aulaConfigService;
        }

        [HttpGet]
        public IActionResult Form(int? aulaId)
        {
            if (aulaId != null)
            {
                var aula = _aulaService.BuscarPorId(aulaId.Value);

                if (aula != null)
                {
                    var aulaConfig = _aulaConfigService.Buscar();
                    if (aulaConfig == null)
                    {
                        TempData["Error"] = "Aula não configurada, favor entrar em contato com o admistrado do sistema!";
                        return RedirectToAction("Calendario", "Professor");
                    }

                    if (aulaConfig.LancamentoAtrasado || aula.DataLimite == null || aula.DataLimite > NowHorarioBrasilia.GetNow())
                    {
                        ViewBag.Historico = Historico(aula);
                        return View(aula);
                    }
                    else
                    {
                        TempData["Error"] = "Data limite para lançamento dessa aula expirou. Favor entrar em contato com a coordenação!";
                        return RedirectToAction("Calendario", "Professor");
                    }

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Form(Aula aula, List<int> Estrelas, int Hora)
        {

            foreach (var model in ModelState.Where(m => m.Key.Contains(".PacoteCompra.")).ToList())
            {
                ModelState.Remove(model.Key);
            }

            if (aula.AulaDada)
            {
                var aulaConfig = _aulaConfigService.Buscar();

                if (aulaConfig.DescAtividadesObrigatorio)
                {
                    if (aula.DescAtividades == null)
                    {
                        ModelState.AddModelError("DescAtividades", "Descrição de atividades obrigatória");
                    }
                    else if (aula.DescAtividades.Length < aulaConfig.MinCaracteresDescAtividades + 9)
                    {
                        ModelState.AddModelError("DescAtividades", "Descrição não atingiu o mínimo de caracteres exigidos.");
                    }
                }

            }

            var matriculas = new List<Matricula>();
            var pacoteCompras = new List<PacoteCompra>();

            if(aula.Chamadas != null)
            {
                foreach (var chamada in aula.Chamadas)
                {
                    var matricula = _matriculaService.BuscarPorId(_chamadaService.BuscarPorId(chamada.Id.Value).PacoteCompra.MatriculaId);
                    var modulo = chamada.PacoteCompra.Matricula.Modulo;
                    var estrelas = chamada.PacoteCompra.Matricula.Estrelas;
                    chamada.PacoteCompra = null;
                    matricula.Modulo = modulo;
                    matricula.Estrelas = estrelas;
                    matriculas.Add(matricula);
                }
            }
                
            if (ModelState.IsValid)
            {
                foreach (var matricula in matriculas)
                {
                    _matriculaService.Alterar(matricula);
                }
                if (aula.Chamadas != null)
                {
                    foreach (var chamada in aula.Chamadas)
                    {

                        var reposicao = _reposicaoService.BuscarPorChamada(chamada.Id.Value);
                        if (!aula.AulaDada) chamada.Presenca = null;
                        var gravarChamada = _chamadaService.BuscarPorId(chamada.Id.Value);
                        gravarChamada.Observacao = chamada.Observacao;
                        gravarChamada.Presenca = chamada.Presenca;
                        _chamadaService.Alterar(gravarChamada);
                        if (reposicao != null)
                        {
                            reposicao.DispSalaId = null;
                            _reposicaoService.Alterar(reposicao);
                        }
                    }
                }
                if (aula.Demostrativas != null)
                {
                    var i = 0;
                    foreach (var demostrativa in aula.Demostrativas)
                    {
                        if (!aula.AulaDada) demostrativa.Presenca = null;
                        if (Estrelas.Count > 0)
                        {
                            demostrativa.Estrelas = Estrelas[i];
                        }
                        _demostrativaService.Alterar(demostrativa);
                        i++;
                    }
                }

                aula.Data = aula.Data.AddHours(Hora);
                _aulaService.Alterar(aula);
                return RedirectToAction("Calendario", "Professor", new { professorId = aula.ProfessorId });
            }
            else
            {
                aula.Professor = _professorService.BuscarPorId(aula.ProfessorId);
                aula.Curso = _cursoService.BuscarPorId(aula.CursoId);
                aula.Sala = _salaService.BuscarPorId(aula.SalaId);
                aula.Chamadas = _aulaService.BuscarPorId(aula.Id.Value).Chamadas;
                aula.Demostrativas = _aulaService.BuscarPorId(aula.Id.Value).Demostrativas;
                ViewBag.Historico = Historico(aula);
                return View(aula);
            }
        }

        [HttpGet]
        public IActionResult Aula(int? aulaId)
        {
            if (aulaId != null)
            {
                var aula = _aulaService.BuscarPorId(aulaId.Value);
                if (aula != null)
                {
                    var professores = _professorService.BuscarProfessorPorCurso(aula.CursoId);
                    var listProfessores = new List<SelectListItem>();
                    foreach (var professor in professores)
                    {
                        var selectListItem = new SelectListItem
                        {
                            Value = professor.Id.ToString(),
                            Text = professor.Nome
                        };
                        listProfessores.Add(selectListItem);
                    }
                    ViewBag.professores = listProfessores;

                    var salas = _salaService.BuscarTodos();
                    var listSalas = new List<SelectListItem>();
                    foreach (var sala in salas)
                    {
                        var selectListItem = new SelectListItem
                        {
                            Value = sala.Id.ToString(),
                            Text = sala.Nome
                        };
                        listSalas.Add(selectListItem);
                    }
                    ViewBag.salas = listSalas;

                    return View(aula);
                }
            }

            return RedirectToAction("Index", "Aluno");
        }

        [HttpPost]
        public IActionResult Aula(Aula aula, int? Hora)
        {

            if (Hora == null)
            {
                ModelState.AddModelError("Hora", "Hora deve ser preenchida!");
            }

            if (ModelState.IsValid)
            {
                var aulaConfig = _aulaConfigService.Buscar();
                if (aulaConfig == null)
                {
                    aulaConfig.TempoLimiteLancamento = 72;
                }

                aula.Data.AddHours(-aula.Data.Hour);
                aula.Data.AddHours(Hora.Value);
                aula.DataLimite.AddHours(-aula.DataLimite.Hour);
                aula.DataLimite = aula.Data;
                aula.DataLimite.AddHours(aulaConfig.TempoLimiteLancamento);

                foreach (var chamada in aula.Chamadas)
                {
                    if (aula.AulaDada == false)
                    {
                        chamada.Presenca = null;
                    }
                    _chamadaService.Alterar(chamada);
                }

                _aulaService.Alterar(aula);
                TempData["Success"] = "Aula alterado com sucesso";
                return RedirectToAction("Index", "Aula");
            }
            else
            {
                var professores = _professorService.BuscarProfessorPorCurso(aula.CursoId);
                var listProfessores = new List<SelectListItem>();
                foreach (var professor in professores)
                {
                    var selectListItem = new SelectListItem
                    {
                        Value = professor.Id.ToString(),
                        Text = professor.Nome
                    };
                    listProfessores.Add(selectListItem);
                }

                var salas = _salaService.BuscarTodos();
                var listSalas = new List<SelectListItem>();
                foreach (var sala in salas)
                {
                    var selectListItem = new SelectListItem
                    {
                        Value = sala.Id.ToString(),
                        Text = sala.Nome
                    };
                    listSalas.Add(selectListItem);
                }
                ViewBag.salas = listSalas;

                ViewBag.professores = listProfessores;
                return View(aula);
            }
        }

        public IActionResult AulasAtrasadas()
        {
            var professorPorUsuario = _professorService.BuscarPorId(_pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value);
            if (professorPorUsuario == null)
            {
                return View(_aulaService.Atrasadas());
            }
            else
            {
                return View(_aulaService.AtrasadasPorProfessor(professorPorUsuario.Id.Value));
            }
        }

        public JsonResult AulasAtrasadasJoson()
        {
            var professor = _professorService.BuscarPorId(_pessoaService.BusacarPorUserName(User.Identity.Name).Id.Value);
            var JsonTotal = new JsonAluasAtrasadas
            {
                Total = 0
            };
            if (professor != null)
            {
                JsonTotal.Total = _aulaService.AtrasadasPorProfessor(professor.Id.Value).Count;
            }

            return Json(JsonTotal);
        }

        private List<Chamada> Historico(Aula aula)
        {
            var historico = new List<Chamada>();
            foreach (var matricula in aula.Chamadas.Select(c => c.PacoteCompra.Matricula))
            {
                foreach (var chamada in _chamadaService.HistoricoAlunoCurso(matricula))
                {
                    historico.Add(chamada);
                }
            }

            return historico;
        }
        [HttpPost]
        public void AutoSaveDescAtividades(int id, string conteudo)
        {
            _aulaService.AtualizarDescAtividades(id, conteudo);
        }

        [HttpPost]
        public void AutoSaveObservacao(int id, string conteudo)
        {
            _chamadaService.AtualizarObservacao(id, conteudo);
        }

        public IActionResult HistoricoCompleto(int alunoId, int cursoId)
        {
            return View(_chamadaService.HistoricoCompleto(alunoId, cursoId));
        }
    }
}

class JsonAluasAtrasadas
{
    public int Total { get; set; }
}
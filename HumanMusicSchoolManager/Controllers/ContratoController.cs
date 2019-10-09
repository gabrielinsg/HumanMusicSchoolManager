using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Models.Models;
using HumanMusicSchoolManager.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanMusicSchoolManager.Controllers
{
    [Authorize(Roles = "Admin, Atendimento, Coordenacao")]
    public class ContratoController : Controller
    {
        private readonly IContratoService _contratoService;
        private readonly IPacoteCompraService _pacoteCompraService;

        public ContratoController(IContratoService contratoService,
            IPacoteCompraService pacoteCompraService)
        {
            this._contratoService = contratoService;
            this._pacoteCompraService = pacoteCompraService;
        }

        public IActionResult Index()
        {
            return View(_contratoService.BuscarTodos());
        }

        [HttpGet]
        public IActionResult Form(int? contratoId)
        {
            if (contratoId == null)
            {
                return View();
            }
            else
            {
                var contrato = _contratoService.BuscarPorId(contratoId.Value);
                if (contrato != null)
                {
                    return View(contrato);
                }
                else
                {
                    TempData["Error"] = "Contrato não encontrato!";
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        public IActionResult Form(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                var mensagem = "";
                if (contrato.Id == null)
                {
                    _contratoService.Cadastrar(contrato);
                    mensagem = "Contrato Cadastrado com sucesso!";
                }
                else
                {
                    _contratoService.Alterar(contrato);
                    mensagem = "Contrato Alterado com sucesso!";
                }
                TempData["Success"] = mensagem;
                return RedirectToAction("Form");
            }
            else
            {
                return View(contrato);
            }
        }

        public IActionResult Excluir(int? contratoId)
        {
            if (contratoId != null)
            {
                _contratoService.Excluir(contratoId.Value);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Contrato não encontrado!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult BuscarPorNome(string nome)
        {
            var contrato = _contratoService.BuscarPorNome(nome);
            return Json(contrato);
        }

        [Authorize(Roles = "Admin, Atendimento, Coordenacao, Aluno")]
        public IActionResult Contrato(int? pacoteCompraId)
        {
            if (pacoteCompraId != null)
            {
                var pacoteCompra = _pacoteCompraService.BuscarPorId(pacoteCompraId.Value);

                if (pacoteCompra != null)
                {
                    var contrato = pacoteCompra.PacoteAula.Contrato;

                    //Responsável Financeiro
                    var respFinanceiro = pacoteCompra.Matricula.RespFinanceiro;
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.Nome}", respFinanceiro.Nome);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.CPF}", respFinanceiro.CPF);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.RG}", respFinanceiro.RG);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.DataNasc}", respFinanceiro.DataNascimento.ToString("dd/MM/yyyy"));
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.Idade}", ((int.Parse(NowHorarioBrasilia.GetNow().ToString("yyyyMMdd")) - int.Parse(respFinanceiro.DataNascimento.ToString("yyyyMMdd"))) / 10000).ToString());
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.Logradouro}", respFinanceiro.Endereco.Logradouro);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.Numero}", respFinanceiro.Endereco.Numero.ToString());
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.Cidade}", respFinanceiro.Endereco.Cidade);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.UF}", respFinanceiro.Endereco.UF.ToString());
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.Bairro}", respFinanceiro.Endereco.Bairro);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.CEP}", respFinanceiro.Endereco.CEP);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.Cel}", respFinanceiro.Cel);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.Tel}", respFinanceiro.Tel);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.Email}", respFinanceiro.Email);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.Nacionalidade}", respFinanceiro.Nacionalidade);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.Naturalidade}", respFinanceiro.Naturalidade);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.EstadoCivil}", respFinanceiro.EstadoCivil.GetType()
                        .GetMember(respFinanceiro.EstadoCivil.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .GetName());
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.Profissao}", respFinanceiro.Profissao);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Responsavel.OrgaoExpedidor}", respFinanceiro.OrgaoExpedidor);

                    //Aluno
                    var aluno = pacoteCompra.Matricula.Aluno;
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.Nome}", aluno.Nome);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.CPF}", aluno.CPF);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.RG}", aluno.RG);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.DataNasc}", aluno.DataNascimento.ToString("dd/MM/yyyy"));
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.Idade}", ((int.Parse(NowHorarioBrasilia.GetNow().ToString("yyyyMMdd")) - int.Parse(aluno.DataNascimento.ToString("yyyyMMdd"))) / 10000).ToString());
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.Logradouro}", aluno.Endereco.Logradouro);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.Numero}", aluno.Endereco.Numero.ToString());
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.Cidade}", aluno.Endereco.Cidade);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.UF}", aluno.Endereco.UF.ToString());
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.Bairro}", aluno.Endereco.Bairro);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.CEP}", aluno.Endereco.CEP);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.Cel}", aluno.Cel);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.Tel}", aluno.Tel);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Aluno.email}", aluno.Email);

                    //Pacote
                    var pacote = pacoteCompra.PacoteAula;
                    contrato.Conteudo = contrato.Conteudo.Replace("{Pacote.Nome}", pacote.Nome);
                    var valorTotal = pacote.Valor - (pacoteCompra.Desconto != null ? pacoteCompra.Desconto : 0);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Pacote.Valor}", ((double)valorTotal).ToString("R$ #,###.00"));
                    contrato.Conteudo = contrato.Conteudo.Replace("{Pacote.ValorExtenso}", ToExtenso(valorTotal.Value));
                    contrato.Conteudo = contrato.Conteudo.Replace("{Pacote.QtdParcelas}", pacoteCompra.QtdParcela.ToString());
                    contrato.Conteudo = contrato.Conteudo.Replace("{Pacote.Desconto}", pacoteCompra.Desconto.ToString());
                    contrato.Conteudo = contrato.Conteudo.Replace("{Pacote.TipoAula}", pacote.TipoAula.GetType()
                        .GetMember(pacote.TipoAula.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .GetName());
                    contrato.Conteudo = contrato.Conteudo.Replace("{Pacote.PrimeiraAula}", pacoteCompra.Chamadas.OrderBy(c => c.Aula.Data).FirstOrDefault().Aula.Data.ToString("dd/MM/yyyy"));


                    //Matricula
                    var matricula = pacoteCompra.Matricula;
                    contrato.Conteudo = contrato.Conteudo.Replace("{Matricula.Curso}", matricula.Curso.Nome);
                    contrato.Conteudo = contrato.Conteudo.Replace("{Matricula.DataMatricula}", matricula.DataMatricula.ToString("dd/MM/yyyy"));
                    contrato.Conteudo = contrato.Conteudo.Replace("{Matricula.Professor}",(matricula.DispSala is null ? "" : matricula.DispSala.Professor.Nome));
                    contrato.Conteudo = contrato.Conteudo.Replace("{Matricula.Sala}", (matricula.DispSala is null ? "" : matricula.DispSala.Sala.Nome));
                    contrato.Conteudo = contrato.Conteudo.Replace("{Matricula.Hora}", (matricula.DispSala is null ? "" : matricula.DispSala.Hora.ToString("00:'00'h")));

                    //Financeiro
                    var linhas = "";
                    var cont = 1;
                    foreach (var financeiro in pacoteCompra.Financeiros)
                    {
                        if (financeiro.Valor == null) { financeiro.Valor = 0; }
                        if (financeiro.Desconto == null) { financeiro.Desconto = 0; }
                        if (financeiro.Multa == null) { financeiro.Multa = 0; }
                        var total = financeiro.Valor - financeiro.Desconto + financeiro.Multa;
                        linhas += "<tr>" +
                            "<td>" + cont + "</td>" +
                            "<td>" + (financeiro.Valor == 0 ? "" : financeiro.Valor.ToString()) + "</td>" +
                            "<td>" + (financeiro.Desconto == 0 ? "" : financeiro.Desconto.ToString()) + "</td>" +
                            "<td>" + (financeiro.Multa == 0 ? "" : financeiro.Multa.ToString()) + "</td>" +
                            "<td>" + total + "</td>" +
                            "<td>" + (financeiro.ValorPago == null ? "" : financeiro.ValorPago.ToString()) + "</td>" +
                            "<td>" + financeiro.FormaPagamento.GetType()
                            .GetMember(financeiro.FormaPagamento.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName() + "</td>" +
                            "<td>" + (financeiro.DataVencimento == null ? "" : financeiro.DataVencimento.ToString("dd/MM/yyyy")) + "</td>" +
                            "<td>" + (financeiro.DataPagamento == null ? "" : ((DateTime)financeiro.DataPagamento).ToString("dd/MM/yyyy")) + "</td>" +
                            "</tr>";
                        cont++;
                    }
                    var parcelas = "<table>"
                                    + "<thead>"
                                        + "<tr>"
                                            + "<th> Parcela </ th >"
                                            + "<th>Valor</th>"
                                            + "<th> Desconto </ th >"
                                            + "<th>Multa</th>"
                                            + "<th> Total </ th >"
                                            + "<th>Valor Pago</th>"
                                            + "<th> Forma de Pagamento</th>"
                                            + "<th> Data de Vencimento</th>"
                                            + "<th> Data do Pagamento</th>"
                                        + "</tr>"
                                    + "</thead>"
                                    + "<tbody>"
                                        + linhas
                                    + "</tbody>"
                                + "</table>";

                    contrato.Conteudo = contrato.Conteudo.Replace("{Financeiro}", parcelas);

                    ViewBag.AlunoId = pacoteCompra.Matricula.AlunoId;

                    return View(contrato);

                }
                else
                {
                    TempData["Error"] = "Contrato não encontrato";
                    return RedirectToAction("Index", "Aluno");
                }
            }
            else
            {
                TempData["Error"] = "Contrato não encontrato";
                return RedirectToAction("Index", "Aluno");
            }
        }

        public static string ToExtenso(decimal valor)
        {
            if (valor <= 0 | valor >= 1000000000000000)
                return "Valor não suportado pelo sistema.";
            else
            {
                string strValor = valor.ToString("000000000000000.00");
                string valor_por_extenso = string.Empty;

                for (int i = 0; i <= 15; i += 3)
                {
                    valor_por_extenso += escreva_parte(Convert.ToDecimal(strValor.Substring(i, 3)));
                    if (i == 0 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                            valor_por_extenso += " TRILHÃO" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1)
                            valor_por_extenso += " TRILHÕES" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 3 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                            valor_por_extenso += " BILHÃO" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1)
                            valor_por_extenso += " BILHÕES" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 6 & valor_por_extenso != string.Empty)
                    {
                        if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                            valor_por_extenso += " MILHÃO" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1)
                            valor_por_extenso += " MILHÕES" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 9 & valor_por_extenso != string.Empty)
                        if (Convert.ToInt32(strValor.Substring(9, 3)) > 0)
                            valor_por_extenso += " MIL" + ((Convert.ToDecimal(strValor.Substring(12, 3)) > 0) ? " E " : string.Empty);

                    if (i == 12)
                    {
                        if (valor_por_extenso.Length > 8)
                            if (valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "BILHÃO" | valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "MILHÃO")
                                valor_por_extenso += " DE";
                            else
                                if (valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "BILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "MILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 8, 7) == "TRILHÕES")
                                valor_por_extenso += " DE";
                            else
                                    if (valor_por_extenso.Substring(valor_por_extenso.Length - 8, 8) == "TRILHÕES")
                                valor_por_extenso += " DE";

                        if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                            valor_por_extenso += " REAL";
                        else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                            valor_por_extenso += " REAIS";

                        if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && valor_por_extenso != string.Empty)
                            valor_por_extenso += " E ";
                    }

                    if (i == 15)
                        if (Convert.ToInt32(strValor.Substring(16, 2)) == 1)
                            valor_por_extenso += " CENTAVO";
                        else if (Convert.ToInt32(strValor.Substring(16, 2)) > 1)
                            valor_por_extenso += " CENTAVOS";
                }
                return valor_por_extenso;
            }
        }

        static string escreva_parte(decimal valor)
        {
            if (valor <= 0)
                return string.Empty;
            else
            {
                string montagem = string.Empty;
                if (valor > 0 & valor < 1)
                {
                    valor *= 100;
                }
                string strValor = valor.ToString("000");
                int a = Convert.ToInt32(strValor.Substring(0, 1));
                int b = Convert.ToInt32(strValor.Substring(1, 1));
                int c = Convert.ToInt32(strValor.Substring(2, 1));

                if (a == 1) montagem += (b + c == 0) ? "CEM" : "CENTO";
                else if (a == 2) montagem += "DUZENTOS";
                else if (a == 3) montagem += "TREZENTOS";
                else if (a == 4) montagem += "QUATROCENTOS";
                else if (a == 5) montagem += "QUINHENTOS";
                else if (a == 6) montagem += "SEISCENTOS";
                else if (a == 7) montagem += "SETECENTOS";
                else if (a == 8) montagem += "OITOCENTOS";
                else if (a == 9) montagem += "NOVECENTOS";

                if (b == 1)
                {
                    if (c == 0) montagem += ((a > 0) ? " E " : string.Empty) + "DEZ";
                    else if (c == 1) montagem += ((a > 0) ? " E " : string.Empty) + "ONZE";
                    else if (c == 2) montagem += ((a > 0) ? " E " : string.Empty) + "DOZE";
                    else if (c == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TREZE";
                    else if (c == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUATORZE";
                    else if (c == 5) montagem += ((a > 0) ? " E " : string.Empty) + "QUINZE";
                    else if (c == 6) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSEIS";
                    else if (c == 7) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSETE";
                    else if (c == 8) montagem += ((a > 0) ? " E " : string.Empty) + "DEZOITO";
                    else if (c == 9) montagem += ((a > 0) ? " E " : string.Empty) + "DEZENOVE";
                }
                else if (b == 2) montagem += ((a > 0) ? " E " : string.Empty) + "VINTE";
                else if (b == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TRINTA";
                else if (b == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUARENTA";
                else if (b == 5) montagem += ((a > 0) ? " E " : string.Empty) + "CINQUENTA";
                else if (b == 6) montagem += ((a > 0) ? " E " : string.Empty) + "SESSENTA";
                else if (b == 7) montagem += ((a > 0) ? " E " : string.Empty) + "SETENTA";
                else if (b == 8) montagem += ((a > 0) ? " E " : string.Empty) + "OITENTA";
                else if (b == 9) montagem += ((a > 0) ? " E " : string.Empty) + "NOVENTA";

                if (strValor.Substring(1, 1) != "1" & c != 0 & montagem != string.Empty) montagem += " E ";

                if (strValor.Substring(1, 1) != "1")
                    if (c == 1) montagem += "UM";
                    else if (c == 2) montagem += "DOIS";
                    else if (c == 3) montagem += "TRÊS";
                    else if (c == 4) montagem += "QUATRO";
                    else if (c == 5) montagem += "CINCO";
                    else if (c == 6) montagem += "SEIS";
                    else if (c == 7) montagem += "SETE";
                    else if (c == 8) montagem += "OITO";
                    else if (c == 9) montagem += "NOVE";

                return montagem;
            }
        }
    }
}
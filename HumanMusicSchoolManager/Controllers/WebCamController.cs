using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HumanMusicSchoolManager.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace HumanMusicSchoolManager.Controllers
{
    public class WebCamController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public WebCamController(
            IWebHostEnvironment hostingEnvironment,
            ApplicationDbContext context)
        {
            this._environment = hostingEnvironment;
            this._context = context;
        }
        

        [HttpPost]
        public IActionResult Upload(string arquivo, int pessoaId, string rota)
        {
            ViewBag.PessoaId = pessoaId;
            ViewBag.Rota = rota;

            try
            {
                //verifica se existem arquivos
                if (arquivo != null)
                {



                    var pessoa = _context.Pessoas.First(p => p.Id == pessoaId);
                    pessoa.Foto = arquivo;
                    _context.SaveChanges();


                    return RedirectToAction(rota, rota, new { pessoaId });
                }
                else
                {
                    ViewBag.PessoaId = pessoaId;
                    ViewBag.Rota = rota;
                    return View();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Index(int pessoaId, string rota)
        {
            ViewBag.PessoaId = pessoaId;
            ViewBag.Rota = rota;
            return View();
        }

        [HttpPost]
        public IActionResult CapturaImagemPost(int pessoaId)
        {
            try
            {
                //obtem a coleção de arquivos enviada via Post no formulário
                var arquivos = HttpContext.Request.Form.Files;
                //verifica se existem arquivos
                if (arquivos != null)
                {
                    foreach (var arquivo in arquivos)
                    {
                        SalvarArquivo(pessoaId, arquivo);
                    }
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SalvarArquivo(int pessoaId, IFormFile arquivo)
        {
            if (arquivo.Length > 0)
            {
                // obtem o nome do arquivo
                var nomeArquivo = arquivo.FileName;
                // Gera um Guid para definir um arquivo com nome unico
                var nomeArquivoUnico = Convert.ToString(Guid.NewGuid());
                // Obtém a extensão do arquivo
                var arquivoExtensao = Path.GetExtension(nomeArquivo);
                // Concatena o nome do arquivo unico + a extensão
                var novoNomeArquivo = string.Concat(nomeArquivoUnico, arquivoExtensao);
                //  Gera o caminho para armazenar a imagem na pasta criada
                var caminhoArquivo = Path.Combine(_environment.WebRootPath, "ImagensCapturadas")
                + $@"\{novoNomeArquivo}";
                if (!string.IsNullOrEmpty(caminhoArquivo))
                {
                    // armazena a imagem na pasta definida
                    SalvaImagemLocal(arquivo, caminhoArquivo);
                }
                var imagemBytes = System.IO.File.ReadAllBytes(caminhoArquivo);
                if (imagemBytes != null)
                {
                    // salvasr a imagem no banco de dados
                    SalvaImagemDatabase(imagemBytes, pessoaId);
                    //string caminho = "\\ImagensCapturadas\\" + novoNomeArquivo;
                    //SalvaCaminhoImagem(caminho, pessoaId);
                }
                if (!string.IsNullOrEmpty(caminhoArquivo))
                {
                    System.IO.File.Delete(caminhoArquivo);
                }
            }
        }

        private void SalvaImagemLocal(IFormFile arquivo, string nomeArquivo)
        {
            //recebe o arquivo a ser salvo e o seu nome
            using (FileStream fs = System.IO.File.Create(nomeArquivo))
            {
                //copia a imagem na pasta
                arquivo.CopyTo(fs);
                fs.Flush();
            }
        }


        private void SalvaImagemDatabase(byte[] imagemBytes, int pessoaId)
        {
            try
            {
                if (imagemBytes != null)
                {


                    string base64String = Convert.ToBase64String(imagemBytes, 0, imagemBytes.Length);
                    string imagemUrl = string.Concat("data:image/jpg;base64,", base64String);
                    var pessoa = _context.Pessoas.Find(pessoaId);
                    pessoa.Foto = imagemUrl;
                    _context.Pessoas.Update(pessoa);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SalvaCaminhoImagem(string caminho, int pessoaId)
        {
            try
            {
                var pessoa = _context.Pessoas.Find(pessoaId);

                if (pessoa.Foto != null)
                {
                    var caminhoArquivo = _environment.WebRootPath + pessoa.Foto;
                    try
                    {
                        System.IO.File.Delete(caminhoArquivo);
                    }
                    catch
                    {
                        Console.WriteLine("Imagem não encontrada para exclusão: " + pessoa.Foto);
                    }

                }

                pessoa.Foto = caminho;
                _context.Pessoas.Update(pessoa);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
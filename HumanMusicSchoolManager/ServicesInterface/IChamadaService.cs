using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IChamadaService
    {
        void Cadastrar(Chamada chamada);
        void Alterar(Chamada chamada);
        Chamada BuscarPorId(int chamadaId);
        void Excluir(int chamadaId);
        List<Chamada> HistoricoAlunoCurso(Matricula matricula);
        void AtualizarObservacao(int id, string conteudo);
        List<Chamada> HistoricoCompleto(int alunoId, int cursoId);
    }
}

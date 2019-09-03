using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IDemostrativaService
    {
        void Cadastrar(Demostrativa Demostrativa);
        List<Demostrativa> BuscarTodos();
        Demostrativa BuscarPorId(int DemostrativaId);
        void Excluir(int DemostrativaId);
        void Alterar(Demostrativa Demostrativa);
        List<Demostrativa> DemostrativasAbertas();
        List<Demostrativa> DemostrativasNaoContrataram(DateTime inicial, DateTime final);
        void AtualizarObservacao(int id, string conteudo);
        void AtualizarConfirmado(int id, Confirmado confirmado);
    }
}

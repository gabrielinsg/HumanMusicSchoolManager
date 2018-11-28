using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IDemostrativaService
    {
        void Cadastrar(Demostrativa demostrativa);
        void Alterar(Demostrativa demostrativa);
        List<Demostrativa> BuscarTodos();
        Demostrativa BuscarPorId(int demostrativaId);
        void Excluir(int demostrativaId);
    }
}

using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.ServicesInterface
{
    public interface IPessoaService
    {
        List<Pessoa> BuscarTodos();
        Pessoa BuscarPorId(int pessoaId);
        Pessoa Cadastrar(Pessoa pessoa);
    }
}

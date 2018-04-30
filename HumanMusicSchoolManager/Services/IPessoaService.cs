using HumanMusicSchoolManager.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public interface IPessoaService
    {
        List<Pessoa> BuscarTodos();
        Pessoa BuscarPorId(int pessoaId);
    }
}

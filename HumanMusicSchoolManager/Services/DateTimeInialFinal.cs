using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Services
{
    public static class DateTimeEntradaSaida
    {
        public static DateTime Inicial(DateTime inicial)
        {
            inicial = inicial.AddHours(-inicial.Hour);
            inicial = inicial.AddMinutes(-inicial.Minute);
            inicial = inicial.AddMilliseconds(-inicial.Millisecond);
            return inicial;
        }

        public static DateTime Final(DateTime final)
        {
            final = final.AddHours(-final.Hour);
            final = final.AddMinutes(-final.Minute);
            final = final.AddMilliseconds(-final.Millisecond);
            final = final.AddHours(23);
            return final;
        }
    }
}

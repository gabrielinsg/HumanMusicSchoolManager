using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanMusicSchoolManager.Models.Models
{
    public static class NowHorarioBrasilia
    {
        public static DateTime GetNow()
        {
            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo kstZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"); // Brasilia/BRA
            return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, kstZone);
        }
    }
}

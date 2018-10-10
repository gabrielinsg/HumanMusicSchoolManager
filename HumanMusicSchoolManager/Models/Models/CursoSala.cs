namespace HumanMusicSchoolManager.Models.Models
{
    public class CursoSala
    {
        public int CursoId { get; set; }
        public int SalaId { get; set; }
        public Curso Curso { get; set; }
        public Sala Sala { get; set; }
    }
}
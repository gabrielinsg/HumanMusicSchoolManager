namespace HumanMusicSchoolManager.Models.Models
{
    public class CursoSala
    {
        public int CursoId { get; set; }
        public int SalaId { get; set; }
        public virtual Curso Curso { get; set; }
        public virtual Sala Sala { get; set; }
    }
}
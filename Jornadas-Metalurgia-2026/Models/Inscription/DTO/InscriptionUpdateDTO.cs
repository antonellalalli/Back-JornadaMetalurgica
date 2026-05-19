namespace Jornadas_Metalurgia_2026.Models.Inscription.DTO
{
    public class InscriptionUpdateDTO
    {
        public string InscriptionType { get; set; }
        public string StudentEmail { get; set; }

        public string PresentationTitle { get; set; }
        public string Presentation { get; set; }
        public List<string>? Participants { get; set; }

    }
}

using SQLite;

namespace Scores.Models
{
    public class Equipe
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public int NbTotalMatchs { get; set; }
        public int NbTotalPoints { get; set; }
        public int TotalScores { get; set; } // Nb total des scores marqués par l'équipe
        public int TotalScoresAdversaires { get; set; } // Nb total des scores marqués par tous les équipes adversaires
    }
}

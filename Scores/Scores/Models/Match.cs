using SQLite;
using System;

namespace Scores.Models
{
    public class Match
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdEquipeHome { get; set; }
        public int IdEquipeAway { get; set; }
        public int ScoreEquipeHome { get; set; }
        public int ScoreEquipeAway { get; set; }
        public DateTime DateMatch { get; set; }

        [Ignore]
        public string NomEquipeHome { get; set; }
        [Ignore]
        public string NomEquipeAway { get; set; }
    }
}

using SQLite;

namespace PlacesFavoris.Models
{
    public class Configuration
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string IdUtilisateur { get; set; }
        public bool IsSwitchLieuxConnusToggled { get; set; }
        public bool IsSwitchLieuxSouhaitesToggled { get; set; }
        public bool IsSwitchLieuxVisitesToggled { get; set; }
        public double DegreLatitude { get; set; }
        public double DegreLongitude { get; set; }
    }
}

using SQLite;

namespace PlacesFavoris.Models
{
    public class Lieu
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string IdUtilisateur { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public Categorie? Categorie { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsCategorieToggled { get; set; }
    }
}

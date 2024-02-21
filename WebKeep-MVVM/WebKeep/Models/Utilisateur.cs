using SQLite;

namespace WebKeep.Models
{
    public class Utilisateur
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NomUtilisateur { get; set; }
        public string MotDePasse { get; set; }
    }
}

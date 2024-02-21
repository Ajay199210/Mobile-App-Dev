using SQLite;
using System;

namespace WebKeep.Models
{
    public class Site
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdUtilisateur { get; set; }
        public string Nom { get; set; }
        public string Lien { get; set; }
        public string Note { get; set; }
        public DateTime DateCreation { get; set; }
    }
}

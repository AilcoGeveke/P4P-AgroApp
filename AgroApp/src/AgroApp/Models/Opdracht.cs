using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Opdracht
    {
        public Customer selectedKlant;
        public User[] selectedGebruikers;
        public string locatie;
        public string beschrijving;
        public DateTime? datum;
        public int idOpdracht;

        public Opdracht(int idOpdracht, string locatie, string beschrijving, DateTime? datum, Customer selectedKlant = null)
        {
            this.idOpdracht = idOpdracht;
            this.locatie = locatie;
            this.beschrijving = beschrijving;
            this.datum = datum;
            this.selectedKlant = selectedKlant;
        }
    }
}

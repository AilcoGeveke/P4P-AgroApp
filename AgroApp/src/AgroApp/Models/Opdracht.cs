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
        public DateTime datum;
        public int idOpdracht;
        public int idKlant;

        public Opdracht(int idOpdracht, string locatie, string beschrijving, int idKlant, DateTime datum)
        {
            this.idOpdracht = NullCheck.CastDBValue<int>(idOpdracht);
            this.locatie = NullCheck.CastDBValue<string>(locatie);
            this.beschrijving = NullCheck.CastDBValue<string>(beschrijving);
            this.idKlant = NullCheck.CastDBValue<int>(idKlant);
            this.datum = NullCheck.CastDBValue<DateTime>(datum);
        }
    }
}

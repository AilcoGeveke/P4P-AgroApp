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
    }
}

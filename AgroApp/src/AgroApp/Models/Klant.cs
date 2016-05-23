using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Klant
    {

        public int IdKlant { private set; get; }
        public string Naam { private set; get; }
        public string Adres { private set; get; }

        public Klant(int idKlant = -1, string naam = "", string adres = "")
        {
            IdKlant = idKlant;
            Naam = naam;
            Adres = adres;
        }
    }
}
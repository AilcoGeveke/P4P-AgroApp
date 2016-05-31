using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Hulpstuk
    {
        public int IdHulpstuk { private set; get; }
        public string Type { private set; get; }
        public int Nummer { private set; get; }
        public string Naam { private set; get; }

        public Hulpstuk(int idHulpstuk = -1, string type = "", int nummer = 0, string naam = "")
        {
            IdHulpstuk = idHulpstuk;
            Type = type;
            Nummer = nummer;
            Naam = naam;
        }
    }
}

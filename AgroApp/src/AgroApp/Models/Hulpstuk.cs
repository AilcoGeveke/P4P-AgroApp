using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Hulpstuk
    {
        public int IdHulpstuk { set; get; }
        public int Nummer { set; get; }
        public string Naam { set; get; }


        public Hulpstuk(int idHulpstuk = -1, int nummer = 0, string naam = "")
        {
            IdHulpstuk = idHulpstuk;
            Nummer = nummer;
            Naam = naam;
        }
    }
}

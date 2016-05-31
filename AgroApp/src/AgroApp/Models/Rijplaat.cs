using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Rijplaat
    {
        public int IdRijplaat { private set; get; }
        public int Groot { private set; get; }
        public int Klein { private set; get; }
        public int Kunstof { private set; get; }

        public Rijplaat(int idRijplaat = -1, int groot = 0, int klein = 0, int kunstof = 0)
        {
            IdRijplaat = idRijplaat;
            Groot = groot;
            Klein = klein;
            Kunstof = kunstof;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Gewicht
    {
        public int IdGewicht { private set; get; }
        public string Type { private set; get; }
        public int VolGewicht { private set; get; }
        public int LeegGewicht { private set; get; }
        public int NettoGewicht { private set; get; }
        public string Richting { private set; get; }
        public int IdWerktijd { private set; get; }

        public Gewicht(int idGewicht = -1, string type = "", int volGewicht = 0, int leegGewicht = 0, int nettoGewicht = 0, string richting = "", int idWerktijd = -1)
        {
            IdGewicht = idGewicht;
            Type = type;
            VolGewicht = volGewicht;
            LeegGewicht = leegGewicht;
            NettoGewicht = nettoGewicht;
            Richting = richting;
            IdWerktijd = idWerktijd;
        }
    }
}

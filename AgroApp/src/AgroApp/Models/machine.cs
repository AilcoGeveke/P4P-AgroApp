using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Machine { 

        public int IdMachine { private set; get; }
        public string Type { private set; get; }
        public int Nummer { private set; get; }
        public string Naam { private set; get; }
        public string Kenteken { private set; get; }
        public string Status { private set; get; }

        public Machine(int idMachine, string type, int nummer, string naam, string kenteken, string status)
        {
            IdMachine = idMachine;
            Type = type;
            Nummer = nummer;
            Naam = naam;
            Kenteken = kenteken;
            Status = status;
        }

    }
}

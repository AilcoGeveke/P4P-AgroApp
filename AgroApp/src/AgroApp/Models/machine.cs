using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Machine { 

        public int IdMachine { private set; get; }
        public string Type { private set; get; }
        public int Number { private set; get; }
        public string Name { private set; get; }
        public string Tag { private set; get; }
        public string Status { private set; get; }
        
        public Machine(int idMachine = -1, string type = "", int number = 0, string name="", string tag="", string status="")
        {
            IdMachine = idMachine;
            Type = type;
            Number = number;
            Name = name;
            Tag = tag;
            Status = status;
        }
    }
}

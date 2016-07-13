using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Machine
    {
        public int IdMachine { set; get; }
        public string Type { set; get; }
        public int Number { set; get; }
        public string Name { set; get; }
        public string Tag { set; get; }
        public string Status { set; get; }

        public Machine() { }

        public Machine(int idMachine = -1, string type = "", int number = 0, string name = "", string tag = "", string status = "")
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

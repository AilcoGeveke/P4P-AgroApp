using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Cargo
    {
        public int IdCargo { set; get; }
        public long Date { set; get; }
        public string Type { set; get; }
        public int FullLoad { set; get; }
        public int EmptyLoad { set; get; }
        public int NetLoad { set; get; }
        public string Direction { set; get; }

        public Cargo() { }

        public Cargo(int idCargo, long date, string type, int fullLoad, int emptyLoad, int netLoad, string direction)

        {
            IdCargo = idCargo;
            Date = date;
            Type = type;
            FullLoad = fullLoad;
            EmptyLoad = emptyLoad;
            NetLoad = netLoad;
            Direction = direction;
        }
    }
}

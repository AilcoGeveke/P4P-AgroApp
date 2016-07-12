using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Cargo
    {
        public int IdCargo { private set; get; }
        public int IdTimesheetPart { private set; get; }
        public DateTime? Date { private set; get; }
        public string Type { private set; get; }
        public int FullLoad { private set; get; }
        public int EmptyLoad { private set; get; }
        public int NetLoad { private set; get; }
        public string Direction { private set; get; }

        public Cargo(int idCargo = -1, int idTimesheetPart = -1, DateTime? date = null, string type = "", int fullLoad = 0, int emptyLoad = 0, int netLoad = 0, string direction = "")
        {
            IdCargo = idCargo;
            IdTimesheetPart = idTimesheetPart;
            Type = type;
            FullLoad = fullLoad;
            EmptyLoad = emptyLoad;
            NetLoad = netLoad;
            Direction = direction;
        }
    }
}

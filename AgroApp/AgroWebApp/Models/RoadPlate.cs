using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class RoadPlate
    {
        public int IdRoadplate { set; get; }
        public int Small { set; get; }
        public int Large { set; get; }
        public int Plastic { set; get;}
        public string Direction { get; set; }

        public RoadPlate() { }

        public RoadPlate(int idRoadplate, int small, int large, int plastic, string direction)
        {
            IdRoadplate = idRoadplate;
            Small = small;
            Large = large;
            Plastic = plastic;
            Direction = direction;
        }
    }
}

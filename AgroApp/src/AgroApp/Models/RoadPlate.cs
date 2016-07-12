using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class RoadPlate
    {
        public int IdRoadPlate { private set; get; }
        public int Large { private set; get; }
        public int Small { private set; get; }
        public int Plastic { private set; get; }
        public string Direction { private set; get; }

        public RoadPlate(int idRoadPlate = -1, int large = 0, int small = 0, int plastic = 0, string direction = "")
        {
            IdRoadPlate = idRoadPlate;
            Large = large;
            Small = small;
            Plastic = plastic;
            Direction = direction;
        }
    }
}

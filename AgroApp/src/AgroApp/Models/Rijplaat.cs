using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class RoadPlate
    {
        public int idRoadPlate { private set; get; }
        public int large { private set; get; }
        public int small { private set; get; }
        public int plastic { private set; get; }
        public string direction { private set; get; }

        public RoadPlate(int idroadplate = -1, int Large = 0, int Small = 0, int Plastic = 0, string Direction = "")
        {
            idRoadPlate = idroadplate;
            large = Large;
            small = Small;
            plastic = Plastic;
            direction = Direction;
        }
    }
}

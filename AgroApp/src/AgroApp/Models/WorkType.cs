using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class WorkType
    {
        public int IdWorkType { private set; get; }
        public string Type { private set; get; }

        public WorkType(int idWorkType = -1, string type = "")
        {
            IdWorkType = idWorkType;
            Type = type;
        }
    }
}

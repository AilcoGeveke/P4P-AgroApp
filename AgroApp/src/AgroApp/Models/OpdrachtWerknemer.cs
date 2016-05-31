using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class OpdrachtWerknemer
    {
        public int idOpdrachtWerknemer {set; get; }
        public Opdracht Opdracht {set; get; }
        public User Werknemer {set; get; }

    }
}

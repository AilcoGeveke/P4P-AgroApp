using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Attachment
    {
        public int IdAttachment { private set; get; }
        public int Number { private set; get; }
        public string Name { private set; get; }


        public Attachment(int idAttachment = -1, int number = 0, string name = "")
        {
            IdAttachment = idAttachment;
            Number = number;
            Name = name;
        }
    }
}

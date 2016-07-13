using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Attachment
    {
        public int IdAttachment { set; get; }
        public int Number { set; get; }
        public string Name { set; get; }

        public Attachment() { }

        public Attachment(int idAttachment, int number, string name)
        {
            IdAttachment = idAttachment;
            Number = number;
            Name = name;
        }
    }
}

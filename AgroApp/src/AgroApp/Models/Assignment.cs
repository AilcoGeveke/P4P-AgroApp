using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Assignment
    {
        public Customer Customer;
        public User[] Users;
        public string Location;
        public string Description;
        public DateTime? Date;
        public int idOpdracht;

        public int gebruikerCount;

        public Assignment(int idOpdracht, string locatie, string beschrijving, DateTime? datum, Customer selectedKlant = null)
        {
            this.idOpdracht = idOpdracht;
            this.Location = locatie;
            this.Description = beschrijving;
            this.Date = datum;
            this.Customer = selectedKlant;
        }
    }
}

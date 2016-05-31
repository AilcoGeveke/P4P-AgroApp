using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Werkbon
    {
        public User Gebruiker { private set; get; }
        public DateTime Datum { private set; get; }
        public Customer Klant { private set; get; }
        public string Mankeuze { private set; get; }
        public Machine[] Machines { private set; get; }
        public Hulpstuk[] Hulpstukken { private set; get; }
        public DateTime VanTijd { private set; get; }
        public DateTime TotTijd { private set; get; }
        public int TotaalTijd { private set; get; }
        public Gewicht[] Gewichten { private set; get; }
        public Rijplaat IngaandeRijplaten { private set; get; }
        public Rijplaat UitgaandeRijplaten { private set; get; }
        public string VerbruikteMaterialen { private set; get; }
        public string Opmerking { private set; get; }
        public int IdOpdracht { private set; get; }

        public Werkbon(User selectedGebruiker, DateTime datum, Customer klant, string mankeuze, int totaalTijd, Machine[] machines = null, Hulpstuk[] hulpstukken = null, DateTime vanTijd = new DateTime(), DateTime totTijd = new DateTime(), Gewicht[] gewichten = null, Rijplaat ingaandeRijplaten = null, Rijplaat uitgaandeRijplaten = null, string verbruikteMaterialen = "", string opmerking = "")
        {
            Gebruiker = selectedGebruiker;
            Datum = datum;
            Klant = klant;
            Mankeuze = mankeuze;
            Machines = machines;
            Hulpstukken = hulpstukken;
            VanTijd = vanTijd;
            TotTijd = totTijd;
            TotaalTijd = totaalTijd;
            Gewichten = gewichten;
            IngaandeRijplaten = ingaandeRijplaten;
            UitgaandeRijplaten = uitgaandeRijplaten;
            VerbruikteMaterialen = verbruikteMaterialen;
            Opmerking = opmerking;
        }
    }
}

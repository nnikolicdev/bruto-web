using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bruto_web.Models
{
    public class BrutoViewModel
    {
        public int Neto { get; set; }
        public int Bruto { get; set; }

        public int PIO { get; set; }

        // Zdravstveno Osiguranje
        public int ZO { get; set; }

        // Osiguranje od nezaposlenosti
        public int ODN { get; set; }

        public int Tax { get; set; }

        public int Total { get; set; }

        public int WithoutTax { get; set; }

        public BrutoViewModel(
            int neto,
            int bruto, 
            double pio, 
            double zo, 
            double odn,
            double porez,
            int total) 
        {
            Neto = neto;
            Bruto = bruto;
            PIO = (int) Math.Round(pio);
            ZO = (int) Math.Round(zo);
            ODN = (int) Math.Round(odn);
            Tax = (int) Math.Round(porez);
            Total = total;
            WithoutTax = total - Tax;
        }

    }
}

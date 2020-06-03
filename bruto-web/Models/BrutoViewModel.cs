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

        public int Total { get; set; }

        public BrutoViewModel(int neto, int bruto, double pio, double zo, double odn) 
        {
            Neto = neto;
            Bruto = bruto;
            PIO = (int) Math.Round(pio);
            ZO = (int) Math.Round(zo);
            ODN = (int) Math.Round(odn);
            Total = PIO + ZO + ODN;
        }

    }
}

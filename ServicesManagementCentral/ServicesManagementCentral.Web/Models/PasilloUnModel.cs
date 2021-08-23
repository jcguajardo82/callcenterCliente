using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class PasilloUnModel
    {

        public int Id_Num_UN { get; set; }
        public int Id_Cnsc_Pasillo { get; set; }
        public int Id_Num_PasilloTipo { get; set; }
        public string Nom_PasilloTipo { get; set; }
        public int Id_Num_PasilloLado { get; set; }
        public string Nom_PasilloLado { get; set; }
        public string Nom_Pasillo { get; set; }
        public int Num_Orden { get; set; }
        public DateTime Fec_Movto { get; set; }
    }
}
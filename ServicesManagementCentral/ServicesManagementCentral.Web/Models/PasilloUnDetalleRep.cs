using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class PasilloUnDetalleRep
    {
        public string Desc_Div { get; set; }
        public string Num_OrdenPasillo { get; set; }
        public string Nom_Pasillo { get; set; }
        public int Id_Num_Categ { get; set; }
        public string Desc_Categ { get; set; }
        public int Num_OrdenLinea { get; set; }
        public int Id_Num_Linea { get; set; }
        public string Desc_Linea { get; set; }
 
    }
}
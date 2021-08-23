using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class PasilloUnLinea
    {
        public int Id_Num_Linea { get; set; }
        public string Desc_Linea { get; set; }
        public int Id_Num_Categ { get; set; }
        public string Desc_Categ { get; set; }
        public int Id_Num_Div { get; set; }
        public string Desc_Div { get; set; }
        public int Num_Orden { get; set; }
    }
}
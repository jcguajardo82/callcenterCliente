using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class LineaModel
    {
        public int Id_Num_Linea { get; set; }
        public string Desc_Linea { get; set; }
        public int Id_Num_Categ { get; set; }
        public int Id_Num_Div { get; set; }
    }
}
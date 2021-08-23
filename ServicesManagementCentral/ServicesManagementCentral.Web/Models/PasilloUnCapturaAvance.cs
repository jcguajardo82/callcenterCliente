using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class PasilloUnCapturaAvance
    {
        public string tienda { get; set; }
        public string division { get; set; }
        public string categoria { get; set; }
        public string numLinea { get; set; }
        public string nombreLinea { get; set; }
        public string statusLinea { get; set; }
        public string nombrePasillo { get; set; }
    }
}
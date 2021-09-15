using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class Archivos
    {
        public string OrdenRma { get; set; }
        public List<LstArchivos> archivos { get; set; }
        
    }

    public class LstArchivos
    {
        public string filename { get; set; }
        public string mimetype { get; set; }
        public string content { get; set; }

    }
}
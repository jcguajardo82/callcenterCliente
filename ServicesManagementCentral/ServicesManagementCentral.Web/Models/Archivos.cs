using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class Archivos
    {
        public string orderRMA { get; set; }
        public List<LstArchivos> archivo { get; set; }
        
    }

    public class LstArchivos
    {
        public string filename { get; set; }
        public string mimetype { get; set; }
        public string content { get; set; }

    }
}
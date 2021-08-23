using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class MotivoCambioFP
    {
        public int Id_Num_MotCmbFP { get; set; }
        public string Desc_MotCmbFP { get; set; }
        public bool Bit_Eliminado { get; set; }
        public DateTime Fec_Movto { get; set; }
    }
}
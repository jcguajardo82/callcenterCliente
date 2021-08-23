using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class CarrierModel
    {

        public Int32 Id_Carrier { get; set; }//int
        public string Name { get; set; }//varchar
        public Int32 Id_Num_UN { get; set; }//int
        public DateTime Fec_Movto { get; set; }//smalldatetime
        public Boolean Activo { get; set; }//bit

    }
}
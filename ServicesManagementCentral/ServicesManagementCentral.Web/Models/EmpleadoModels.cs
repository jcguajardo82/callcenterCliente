using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class EmpleadoModels
    {

        public string Num_Empl { get; set; }
        public string Ids_Num_UN { get; set; }
        public string Id_Num_DptoNomina { get; set; }
        public string Id_Num_PuestoNomina { get; set; }
        public string Desc_PuestoNomina { get; set; }
        public string Apellido_Paterno { get; set; }
        public string Apellido_Materno { get; set; }
        public string Nombre { get; set; }
        public string FecX_Ingreso { get; set; }
        public string FecX_Nacimiento { get; set; }
        public string Bit_Activo { get; set; }

    }
}
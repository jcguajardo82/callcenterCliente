using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class PasilloUnReporteMapModel
    {
        public List<DivisionModel> Division{ get; set; }
        public List<CategoriaModel> Categoria { get; set; }
        public List<PasilloUnDetalleRep> PasilloDetalle { get; set; }
        public List<PasilloUnRepPasillo> Pasillo { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class PasilloUnGuia
    {

        public string Guia { get; set; }
        public string Chofer { get; set; }
        public string FecAsig { get; set; }
        public string NumOrden { get; set; }
        public string FecEntrega { get; set; }
        public string Direccion { get; set; }
        public string FormaPago { get; set; }
        public string Cuenta { get; set; }

        public string NomClieTar { get; set; }
        public string Expira { get; set; }
        public string Banco { get; set; }

        public string NoAfileacion { get; set; }
        public string ImpTot { get; set; }
        public string Contenedores { get; set; }
        public string Bolsas { get; set; }
        public string Hieleras { get; set; }
        public string TermPort { get; set; }
        public string Cliente { get; set; }


        public List<PasilloUnGuiaArt> ArtSurtidos { get; set; }
        public List<PasilloUnGuiaArt> ArtNoSurtidos { get; set; } 
    }

    public class PasilloUnGuiaArt
    {
        public string Num { get; set; }
        public string Sku { get; set; }
        public string CodBar { get; set; }
        public string Desc { get; set; }
        public string CantPedida { get; set; }
        public string CantSurtida { get; set; }
        public string CantCobrada { get; set; }
        public string PrecioNormal { get; set; }
        public string PrecioOferta { get; set; }
        public string PrecioCobrado { get; set; }
        public string TotalPedido { get; set; }
        public string TotalSurtido { get; set; }
        public string TotalCobrado { get; set; }
    }
}
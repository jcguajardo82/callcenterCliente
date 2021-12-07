using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class LogytRequestModel
    {
        public string Courier { get; set; } = "EST";
        public string Reference { get; set; } = "Referencia";
        public int NumberLabels { get; set; } = 1;
        public decimal Volume { get; set; } = 1;
        //Peso
        public decimal Weight { get; set; } = 1;
        public string Content { get; set; } = "Documentos";
        public bool CustomerPickUp { get; set; } = false;
        public string AditionalInfo { get; set; } = "Informacion adicional";
        public string ServiceType { get; set; } = "70";
        public LogytAddressModel Destination { get; set; }
        public LogytAddressModel Origin { get; set; }

    }
    public class LogytResponseModels
    {
        public string Error { get; set; }
        public string[] Messages { get; set; }
        public string Reference { get; set; }
        public Labels[] Labels { get; set; }
    }

    public class Labels
    {
        public string[] Folios { get; set; }
        public byte[] PDF { get; set; }
    }


    public class LogytAddressModel
    {


        public string Address1 { get; set; } = "public string addr1";
        public string Address2 { get; set; } = "public string Addr2";
        public string City { get; set; } = "Ciudad";
        public string ContactName { get; set; } = "Cliente";
        public string CorporateName { get; set; } = "Corporate";
        public string CustomerNumber { get; set; } = "1234568";
        public string Neighborhood { get; set; } = "neighborhood";
        public string PhoneNumber { get; set; } = "1111111";
        public string State { get; set; } = "Mexico";
        //Código postal destino
        public string ZipCode { get; set; } = "01000";


    }
}
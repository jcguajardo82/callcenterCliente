using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class ResponseModels
    {

        public string Cve_RespCode { get; set; } = "00";
        public string Guia { get; set; } = "";
        public string Desc_MensajeError { get; set; } = "";
        public byte[] pdf { get; set; }
    }

    public class EstafetaRequestModel
    {

        public string aditionalInfo { get; set; } = "Informacion adicional";
        //Contenido
        public string content { get; set; } = "Contenido";
        //Centro de costos
        public string costCenter { get; set; } = "CCtos";
        //Ocurre
        public bool deliveryToEstafetaOffice { get; set; } = false;
        //En caso de envio a otro pais, solo siglasWS de generación de guías Estafeta Versión 2 Fecha: 31/01/2011
        //Estafeta Mexicana S.A.de C.V. 36
        public string destinationCountryId { get; set; } = "MX";
        //Tipo de envio 1{get; set;} =SOBRE 4{get; set;} =PAQUETE
        public int parcelTypeId { get; set; } = 4;
        //Referencia
        public string reference { get; set; } = "Referencia";
        //Peso
        public int weight { get; set; } = 1;
        //Número de etiquetas solicitadas
        public int numberOfLabels { get; set; } = 1;
        //Código postal de Origen para enrutamiento
        public string originZipCodeForRouting { get; set; } = "62250";
        //Servicio que se usará
        public string serviceTypeId { get; set; } = "70";
        //Numero de oficina que corresponde al cliente
        public string officeNum { get; set; } = "421";
        //Documento de retorno
        public bool returnDocument { get; set; } = true;
        //Servicio del documento de retorno
        public string serviceTypeIdDocRet { get; set; } = "50";
        //Fecha de vigencia
        public string effectiveDate { get; set; } = "20110525";
        //Descripcion del contenido
        public string contentDescription { get; set; } = "Descripcion del contenido del paquete";

        public AddressModel DestinationInfo { get; set; }

        public AddressModel OriginInfo { get; set; }

    }


    public class AddressModel
    {


        public string address1 { get; set; } = "public string addr1";
        public string address2 { get; set; } = "public string Addr2";
        public string city { get; set; } = "Ciudad";
        public string contactName { get; set; } = "Cliente";
        public string corporateName { get; set; } = "Corporate";
        public string customerNumber { get; set; } = "1234568";
        public string neighborhood { get; set; } = "neighborhood";
        public string phoneNumber { get; set; } = "1111111";
        public string cellPhone { get; set; } = "0447777777777";
        public string state { get; set; } = "Mexico";
        //Código postal destino
        public string zipCode { get; set; } = "01000";


    }
}
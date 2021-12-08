using ServicesManagement.Web.DAL;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using ServicesManagement.Web.DAL.Embarques;
using System.Configuration;

namespace ServicesManagement.Web.Controllers
{
    /// <summary>
    /// home controller
    /// </summary>
    public class HomeController : Controller
    {
        public HomeController() { }

        public ActionResult Index()
        {

            int OrderId = 0;
            if (Request.QueryString["order"] != null)
            {
                OrderId = int.Parse(Request.QueryString["order"].ToString());
                //---
                var esVIG = bool.Parse((DALCallCenter.verificacion_link_sUP(OrderId).Tables[0].Rows[0][0].ToString()));
                //---
                if (esVIG)
                {
                    var ds = (DALCallCenter.OrderFacts_ArticulosRMA(OrderId));               
                
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.Order = DataTableToModel.ConvertTo<Order>(ds.Tables[0]).FirstOrDefault();
                    //validar si tiene guía FOR
                    ViewBag.Products = DataTableToModel.ConvertTo<Product>(ds.Tables[1]);
                    ViewBag.Detail = DataTableToModel.ConvertTo<Detail>(ds.Tables[2]).FirstOrDefault();
                }
                }
            }
            ViewBag.OrderId = OrderId;

            return View();
        }

        public FileResult GenerarGuiaDevolucion(string UeNo, int Product, int Pieces)
        {
            string tarifa = string.Empty, guia = string.Empty, servicioPaq = string.Empty, noguia = string.Empty, pdffile = string.Empty; 
            decimal pesoCalculado = 0; 
            try
            {
                DataSet ds = DALEmbarques.upCorpOms_Cns_UeNoTrackingInfo(UeNo, Product);
                List<TrackingInfo> lstTrackingInfo = DataTableToModel.ConvertTo<TrackingInfo>(ds.Tables[0]);
                if (lstTrackingInfo.Count > 0)
                {
                    //[upCorpOms_Cns_UeNoTrackingInfoExist]
                    foreach (var item in lstTrackingInfo)
                    {
                        #region Guias
                        var FolioDisp = DALEmbarques.upCorpOms_Cns_NextTracking().Tables[0].Rows[0]["NextTracking"].ToString();

                        int type = 1;

                        if (item.PackageType.Equals("CJA") || item.PackageType.Equals("EMB") || item.PackageType.Equals("STC"))
                            type = 4;

                        pesoCalculado = PesoCalculado(Product, Pieces);
                        int peso = decimal.ToInt32(pesoCalculado);

                        guia = CreateGuiaEstafeta(UeNo, item.OrderNo, peso, type);

                        var datos = guia.Split(',');
                        noguia = datos[0];
                        pdffile = datos[1];

                        servicioPaq = item.TrackingServiceName; //esta variable sera dinamica

                        DataSet TiE = DALEmbarques.upCorpOms_Cns_UeNoTrackingInfoExist(UeNo);
                        string TipoSeguimiento = DALEmbarques.upCorpOms_Cns_UeNoTrackingInfoExist(UeNo).Tables[0].Rows[0]["TrackingType"].ToString();
                        if (TipoSeguimiento != "DEVOLUCION")
                        {

                            string GuiaEstatus = "CREADA";

                            var cabeceraGuia = DALEmbarques.upCorpOms_Ins_UeNoTracking(UeNo, item.OrderNo, FolioDisp, "DEVOLUCION",
                            item.PackageType, item.PackageLength, item.PackageWidth, item.PackageHeight, item.PackageWeight,
                            User.Identity.Name, servicioPaq, guia.Split(',')[0], guia.Split(',')[1], GuiaEstatus, null).Tables[0].Rows[0][0];

                            #endregion
                            DALEmbarques.upCorpOms_Ins_UeNoTrackingDetail(UeNo, item.OrderNo, FolioDisp, "DEVOLUCION",
                                Product, item.Barcode, item.ProductName, User.Identity.Name);
                        }
                        //else
                        //{
                        //    noguia = DALEmbarques.upCorpOms_Cns_UeNoTrackingInfoExist(UeNo).Tables[0].Rows[0]["IdTrackingService"].ToString();
                        //    //Convert.ToBase64String((byte[])(item["FotoURL"]))
                        //    byte[] data = (byte[])(DALEmbarques.upCorpOms_Cns_UeNoTrackingInfoExist(UeNo).Tables[0].Rows[0]["pdf"]);
                        //    pdffile = Convert.ToBase64String(data);
                        //}
                    }
                    byte[] FileBytes = Convert.FromBase64String(pdffile);
                    return File(FileBytes, "application/pdf", "GuiaDevolucion" + Product.ToString() + ".pdf");
                }
                else
                {
                    string name = "NoSeEncontroInfo.txt";
                    return File(name, "text/plain");
                }
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return File("", "application/pdf");
                //return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        private decimal PesoCalculado(int Product, decimal Quantity)
        {
            decimal sumPeso = 0;

            List<WeightByProducts> lstPesos = DataTableToModel.ConvertTo<WeightByProducts>(DALServicesM.GetDimensionsByProducts(Product.ToString()).Tables[0]);

            foreach (var item in lstPesos)
            {
                if (item.PesoVol > item.Peso)
                {
                    sumPeso = sumPeso + (item.PesoVol * Quantity);
                }
                else
                {
                    sumPeso = sumPeso + (item.Peso * Quantity);
                }

            }

            if (sumPeso < 1)
                sumPeso = 1;

            return sumPeso;

        }
        public string CreateGuiaEstafeta(string UeNo, int OrderNo, int weight, int typeId)
        {
            var ServiceTypeId = 1;
            DataSet ds = new DataSet();
            DataSet dsO = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@UeNo", UeNo);
                parametros.Add("@OrderNo", OrderNo);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Sel_EstafetaInfo]", false, parametros);



                System.Collections.Hashtable parametros2 = new System.Collections.Hashtable();
                parametros2.Add("@UeNo", UeNo);


                dsO = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_UeNoOriginInfo]", false, parametros2);

            }
            catch (SqlException ex)
            {
                return "ERRSQL";
            }
            catch (System.Exception ex)
            {
                return "ERR";
            }

            EstafetaRequestModel m = new EstafetaRequestModel();
            foreach (DataRow r in dsO.Tables[0].Rows)
            {


                m.DestinationInfo = new AddressModel();

                m.DestinationInfo.address1 = r["address1"].ToString();
                m.DestinationInfo.address2 = r["address2"].ToString();
                m.DestinationInfo.cellPhone = r["cellPhone"].ToString();
                m.DestinationInfo.city = r["city"].ToString();
                m.DestinationInfo.contactName = r["contactName"].ToString();
                m.DestinationInfo.corporateName = r["corporateName"].ToString();
                m.DestinationInfo.customerNumber = r["customerNumber"].ToString();
                m.DestinationInfo.neighborhood = r["neighborhood"].ToString();
                m.DestinationInfo.phoneNumber = r["phone"].ToString();
                m.DestinationInfo.state = r["state"].ToString();
                m.DestinationInfo.zipCode = r["zipCode"].ToString();

            }

            foreach (DataRow r in ds.Tables[0].Rows)
            {

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //m.serviceTypeId = "60";
                //m.serviceTypeId = System.Configuration.ConfigurationManager.AppSettings["val_serviceTypeId"];
                m.serviceTypeId = r["ServiceType"].ToString();
                if (weight >= 70)
                {
                    m.serviceTypeId = "L0";
                }


                m.OriginInfo = new AddressModel();

                m.OriginInfo.address1 = r["Address1"].ToString();
                m.OriginInfo.address2 = r["Address2"].ToString();
                m.OriginInfo.cellPhone = r["Phone"].ToString();
                m.OriginInfo.city = r["City"].ToString();
                m.OriginInfo.contactName = r["CustomerName"].ToString();
                //m.OriginInfo.corporateName = r["CustomerName"].ToString();
                m.OriginInfo.corporateName = r["UeNo"].ToString();
                m.OriginInfo.customerNumber = r["CustomerNo"].ToString();
                m.OriginInfo.neighborhood = r["NameReceives"].ToString();
                m.OriginInfo.phoneNumber = r["Phone"].ToString();
                m.OriginInfo.state = r["StateCode"].ToString();
                m.OriginInfo.zipCode = r["PostalCode"].ToString();

                m.reference = r["Reference"].ToString();
                m.originZipCodeForRouting = r["PostalCode"].ToString();
                m.weight = weight; // lo capturado en el modal
                m.parcelTypeId = typeId; // 1 - sobre, 4 - paquete
                m.effectiveDate = r["effectiveDate"].ToString();

                string json2 = JsonConvert.SerializeObject(m);

                Soriana.FWK.FmkTools.RestResponse r2 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_Estafeta_Guia"], "", json2);

                string msg = r2.message;

                ResponseModels re = JsonConvert.DeserializeObject<ResponseModels>(r2.message);

                string pdfcadena2 = Convert.ToBase64String(re.pdf, Base64FormattingOptions.None);

                //return re.Guia + "," + re.pdf;
                return re.Guia + "," + pdfcadena2;

            }

            return string.Empty;

        }
        public string CreateGuiaLogyt(string UeNo, int OrderNo, int weight, int typeId)
        {
            var ServiceTypeId = 1;
            DataSet ds = new DataSet();
            DataSet dsO = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@UeNo", UeNo);
                parametros.Add("@OrderNo", OrderNo);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Sel_EstafetaInfo]", false, parametros);



                System.Collections.Hashtable parametros2 = new System.Collections.Hashtable();
                parametros2.Add("@UeNo", UeNo);


                dsO = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_UeNoOriginInfo]", false, parametros2);

            }
            catch (SqlException ex)
            {
                return "ERRSQL";
            }
            catch (System.Exception ex)
            {
                return "ERR";
            }

            LogytRequestModel m = new LogytRequestModel();
            foreach (DataRow r in dsO.Tables[0].Rows)
            {


                m.Destination = new LogytAddressModel();

                m.Destination.Address1 = r["address1"].ToString();
                m.Destination.Address2 = r["address2"].ToString();
                m.Destination.City = r["city"].ToString();
                m.Destination.ContactName = r["contactName"].ToString();
                m.Destination.CorporateName = r["corporateName"].ToString();
                m.Destination.CustomerNumber = r["customerNumber"].ToString();
                m.Destination.Neighborhood = r["neighborhood"].ToString();
                m.Destination.PhoneNumber = r["phone"].ToString();
                m.Destination.State = r["state"].ToString();
                m.Destination.ZipCode = r["zipCode"].ToString();

            }

            foreach (DataRow r in ds.Tables[0].Rows)
            {

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //m.ServiceType = System.Configuration.ConfigurationManager.AppSettings["val_serviceTypeId"];
                m.ServiceType = r["ServiceType"].ToString();
                if (weight >= 70)
                {
                    m.ServiceType = "L0";
                }


                m.Origin = new LogytAddressModel();

                m.Origin.Address1 = r["Address1"].ToString();
                m.Origin.Address2 = r["Address2"].ToString();
                m.Origin.City = r["City"].ToString();
                m.Origin.ContactName = r["CustomerName"].ToString();
                //m.Origin.corporateName = r["CustomerName"].ToString();
                m.Origin.CorporateName = r["UeNo"].ToString();
                m.Origin.CustomerNumber = r["CustomerNo"].ToString();
                m.Origin.Neighborhood = r["NameReceives"].ToString();
                m.Origin.PhoneNumber = r["Phone"].ToString();
                m.Origin.State = r["StateCode"].ToString();
                m.Origin.ZipCode = r["PostalCode"].ToString();

                m.Reference = r["Reference"].ToString();
                //m.originZipCodeForRouting = r["PostalCode"].ToString();
                m.Weight = weight; // lo capturado en el modal
                m.Volume = weight;

                string json2 = JsonConvert.SerializeObject(m);

                Soriana.FWK.FmkTools.RestResponse r2 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_Logyt_Guia"], "", json2);

                string msg = r2.message;

                LogytResponseModels re = JsonConvert.DeserializeObject<LogytResponseModels>(r2.message);

                string pdfcadena2 = Convert.ToBase64String(re.Labels[0].PDF, Base64FormattingOptions.None);

                //return re.Guia + "," + re.pdf;
                return re.Labels[0].Folios[0] + "," + pdfcadena2;

            }

            return string.Empty;

        }

        //upload imagenes
        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object 
            if (Request.Files.Count > 0)
            {
                try
                {
                    var LstArchivos = new Archivos();
                    LstArchivos.archivos = new List<LstArchivos>();
                    LstArchivos.OrdenRma = Request.Form["OrdenRma"].ToString();
                    var nomArch = Request.Form["nomArch"].ToString();

                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        System.IO.Stream str, str1; String strmContents;
                        Int32 counter, strLen, strRead, strRead1;
                        // Create a Stream object.
                        str = Request.InputStream;
                        //----
                        HttpPostedFileBase file = files[i];
                        str1 = file.InputStream;
                        byte[] strArr1 = new byte[Convert.ToInt32(file.ContentLength)];
                        // Read stream into byte array.
                        strRead1 = str1.Read(strArr1, 0, Convert.ToInt32(file.ContentLength));
                        string base64String = Convert.ToBase64String(strArr1);
                        //----                       
                        // Find number of bytes in stream.
                        strLen = Convert.ToInt32(str.Length);
                        // Create a byte array.
                        byte[] strArr = new byte[strLen];
                        // Read stream into byte array.
                        strRead = str.Read(strArr, 0, strLen);

                        // Convert byte array to a text string.
                        strmContents = "";
                        for (counter = 0; counter < strLen; counter++)
                        {
                            strmContents = strmContents + strArr[counter].ToString();
                        }

                        //HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        DateTime dt = DateTime.Now;
                        string dateTime = dt.ToString("yyyyMMddHHmmssfff");
                        fname = string.Format("{0}_{1}_{2}", nomArch, LstArchivos.OrdenRma, dateTime);

                        LstArchivos.archivos.Add(new LstArchivos
                        {
                            filename = fname,
                            mimetype = "image/png",
                            content = base64String
                        }) ;
                    }

                    string json2 = string.Empty;
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    //json2 = js.Serialize(o);

                    json2 = JsonConvert.SerializeObject(LstArchivos);

                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    Soriana.FWK.FmkTools.RestResponse r = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["api_SubirFotos"], "", json2);

                    //RestClient restClient = new RestClient(System.Configuration.ConfigurationManager.AppSettings["api_SubirFotos"]);
                    //RestRequest restRequest = new RestRequest("ordenrma");
                    //restRequest.RequestFormat = DataFormat.Json;
                    //restRequest.Method = Method.POST;
                    //restRequest.AddJsonBody(json2);

                    ////restRequest.AddParameter("folder", tipoFolder);
                    //var response = restClient.Execute(restRequest);

                    if (r.code != "00")
                    {
                        return Json("ERROR al subir la evidencia!");
                    } 
                    return Json("La evidencia fue grabada correctamente!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

    }
}
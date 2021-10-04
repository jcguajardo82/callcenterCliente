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

                var ds = (DALCallCenter.OrderFacts_ArticulosRMA(OrderId));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.Order = DataTableToModel.ConvertTo<Order>(ds.Tables[0]).FirstOrDefault();
                    ViewBag.Products = DataTableToModel.ConvertTo<Product>(ds.Tables[1]);
                    ViewBag.Detail = DataTableToModel.ConvertTo<Detail>(ds.Tables[2]).FirstOrDefault();
                }
            }
            ViewBag.OrderId = OrderId;

            return View();
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
                        return Json("File Uploaded ERROR!");
                    } 
                    return Json("File Uploaded Successfully!");
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
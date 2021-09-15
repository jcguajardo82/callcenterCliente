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
                    LstArchivos.orderRMA = Request.Form["OrdenRma"].ToString();

                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        System.IO.Stream str; String strmContents;
                        Int32 counter, strLen, strRead;
                        // Create a Stream object.
                        str = Request.InputStream;
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

                        HttpPostedFileBase file = files[i];
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

                        LstArchivos.archivo.Add(new LstArchivos
                        {
                            filename = fname,
                            mimetype = "image/png",
                            content = strmContents
                        }) ;
                    }

                    string json2 = string.Empty;
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    //json2 = js.Serialize(o);

                    json2 = JsonConvert.SerializeObject(LstArchivos);

                    RestClient restClient = new RestClient(System.Configuration.ConfigurationManager.AppSettings["api_Upload_Files"]);
                    RestRequest restRequest = new RestRequest("ordenrma");
                    restRequest.RequestFormat = DataFormat.Json;
                    restRequest.Method = Method.POST;
                    restRequest.AddJsonBody(json2);

                    //restRequest.AddParameter("folder", tipoFolder);
                    var response = restClient.Execute(restRequest);

                    if (response.StatusDescription.Equals("Bad Request"))
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
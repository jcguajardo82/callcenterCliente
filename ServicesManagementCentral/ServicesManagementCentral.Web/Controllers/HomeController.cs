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
            }

            var ds = (DALCallCenter.OrderFacts_ArticulosRMA(OrderId));

            ViewBag.Order = DataTableToModel.ConvertTo<Order>(ds.Tables[0]).FirstOrDefault();
            ViewBag.Products = DataTableToModel.ConvertTo<Product>(ds.Tables[1]);
            ViewBag.Detail = DataTableToModel.ConvertTo<Detail>(ds.Tables[2]).FirstOrDefault();

            return View();
        }

    }
}
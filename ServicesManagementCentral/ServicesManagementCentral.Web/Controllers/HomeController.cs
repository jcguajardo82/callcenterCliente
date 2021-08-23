using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    /// <summary>
    /// home controller
    /// </summary>
    public class HomeController : BaseController
    {
        public HomeController():base(System.Web.HttpContext.Current)
        {
           
        }

        /// <summary>
        /// home index view
        /// </summary>
        //public ActionResult Index()
        //{
        //    //if (Login == null)
        //    //{
        //    //    return RedirectToAction("Login", "Security");
        //    //}

        //    if (Login != null)
        //    {
        //        return RedirectToAction("Index", "Ordenes");
        //    }

        //    return RedirectToAction("Index", "CPanel");
        //    //return RedirectToAction("Index", "Ordenes");
        //    //  return View();
        //}

        public ActionResult Index()
        {
            //if (Login == null)
            //{
            //    return RedirectToAction("Login", "Security");
            //}

            if (Login != null)
            {

                if (Login.Username.Trim().Equals("sysAdmin") && Login.Password.Trim().Equals("soriana2021"))
                {
                    return RedirectToAction("Callcenter", "Callcenter");
                }
                else
                {
                    Session["userFail"] = "Usuario o Password incorrecto";
                    return RedirectToAction("Login", "Security");
                }
            }
            return RedirectToAction("Login", "Security");

            //return RedirectToAction("Index", "CPanel");
            //return RedirectToAction("Index", "Ordenes");
            //  return View();
        }

    }
}
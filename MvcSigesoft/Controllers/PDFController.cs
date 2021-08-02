using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcSigesoft.Controllers
{
    public class PDFController : Controller
    {
        //
        // GET: /PDF/

        public ActionResult Index(string id)
        {
            var pstrRutaReportes = Server.MapPath("~/FichasCovid/");
            string filePath = pstrRutaReportes + id +".pdf";

            var result = System.IO.File.Exists(filePath);

            if (!result) return View();
            
       
            return File(filePath, "application/pdf");
        }

    }
}

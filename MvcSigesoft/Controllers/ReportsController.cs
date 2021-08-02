using MvcSigesoft.ViewModels;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MvcSigesoft.Controllers
{
    public class ReportsController : Controller
    {
        public ActionResult ReportFichasCovid19()
        {
            var lista = new List<ReporteViewModel>();
            return View(lista);
        }

        [HttpPost]
        public JsonResult Filter(parametros param)
        {
            try
            {
                var lista = new List<ReporteViewModel>();

                ServiceBL oServiceBL = new ServiceBL();
                lista = oServiceBL.ReporteFichasCovid19(param.FechaInicio, param.FechaFin);
                return new JsonResult()
                {
                    Data = lista,
                    MaxJsonLength = 86753090
                };
            }
            catch (Exception ex)
            {
                
                throw;
            }
            
        }

    }

    public class parametros
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}

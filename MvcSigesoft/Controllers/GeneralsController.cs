
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using SigesoftWebUI.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcSigesoft.Controllers
{
    public class GeneralsController : GenericController
    {
        [HttpGet]
        public ActionResult Index(string optionCovid, string optionCertificado, string optionDescenso)
        {
            var lista = new List<ListaCovid19>();
            Covid19BL oCovid19BL = new Covid19BL();
            var ComponentsId = new List<string>();

            var fechaServicio = DateTime.Now.Date;

            if (optionCovid == null && optionCertificado == null && optionDescenso == null)
            {
                ComponentsId.Add(Sigesoft.Common.Constants.COVID_ID);
                ComponentsId.Add(Sigesoft.Common.Constants.CERTIFICADO_COVID_ID);
                ComponentsId.Add(Sigesoft.Common.Constants.CERTIFICADO_DESCENSO_COVID_ID);

                lista = oCovid19BL.ObtenerLista(fechaServicio, ComponentsId);
                return View(lista);
            }
 
            if (!bool.Parse(optionCovid) && !bool.Parse(optionCertificado)&& !bool.Parse(optionDescenso))return View(new List<ListaCovid19>());

            if (bool.Parse(optionCovid))
            {
                 ComponentsId.Add(Sigesoft.Common.Constants.COVID_ID);                
            }

            if (bool.Parse(optionCertificado))
            {
                ComponentsId.Add(Sigesoft.Common.Constants.CERTIFICADO_COVID_ID);
            }

            if (bool.Parse(optionDescenso))
            {
                ComponentsId.Add(Sigesoft.Common.Constants.CERTIFICADO_DESCENSO_COVID_ID);
            }

            lista = oCovid19BL.ObtenerLista(fechaServicio, ComponentsId);
            return View(lista);
        }

        [HttpGet]
        public JsonResult EsEditable(string serviceComponentId)
        {
            OperationResult objOperationResult = new OperationResult();
            Covid19BL _objCalendarBL = new Covid19BL();

            var obj = _objCalendarBL.EsEditable(serviceComponentId);
            return Json(obj, JsonRequestBehavior.AllowGet);  

        }
    }
}

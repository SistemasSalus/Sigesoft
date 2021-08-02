
using MvcSigesoft.Security;
using MvcSigesoft.ViewModels.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SigesoftWebUI.Controllers.Base
{
    [AuthorizeFilter]
    public class GenericController : Controller
    {
        private SessionModel _sessionUsuario;
        public GenericController()
        {

        }

        public GenericController(SessionModel sessionModel)
        {
            SessionUsuario = sessionModel;
        }

        public SessionModel SessionUsuario
        {
            get
            {
                return _sessionUsuario ?? HttpSessionContext.CurrentAccount();
            }
            private set
            {
                _sessionUsuario = value;
            }
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (SessionUsuario == null)
                {
                    string urlSignOut = ObtenerUrlCerrarSesion();
                    filterContext.Result = new RedirectResult(urlSignOut);
                    return;
                }

                GetUserDataViewBag();

                base.OnActionExecuting(filterContext);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void GetUserDataViewBag()
        {
            ViewBag.SessionTimeout = HttpContext.Session.Timeout;
        }

        private string ObtenerUrlCerrarSesion()
        {
            string URLSignOut = string.Empty;
            if (Request.UrlReferrer != null && Request.UrlReferrer.ToString().Contains(Request.Url.Host))
                URLSignOut = "/Generals/SesionExpirada";
            else
                URLSignOut = "/Generals/UserUnknown";

            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            return URLSignOut;
        }
    }
}
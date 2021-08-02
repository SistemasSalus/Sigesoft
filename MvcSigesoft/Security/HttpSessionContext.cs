using MvcSigesoft.ViewModels.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcSigesoft.Security
{
    public class HttpSessionContext
    {
        private static string USER_APPLICATION = Resources.Constante.SessionUsuario;
        /// <summary>
        /// Obtiene la sesión actual.
        /// </summary>
        /// <returns></returns>
        public static SessionModel CurrentAccount()
        {
            var result = HttpContext.Current.Session != null ? (SessionModel)HttpContext.Current.Session[USER_APPLICATION] : null;

            return result;
        }
        /// <summary>
        /// Obtiene la sesión actual.
        /// </summary>
        /// <returns></returns>
        public static void SetAccount(SessionModel account)
        {
            //FormsAuthentication.SetAuthCookie(account.Usuario.UserName, true);
            HttpContext.Current.Session.Add(USER_APPLICATION, account);
        }



        public static void RemoveAccount()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
        }

        public static RedirectResult LogOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();

            string LogoutUrl = string.Format("{0}", FormsAuthentication.LoginUrl);

            return new RedirectResult(LogoutUrl);
        }
    }
}
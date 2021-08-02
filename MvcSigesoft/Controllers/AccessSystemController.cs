using MvcSigesoft.ViewModels;
using MvcSigesoft.ViewModels.Session;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcSigesoft.Controllers
{
    public class AccessSystemController : Controller
    {
        //
        // GET: /Login/
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = null)
        {
            CombosSedes();

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            SecurityBL _securityBL = new SecurityBL();
            OperationResult objOperationResult = new OperationResult();
            try
            {
                var oLoginDto = new LoginDto();
                oLoginDto.v_UserName = model.Username;
                oLoginDto.v_Password = model.Password;
                oLoginDto.i_NodoId = model.NodoId;
                
                #region USUARIOS TEMPORALES BACKUS
                if (model.NodoId != 3)
                {
                    var usuarioValidado = ValidarUsuariosTemporales(model.Username.Trim().ToLower(), model.Password, model.NodoId);
                    if (usuarioValidado.i_UsuarioRegcovidId > 0)
                    {
                         var sessionModel = new SessionModel();
                        sessionModel.UserName = usuarioValidado.v_UserName;
                        sessionModel.FullName = "USUARIO TEMPORAL";
                        sessionModel.SystemUserId = 11;

                        sessionModel.NodeId = usuarioValidado.i_NodeId;
                        sessionModel.Nodo = usuarioValidado.v_NodeName;

                        sessionModel.EmpresaId = usuarioValidado.v_OrganizationId;
                        sessionModel.Empresa = usuarioValidado.v_OrganizationName;

                        sessionModel.ProtocolId = usuarioValidado.v_ProtocolId;
                        sessionModel.Protocol = usuarioValidado.v_ProtocolName;                        

                        FormsAuthentication.SetAuthCookie(sessionModel.UserName, false);

                        MvcSigesoft.Security.HttpSessionContext.SetAccount(sessionModel);
                        
                        if (model.NodoId == 109)
                        {
                            return RedirectToAction("ReportFichasCovid19", "Reports");
                        }
                        else if (model.NodoId == (int)SedeBackus.OtrasClinicas)
                        {
                            return RedirectToAction("Index", "OtrasClinicas");
                        }
                        else {
                            return RedirectToAction("Index", "PruebaRapidaCovid19");
                            
                        }
                    }
                    else
                    {
                        CombosSedes();
                        ModelState.AddModelError("", "Contraseña o identificador de usuario incorrectos. Escriba la contraseña y el identificador de usuario correctos e inténtelo de nuevo.");
                        return View(model);
                    }


                }
                #endregion

                #region USUARIOS DE SIGESOFT
                var result = _securityBL.ValidateSystemUser(ref objOperationResult, oLoginDto.i_NodoId, model.Username, Sigesoft.Common.Utils.Encrypt(model.Password));

                if (result != null)
                {
                    var sessionModel = new SessionModel();
                    sessionModel.UserName = result.v_UserName;
                    sessionModel.FullName = result.v_PersonName;
                    sessionModel.SystemUserId = result.i_SystemUserId;
                    sessionModel.NodeId = model.NodoId;
                    sessionModel.Nodo = "Lima";
                    FormsAuthentication.SetAuthCookie(sessionModel.UserName, false);

                    MvcSigesoft.Security.HttpSessionContext.SetAccount(sessionModel);
                }
                else
                {
                    CombosSedes();
                    ModelState.AddModelError("", "Contraseña o identificador de usuario incorrectos. Escriba la contraseña y el identificador de usuario correctos e inténtelo de nuevo.");
                    return View(model);
                }
                #endregion
           

            }
            catch (Exception ex)
            {
                CombosSedes();
                ModelState.AddModelError("", "Error al procesar la solicitud.");
                return RedirectToAction("Login", "AccessSystem");
            }

            return RedirectToLocal(returnUrl);
        }

        private UsuarioRegCovidBE ValidarUsuariosTemporales(string username, string pass, int sede)
        {

            Covid19BL oCovid19BL = new Covid19BL();
            return oCovid19BL.ValidarUSuarioRegCovid(username, pass, sede);
            ////CONO NORTE
            //if (sede == 100)
            //{
            //    if (username == "micky.sarmiento" && pass == "47005150")
            //    {
            //        return true;
            //    }
            //    else if (username == "jenny.saman" && pass == "74125585")
            //    {
            //        return true;
            //    }
            //    else if (username == "frenesi.caillahua" && pass == "71722427")
            //    {
            //        return true;
            //    }
            //    else if (username == "zoila.sotomayor" && pass == "41847803")
            //    {
            //        return true;
            //    }
            //}//RIMAC
            //else if (sede == 101)
            //{
            //    if (username == "any.mena" && pass == "76282383")
            //    {
            //        return true;
            //    }
            //    else if (username == "delia.aniceto" && pass == "48381533")
            //    {
            //        return true;
            //    }
            //    else if (username == "virginia.marquez" && pass == "70827865")
            //    {
            //        return true;
            //    }
            //    else if (username == "gisela.alaya" && pass == "47423216")
            //    {
            //        return true;
            //    }
            //}//ATE
            //else if (sede == 102)
            //{
            //    if (username == "sabina.escobar" && pass == "47859922")
            //    {
            //        return true;
            //    }
            //    else if (username == "holmer.tellez" && pass == "42915114")
            //    {
            //        return true;
            //    }
            //    else if (username == "merusi.ramos" && pass == "47468047")
            //    {
            //        return true;
            //    }
            //    else if (username == "nicolas.palomino" && pass == "43175271")
            //    {
            //        return true;
            //    }
            //    else if (username == "ana.coaquira" && pass == "76325704")
            //    {
            //        return true;
            //    }
            //    else if (username == "colqui.bianca" && pass == "47468047")
            //    {
            //        return true;
            //    }
            //    else if (username == "palomino.nicolaz" && pass == "43175271")
            //    {
            //        return true;
            //    }
            //    else if (username == "coaquira.ana" && pass == "76325704")
            //    {
            //        return true;
            //    }
            //    else if (username == "delia.nacion" && pass == "48381533")
            //    {
            //        return true;
            //    }
            //    else if (username == "colqui.kerli" && pass == "70232041")
            //    {
            //        return true;
            //    }
            //    else if (username == "alifram.gallardo" && pass == "002767660")
            //    {
            //        return true;
            //    }
            //    else if (username == "antony.llerena" && pass == "48653246")
            //    {
            //        return true;
            //    }
            //    else if (username == "jean.muñoz" && pass == "987479567")
            //    {
            //        return true;
            //    }
            //    else if (username == "olazabal.luis" && pass == "42585151")
            //    {
            //        return true;
            //    }
            //    else if (username == "carlos.aguila" && pass == "70476141")
            //    {
            //        return true;
            //    }
            //    else if (username == "cesar.herrera" && pass == "70086653")
            //    {
            //        return true;
            //    }
            //}//CONO SUR
            //else if (sede == 103)
            //{
            //    if (username == "aitza.napa" && pass == "73778940")
            //    {
            //        return true;
            //    }
            //    else if (username == "mareli.reyes" && pass == "43924118")
            //    {
            //        return true;
            //    }
            //    else if (username == "delia.aniceto" && pass == "48381533")
            //    {
            //        return true;
            //    }
            //    //else if (username == "nicolas.palomino" && pass == "43175271")
            //    //{

            //    //}
            //}//CALLAO
            //else if (sede == 104)
            //{
            //    if (username == "leoncio.antaurco" && pass == "73971187")
            //    {
            //        return true;
            //    }
            //    else if (username == "leydi.pampañaupa" && pass == "48122416")
            //    {
            //        return true;
            //    }
            //    else if (username == "yeraldin.morales" && pass == "45626191")
            //    {
            //        return true;
            //    }
            //    else if (username == "milagros.sanchez" && pass == "15740178")
            //    {
            //        return true;
            //    }
            //    else if (username == "felix.magallanes" && pass == "73235232")
            //    {
            //        return true;
            //    }
            //}//VEGUETA
            //else if (sede == (int)SedeBackus.Vegueta || sede == (int)SedeBackus.Trujillo || sede == (int)SedeBackus.Chimbote || sede == (int)SedeBackus.Piura || sede == (int)SedeBackus.Tumbes || sede == (int)SedeBackus.Talara)
            //{
            //    if (username == "cesar.herrera" && pass == "70086653")
            //    {
            //        return true;
            //    }
            //    else if (username == "carlos.aguila" && pass == "70476141")
            //    {
            //        return true;
            //    }

            //}//RUTA 2
            //else if (sede == (int)SedeBackus.TingoMaria || sede == (int)SedeBackus.Huanuco || sede == (int)SedeBackus.Chanchamayo || sede == (int)SedeBackus.satipo)
            //{
            //    if (username == "alifram.gallardo" && pass == "002767660")
            //    {
            //        return true;
            //    }
            //}//RUTA 3
            //else if (sede == (int)SedeBackus.Nazca || sede == (int)SedeBackus.Ica || sede == (int)SedeBackus.Canete)
            //{
            //    if (username == "jean.muñoz" && pass == "987479567")
            //    {
            //        return true;
            //    }
            //    else if (username == "antony.llerena" && pass == "48653246")
            //    {
            //        return true;
            //    }
            //}
            ////Chiclayo
            //else if (sede == 108)
            //{
            //    if (username == "ceci.moran" && pass == "44250204")
            //    {
            //        return true;
            //    }
            //    else if (username == "felicita.rodriguez" && pass == "46673224")
            //    {
            //        return true;
            //    }
            //    else if (username == "ana.colunche" && pass == "46795332")
            //    {
            //        return true;
            //    }
            //    else if (username == "nathaly.miñope" && pass == "47163135")
            //    {
            //        return true;
            //    }

            //    else if (username == "scarly.farro" && pass == "7519608")
            //    {
            //        return true;
            //    }
            //    else if (username == "lady.chinchay" && pass == "71499559")
            //    {
            //        return true;
            //    }
            //    else if (username == "jessenia.quiroz" && pass == "47806354")
            //    {
            //        return true;
            //    }
            //}
            ////AREQUIPA
            //else if (sede == (int)SedeBackus.PLANTA_AREQUIPA)
            //{
            //    if (username == "alifram.gallardo" && pass == "002767660")
            //    {
            //        return true;
            //    }
            //    else if (username == "antony.llerena" && pass == "48653246")
            //    {
            //        return true;
            //    }
            //    else if (username == "jean.muñoz" && pass == "987479567")
            //    {
            //        return true;
            //    }
            //    else if (username == "holmer.tellez" && pass == "42915114")
            //    {
            //        return true;
            //    }
            //}
            //else if (sede == 109)
            //{
            //    if (username == "reportes" && pass == "reportes")
            //    {
            //        return true;
            //    }
            //}
            //else if (sede == 151)
            //{
            //    if (username == "valeska.dugarte" && pass == "095358052")
            //    {
            //        return true;
            //    }
            //    else if (username == "externo" && pass == "externo")
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        //private bool ValidarUsuariosTemporales(string username, string pass, int sede)
        //{
        //    SystemParameterBL oSystemParameterBL = new SystemParameterBL();
        //    return oSystemParameterBL.ValidarUsuarioTemporal(username, pass);
        //}
        [AllowAnonymous]
        public ActionResult UserUnknown()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            return CerrarSesion();
        }

        private ActionResult CerrarSesion()
        {
            var rolUsuario = string.Empty;
            var userData = MvcSigesoft.Security.HttpSessionContext.CurrentAccount();
            if (userData != null)
            {
                rolUsuario = userData.UserName;
            }

            var urlSignOut = string.Empty;

            FormsAuthentication.SignOut();
            System.Web.HttpContext.Current.Session.Abandon();
            System.Web.HttpContext.Current.Session.Clear();


            urlSignOut = string.Format("{0}", System.Web.Security.FormsAuthentication.LoginUrl);


            return Redirect(urlSignOut);
        }


        [AllowAnonymous]
        public ActionResult SesionExpirada(string returnUrl)
        {
            AsignarUrlRetorno(returnUrl);
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Generals");
        }

        protected virtual void AsignarUrlRetorno(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnURL = returnUrl;
            }
        }

        public void CombosSedes()
        {
            Covid19BL oCovid19BL = new Covid19BL();
            List<SelectListItem> listaNodos = new List<SelectListItem>();
            var objnodos = oCovid19BL.ListarNodosCovid();
            foreach (var nodo in objnodos)
            {
                listaNodos.Add(new SelectListItem { Text = nodo.v_NodeName, Value = nodo.i_NodeId.ToString() });
            }

            
            //listaNodos.Add(new SelectListItem { Text = "SALUS LABORIS-CSO Chiclayo", Value = "108" });
            //listaNodos.Add(new SelectListItem { Text = "SALUS LABORIS-CSO Lima", Value = "3" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-CONO NORTE", Value = "100" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-RIMAC", Value = "101" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-ATE", Value = "102" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-CONO SUR", Value = "103" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-CALLAO", Value = "104" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-VEGUETA", Value = "105" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-TINGOMARIA", Value = "106" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-NAZCA", Value = "107" });

            //listaNodos.Add(new SelectListItem { Text = "BACKUS-TRUJILLO", Value = "110" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-CHIMBOTE", Value = "111" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-PIURA", Value = "112" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-TUMBES", Value = "113" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-TALARA", Value = "114" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-HUANUCO", Value = "116" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-CHANCHAMAYO", Value = "117" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-ICA", Value = "118" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-CAÑETE", Value = "120" });
            //listaNodos.Add(new SelectListItem { Text = "BACKUS-SATIPO", Value = "121" });

            //listaNodos.Add(new SelectListItem { Text = "PLANTA-AREQUIPA", Value = "163" });

            //listaNodos.Add(new SelectListItem { Text = "REPORTES", Value = "109" });
            //listaNodos.Add(new SelectListItem { Text = "OTRAS CLÍNICAS", Value = "151" });
            

            ViewBag.Nodos = listaNodos;
        }

        private string ObtenerSedeDeUsuarioTemperal(int nodeId)
        {
            var value = "";
            if (nodeId == (int)SedeBackus.ConoNorte)
            {
                value = "CONO NORTE";
            }
            else if (nodeId == (int)SedeBackus.Rimac)
            {
                value = "RIMAC";
            }
            else if (nodeId == (int)SedeBackus.Ate)
            {
                value = "ATE";
            }
            else if (nodeId == (int)SedeBackus.ConoSur)
            {
                value = "CONO SUR";
            }
            else if (nodeId == (int)SedeBackus.Callao)
            {
                value = "CALLAO";
            }
            else if (nodeId == (int)SedeBackus.Vegueta)
            {
                value = "VEGUETA PROVINCIA";
            }
            else if (nodeId == (int)SedeBackus.TingoMaria)
            {
                value = "TINGO MARIA";
            }
            else if (nodeId == (int)SedeBackus.Nazca)
            {
                value = "NAZCA";
            }
            else if (nodeId == (int)SedeBackus.Trujillo)
            {
                value = "TRUJILLO";
            }
            else if (nodeId == (int)SedeBackus.Chimbote)
            {
                value = "CHIMBOTE";
            }
            else if (nodeId == (int)SedeBackus.Piura)
            {
                value = "PIURA";
            }
            else if (nodeId == (int)SedeBackus.Tumbes)
            {
                value = "TUMBES";
            }
            else if (nodeId == (int)SedeBackus.Talara)
            {
                value = "TALARA";
            }
            else if (nodeId == (int)SedeBackus.Huanuco)
            {
                value = "HUANUCO";
            }
            else if (nodeId == (int)SedeBackus.Chanchamayo)
            {
                value = "CHANCHAMAYO";
            }
            else if (nodeId == (int)SedeBackus.Ica)
            {
                value = "ICA";
            }
            else if (nodeId == (int)SedeBackus.Canete)
            {
                value = "CAÑETE";
            }
            else if (nodeId == (int)SedeBackus.satipo)
            {
                value = "SATIPO";
            }
            else if (nodeId == (int)SedeBackus.PLANTA_AREQUIPA)
            {
                value = "PLANTA - AREQUIPA";
            }
            else if (nodeId == (int)SedeBackus.OtrasClinicas)
            {
                value = "OTRAS CLINICAS";
            }


            else if (nodeId == 108)
            {
                value = "CHICLAYO";
            }
            else if (nodeId == 109)
            {
                value = "CHICLAYO";
            }
            return value;
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult LoginExterno(LoginExternoViewModel model, string returnUrl = null)
        //{
        //    SecurityBL _securityBL = new SecurityBL();
        //    OperationResult objOperationResult = new OperationResult();
        //    try
        //    {
        //        var oLoginDto = new LoginDto();
        //        oLoginDto.v_UserName = model.Usuario;
        //        oLoginDto.v_Password = model.Password;
        //        oLoginDto.i_NodoId = model.NodoId;

        //        #region USUARIOS TEMPORALES BACKUS
        //        if (model.NodoId != 3)
        //        {
        //            if (ValidarUsuariosExternosTemporales(model.Usuario.Trim().ToLower(), model.Password, model.NodoId))
        //            {
        //                var sessionModel = new SessionModel();
        //                sessionModel.UserName = model.Usuario;
        //                sessionModel.FullName = "USUARIO TEMPORAL";
        //                sessionModel.SystemUserId = 11;
        //                sessionModel.NodeId = model.NodoId;
        //                sessionModel.Nodo = ObtenerSedeDeUsuarioTemperal(model.NodoId);
        //                FormsAuthentication.SetAuthCookie(sessionModel.UserName, false);

        //                MvcSigesoft.Security.HttpSessionContext.SetAccount(sessionModel);

        //                if (model.NodoId == 109)
        //                {
        //                    return RedirectToAction("ReportFichasCovid19", "Reports");
        //                }
        //                else
        //                {
        //                    return RedirectToAction("Index", "PruebaRapidaCovid19");

        //                }
        //            }
        //            else
        //            {
        //                CombosSedes();
        //                ModelState.AddModelError("", "Contraseña o identificador de usuario incorrectos. Escriba la contraseña y el identificador de usuario correctos e inténtelo de nuevo.");
        //                return View(model);
        //            }


        //        }
        //        #endregion

        //        #region USUARIOS DE SIGESOFT
        //        var result = _securityBL.ValidateSystemUser(ref objOperationResult, oLoginDto.i_NodoId, model.Usuario, Sigesoft.Common.Utils.Encrypt(model.Password));

        //        if (result != null)
        //        {
        //            var sessionModel = new SessionModel();
        //            sessionModel.UserName = result.v_UserName;
        //            sessionModel.FullName = result.v_PersonName;
        //            sessionModel.SystemUserId = result.i_SystemUserId;
        //            sessionModel.NodeId = model.NodoId;
        //            sessionModel.Nodo = "Lima";
        //            FormsAuthentication.SetAuthCookie(sessionModel.UserName, false);

        //            MvcSigesoft.Security.HttpSessionContext.SetAccount(sessionModel);
        //        }
        //        else
        //        {
        //            CombosSedes();
        //            ModelState.AddModelError("", "Contraseña o identificador de usuario incorrectos. Escriba la contraseña y el identificador de usuario correctos e inténtelo de nuevo.");
        //            return View(model);
        //        }
        //        #endregion


        //    }
        //    catch (Exception ex)
        //    {
        //        CombosSedes();
        //        ModelState.AddModelError("", "Error al procesar la solicitud.");
        //        return RedirectToAction("Login", "AccessSystem");
        //    }

        //    return RedirectToLocal(returnUrl);
        //}
        
    }
}

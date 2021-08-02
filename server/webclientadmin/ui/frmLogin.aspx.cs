using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sigesoft.Server.WebClientAdmin.BLL;
using Sigesoft.Server.WebClientAdmin.BE;
using Sigesoft.Common;

//using FineUI;
using System.Web.Security;
namespace Sigesoft.Server.WebClientAdmin.UI
{
    public partial class frmLogin1 : System.Web.UI.Page
    {
        SecurityBL _objSecurityBL = new SecurityBL();
         PacientBL _objPacientBL = new PacientBL();
        SystemParameterBL _objSystemParameterBL = new SystemParameterBL();
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }
               
        protected void go_Click(object sender, EventArgs e)
        {

            // Autenticación de usuario
            OperationResult objOperationResult1 = new OperationResult();

            //Validar si es un paciente
            var objPaciente = _objPacientBL.ValidarPersonaWeb(ref objOperationResult1, email.Value.Trim(), password.Value.Trim());

            if (objPaciente != null)
            {
                string decimalNumber = objPaciente.v_DocNumber;

                int number = int.Parse(decimalNumber);

                string hex = number.ToString("x");

                var x = hex.ToString();
                if (x == email.Value.Trim())
                {
                    Session["IdPersona"] = objPaciente.v_PersonId;
                    FormsAuthentication.RedirectFromLoginPage("Trabajador", true);
                }
                else
                {
                    Text1.Value = "Usuario o Password incorrectos.";
                }
            }
            else
            {
                var objSystemUser = _objSecurityBL.ValidateSystemUser(ref objOperationResult1,
                                                                      1,
                                                                      email.Value.Trim(),
                                                                      Sigesoft.Common.Utils.Encrypt(password.Value.Trim()));
                if (objSystemUser != null)
                {
                    ClientSession clientSession = new ClientSession();
                    clientSession.i_SystemUserId = objSystemUser.i_SystemUserId;
                    clientSession.v_UserName = objSystemUser.v_UserName;
                    clientSession.i_CurrentExecutionNodeId = 1;
                    clientSession.i_CurrentOrganizationId = 0;
                    clientSession.v_PersonId = objSystemUser.v_PersonId;
                    clientSession.i_SystemUserTypeId = (int)objSystemUser.i_SystemUserTypeId.Value;
                    clientSession.i_ProfesionId = objSystemUser.i_ProfesionId;
                    Session["objClientSession"] = clientSession;

                    FormsAuthentication.RedirectFromLoginPage(objSystemUser.v_UserName, true);

                }
                else
                {
                    Text1.Value = "Datos no válidos!!!";
                   
                }
            }

          
            //{
            //    // Autenticación de usuario

            //    var objSystemUser = _objSecurityBL.ValidateSystemUser(ref objOperationResult1,
            //                                                            1,
            //                                                           email.Value.Trim(),
            //                                                            Sigesoft.Common.Utils.Encrypt( password.Value.Trim()));
            //    if (objSystemUser != null)
            //    {
            //        ClientSession clientSession = new ClientSession();
            //        clientSession.i_SystemUserId = objSystemUser.i_SystemUserId;
            //        clientSession.v_UserName = objSystemUser.v_UserName;
            //        clientSession.i_CurrentExecutionNodeId = 1;
            //        clientSession.i_CurrentOrganizationId =objSystemUser.i_CurrentOrganizationId== null?0: objSystemUser.i_CurrentOrganizationId.Value;//Verifica si es con dx o sin dx
            //        clientSession.v_PersonId = objSystemUser.v_PersonId;
            //        clientSession.i_SystemUserTypeId = (int)objSystemUser.i_SystemUserTypeId.Value;
            //        clientSession.i_ProfesionId = objSystemUser.i_ProfesionId;
                    
            //        //Obtener RoleID
            //        var obj = _objSecurityBL.ObtenerRolIdUser(ref objOperationResult1, 9, objSystemUser.i_SystemUserId);
            //        if (obj != null)
            //        {
            //            clientSession.i_RoleId = obj.i_RolId;
            //        }
            //        Session["objClientSession"] = clientSession;

            //        FormsAuthentication.RedirectFromLoginPage(objSystemUser.v_UserName, true);
            //    }
              

            //}
           

        }
    }
}
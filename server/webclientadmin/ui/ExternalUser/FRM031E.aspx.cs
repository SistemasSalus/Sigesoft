using FineUI;
using Sigesoft.Common;
using Sigesoft.Server.WebClientAdmin.BE;
using Sigesoft.Server.WebClientAdmin.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Sigesoft.Server.WebClientAdmin.UI.ExternalUser
{
    public partial class FRM031E : System.Web.UI.Page
    {
        ServiceBL _serviceBL = new ServiceBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<MyListWeb> ListaServicios = (List<MyListWeb>)Session["objLista"];

                btnSaveRefresh.OnClientClick = winEdit1.GetSaveStateReference(hfRefresh.ClientID) + winEdit1.GetShowReference("../ExternalUser/FRM031F.aspx");
               
                var serviceComponents = _serviceBL.GetServiceComponentsForManagementReport(ListaServicios[0].IdServicio);

                #region Examen For Print

                string[] examenForPrint = new string[] 
            { 
                Constants.ALTURA_ESTRUCTURAL_ID,
                Constants.ALTURA_7D_ID,
                Constants.ODONTOGRAMA_ID,
                Constants.OSTEO_MUSCULAR_ID,
                Constants.OFTALMOLOGIA_ID,
                Constants.RX_ID,
                Constants.PRUEBA_ESFUERZO_ID,
                Constants.ELECTROCARDIOGRAMA_ID,
                Constants.TAMIZAJE_DERMATOLOGIO_ID,
                Constants.PSICOLOGIA_ID,
                Constants.AUDIOMETRIA_ID,
                Constants.ESPIROMETRIA_ID,
                //Constants.LABORATORIO_ID,
                Constants.GINECOLOGIA_ID,
                Constants.EVALUACION_PSICOLABORAL

            };

                #endregion
                // Cargar ListBox de examenes
                serviceComponents = serviceComponents.FindAll(p => examenForPrint.Contains(p.v_ComponentId));

                //serviceComponents.Insert(0, new ServiceComponentList { v_ComponentName = "Certificado de Aptitud", v_ComponentId = Constants.INFORME_CERTIFICADO_APTITUD });
                serviceComponents.Insert(1, new ServiceComponentList { v_ComponentName = "Historia Ocupacional", v_ComponentId = Constants.INFORME_HISTORIA_OCUPACIONAL });

                // Si la prueba de RX esta entonces tambien insertar <Informe Radiografico OIT>
                var findRX = serviceComponents.Find(p => p.v_ComponentId == Constants.RX_ID);
                var findEspiro = serviceComponents.Find(p => p.v_ComponentId == Constants.ESPIROMETRIA_ID);

                if (findRX != null)
                {
                    var newPosition = serviceComponents.IndexOf(findRX) + 1;
                    serviceComponents.Insert(newPosition, new ServiceComponentList { v_ComponentName = "Informe Radiografico OIT", v_ComponentId = Constants.INFORME_RADIOGRAFICO_OIT });
                }

                if (findEspiro != null)
                {
                    var newPosition = serviceComponents.IndexOf(findEspiro) + 1;
                    serviceComponents.Insert(newPosition, new ServiceComponentList { v_ComponentName = "Cuestionario de Espirometria", v_ComponentId = Constants.ESPIROMETRIA_CUESTIONARIO_ID });
                }

                chklExamenes.DataTextField = "v_ComponentName";
                chklExamenes.DataValueField = "v_ComponentId";
                chklExamenes.DataSource = serviceComponents;
                chklExamenes.DataBind();
            }
        }

        protected void chklExamenes_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> lista = new List<string>();

            int selectedCount = chklExamenes.SelectedItemArray.Length;

            //var lista = List<ServiceComponentList>()
            foreach (var item in chklExamenes.SelectedItemArray)
            {
                lista.Add(item.Value);
            }

            Session["objListaExamenes"] = lista;

            if (selectedCount > 0)
            {
                btnSaveRefresh.Enabled = true;
            }
            else
            {
                btnSaveRefresh.Enabled = false;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());

        }

    }
}
using Sigesoft.Server.WebClientAdmin.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sigesoft.Server.WebClientAdmin.UI.ExternalUser
{
    public partial class frmPlacasRx : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<MyListWeb> ListaServicios = (List<MyListWeb>)Session["objLista"];
            var idele = ListaServicios[0].IdServicio + "_" + ListaServicios[0].Paciente;
        }
    }
}
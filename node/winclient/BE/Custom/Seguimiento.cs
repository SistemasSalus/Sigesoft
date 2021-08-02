using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class Seguimiento
    {
        public string ServiceId{ get; set; }
        public string Trabajador { get; set; }
        public DateTime? d_ServiceDate { get; set; }
        public string Estado { get; set; }
        public string Categoria { get; set; }
        public string Comentario { get; set; }

        public int? i_ServiceStatusId { get; set; }
        public string v_CustomerOrganizationId { get; set; }
        public string v_CustomerLocationId { get; set; }
        public int? i_MasterServiceId { get; set; }
        public int? i_ServiceTypeId { get; set; }
        public int? i_EsoTypeId { get; set; }
        public string v_ProtocolId { get; set; }
        public int? i_AptitudeStatusId { get; set; }

        public List<SeguimientoDetalle> SeguimientoDetalle { get; set; }
    }
}

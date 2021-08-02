using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Server.WebClientAdmin.BE.Custom
{
    public class ResultsWebxSystemUser
    {
        public string v_ServiceId { get; set; }
        public string v_IdTrabajador { get; set; }
        public string v_Trabajador { get; set; }
        public DateTime? d_ServiceDate { get; set; }
        public string EmpresaCliente { get; set; }
        public string v_ProtocolId { get; set; }
        public string Protocolo { get; set; }
        public string v_AptitudeStatusName { get; set; }
        public string Pendiente { get; set; }
    }
}

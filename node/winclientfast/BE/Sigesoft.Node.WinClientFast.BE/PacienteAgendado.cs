using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClientFast.BE
{
    public class PacienteAgendado
    {
        public DateTime? d_CircuitStartDate { get; set; }
        public string Paciente { get; set; }
        public string NroDocum { get; set; }
        public int? Edad { get; set; }
        public string Empleador { get; set; }
        public string Cliente { get; set; }
        public string Protocolo { get; set; }
        public string v_Value1 { get; set; }
        public string v_CurrentOccupation { get; set; }
        public int? i_IsVipId { get; set; }
        public string v_ServiceId { get; set; }
        public string v_PersonId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class CalendarInterfaceLS
    {
        public long i_Correlativo { get; set; }
        public string v_Nombres { get; set; }
        public string v_ApellidoPaterno { get; set; }
        public string v_ApellidoMaterno { get; set; }
        public int i_IDTipoDocumento { get; set; }
        public string v_TipoDocumento { get; set; }
        public string v_NumeroDocumento { get; set; }
        public int i_IDGenero { get; set; }
        public string v_Genero { get; set; }
        public string v_FechaNacimiento { get; set; }
        public string v_PuestodeTrabajo { get; set; }

        public string v_ProtocolId { get; set; }
    }
}

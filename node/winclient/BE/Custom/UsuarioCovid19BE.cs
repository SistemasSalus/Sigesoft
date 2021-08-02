using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class UsuarioCovid19BE
    {
        public int id { get; set; }
        public bool estado { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
        public string tecnico { get; set; }
    }

    public class UsuarioRegCovidBE 
    { 
        public int i_UsuarioRegcovidId { get; set; }
        public int i_NodeId { get; set; }
        public string v_NodeName { get; set; }
        public string v_OrganizationId { get; set; }
        public string v_OrganizationName { get; set; }
        public string v_ProtocolId { get; set; }
        public string v_ProtocolName { get; set; }
        public string v_UserName { get; set; }

    }

}

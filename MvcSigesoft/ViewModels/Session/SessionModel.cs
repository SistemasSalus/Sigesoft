using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcSigesoft.ViewModels.Session
{
    public class SessionModel
    {
        public string Empresa { get; set; }
        public string EmpresaId { get; set; }

        public string Protocol { get; set; }
        public string ProtocolId { get; set; }

        public int NodeId { get; set; }
        public string Nodo { get; set; }

        public int SystemUserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        
    }
}
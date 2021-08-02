using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClientFast.BE
{
    public class SystemUserList
    {
        public int i_SystemUserId { get; set; }
        public string v_PersonId { get; set; }
        public int? i_NodeId { get; set; }
        public string v_UserName { get; set; }
        public string v_Password { get; set; }
        public string v_SecretQuestion { get; set; }
        public string v_SecretAnswer { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public string v_InsertUser { get; set; }
        public string v_UpdateUser { get; set; }
        public string v_NodeName { get; set; }

        public string v_PersonName { get; set; }
        public string v_DocNumber { get; set; }
        public DateTime? d_ExpireDate { get; set; }
        public string v_ProtocolId { get; set; }

        public string v_ProtocolSystemUserId { get; set; }

        public int? i_SystemUserTypeId { get; set; }
        public int? i_ProfesionId { get; set; }
        public int? i_RolId { get; set; }

        public string v_EmpresaClienteId { get; set; }

        public int? i_CurrentOrganizationId { get; set; }
    }
}

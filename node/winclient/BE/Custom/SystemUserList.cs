using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sigesoft.Node.WinClient.BE
{
    public class SystemUserList
    {
        [DataMember]
        public int i_SystemUserId { get; set; }
        [DataMember]
        public string v_PersonId { get; set; }
        [DataMember]
        public int? i_NodeId { get; set; }
        [DataMember]
        public string v_UserName { get; set; }
        [DataMember]
        public string v_Password { get; set; }
        [DataMember]
        public string v_SecretQuestion { get; set; }
        [DataMember]
        public string v_SecretAnswer { get; set; }
        [DataMember]
        public int? i_IsDeleted { get; set; }
        [DataMember]
        public int? i_InsertUserId { get; set; }
        [DataMember]
        public DateTime? d_InsertDate { get; set; }
        [DataMember]
        public int? i_UpdateUserId { get; set; }
        [DataMember]
        public DateTime? d_UpdateDate { get; set; }
        [DataMember]
        public string v_InsertUser { get; set; }
        [DataMember]
        public string v_UpdateUser { get; set; }
        [DataMember]
        public string v_NodeName { get; set; }

        [DataMember]
        public string v_PersonName { get; set; }
        [DataMember]
        public string v_DocNumber { get; set; }
        [DataMember]
        public DateTime? d_ExpireDate { get; set; }
        [DataMember]
        public string v_ProtocolId { get; set; }

        public string v_ProtocolSystemUserId { get; set; }

        public int? i_SystemUserTypeId { get; set; }
        public int? i_ProfesionId { get; set; }
        public int? i_RolId { get; set; }

        public string v_EmpresaClienteId { get; set; }

        public int? i_CurrentOrganizationId { get; set; }
     
    }
}

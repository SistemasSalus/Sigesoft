//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:19
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    [DataContract()]
    public partial class usuarioregcovidDto
    {
        [DataMember()]
        public Int32 i_UsuarioRegcovidId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_NodeId { get; set; }

        [DataMember()]
        public String v_OrganizationId { get; set; }

        [DataMember()]
        public String v_ProtocolId { get; set; }

        [DataMember()]
        public String v_NodeName { get; set; }

        [DataMember()]
        public String v_UserName { get; set; }

        [DataMember()]
        public String v_Password { get; set; }

        [DataMember()]
        public Nullable<Int32> i_IsDeleted { get; set; }

        [DataMember()]
        public List<usuarioregcovidorganizationDto> usuarioregcovidorganization { get; set; }

        public usuarioregcovidDto()
        {
        }

        public usuarioregcovidDto(Int32 i_UsuarioRegcovidId, Nullable<Int32> i_NodeId, String v_OrganizationId, String v_ProtocolId, String v_NodeName, String v_UserName, String v_Password, Nullable<Int32> i_IsDeleted, List<usuarioregcovidorganizationDto> usuarioregcovidorganization)
        {
			this.i_UsuarioRegcovidId = i_UsuarioRegcovidId;
			this.i_NodeId = i_NodeId;
			this.v_OrganizationId = v_OrganizationId;
			this.v_ProtocolId = v_ProtocolId;
			this.v_NodeName = v_NodeName;
			this.v_UserName = v_UserName;
			this.v_Password = v_Password;
			this.i_IsDeleted = i_IsDeleted;
			this.usuarioregcovidorganization = usuarioregcovidorganization;
        }
    }
}

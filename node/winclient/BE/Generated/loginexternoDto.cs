//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:39
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
    public partial class loginexternoDto
    {
        [DataMember()]
        public Int32 i_Id { get; set; }

        [DataMember()]
        public String v_Usuario { get; set; }

        [DataMember()]
        public String v_Password { get; set; }

        [DataMember()]
        public String v_Nombres { get; set; }

        [DataMember()]
        public String v_Apellidos { get; set; }

        [DataMember()]
        public String v_OrganizationId { get; set; }

        [DataMember()]
        public DateTime d_FechaRegistro { get; set; }

        public loginexternoDto()
        {
        }

        public loginexternoDto(Int32 i_Id, String v_Usuario, String v_Password, String v_Nombres, String v_Apellidos, String v_OrganizationId, DateTime d_FechaRegistro)
        {
			this.i_Id = i_Id;
			this.v_Usuario = v_Usuario;
			this.v_Password = v_Password;
			this.v_Nombres = v_Nombres;
			this.v_Apellidos = v_Apellidos;
			this.v_OrganizationId = v_OrganizationId;
			this.d_FechaRegistro = d_FechaRegistro;
        }
    }
}

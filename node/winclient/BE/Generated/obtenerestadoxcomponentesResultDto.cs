//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:07
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
    public partial class obtenerestadoxcomponentesResultDto
    {
        [DataMember()]
        public String v_Name { get; set; }

        [DataMember()]
        public String Consultorio { get; set; }

        [DataMember()]
        public String Llamado { get; set; }

        [DataMember()]
        public String Estado { get; set; }

        public obtenerestadoxcomponentesResultDto()
        {
        }

        public obtenerestadoxcomponentesResultDto(String v_Name, String consultorio, String llamado, String estado)
        {
			this.v_Name = v_Name;
			this.Consultorio = consultorio;
			this.Llamado = llamado;
			this.Estado = estado;
        }
    }
}
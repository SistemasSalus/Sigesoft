//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:12
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
    public partial class regcovid_positivosenviadosreg_vs_positivosrecividosvigResult1Dto
    {
        [DataMember()]
        public String NombreCompleto { get; set; }

        [DataMember()]
        public String ApePaterno { get; set; }

        [DataMember()]
        public String ApeMaterno { get; set; }

        [DataMember()]
        public String Dni { get; set; }

        public regcovid_positivosenviadosreg_vs_positivosrecividosvigResult1Dto()
        {
        }

        public regcovid_positivosenviadosreg_vs_positivosrecividosvigResult1Dto(String nombreCompleto, String apePaterno, String apeMaterno, String dni)
        {
			this.NombreCompleto = nombreCompleto;
			this.ApePaterno = apePaterno;
			this.ApeMaterno = apeMaterno;
			this.Dni = dni;
        }
    }
}
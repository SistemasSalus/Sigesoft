//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:09
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
    public partial class regcovid_get_sedesResultDto
    {
        [DataMember()]
        public String Sede { get; set; }

        [DataMember()]
        public Nullable<Int32> SedeId { get; set; }

        public regcovid_get_sedesResultDto()
        {
        }

        public regcovid_get_sedesResultDto(String sede, Nullable<Int32> sedeId)
        {
			this.Sede = sede;
			this.SedeId = sedeId;
        }
    }
}
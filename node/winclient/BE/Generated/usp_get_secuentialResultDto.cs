//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:15
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
    public partial class usp_get_secuentialResultDto
    {
        [DataMember()]
        public String NewId { get; set; }

        public usp_get_secuentialResultDto()
        {
        }

        public usp_get_secuentialResultDto(String newId)
        {
			this.NewId = newId;
        }
    }
}

//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:03
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
    public partial class serversyncstatusDto
    {
        [DataMember()]
        public Int32 i_NodeId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_Enabled { get; set; }

        public serversyncstatusDto()
        {
        }

        public serversyncstatusDto(Int32 i_NodeId, Nullable<Int32> i_Enabled)
        {
			this.i_NodeId = i_NodeId;
			this.i_Enabled = i_Enabled;
        }
    }
}

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
    public partial class regcovid_get_nodesResultDto
    {
        [DataMember()]
        public Nullable<Int32> NodeId { get; set; }

        [DataMember()]
        public String NodeName { get; set; }

        public regcovid_get_nodesResultDto()
        {
        }

        public regcovid_get_nodesResultDto(Nullable<Int32> nodeId, String nodeName)
        {
			this.NodeId = nodeId;
			this.NodeName = nodeName;
        }
    }
}

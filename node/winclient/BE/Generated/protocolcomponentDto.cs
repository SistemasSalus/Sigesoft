//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:57
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
    public partial class protocolcomponentDto
    {
        [DataMember()]
        public String v_ProtocolComponentId { get; set; }

        [DataMember()]
        public String v_ProtocolId { get; set; }

        [DataMember()]
        public String v_ComponentId { get; set; }

        [DataMember()]
        public Nullable<Single> r_Price { get; set; }

        [DataMember()]
        public Nullable<Int32> i_OperatorId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_Age { get; set; }

        [DataMember()]
        public Nullable<Int32> i_GenderId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_IsConditionalId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_IsDeleted { get; set; }

        [DataMember()]
        public Nullable<Int32> i_InsertUserId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_InsertDate { get; set; }

        [DataMember()]
        public Nullable<Int32> i_UpdateUserId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_UpdateDate { get; set; }

        [DataMember()]
        public protocolDto protocol { get; set; }

        [DataMember()]
        public componentDto component { get; set; }

        public protocolcomponentDto()
        {
        }

        public protocolcomponentDto(String v_ProtocolComponentId, String v_ProtocolId, String v_ComponentId, Nullable<Single> r_Price, Nullable<Int32> i_OperatorId, Nullable<Int32> i_Age, Nullable<Int32> i_GenderId, Nullable<Int32> i_IsConditionalId, Nullable<Int32> i_IsDeleted, Nullable<Int32> i_InsertUserId, Nullable<DateTime> d_InsertDate, Nullable<Int32> i_UpdateUserId, Nullable<DateTime> d_UpdateDate, protocolDto protocol, componentDto component)
        {
			this.v_ProtocolComponentId = v_ProtocolComponentId;
			this.v_ProtocolId = v_ProtocolId;
			this.v_ComponentId = v_ComponentId;
			this.r_Price = r_Price;
			this.i_OperatorId = i_OperatorId;
			this.i_Age = i_Age;
			this.i_GenderId = i_GenderId;
			this.i_IsConditionalId = i_IsConditionalId;
			this.i_IsDeleted = i_IsDeleted;
			this.i_InsertUserId = i_InsertUserId;
			this.d_InsertDate = d_InsertDate;
			this.i_UpdateUserId = i_UpdateUserId;
			this.d_UpdateDate = d_UpdateDate;
			this.protocol = protocol;
			this.component = component;
        }
    }
}

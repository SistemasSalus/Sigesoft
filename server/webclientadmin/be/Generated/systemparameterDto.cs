//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2020/07/24 - 14:15:49
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Sigesoft.Server.WebClientAdmin.BE
{
    [DataContract()]
    public partial class systemparameterDto
    {
        [DataMember()]
        public Int32 i_GroupId { get; set; }

        [DataMember()]
        public Int32 i_ParameterId { get; set; }

        [DataMember()]
        public String v_Value1 { get; set; }

        [DataMember()]
        public String v_Value2 { get; set; }

        [DataMember()]
        public String v_Field { get; set; }

        [DataMember()]
        public Nullable<Int32> i_ParentParameterId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_Sort { get; set; }

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

        public systemparameterDto()
        {
        }

        public systemparameterDto(Int32 i_GroupId, Int32 i_ParameterId, String v_Value1, String v_Value2, String v_Field, Nullable<Int32> i_ParentParameterId, Nullable<Int32> i_Sort, Nullable<Int32> i_IsDeleted, Nullable<Int32> i_InsertUserId, Nullable<DateTime> d_InsertDate, Nullable<Int32> i_UpdateUserId, Nullable<DateTime> d_UpdateDate)
        {
			this.i_GroupId = i_GroupId;
			this.i_ParameterId = i_ParameterId;
			this.v_Value1 = v_Value1;
			this.v_Value2 = v_Value2;
			this.v_Field = v_Field;
			this.i_ParentParameterId = i_ParentParameterId;
			this.i_Sort = i_Sort;
			this.i_IsDeleted = i_IsDeleted;
			this.i_InsertUserId = i_InsertUserId;
			this.d_InsertDate = d_InsertDate;
			this.i_UpdateUserId = i_UpdateUserId;
			this.d_UpdateDate = d_UpdateDate;
        }
    }
}

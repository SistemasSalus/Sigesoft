//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2020/07/24 - 14:15:08
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
    public partial class noxioushabitsDto
    {
        [DataMember()]
        public String v_NoxiousHabitsId { get; set; }

        [DataMember()]
        public String v_PersonId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_TypeHabitsId { get; set; }

        [DataMember()]
        public String v_Frequency { get; set; }

        [DataMember()]
        public String v_Comment { get; set; }

        [DataMember()]
        public String v_DescriptionHabit { get; set; }

        [DataMember()]
        public String v_DescriptionQuantity { get; set; }

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
        public Nullable<Int32> i_AnswerId { get; set; }

        [DataMember()]
        public personDto person { get; set; }

        public noxioushabitsDto()
        {
        }

        public noxioushabitsDto(String v_NoxiousHabitsId, String v_PersonId, Nullable<Int32> i_TypeHabitsId, String v_Frequency, String v_Comment, String v_DescriptionHabit, String v_DescriptionQuantity, Nullable<Int32> i_IsDeleted, Nullable<Int32> i_InsertUserId, Nullable<DateTime> d_InsertDate, Nullable<Int32> i_UpdateUserId, Nullable<DateTime> d_UpdateDate, Nullable<Int32> i_AnswerId, personDto person)
        {
			this.v_NoxiousHabitsId = v_NoxiousHabitsId;
			this.v_PersonId = v_PersonId;
			this.i_TypeHabitsId = i_TypeHabitsId;
			this.v_Frequency = v_Frequency;
			this.v_Comment = v_Comment;
			this.v_DescriptionHabit = v_DescriptionHabit;
			this.v_DescriptionQuantity = v_DescriptionQuantity;
			this.i_IsDeleted = i_IsDeleted;
			this.i_InsertUserId = i_InsertUserId;
			this.d_InsertDate = d_InsertDate;
			this.i_UpdateUserId = i_UpdateUserId;
			this.d_UpdateDate = d_UpdateDate;
			this.i_AnswerId = i_AnswerId;
			this.person = person;
        }
    }
}
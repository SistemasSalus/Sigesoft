//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:18
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
    public partial class uspgetemployeesResultDto
    {
        [DataMember()]
        public Nullable<Guid> Id { get; set; }

        [DataMember()]
        public Int32 IdentificationType { get; set; }

        [DataMember()]
        public String NationalIDNumber { get; set; }

        [DataMember()]
        public Int32 Gender { get; set; }

        [DataMember()]
        public String FirstName { get; set; }

        [DataMember()]
        public String MiddleName { get; set; }

        [DataMember()]
        public String LastName { get; set; }

        [DataMember()]
        public Nullable<DateTime> BirthDate { get; set; }

        [DataMember()]
        public String EmailAddress { get; set; }

        [DataMember()]
        public String Job { get; set; }

        [DataMember()]
        public String AddressLine1 { get; set; }

        [DataMember()]
        public Nullable<Int32> Hire { get; set; }

        [DataMember()]
        public Nullable<Int32> Headquarter { get; set; }

        [DataMember()]
        public Nullable<Int32> Employer { get; set; }

        public uspgetemployeesResultDto()
        {
        }

        public uspgetemployeesResultDto(Nullable<Guid> id, Int32 identificationType, String nationalIDNumber, Int32 gender, String firstName, String middleName, String lastName, Nullable<DateTime> birthDate, String emailAddress, String job, String addressLine1, Nullable<Int32> hire, Nullable<Int32> headquarter, Nullable<Int32> employer)
        {
			this.Id = id;
			this.IdentificationType = identificationType;
			this.NationalIDNumber = nationalIDNumber;
			this.Gender = gender;
			this.FirstName = firstName;
			this.MiddleName = middleName;
			this.LastName = lastName;
			this.BirthDate = birthDate;
			this.EmailAddress = emailAddress;
			this.Job = job;
			this.AddressLine1 = addressLine1;
			this.Hire = hire;
			this.Headquarter = headquarter;
			this.Employer = employer;
        }
    }
}

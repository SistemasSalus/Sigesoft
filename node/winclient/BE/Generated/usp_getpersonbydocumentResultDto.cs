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
    public partial class usp_getpersonbydocumentResultDto
    {
        [DataMember()]
        public String PersonId { get; set; }

        [DataMember()]
        public String FirstName { get; set; }

        [DataMember()]
        public String FirstLastName { get; set; }

        [DataMember()]
        public String SecondLastName { get; set; }

        [DataMember()]
        public Nullable<Int32> DocTypeId { get; set; }

        [DataMember()]
        public String DocNumber { get; set; }

        [DataMember()]
        public Nullable<Int32> SexTypeId { get; set; }

        [DataMember()]
        public Nullable<DateTime> Birthdate { get; set; }

        [DataMember()]
        public String Mail { get; set; }

        [DataMember()]
        public String TelephoneNumber { get; set; }

        [DataMember()]
        public String CurrentOccupation { get; set; }

        [DataMember()]
        public String AdressLocation { get; set; }

        public usp_getpersonbydocumentResultDto()
        {
        }

        public usp_getpersonbydocumentResultDto(String personId, String firstName, String firstLastName, String secondLastName, Nullable<Int32> docTypeId, String docNumber, Nullable<Int32> sexTypeId, Nullable<DateTime> birthdate, String mail, String telephoneNumber, String currentOccupation, String adressLocation)
        {
			this.PersonId = personId;
			this.FirstName = firstName;
			this.FirstLastName = firstLastName;
			this.SecondLastName = secondLastName;
			this.DocTypeId = docTypeId;
			this.DocNumber = docNumber;
			this.SexTypeId = sexTypeId;
			this.Birthdate = birthdate;
			this.Mail = mail;
			this.TelephoneNumber = telephoneNumber;
			this.CurrentOccupation = currentOccupation;
			this.AdressLocation = adressLocation;
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sigesoft.Node.WinClient.BE
{
    [DataContract]
    public class OrganizationList
    {

        [DataMember]
        public string v_OrganizationId { get; set; }
        [DataMember]
        public Int32 i_OrganizationTypeId { get; set; }
        [DataMember]
        public string v_OrganizationTypeIdName { get; set; }
        [DataMember]
        public Int32 i_UserInterfaceOrder { get; set; }
        [DataMember]
        public Int32 i_SectorTypeId { get; set; }
        [DataMember]
        public string v_SectorTypeIdName { get; set; }
        [DataMember]
        public string v_IdentificationNumber { get; set; }
        [DataMember]
        public string v_Name { get; set; }
        [DataMember]
        public string v_Address { get; set; }
        [DataMember]
        public string v_PhoneNumber { get; set; }
        [DataMember]
        public string v_Mail { get; set; }
        [DataMember]
        public string v_CreationUser { get; set; }
        [DataMember]
        public string v_UpdateUser { get; set; }
        [DataMember]
        public DateTime? d_CreationDate { get; set; }
        [DataMember]
        public DateTime? d_UpdateDate { get; set; }
        [DataMember]
        public int? i_IsDeleted { get; set; }

        public string v_LocationId { get; set; }
        public bool b_Seleccionar { get; set; }
    }
}

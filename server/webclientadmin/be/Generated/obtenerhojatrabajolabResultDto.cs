//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2020/07/24 - 14:14:31
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
    public partial class obtenerhojatrabajolabResultDto
    {
        [DataMember()]
        public String ID { get; set; }

        [DataMember()]
        public String column1 { get; set; }

        [DataMember()]
        public Nullable<Int32> EDAD { get; set; }

        [DataMember()]
        public String SEXO { get; set; }

        [DataMember()]
        public String NOMBRE { get; set; }

        [DataMember()]
        public String GRUPO { get; set; }

        [DataMember()]
        public String C { get; set; }

        [DataMember()]
        public String PH { get; set; }

        [DataMember()]
        public String D { get; set; }

        [DataMember()]
        public String BQ { get; set; }

        [DataMember()]
        public String CEL { get; set; }

        [DataMember()]
        public String LEU { get; set; }

        [DataMember()]
        public String HEM { get; set; }

        [DataMember()]
        public String OBS { get; set; }

        [DataMember()]
        public String P_ { get; set; }

        [DataMember()]
        public String P_1 { get; set; }

        [DataMember()]
        public String P_2 { get; set; }

        public obtenerhojatrabajolabResultDto()
        {
        }

        public obtenerhojatrabajolabResultDto(String iD, String column1, Nullable<Int32> eDAD, String sEXO, String nOMBRE, String gRUPO, String c, String pH, String d, String bQ, String cEL, String lEU, String hEM, String oBS, String p_, String p_1, String p_2)
        {
			this.ID = iD;
			this.column1 = column1;
			this.EDAD = eDAD;
			this.SEXO = sEXO;
			this.NOMBRE = nOMBRE;
			this.GRUPO = gRUPO;
			this.C = c;
			this.PH = pH;
			this.D = d;
			this.BQ = bQ;
			this.CEL = cEL;
			this.LEU = lEU;
			this.HEM = hEM;
			this.OBS = oBS;
			this.P_ = p_;
			this.P_1 = p_1;
			this.P_2 = p_2;
        }
    }
}

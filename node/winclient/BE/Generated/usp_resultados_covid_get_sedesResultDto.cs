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
    public partial class usp_resultados_covid_get_sedesResultDto
    {
        [DataMember()]
        public String column0 { get; set; }

        [DataMember()]
        public String column1 { get; set; }

        public usp_resultados_covid_get_sedesResultDto()
        {
        }

        public usp_resultados_covid_get_sedesResultDto(String column0, String column1)
        {
			this.column0 = column0;
			this.column1 = column1;
        }
    }
}

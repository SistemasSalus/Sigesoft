//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:14
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
    public partial class regcovid_sp_getempresasprincipalesResultDto
    {
        [DataMember()]
        public String id { get; set; }

        [DataMember()]
        public String nombre { get; set; }

        [DataMember()]
        public String nombreAbrev { get; set; }

        public regcovid_sp_getempresasprincipalesResultDto()
        {
        }

        public regcovid_sp_getempresasprincipalesResultDto(String id, String nombre, String nombreAbrev)
        {
			this.id = id;
			this.nombre = nombre;
			this.nombreAbrev = nombreAbrev;
        }
    }
}
//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:17
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
    public partial class usp_resultados_covid_get_resultadosResultDto
    {
        [DataMember()]
        public Nullable<Int64> Nro { get; set; }

        [DataMember()]
        public String Trabajador { get; set; }

        [DataMember()]
        public String Sede { get; set; }

        [DataMember()]
        public String Examen { get; set; }

        [DataMember()]
        public String Fecha { get; set; }

        [DataMember()]
        public String Archivo { get; set; }

        public usp_resultados_covid_get_resultadosResultDto()
        {
        }

        public usp_resultados_covid_get_resultadosResultDto(Nullable<Int64> nro, String trabajador, String sede, String examen, String fecha, String archivo)
        {
			this.Nro = nro;
			this.Trabajador = trabajador;
			this.Sede = sede;
			this.Examen = examen;
			this.Fecha = fecha;
			this.Archivo = archivo;
        }
    }
}

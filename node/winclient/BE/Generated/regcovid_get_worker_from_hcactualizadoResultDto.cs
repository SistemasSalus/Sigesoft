//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:10
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
    public partial class regcovid_get_worker_from_hcactualizadoResultDto
    {
        [DataMember()]
        public String Filtro { get; set; }

        [DataMember()]
        public Nullable<Int32> HcActualizadoId { get; set; }

        [DataMember()]
        public String HC { get; set; }

        [DataMember()]
        public String EmpresaEmpleadora { get; set; }

        [DataMember()]
        public String ApellidoPaterno { get; set; }

        [DataMember()]
        public String ApellidoMaterno { get; set; }

        [DataMember()]
        public String Nombres { get; set; }

        [DataMember()]
        public String Dni { get; set; }

        [DataMember()]
        public String Sexo { get; set; }

        [DataMember()]
        public String Sede { get; set; }

        [DataMember()]
        public Nullable<Int32> SedeId { get; set; }

        [DataMember()]
        public String Puesto { get; set; }

        [DataMember()]
        public Nullable<DateTime> FechaNacimiento { get; set; }

        [DataMember()]
        public Nullable<Int32> SexoId { get; set; }

        [DataMember()]
        public Nullable<Int32> HcId { get; set; }

        [DataMember()]
        public String Telefono { get; set; }

        [DataMember()]
        public String Email { get; set; }

        [DataMember()]
        public String Direccion { get; set; }

        public regcovid_get_worker_from_hcactualizadoResultDto()
        {
        }

        public regcovid_get_worker_from_hcactualizadoResultDto(String filtro, Nullable<Int32> hcActualizadoId, String hC, String empresaEmpleadora, String apellidoPaterno, String apellidoMaterno, String nombres, String dni, String sexo, String sede, Nullable<Int32> sedeId, String puesto, Nullable<DateTime> fechaNacimiento, Nullable<Int32> sexoId, Nullable<Int32> hcId, String telefono, String email, String direccion)
        {
			this.Filtro = filtro;
			this.HcActualizadoId = hcActualizadoId;
			this.HC = hC;
			this.EmpresaEmpleadora = empresaEmpleadora;
			this.ApellidoPaterno = apellidoPaterno;
			this.ApellidoMaterno = apellidoMaterno;
			this.Nombres = nombres;
			this.Dni = dni;
			this.Sexo = sexo;
			this.Sede = sede;
			this.SedeId = sedeId;
			this.Puesto = puesto;
			this.FechaNacimiento = fechaNacimiento;
			this.SexoId = sexoId;
			this.HcId = hcId;
			this.Telefono = telefono;
			this.Email = email;
			this.Direccion = direccion;
        }
    }
}
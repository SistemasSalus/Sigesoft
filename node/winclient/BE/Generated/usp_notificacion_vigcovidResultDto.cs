//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:16
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
    public partial class usp_notificacion_vigcovidResultDto
    {
        [DataMember()]
        public Int32 Id { get; set; }

        [DataMember()]
        public String Trabajador { get; set; }

        [DataMember()]
        public DateTime FechaIngreso { get; set; }

        [DataMember()]
        public String Dni { get; set; }

        [DataMember()]
        public Nullable<Int32> Edad { get; set; }

        [DataMember()]
        public String Empleadora { get; set; }

        [DataMember()]
        public String PuestoTrabajo { get; set; }

        [DataMember()]
        public String EmailTrabajador { get; set; }

        [DataMember()]
        public String EmailAnalista { get; set; }

        [DataMember()]
        public String EmailBP { get; set; }

        [DataMember()]
        public String EmailChampion { get; set; }

        [DataMember()]
        public String EmailSeguridadFisica { get; set; }

        [DataMember()]
        public String MedicoVigila { get; set; }

        public usp_notificacion_vigcovidResultDto()
        {
        }

        public usp_notificacion_vigcovidResultDto(Int32 id, String trabajador, DateTime fechaIngreso, String dni, Nullable<Int32> edad, String empleadora, String puestoTrabajo, String emailTrabajador, String emailAnalista, String emailBP, String emailChampion, String emailSeguridadFisica, String medicoVigila)
        {
			this.Id = id;
			this.Trabajador = trabajador;
			this.FechaIngreso = fechaIngreso;
			this.Dni = dni;
			this.Edad = edad;
			this.Empleadora = empleadora;
			this.PuestoTrabajo = puestoTrabajo;
			this.EmailTrabajador = emailTrabajador;
			this.EmailAnalista = emailAnalista;
			this.EmailBP = emailBP;
			this.EmailChampion = emailChampion;
			this.EmailSeguridadFisica = emailSeguridadFisica;
			this.MedicoVigila = medicoVigila;
        }
    }
}

//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2020/07/24 - 14:14:53
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
    public partial class headcountDto
    {
        [DataMember()]
        public Int32 i_HcId { get; set; }

        [DataMember()]
        public String v_Nif { get; set; }

        [DataMember()]
        public String v_NombreCompleto { get; set; }

        [DataMember()]
        public String v_Soc { get; set; }

        [DataMember()]
        public String v_Sharp { get; set; }

        [DataMember()]
        public String v_NroPers { get; set; }

        [DataMember()]
        public String v_Apellido { get; set; }

        [DataMember()]
        public String v_SegundoApellido { get; set; }

        [DataMember()]
        public String v_NombrePila { get; set; }

        [DataMember()]
        public String v_ClaveSexo { get; set; }

        [DataMember()]
        public String v_Aliq { get; set; }

        [DataMember()]
        public String v_AreaNomina { get; set; }

        [DataMember()]
        public String v_SdvPer { get; set; }

        [DataMember()]
        public String v_SubDivPersona { get; set; }

        [DataMember()]
        public String v_StatusOcupacion { get; set; }

        [DataMember()]
        public String v_Cife { get; set; }

        [DataMember()]
        public String v_ClaseFecha { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_Fecha { get; set; }

        [DataMember()]
        public String v_GrupoPersonal { get; set; }

        [DataMember()]
        public String v_Posicion { get; set; }

        [DataMember()]
        public String v_PosicionNombre { get; set; }

        [DataMember()]
        public String v_UnOrg { get; set; }

        [DataMember()]
        public String v_UnidadOrganizativa { get; set; }

        [DataMember()]
        public String v_IndicadorExceptions { get; set; }

        [DataMember()]
        public String v_DivP { get; set; }

        [DataMember()]
        public String v_DivisionPersonal { get; set; }

        [DataMember()]
        public String v_CECoste { get; set; }

        [DataMember()]
        public String v_CentroCoste { get; set; }

        [DataMember()]
        public String v_RelacionLaboral { get; set; }

        [DataMember()]
        public String v_Apers { get; set; }

        [DataMember()]
        public String v_Area { get; set; }

        [DataMember()]
        public String v_GrProf { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_Desde { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_Hasta { get; set; }

        [DataMember()]
        public String v_ClaseContrato { get; set; }

        [DataMember()]
        public String v_FinContrato { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_FechaNacimiento { get; set; }

        [DataMember()]
        public String v_EPS { get; set; }

        [DataMember()]
        public String v_Sindicato { get; set; }

        [DataMember()]
        public Nullable<Int32> i_Edad { get; set; }

        [DataMember()]
        public Nullable<Int32> i_IsDeleted { get; set; }

        public headcountDto()
        {
        }

        public headcountDto(Int32 i_HcId, String v_Nif, String v_NombreCompleto, String v_Soc, String v_Sharp, String v_NroPers, String v_Apellido, String v_SegundoApellido, String v_NombrePila, String v_ClaveSexo, String v_Aliq, String v_AreaNomina, String v_SdvPer, String v_SubDivPersona, String v_StatusOcupacion, String v_Cife, String v_ClaseFecha, Nullable<DateTime> d_Fecha, String v_GrupoPersonal, String v_Posicion, String v_PosicionNombre, String v_UnOrg, String v_UnidadOrganizativa, String v_IndicadorExceptions, String v_DivP, String v_DivisionPersonal, String v_CECoste, String v_CentroCoste, String v_RelacionLaboral, String v_Apers, String v_Area, String v_GrProf, Nullable<DateTime> d_Desde, Nullable<DateTime> d_Hasta, String v_ClaseContrato, String v_FinContrato, Nullable<DateTime> d_FechaNacimiento, String v_EPS, String v_Sindicato, Nullable<Int32> i_Edad, Nullable<Int32> i_IsDeleted)
        {
			this.i_HcId = i_HcId;
			this.v_Nif = v_Nif;
			this.v_NombreCompleto = v_NombreCompleto;
			this.v_Soc = v_Soc;
			this.v_Sharp = v_Sharp;
			this.v_NroPers = v_NroPers;
			this.v_Apellido = v_Apellido;
			this.v_SegundoApellido = v_SegundoApellido;
			this.v_NombrePila = v_NombrePila;
			this.v_ClaveSexo = v_ClaveSexo;
			this.v_Aliq = v_Aliq;
			this.v_AreaNomina = v_AreaNomina;
			this.v_SdvPer = v_SdvPer;
			this.v_SubDivPersona = v_SubDivPersona;
			this.v_StatusOcupacion = v_StatusOcupacion;
			this.v_Cife = v_Cife;
			this.v_ClaseFecha = v_ClaseFecha;
			this.d_Fecha = d_Fecha;
			this.v_GrupoPersonal = v_GrupoPersonal;
			this.v_Posicion = v_Posicion;
			this.v_PosicionNombre = v_PosicionNombre;
			this.v_UnOrg = v_UnOrg;
			this.v_UnidadOrganizativa = v_UnidadOrganizativa;
			this.v_IndicadorExceptions = v_IndicadorExceptions;
			this.v_DivP = v_DivP;
			this.v_DivisionPersonal = v_DivisionPersonal;
			this.v_CECoste = v_CECoste;
			this.v_CentroCoste = v_CentroCoste;
			this.v_RelacionLaboral = v_RelacionLaboral;
			this.v_Apers = v_Apers;
			this.v_Area = v_Area;
			this.v_GrProf = v_GrProf;
			this.d_Desde = d_Desde;
			this.d_Hasta = d_Hasta;
			this.v_ClaseContrato = v_ClaseContrato;
			this.v_FinContrato = v_FinContrato;
			this.d_FechaNacimiento = d_FechaNacimiento;
			this.v_EPS = v_EPS;
			this.v_Sindicato = v_Sindicato;
			this.i_Edad = i_Edad;
			this.i_IsDeleted = i_IsDeleted;
        }
    }
}

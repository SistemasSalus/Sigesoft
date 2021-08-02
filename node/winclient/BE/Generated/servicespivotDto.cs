//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:12
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
    public partial class servicespivotDto
    {
        [DataMember()]
        public Int32 servicespivot1 { get; set; }

        [DataMember()]
        public String ServiceId { get; set; }

        [DataMember()]
        public String ProtocolId { get; set; }

        [DataMember()]
        public String ComponentId { get; set; }

        [DataMember()]
        public String Component { get; set; }

        [DataMember()]
        public String PersonId { get; set; }

        [DataMember()]
        public String ServiceComponentId { get; set; }

        [DataMember()]
        public String DNI { get; set; }

        [DataMember()]
        public String Nombre { get; set; }

        [DataMember()]
        public String ApellidoP { get; set; }

        [DataMember()]
        public String ApellidoM { get; set; }

        [DataMember()]
        public String NombresApellidos { get; set; }

        [DataMember()]
        public String Correo { get; set; }

        [DataMember()]
        public String Telefono { get; set; }

        [DataMember()]
        public String Direccion { get; set; }

        [DataMember()]
        public String OrganizationId { get; set; }

        [DataMember()]
        public String EmpresaFacturacion { get; set; }

        [DataMember()]
        public String EmpresaPrincipalID { get; set; }

        [DataMember()]
        public String EmpresaPrincipal { get; set; }

        [DataMember()]
        public String Puesto { get; set; }

        [DataMember()]
        public Nullable<DateTime> FechaRegistro { get; set; }

        [DataMember()]
        public Nullable<DateTime> FechaServicio { get; set; }

        [DataMember()]
        public Nullable<Int32> Eliminado { get; set; }

        [DataMember()]
        public String EmpresaEmpleadora { get; set; }

        [DataMember()]
        public Nullable<Int32> TipoEmpresa { get; set; }

        [DataMember()]
        public String Sede { get; set; }

        [DataMember()]
        public String Usuario { get; set; }

        [DataMember()]
        public Nullable<Int32> CorreoEnviado { get; set; }

        [DataMember()]
        public Nullable<Int32> Encuesta { get; set; }

        [DataMember()]
        public Nullable<Int32> Laboratorio { get; set; }

        [DataMember()]
        public Nullable<Int32> Profesion { get; set; }

        [DataMember()]
        public Nullable<Int32> Es_Personal_salud { get; set; }

        [DataMember()]
        public Nullable<Int32> Sintomas { get; set; }

        [DataMember()]
        public Nullable<Boolean> Abdominal { get; set; }

        [DataMember()]
        public Nullable<Int32> Articulaciones { get; set; }

        [DataMember()]
        public Nullable<Boolean> Asma { get; set; }

        [DataMember()]
        public Nullable<Boolean> Cancer { get; set; }

        [DataMember()]
        public Nullable<Boolean> Cefalea { get; set; }

        [DataMember()]
        public Nullable<Int32> Clas_clinica { get; set; }

        [DataMember()]
        public Nullable<Boolean> Congestion_nasal { get; set; }

        [DataMember()]
        public Nullable<Boolean> Diabetes { get; set; }

        [DataMember()]
        public Nullable<Boolean> Diarrea { get; set; }

        [DataMember()]
        public Nullable<Boolean> Dolor { get; set; }

        [DataMember()]
        public Nullable<Boolean> Dolor_garganta { get; set; }

        [DataMember()]
        public Nullable<Int32> Domicilio_residencia { get; set; }

        [DataMember()]
        public Nullable<Int32> Embarazo_puerperio { get; set; }

        [DataMember()]
        public Nullable<Boolean> Enf_cardiovasculares { get; set; }

        [DataMember()]
        public Nullable<Boolean> Enf_tratamiento_inmunosupresor { get; set; }

        [DataMember()]
        public Nullable<Boolean> Enf_pulmonar_cronica { get; set; }

        [DataMember()]
        public Nullable<DateTime> Fecha_Prueba { get; set; }

        [DataMember()]
        public Nullable<Boolean> Fiebre_Escalofrio { get; set; }

        [DataMember()]
        public Nullable<Int32> Geolocalizacion_domicilio { get; set; }

        [DataMember()]
        public Nullable<Boolean> Hipertencion_arterial { get; set; }

        [DataMember()]
        public String Inicio_sintomas { get; set; }

        [DataMember()]
        public Nullable<Int32> Insuficiencia_cronica { get; set; }

        [DataMember()]
        public Nullable<Int32> Irritabilidad_confusion { get; set; }

        [DataMember()]
        public Nullable<Boolean> Malestar_general { get; set; }

        [DataMember()]
        public Nullable<Int32> Mayor65 { get; set; }

        [DataMember()]
        public Nullable<Boolean> Muscular { get; set; }

        [DataMember()]
        public Nullable<Int32> Nauseas_vomitos { get; set; }

        [DataMember()]
        public Nullable<Boolean> Obesidad { get; set; }

        [DataMember()]
        public String Otros_sintomas { get; set; }

        [DataMember()]
        public Nullable<Int32> Pecho { get; set; }

        [DataMember()]
        public Nullable<Int32> Personal_salud { get; set; }

        [DataMember()]
        public Nullable<Int32> Procedencia_solicitud { get; set; }

        [DataMember()]
        public Nullable<Int32> Resultado_1ERA { get; set; }

        [DataMember()]
        public Nullable<Int32> Resultado_2DA { get; set; }

        [DataMember()]
        public Nullable<Boolean> Tos { get; set; }

        public servicespivotDto()
        {
        }

        public servicespivotDto(Int32 servicespivot1, String serviceId, String protocolId, String componentId, String component, String personId, String serviceComponentId, String dNI, String nombre, String apellidoP, String apellidoM, String nombresApellidos, String correo, String telefono, String direccion, String organizationId, String empresaFacturacion, String empresaPrincipalID, String empresaPrincipal, String puesto, Nullable<DateTime> fechaRegistro, Nullable<DateTime> fechaServicio, Nullable<Int32> eliminado, String empresaEmpleadora, Nullable<Int32> tipoEmpresa, String sede, String usuario, Nullable<Int32> correoEnviado, Nullable<Int32> encuesta, Nullable<Int32> laboratorio, Nullable<Int32> profesion, Nullable<Int32> es_Personal_salud, Nullable<Int32> sintomas, Nullable<Boolean> abdominal, Nullable<Int32> articulaciones, Nullable<Boolean> asma, Nullable<Boolean> cancer, Nullable<Boolean> cefalea, Nullable<Int32> clas_clinica, Nullable<Boolean> congestion_nasal, Nullable<Boolean> diabetes, Nullable<Boolean> diarrea, Nullable<Boolean> dolor, Nullable<Boolean> dolor_garganta, Nullable<Int32> domicilio_residencia, Nullable<Int32> embarazo_puerperio, Nullable<Boolean> enf_cardiovasculares, Nullable<Boolean> enf_tratamiento_inmunosupresor, Nullable<Boolean> enf_pulmonar_cronica, Nullable<DateTime> fecha_Prueba, Nullable<Boolean> fiebre_Escalofrio, Nullable<Int32> geolocalizacion_domicilio, Nullable<Boolean> hipertencion_arterial, String inicio_sintomas, Nullable<Int32> insuficiencia_cronica, Nullable<Int32> irritabilidad_confusion, Nullable<Boolean> malestar_general, Nullable<Int32> mayor65, Nullable<Boolean> muscular, Nullable<Int32> nauseas_vomitos, Nullable<Boolean> obesidad, String otros_sintomas, Nullable<Int32> pecho, Nullable<Int32> personal_salud, Nullable<Int32> procedencia_solicitud, Nullable<Int32> resultado_1ERA, Nullable<Int32> resultado_2DA, Nullable<Boolean> tos)
        {
			this.servicespivot1 = servicespivot1;
			this.ServiceId = serviceId;
			this.ProtocolId = protocolId;
			this.ComponentId = componentId;
			this.Component = component;
			this.PersonId = personId;
			this.ServiceComponentId = serviceComponentId;
			this.DNI = dNI;
			this.Nombre = nombre;
			this.ApellidoP = apellidoP;
			this.ApellidoM = apellidoM;
			this.NombresApellidos = nombresApellidos;
			this.Correo = correo;
			this.Telefono = telefono;
			this.Direccion = direccion;
			this.OrganizationId = organizationId;
			this.EmpresaFacturacion = empresaFacturacion;
			this.EmpresaPrincipalID = empresaPrincipalID;
			this.EmpresaPrincipal = empresaPrincipal;
			this.Puesto = puesto;
			this.FechaRegistro = fechaRegistro;
			this.FechaServicio = fechaServicio;
			this.Eliminado = eliminado;
			this.EmpresaEmpleadora = empresaEmpleadora;
			this.TipoEmpresa = tipoEmpresa;
			this.Sede = sede;
			this.Usuario = usuario;
			this.CorreoEnviado = correoEnviado;
			this.Encuesta = encuesta;
			this.Laboratorio = laboratorio;
			this.Profesion = profesion;
			this.Es_Personal_salud = es_Personal_salud;
			this.Sintomas = sintomas;
			this.Abdominal = abdominal;
			this.Articulaciones = articulaciones;
			this.Asma = asma;
			this.Cancer = cancer;
			this.Cefalea = cefalea;
			this.Clas_clinica = clas_clinica;
			this.Congestion_nasal = congestion_nasal;
			this.Diabetes = diabetes;
			this.Diarrea = diarrea;
			this.Dolor = dolor;
			this.Dolor_garganta = dolor_garganta;
			this.Domicilio_residencia = domicilio_residencia;
			this.Embarazo_puerperio = embarazo_puerperio;
			this.Enf_cardiovasculares = enf_cardiovasculares;
			this.Enf_tratamiento_inmunosupresor = enf_tratamiento_inmunosupresor;
			this.Enf_pulmonar_cronica = enf_pulmonar_cronica;
			this.Fecha_Prueba = fecha_Prueba;
			this.Fiebre_Escalofrio = fiebre_Escalofrio;
			this.Geolocalizacion_domicilio = geolocalizacion_domicilio;
			this.Hipertencion_arterial = hipertencion_arterial;
			this.Inicio_sintomas = inicio_sintomas;
			this.Insuficiencia_cronica = insuficiencia_cronica;
			this.Irritabilidad_confusion = irritabilidad_confusion;
			this.Malestar_general = malestar_general;
			this.Mayor65 = mayor65;
			this.Muscular = muscular;
			this.Nauseas_vomitos = nauseas_vomitos;
			this.Obesidad = obesidad;
			this.Otros_sintomas = otros_sintomas;
			this.Pecho = pecho;
			this.Personal_salud = personal_salud;
			this.Procedencia_solicitud = procedencia_solicitud;
			this.Resultado_1ERA = resultado_1ERA;
			this.Resultado_2DA = resultado_2DA;
			this.Tos = tos;
        }
    }
}
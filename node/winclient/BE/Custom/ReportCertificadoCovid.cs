using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ReportCertificadoCovid
    {

              public DateTime FechaActual { get; set; }
              public string Empleador {get; set;}
              public string EmpresaPrincipal {get; set;}
              public string Sede {get; set;}
              public string Area {get; set;}
              public string Puesto {get; set;}
              public string NroIdentificacionSalus {get; set;}
              public string ApellidoPaternoSalus {get; set;}
              public string ApellidoMaternoSalus {get; set;}
              public string NombresSalus {get; set;}
              public string TipoDoc {get; set;}
              public string NroDoc {get; set;}
              public string ApellidoPaterno {get; set;}
              public string ApellidoMaterno {get; set;}
              public string Nombres {get; set;}
              public string Edad {get; set;}
              public string Sexo {get; set;}
              public string Celular {get; set;}
              public string OtroTelefono {get; set;}
              public string Domicilio {get; set;}
              public string Direccion {get; set;}
              public string Departamento {get; set;}
              public string Provincia {get; set;}
              public string Geolocalizacion {get; set;}
              public string Temperatura {get; set;}
              public string Peso {get; set;}
              public string Talla {get; set;}
              public string Imc {get; set;}
              public string EspersonalSalud {get; set;}
              public string Profesion {get; set;}
              public string TieneSintomas {get; set;}
              public string FechaSintomas {get; set;}
              public string Tos {get; set;}
              public string DolorGarganta {get; set;}
              public string CongestionNasal {get; set;}
              public string DificultadRespiratoria {get; set;}
              public string Fiebre {get; set;}
              public string Malestar {get; set;}
              public string Diarrea {get; set;}
              public string Nauseas {get; set;}
              public string Cefalea {get; set;}
              public string Irritabilidad {get; set;}
              public string Dolor {get; set;}
              public string Otros {get; set;}
              public string Muscular {get; set;}
              public string Abdominal {get; set;}
              public string Pecho {get; set;}
              public string Articulaciones {get; set;}
              public string OtrosSintomas {get; set;}
              public string FechaPruebaRapida {get; set;}
              public string ProcedenciaSolicitud {get; set;}
              public string ResultadoPrueba {get; set;}
              public string FotografiaPruebaRapida {get; set;}
              public string ResultadoSegundaPrueba {get; set;}
              public string FotografiaSegundaPrueba {get; set;}
              public string Seveidad {get; set;}
              public string Mayor60 {get; set;}
              public string HipertencionArterial {get; set;}
              public string EnfCardio {get; set;}
              public string Diabetes {get; set;}
              public string Obesidad {get; set;}
              public string Asma {get; set;}
              public string EnfPulmonarCronica {get; set;}
              public string InsuficienciaRenal {get; set;}
              public string EbfInmunosupresor {get; set;}
              public string Cancer {get; set;}
              public string Embarazo {get; set;}
              public string PersonalSalud {get; set;}
              public string Pcr {get; set;}
              public string SeguimientoProcede {get; set;}
              public string Observacion {get; set;}
              public int AptitudId { get; set; }
              public string Aptitud {get; set;}
              public byte[] Firma {get; set;}
              public byte[] FirmaTrabajador { get; set; }


              public int ViajeFueraId { get; set; }
              public string ViajeFuera { get; set; }

              public int ContactoCovidId { get; set; }
              public string ContactoCovid { get; set; }

              public int EstablecimientoId { get; set; }
              public string Establecimiento { get; set; }

              public int TomaMedicacionId { get; set; }
              public string TomaMedicacion { get; set; }

              public string Porque { get; set; }

              public DateTime FechaNacimiento { get; set; }

              public string NombresCompletosTrabajador { get; set; }
              public string ServicioId { get; set; }
              public string Protocolo { get; set; }
              public string ValorIgM { get; set; }
              public string ValoIgG { get; set; }
              public string Origen { get; set; }
              public string TomaMuestra { get; set; }

              public string PrimerResultadoCovid { get; set; }
              public string SegundoResultadoCovid { get; set; }
       
    }

   public class ReportCertificadoDescensoCovid
   {

       public DateTime FechaActual { get; set; }
       public string Empleador { get; set; }
       public string EmpresaPrincipal { get; set; }
       public string Sede { get; set; }
       public string Area { get; set; }
       public string Puesto { get; set; }
       public string NroIdentificacionSalus { get; set; }
       public string ApellidoPaternoSalus { get; set; }
       public string ApellidoMaternoSalus { get; set; }
       public string NombresSalus { get; set; }
       public string TipoDoc { get; set; }
       public string NroDoc { get; set; }
       public string ApellidoPaterno { get; set; }
       public string ApellidoMaterno { get; set; }
       public string Nombres { get; set; }
       public string Edad { get; set; }
       public string Sexo { get; set; }
       public string Celular { get; set; }
       public string OtroTelefono { get; set; }
       public string Direccion { get; set; }
       public string Departamento { get; set; }
       public string Provincia { get; set; }

       public string Domicilio { get; set; }
       public string EstablecimientoSalud { get; set; }
       public string TomaMedicacion { get; set; }


       public string CheckEspersonalSalud { get; set; }
       public string Profesion { get; set; }
       public string TieneSintomas { get; set; }
       public string FechaSintomas { get; set; }
       public string Tos { get; set; }
       public string DolorGarganta { get; set; }
       public string CongestionNasal { get; set; }
       public string DificultadRespiratoria { get; set; }
       public string Fiebre { get; set; }
       public string Malestar { get; set; }
       public string Diarrea { get; set; }
       public string Nauseas { get; set; }
       public string Cefalea { get; set; }
       public string Irritabilidad { get; set; }
       public string Dolor { get; set; }
       public string Otros { get; set; }
       public string Muscular { get; set; }
       public string Abdominal { get; set; }
       public string Pecho { get; set; }
       public string Articulaciones { get; set; }
       public string OtrosSintomas { get; set; }       
       public string Seveidad { get; set; }
       public string Mayor60 { get; set; }
       public string HipertencionArterial { get; set; }
       public string EnfCardio { get; set; }
       public string Diabetes { get; set; }
       public string Obesidad { get; set; }
       public string Asma { get; set; }
       public string EnfPulmonarCronica { get; set; }
       public string InsuficienciaRenal { get; set; }
       public string EbfInmunosupresor { get; set; }
       public string Cancer { get; set; }
       public string Embarazo { get; set; }
       public string PreguntaPersonalSalud { get; set; }
       public string Expectoracion { get; set; }


       public string Temperatura { get; set; }
       public string FrecuenciaRespiratoria { get; set; }
       public string SatO2 { get; set; }

       public string ClasificacionClinica { get; set; }    

       public int AptitudId { get; set; }
       public string Aptitud { get; set; }
       public byte[] Firma { get; set; }
       public DateTime FechaNacimiento { get; set; }

   }
}

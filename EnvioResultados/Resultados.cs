using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Dynamic;
using System.IO;

namespace EnvioResultados
{
    public class Resultados
    {
        public DateTime _fecha { get; set; }
        public string _ruta { get; set; }

        public Resultados(DateTime fecha, string ruta)
        {
            _fecha = fecha;
            _ruta = ruta;
        }

        public List<ServiciosEmail> BuscarServiciosPendientesEmail(){

            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var fechaInicio = _fecha.Date;
            var fechaFin = _fecha.AddDays(1);

            var query = (from ser in dbContext.service 
                         join prot in dbContext.protocol on ser.v_ProtocolId equals prot.v_ProtocolId
                         where ser.d_InsertDate >= fechaInicio && ser.d_InsertDate <= fechaFin
                         //&& prot.v_CustomerOrganizationId != Constants.EMPRESA_BACKUS_ID
                         //&& prot.v_CustomerOrganizationId != "N003-OO000000962"
                             && ser.CorreoEnviado == null
                             select new ServiciosEmail{
                             ServiceId =  ser.v_ServiceId,
                             OrganizationId = prot.v_CustomerOrganizationId,
                             ClinicaExterna = ser.ClinicaExternad
                        }).ToList();

            return query;

        }

        public List<ReportCertificadoCovid> GetCovid(ref OperationResult pobjOperationResult, string pstrServiceId)
        {
            //mon.IsActive = true;
            var isDeleted = 0;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var groupUbigeo = 113;
                var query = (from ser in dbContext.service

                             join prot in dbContext.protocol on ser.v_ProtocolId equals prot.v_ProtocolId into prot_join
                             from prot in prot_join.DefaultIfEmpty()

                             join org in dbContext.organization on prot.v_CustomerOrganizationId equals org.v_OrganizationId into org_join
                             from org in org_join.DefaultIfEmpty()

                             join per in dbContext.person on ser.v_PersonId equals per.v_PersonId into per_join
                             from per in per_join.DefaultIfEmpty()

                             join M in dbContext.systemparameter on new { a = per.i_SexTypeId.Value, b = 100 }
                                 equals new { a = M.i_ParameterId, b = M.i_GroupId } into M_join
                             from M in M_join.DefaultIfEmpty()

                             // Ubigeo de la persona *******************************************************
                             join dep in dbContext.datahierarchy on new { a = per.i_DepartmentId.Value, b = groupUbigeo }
                                                  equals new { a = dep.i_ItemId, b = dep.i_GroupId } into dep_join
                             from dep in dep_join.DefaultIfEmpty()

                             join prov in dbContext.datahierarchy on new { a = per.i_ProvinceId.Value, b = groupUbigeo }
                                                   equals new { a = prov.i_ItemId, b = prov.i_GroupId } into prov_join
                             from prov in prov_join.DefaultIfEmpty()

                             join distri in dbContext.datahierarchy on new { a = per.i_DistrictId.Value, b = groupUbigeo }
                                                   equals new { a = distri.i_ItemId, b = distri.i_GroupId } into distri_join
                             from distri in distri_join.DefaultIfEmpty()
                             //*********************************************************************************************

                             join su in dbContext.systemuser on 11 equals su.i_SystemUserId into su_join
                             from su in su_join.DefaultIfEmpty()

                             join pr in dbContext.professional on su.v_PersonId equals pr.v_PersonId into pr_join
                             from pr in pr_join.DefaultIfEmpty()

                             join E in dbContext.servicecomponent on new { a = pstrServiceId, b = Constants.COVID_ID }
                                                                  equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                             // Usuario Medico Evaluador / Medico Aprobador ****************************
                             join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                             from me in me_join.DefaultIfEmpty()

                             join per1 in dbContext.person on me.v_PersonId equals per1.v_PersonId into per1_join
                             from per1 in per1_join.DefaultIfEmpty()


                             let varDpto = dep.v_Value1 == null ? "" : dep.v_Value1
                             let varProv = prov.v_Value1 == null ? "" : prov.v_Value1
                             let varDistri = distri.v_Value1 == null ? "" : distri.v_Value1

                             where (ser.v_ServiceId == pstrServiceId) &&
                                   (ser.i_IsDeleted == isDeleted)

                             select new ReportCertificadoCovid
                             {
                                 FechaActual = ser.d_ServiceDate.Value,
                                 Empleador = org.v_Name,
                                 EmpresaPrincipal = org.v_Name,
                                 Sede = ser.v_Sede,
                                 Area = "----",
                                 Puesto = per.v_CurrentOccupation,
                                 NroIdentificacionSalus = per1.v_DocNumber,
                                 ApellidoPaternoSalus = per1.v_FirstLastName,
                                 ApellidoMaternoSalus = per1.v_SecondLastName,
                                 NombresSalus = per1.v_FirstName,
                                 TipoDoc = "DNI",
                                 NroDoc = per.v_DocNumber,
                                 ApellidoPaterno = per.v_FirstLastName,
                                 ApellidoMaterno = per.v_SecondLastName,
                                 Nombres = per.v_FirstName,
                                 FechaNacimiento = per.d_Birthdate.Value,
                                 Sexo = M.v_Value1,
                                 Celular = per.v_TelephoneNumber,
                                 Direccion = per.v_AdressLocation,
                                 Departamento = varDistri + "-" + varProv + "-" + varDpto, // Santa Anita - Lima - Lima,
                                 Firma = pr.b_SignatureImage,
                                 AptitudId = ser.i_AptitudeStatusId.Value
                             });


                var covid = ValoresComponente(pstrServiceId, Constants.COVID_ID);

                var celulares = query.FirstOrDefault().Celular;
                var celular1 = "";
                var celular2 = "";

                celular1 = celulares.Split(',')[0];
                if (celulares.Length > 12)
                {
                    celular2 = celulares.Split(',')[1];
                }

                var q = (from a in query.ToList()
                         select new ReportCertificadoCovid
                         {
                             FechaActual = a.FechaActual,
                             Empleador = a.Empleador,
                             EmpresaPrincipal = a.EmpresaPrincipal,
                             Sede = a.Sede,
                             Area = a.Area,
                             Puesto = a.Puesto,
                             NroIdentificacionSalus = a.NroIdentificacionSalus,
                             ApellidoPaternoSalus = a.ApellidoPaternoSalus,
                             ApellidoMaternoSalus = a.ApellidoMaternoSalus,
                             NombresSalus = a.NombresSalus,
                             TipoDoc = a.TipoDoc,
                             NroDoc = a.NroDoc,
                             ApellidoPaterno = a.ApellidoPaterno,
                             ApellidoMaterno = a.ApellidoMaterno,
                             Nombres = a.Nombres,
                             Edad = GetAge(a.FechaNacimiento).ToString(),
                             Sexo = a.Sexo,
                             Celular = celular1,
                             OtroTelefono = celular2,
                             Domicilio = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_DOMICILIO_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_DOMICILIO_ID).v_Value1,
                             Direccion = a.Direccion,
                             Departamento = a.Departamento,
                             EspersonalSalud = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_ES_PERSONAL_SALUD_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_ES_PERSONAL_SALUD_ID).v_Value1,
                             Profesion = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_PROFESION_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_PROFESION_ID).v_Value1,
                             Tos = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_TOS_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_TOS_ID).v_Value1,
                             DolorGarganta = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_DOLOR_GARGANTA_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_DOLOR_GARGANTA_ID).v_Value1,
                             CongestionNasal = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_CONGESTION_NASAL_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_CONGESTION_NASAL_ID).v_Value1,
                             DificultadRespiratoria = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_DIFIC_RESPIRA_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_DIFIC_RESPIRA_ID).v_Value1,
                             Fiebre = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_FIEBRE_ESCALOFRIO_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_FIEBRE_ESCALOFRIO_ID).v_Value1,
                             Malestar = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_MALESTAR_GENERAL_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_MALESTAR_GENERAL_ID).v_Value1,
                             FechaSintomas = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_INICIO_SINTOMAS_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_INICIO_SINTOMAS_ID).v_Value1,
                             Muscular = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_MUSCULAR_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_MUSCULAR_ID).v_Value1,
                             Abdominal = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_ABDOMINAL_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_ABDOMINAL_ID).v_Value1,
                             Pecho = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_PECHO_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_PECHO_ID).v_Value1,
                             Articulaciones = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_ARTICULACIONES_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_ARTICULACIONES_ID).v_Value1,

                             TieneSintomas = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_TIENE_SINTOMAS_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_TIENE_SINTOMAS_ID).v_Value1,

                             ProcedenciaSolicitud = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_PROCEDENCIA_SOLICITUD_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_PROCEDENCIA_SOLICITUD_ID).v_Value1,
                             ResultadoPrueba = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID).v_Value1,
                             ResultadoSegundaPrueba = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_RES_2_PRUEBA_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_RES_2_PRUEBA_ID).v_Value1,
                             Seveidad = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_CLASIFICACION_CLINICA_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_CLASIFICACION_CLINICA_ID).v_Value1,

                             Mayor60 = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_MAYOR_60_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_MAYOR_60_ID).v_Value1,
                             HipertencionArterial = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_HIPERTENCION_ARTERIAL_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_HIPERTENCION_ARTERIAL_ID).v_Value1,
                             EnfCardio = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_ENF_CARDIO_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_ENF_CARDIO_ID).v_Value1,

                             Diabetes = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_DIABETES_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_DIABETES_ID).v_Value1,
                             Obesidad = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_OBESIDAD_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_OBESIDAD_ID).v_Value1,
                             Asma = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_ASMA_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_ASMA_ID).v_Value1,

                             EnfPulmonarCronica = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_ENF_PULMONAR_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_ENF_PULMONAR_ID).v_Value1,
                             InsuficienciaRenal = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_INSUFICIENCIA_RENAL_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_INSUFICIENCIA_RENAL_ID).v_Value1,
                             EbfInmunosupresor = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_INMUNOSUPRESOR_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_INMUNOSUPRESOR_ID).v_Value1,

                             Cancer = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_CANCER_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_CANCER_ID).v_Value1,
                             Embarazo = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_EMBARAZO_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_EMBARAZO_ID).v_Value1,
                             PersonalSalud = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_PERSONAL_SALUD_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_PERSONAL_SALUD_ID).v_Value1,

                             Pcr = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.CONTINUIDAD_DE_LA_ATENCION_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.CONTINUIDAD_DE_LA_ATENCION_ID).v_Value1,

                             SeguimientoProcede = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_PROCEDENCIA_SOLICITUD_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_PROCEDENCIA_SOLICITUD_ID).v_Value1,

                             Diarrea = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_DIARREA_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_DIARREA_ID).v_Value1,
                             Nauseas = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_NAUSEAS_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_NAUSEAS_ID).v_Value1,
                             Cefalea = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_CEFALEA_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_CEFALEA_ID).v_Value1,
                             Irritabilidad = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_IRRITABILIDAD_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_IRRITABILIDAD_ID).v_Value1,
                             Dolor = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_DOLOR_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_DOLOR_ID).v_Value1,
                             Otros = covid.Count == 0 ? string.Empty : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_OTROS_ID) == null ? "" : covid.Find(p => p.v_ComponentFieldId == Constants.COVID_OTROS_ID).v_Value1,

                             Aptitud = a.AptitudId.ToString(),

                             Firma = a.Firma
                         }).ToList();


                pobjOperationResult.Success = 1;
                return q;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                
                return null;
            }
        }

        public int GetAge(DateTime? FechaNacimiento)
        {
            return int.Parse((DateTime.Today.AddTicks(-FechaNacimiento.Value.Ticks).Year - 1).ToString());
        }

        public List<ReportCertificadoCovid> ObtenerDatos(string serviceId)
        {
            OperationResult objOperationResult = new OperationResult();
            return GetCovid(ref objOperationResult, serviceId);
        }

        public bool EnviarPdfTrabajador(string serviceId, string organizationId, string resultadoCovid, string sedeEmpresa, int? ClinicaExterna)
        {
            OperationResult objOperationResult = new OperationResult();
            try
            {
                var pstrRutaReportes = _ruta;
                var configEmail = Sigesoft.Node.WinClient.BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 161, "i_ParameterId");

                var empresaData = new OrganizationBL().GetOrganization(ref objOperationResult, organizationId);

                var trabajadorData = new ServiceBL().GetWorkerData(serviceId);
                if (string.IsNullOrEmpty(trabajadorData.Email)) return false;

                var correoTrabajador = trabajadorData.Email;
                string smtp = configEmail[0].Value1.ToLower();
                int port = int.Parse(configEmail[1].Value1);
                string from = configEmail[2].Value1.ToLower();
                string fromPassword = configEmail[4].Value1;
                string subject = "Resultado Covid-19";
                bool enableSsl = Convert.ToBoolean(int.Parse(configEmail[3].Value1));
                string message = string.Format("Buenos días, el resultado del trabajador {0} , es: {1}", trabajadorData.Trabajador, resultadoCovid);

                #region Buscar Archivo

                string ruta = pstrRutaReportes;
                var atachFiles = new List<string>();
                var atachFile = "";

                DirectoryInfo di = new DirectoryInfo(pstrRutaReportes);
                foreach (var fi in di.GetFiles(serviceId+".pdf"))
                {
                    atachFile = ruta + serviceId + ".pdf";
                    
                }

                if (atachFile == "")
                {
                    atachFile = ruta + serviceId + "-N003-ME000000060" + ".pdf";
                }

                atachFiles.Add(atachFile);
                #endregion               

                var copiaCc = ObtenerCorreosPorSede(sedeEmpresa);
                if (ClinicaExterna != null)
                {
                    copiaCc += ";" + ObtenerCorreoClinicaExterna(ClinicaExterna.ToString());    
                }

                var enviarA = correoTrabajador;
                var copiaBcc = "luis.delacruz@saluslaboris.com.pe";
                Sigesoft.Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, enviarA, copiaCc, copiaBcc, subject, message, atachFiles);
                //Sigesoft.Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, "", "albertomerchanc@hotmail.com", "", subject, message, atachFiles);

                if (ActualizarFlagEnvioCorreo(serviceId))
                {
                    return true;
                }
                else
                {
                    return false;
                }


                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string ObtenerCorreoClinicaExterna(string ClinicaExterna)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var query = (from A in dbContext.systemparameter
                         where A.i_GroupId == 280 && A.v_Value1 == ClinicaExterna
                         select A).FirstOrDefault();

            var arry = query.v_Value1.Split('|');

            return arry[2];


        }

        private bool ActualizarFlagEnvioCorreo(string serviceId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objServicio = (from A in dbContext.service where A.v_ServiceId == serviceId select A).FirstOrDefault();

                objServicio.CorreoEnviado = 1;
                // Guardar los cambios
                dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;   
            }
            


        }

        private string ObtenerCorreosPorSede(string sede)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var resultados = (from a in dbContext.location where a.v_OrganizationId == Constants.EMPRESA_BACKUS_ID && a.v_Name.ToUpper().Trim() == sede.ToUpper().Trim() select a).ToList();

            var correos = "";
            foreach (var item in resultados)
            {
                if (item.v_Email != null)
                {
                    correos += item.v_Email + ";";
                }

            }

            return correos;

        }

        public List<ServiceComponentFieldValuesList> ValoresComponente(string pstrServiceId, string pstrComponentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            int rpta = 0;

            try
            {
                var serviceComponentFieldValues = (from A in dbContext.service
                                                   join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                   join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                   join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId
                                                   join E in dbContext.component on B.v_ComponentId equals E.v_ComponentId
                                                   join F in dbContext.componentfields on C.v_ComponentFieldId equals F.v_ComponentFieldId
                                                   join G in dbContext.componentfield on C.v_ComponentFieldId equals G.v_ComponentFieldId
                                                   join H in dbContext.component on F.v_ComponentId equals H.v_ComponentId

                                                   where A.v_ServiceId == pstrServiceId
                                                           && H.v_ComponentId == pstrComponentId
                                                           && B.i_IsDeleted == 0
                                                           && C.i_IsDeleted == 0

                                                   select new ServiceComponentFieldValuesList
                                                   {
                                                       v_ComponentFieldId = G.v_ComponentFieldId,
                                                       v_ComponentFielName = G.v_TextLabel,
                                                       v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                       v_Value1 = D.v_Value1,
                                                       i_GroupId = G.i_GroupId.Value
                                                   });

                var finalQuery = (from a in serviceComponentFieldValues.ToList()

                                  let value1 = int.TryParse(a.v_Value1, out rpta)
                                  join sp in dbContext.systemparameter on new { a = a.i_GroupId, b = rpta }
                                                  equals new { a = sp.i_GroupId, b = sp.i_ParameterId } into sp_join
                                  from sp in sp_join.DefaultIfEmpty()

                                  select new ServiceComponentFieldValuesList
                                  {
                                      v_ComponentFieldId = a.v_ComponentFieldId,
                                      v_ComponentFielName = a.v_ComponentFielName,
                                      v_ServiceComponentFieldsId = a.v_ServiceComponentFieldsId,
                                      v_Value1 = a.v_Value1,
                                      v_Value1Name = sp == null ? "" : sp.v_Value1
                                  }).ToList();


                return finalQuery;
            }
            catch (Exception)
            {

                throw;
            }

        }
        
    }

    public class ServiciosEmail{
        public string ServiceId { get; set; }
        public string OrganizationId { get; set; }
        public int? ClinicaExterna { get; set; }
    }
}

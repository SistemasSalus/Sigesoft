using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesOtrasClinicas
{
   public class ResultadoTrabajador
    {
        private string _serviceId { get; set; }

        public ResultadoTrabajador(string serviceId)
        {
            _serviceId = serviceId;
        }

        public ResultadoTrabajadorBE ObtenerResultadoTrabajador()
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var result = new ResultadoTrabajadorBE();

                var query = (from ser in dbContext.service

                             join serComp in dbContext.servicecomponent on ser.v_ServiceId equals serComp.v_ServiceId

                             join prot in dbContext.protocol on ser.v_ProtocolId equals prot.v_ProtocolId into prot_join
                             from prot in prot_join.DefaultIfEmpty()

                             join org in dbContext.organization on prot.v_CustomerOrganizationId equals org.v_OrganizationId into org_join
                             from org in org_join.DefaultIfEmpty()

                             join per in dbContext.person on ser.v_PersonId equals per.v_PersonId into per_join
                             from per in per_join.DefaultIfEmpty()

                             join K in dbContext.systemparameter on new { a = ser.i_AptitudeStatusId.Value, b = 124 } equals new { a = K.i_ParameterId, b = K.i_GroupId } into K_join
                             from K in K_join.DefaultIfEmpty()

                             join M in dbContext.systemparameter on new { a = ser.TipoEmpresaCovidId.Value, b = 310 } equals new { a = M.i_ParameterId, b = M.i_GroupId } into M_join
                             from M in M_join.DefaultIfEmpty()

                             join G in dbContext.systemparameter on new { a = per.i_SexTypeId.Value, b = 100 } equals new { a = G.i_ParameterId, b = G.i_GroupId } into G_join
                             from G in G_join.DefaultIfEmpty()

                             join H in dbContext.systemparameter on new { a = ser.ClinicaExternad .Value, b = 280 } equals new { a = H.i_ParameterId, b = H.i_GroupId } into H_join
                             from H in H_join.DefaultIfEmpty()

                             where ser.v_ServiceId == _serviceId
                             select new
                             {
                                 ServicioId = ser.v_ServiceId,
                                 FechaExamen = ser.d_ServiceDate.Value,
                                 CentroMedico = H.v_Value1,
                                 EmpresaEmpleadora = org.v_Name,
                                 EmpresaContratista = M.v_Value1,
                                 NombresApellidos = per.v_FirstLastName + " " + per.v_SecondLastName + " " + per.v_FirstName,
                                 Dni = per.v_DocNumber,
                                 Sede = ser.v_Sede,
                                 FechaNacimiento = per.d_Birthdate.Value,
                                 ComponentId = serComp.v_ComponentId,
                                 Celular = per.v_TelephoneNumber,
                                 ProtocolName = prot.v_Name,
                                 ServiceId = ser.v_ServiceId,
                                 Sexo = G.v_Value1,
                                 Tecnico = ser.TecnicoCovid,
                                 FechaRegitro = ser.d_InsertDate.Value
                             }).ToList();


                var datosPersonales = query.FirstOrDefault();
                result.ServicioId = datosPersonales.ServiceId;
                result.Nodo = "CLÍNICA EXTERNA";
                result.FechaExamen = datosPersonales.FechaRegitro; //datosPersonales.d_in;
                result.CentroMedico = datosPersonales.CentroMedico;
                result.EmpresaEmpleadora = datosPersonales.EmpresaEmpleadora;
                result.EmpresaContratista = datosPersonales.EmpresaContratista;
                result.NombresApellidos = datosPersonales.NombresApellidos;
                result.Dni = datosPersonales.Dni;
                result.Sexo = datosPersonales.Sexo;
                result.Sede = datosPersonales.Sede;
                result.FechaNacimiento = datosPersonales.FechaNacimiento;
                result.Edad = GetAge(datosPersonales.FechaNacimiento).ToString();
                result.Tecnico = datosPersonales.Tecnico;
                result.Celular = datosPersonales.Celular;

                var CertificadoCovid19 = query.Find(p => p.ComponentId == Constants.CERTIFICADO_COVID_ID);
                if (CertificadoCovid19 != null)
                {
                    var resultados = ValoresComponente(_serviceId, Constants.CERTIFICADO_COVID_ID);
                    //var Tecnico = ObtenerDatosTecnicoLabCovid19(serviceId, Constants.CERTIFICADO_COVID_ID);
                    //result.Tecnico = Tecnico == null ? "----" : Tecnico;
                    result.Sintomas = resultados.Count == 0 ? string.Empty : resultados.Find(p => p.v_ComponentFieldId == Constants.CERTIFICADO_COVID_TIENE_SINTOMAS_ID) == null ? "" : resultados.Find(p => p.v_ComponentFieldId == Constants.CERTIFICADO_COVID_TIENE_SINTOMAS_ID).v_Value1Name;
                    var resultadoExamen = resultados.Count == 0 ? string.Empty : resultados.Find(p => p.v_ComponentFieldId == Constants.CERTIFICADO_COVID_RES_1_PRUEBA_ID) == null ? "" : resultados.Find(p => p.v_ComponentFieldId == Constants.CERTIFICADO_COVID_RES_1_PRUEBA_ID).v_Value1;

                    if (resultadoExamen == "2") //IgM Positivo
                    {
                        result.ResultadoIgM = "POSITIVO";
                        result.ResultadoIgG = "NEGATIVO";
                    }
                    else if (resultadoExamen == "3") //IgG Positivo
                    {
                        result.ResultadoIgM = "NEGATIVO";
                        result.ResultadoIgG = "POSITIVO";
                    }
                    else if (resultadoExamen == "4") //IgM e IgG positivo
                    {
                        result.ResultadoIgM = "POSITIVO";
                        result.ResultadoIgG = "POSITIVO";
                    }
                    else
                    {
                        result.ResultadoIgM = "NEGATIVO";
                        result.ResultadoIgG = "NEGATIVO";
                    }
                }

                var Covid19 = query.Find(p => p.ComponentId == Constants.COVID_ID);
                if (Covid19 != null)
                {
                    var resultados = ValoresComponente(_serviceId, Constants.COVID_ID);
                    //var Tecnico = ObtenerDatosTecnicoLabCovid19(serviceId, Constants.COVID_ID);
                    //result.Tecnico = Tecnico == null ? "----" : Tecnico;
                    result.Sintomas = resultados.Count == 0 ? string.Empty : resultados.Find(p => p.v_ComponentFieldId == Constants.COVID_TIENE_SINTOMAS_ID) == null ? "" : resultados.Find(p => p.v_ComponentFieldId == Constants.COVID_TIENE_SINTOMAS_ID).v_Value1Name;

                    var resultadoExamen = resultados.Count == 0 ? string.Empty : resultados.Find(p => p.v_ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID) == null ? "" : resultados.Find(p => p.v_ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID).v_Value1;

                    if (resultadoExamen == "2") //IgM Positivo
                    {
                        result.ResultadoIgM = "POSITIVO";
                        result.ResultadoIgG = "NEGATIVO";
                    }
                    else if (resultadoExamen == "3") //IgG Positivo
                    {
                        result.ResultadoIgM = "NEGATIVO";
                        result.ResultadoIgG = "POSITIVO";
                    }
                    else if (resultadoExamen == "4") //IgM e IgG positivo
                    {
                        result.ResultadoIgM = "POSITIVO";
                        result.ResultadoIgG = "POSITIVO";
                    }
                    else
                    {
                        result.ResultadoIgM = "NEGATIVO";
                        result.ResultadoIgG = "NEGATIVO";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private int GetAge(DateTime? FechaNacimiento)
        {
            return int.Parse((DateTime.Today.AddTicks(-FechaNacimiento.Value.Ticks).Year - 1).ToString());

        }

        private List<ServiceComponentFieldValuesList> ValoresComponente(string pstrServiceId, string pstrComponentId)
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
}

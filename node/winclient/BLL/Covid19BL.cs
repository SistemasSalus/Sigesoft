using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BLL
{
    public class Covid19BL
    {
        public List<ListaCovid19> ObtenerLista(DateTime fecha, List<string> componentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                var query = (from A in dbContext.service
                             join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                             join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                             join D in dbContext.organization on C.v_CustomerOrganizationId equals D.v_OrganizationId
                             join E in dbContext.servicecomponent on A.v_ServiceId equals E.v_ServiceId
                             join F in dbContext.component on E.v_ComponentId equals F.v_ComponentId
                             join G in dbContext.systemparameter on new { a = A.i_AptitudeStatusId.Value, b = 124 }
                                      equals new { a = G.i_ParameterId, b = G.i_GroupId }  // ESTADO APTITUD ESO                    
                             where A.d_ServiceDate >= fecha
                             && A.i_IsDeleted == 0
                             && (componentId.Contains(E.v_ComponentId))
                             && E.i_IsRequiredId == 1
                             select new ListaCovid19
                             {
                                 Nodo = A.v_ServiceId.Substring(0, 4),
                                 Nombres = B.v_FirstName,
                                 Estado = "",
                                 Apellidos = B.v_FirstLastName + " " + B.v_SecondLastName,
                                 Protocolo = C.v_Name,
                                 Empresa = D.v_Name,
                                 Puesto = B.v_CurrentOccupation,
                                 ServiceId = A.v_ServiceId,
                                 PersonId = A.v_PersonId,
                                 ComponentId = E.v_ComponentId,
                                 ServiceComponentId = E.v_ServiceComponentId,
                                 Index = F.i_UIIndex,
                                 OrganizationId = C.v_CustomerOrganizationId,
                                 Aptitud = G.v_Value1,
                                 FechaServicio = A.d_ServiceDate.Value,
                                 Telefono =  B.v_TelephoneNumber
                             }
                             ).ToList();

                query = query.ToList().FindAll(p => p.Nodo == "N003");
                query.OrderBy(o1 => o1.ServiceComponentId).ThenBy(o2 => o2.Index).ToList();

                query = query.GroupBy(g => g.ServiceId).Select(s => s.First()).ToList();

                var listServiceComponentsId = query.Select(s => s.ServiceComponentId).ToList();
                var ListServiceId = query.Select(s => s.ServiceId).ToList();

                var valoresEncuesta = EstadoEncuesta(listServiceComponentsId);
                var valoresLab = EstadoEncuestaLaboratorio(listServiceComponentsId);
                var valoresTriaje = EstadoEncuestaTriaje(ListServiceId);

                var listResult = new List<ListaCovid19>();
                foreach (var item in query)
                {
                    var oListaCovid19 = new ListaCovid19();

                    oListaCovid19.Nombres = item.Nombres;

                    oListaCovid19.Apellidos = item.Apellidos;
                    oListaCovid19.Protocolo = item.Protocolo;
                    oListaCovid19.Empresa = item.Empresa;
                    oListaCovid19.Puesto = item.Puesto;
                    oListaCovid19.ServiceId = item.ServiceId;
                    oListaCovid19.PersonId = item.PersonId;
                    oListaCovid19.ComponentId = item.ComponentId;
                    oListaCovid19.ServiceComponentId = item.ServiceComponentId;
                    oListaCovid19.FechaServicio = item.FechaServicio;
                    oListaCovid19.Telefono = item.Telefono;

                    if (item.ComponentId == Constants.COVID_ID)
                    {
                        oListaCovid19.Estado = valoresEncuesta.Find(p => p.ServiceComponentId == item.ServiceComponentId) == null ? "pendiente" : "realizado";
                        oListaCovid19.EstadoTriaje = "No Aplica";
                        oListaCovid19.EstadoLaboratorio = valoresLab.Find(p => p.ServiceComponentId == item.ServiceComponentId) == null ? "pendiente" : "realizado";
                        oListaCovid19.Aptitud = "No Aplica";
                    }
                    else if (item.ComponentId == Constants.CERTIFICADO_COVID_ID)
                    {
                        oListaCovid19.Estado = valoresEncuesta.Find(p => p.ServiceComponentId == item.ServiceComponentId) == null ? "pendiente" : "realizado";
                        oListaCovid19.EstadoTriaje = valoresTriaje.Find(p => p.ServiceId == item.ServiceId) == null ? "pendiente" : "realizado";
                        oListaCovid19.EstadoLaboratorio = valoresLab.Find(p => p.ServiceComponentId == item.ServiceComponentId) == null ? "pendiente" : "realizado";
                        oListaCovid19.Aptitud = item.Aptitud;
                    }
                    else if (item.ComponentId == Constants.CERTIFICADO_DESCENSO_COVID_ID)
                    {
                        oListaCovid19.Estado = valoresEncuesta.Find(p => p.ServiceComponentId == item.ServiceComponentId) == null ? "pendiente" : "realizado";
                        oListaCovid19.EstadoTriaje = valoresTriaje.Find(p => p.ServiceId == item.ServiceId) == null ? "pendiente" : "realizado";
                        oListaCovid19.EstadoLaboratorio = "No Aplica";
                        oListaCovid19.Aptitud = "No Aplica";
                    }
                    oListaCovid19.OrganizationId = item.OrganizationId;
                    listResult.Add(oListaCovid19);
                }

                return listResult;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Valores> EstadoEncuesta(List<string> serviceComponentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                var query = (from A in dbContext.servicecomponentfields
                             join B in dbContext.servicecomponentfieldvalues on A.v_ServiceComponentFieldsId equals B.v_ServiceComponentFieldsId
                             where serviceComponentId.Contains(A.v_ServiceComponentId) && (A.v_ComponentFieldId == Constants.CERTIFICADO_COVID_DOMICILIO_ID || A.v_ComponentFieldId == Constants.COVID_DOMICILIO_ID || A.v_ComponentFieldId == Constants.CERTIFICADO_DESCENSO_COVID_DOMICILIO_ID)
                             select new Valores
                             {
                                 ServiceComponentId = A.v_ServiceComponentId,
                                 Value1 = B.v_Value1
                             }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Valores> EstadoEncuestaLaboratorio(List<string> serviceComponentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                var query = (from A in dbContext.servicecomponentfields
                             join B in dbContext.servicecomponentfieldvalues on A.v_ServiceComponentFieldsId equals B.v_ServiceComponentFieldsId
                             where serviceComponentId.Contains(A.v_ServiceComponentId) && (A.v_ComponentFieldId == Constants.CERTIFICADO_COVID_RES_1_PRUEBA_ID || A.v_ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID)
                             select new Valores
                             {
                                 ServiceComponentId = A.v_ServiceComponentId,
                                 Value1 = B.v_Value1,
                                 ComponentFieldId = A.v_ComponentFieldId
                             }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Valores> EstadoEncuestaTriaje(List<string> serviceId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                var query = new ServiceBL()
                            .ValoresComponente(serviceId, Constants.FUNCIONES_VITALES_ID)
                            .FindAll(p => p.v_ComponentFieldId == Constants.FUNCIONES_VITALES_SAT_O2_ID);

                var list = (from A in query
                            select new Valores
                            {
                                ServiceId = A.v_ServiceId,
                                ServiceComponentId = A.v_ServiceComponentId,
                                Value1 = A.v_Value1
                            }).ToList();



                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ListaCovid19> ObtenerLista(DateTime fecha, string organizationId, string nodoId)
        {
            nodoId = nodoId.PadLeft(3, '0');
            
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                var query = (from A in dbContext.service
                             join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                             join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                             join D in dbContext.organization on C.v_CustomerOrganizationId equals D.v_OrganizationId
                             join E in dbContext.servicecomponent on A.v_ServiceId equals E.v_ServiceId
                             join F in dbContext.component on E.v_ComponentId equals F.v_ComponentId              
                             where A.d_ServiceDate >= fecha
                             && A.i_IsDeleted == 0
                             && E.v_ComponentId == Constants.COVID_ID
                             && C.v_CustomerOrganizationId == organizationId
                             && A.v_ServiceId.Contains("N" + nodoId)
                             select new ListaCovid19
                             {
                                 Nombres = B.v_FirstName,
                                 Estado = "",
                                 Apellidos = B.v_FirstLastName + " " + B.v_SecondLastName,
                                 Protocolo = C.v_Name,
                                 Empresa = D.v_Name,
                                 Puesto = B.v_CurrentOccupation,
                                 ServiceId = A.v_ServiceId,
                                 PersonId = A.v_PersonId,
                                 ComponentId = E.v_ComponentId,
                                 ServiceComponentId = E.v_ServiceComponentId,
                                 Index = F.i_UIIndex,
                                 OrganizationId = C.v_CustomerOrganizationId,
                                 Telefono =B.v_TelephoneNumber
                             }
                             ).ToList();

                query.OrderBy(o1 => o1.ServiceComponentId).ThenBy(o2 => o2.Index).ToList();

                query = query.GroupBy(g => g.ServiceId).Select(s => s.First()).ToList();

                var listServiceComponentsId = query.Select(s => s.ServiceComponentId).ToList();
                var ListServiceId = query.Select(s => s.ServiceId).ToList();

                var valoresEncuesta = EstadoEncuesta(listServiceComponentsId);
                var valoresLab = EstadoEncuestaLaboratorio(listServiceComponentsId);
                var listResult = new List<ListaCovid19>();
                foreach (var item in query)
                {
                    var oListaCovid19 = new ListaCovid19();

                    oListaCovid19.Nombres = item.Nombres;

                    oListaCovid19.Apellidos = item.Apellidos;
                    oListaCovid19.Protocolo = item.Protocolo;
                    oListaCovid19.Empresa = item.Empresa;
                    oListaCovid19.Puesto = item.Puesto;
                    oListaCovid19.ServiceId = item.ServiceId;
                    oListaCovid19.PersonId = item.PersonId;
                    oListaCovid19.ComponentId = item.ComponentId;
                    oListaCovid19.ServiceComponentId = item.ServiceComponentId;
                    oListaCovid19.Estado = valoresEncuesta.Find(p => p.ServiceComponentId == item.ServiceComponentId) == null ? "pendiente" : "realizado";
                    oListaCovid19.EstadoLaboratorio = valoresLab.Find(p => p.ServiceComponentId == item.ServiceComponentId) == null ? "pendiente" : "realizado";
                    oListaCovid19.OrganizationId = item.OrganizationId;
                    oListaCovid19.Telefono = item.Telefono;
                    listResult.Add(oListaCovid19);
                }

                var listaResultadosLab = valoresLab.FindAll(p => p.ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID);
                if (listResult.Count > 0)
                {
                    listResult[0].CountNegativo = listaResultadosLab.FindAll(p => p.Value1 == "0").Count;
                    listResult[0].CountNoValido = listaResultadosLab.FindAll(p => p.Value1 == "1").Count;
                    listResult[0].CountIgMPositivo = listaResultadosLab.FindAll(p => p.Value1 == "2").Count;
                    listResult[0].CountIgGPositivo = listaResultadosLab.FindAll(p => p.Value1 == "3").Count;
                    listResult[0].CountIgMIgGPositivo = listaResultadosLab.FindAll(p => p.Value1 == "4").Count;
                }
                

                return listResult;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ListaCovid19> ObtenerListaOtrasClinicas(DateTime fecha, string organizationId, string nodoId, int ClinicaId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                var fechaFin = fecha.AddDays(+1);
                var query = (from A in dbContext.service
                             join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                             join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                             join D in dbContext.organization on C.v_CustomerOrganizationId equals D.v_OrganizationId
                             join E in dbContext.servicecomponent on A.v_ServiceId equals E.v_ServiceId
                             join F in dbContext.component on E.v_ComponentId equals F.v_ComponentId
                             where A.d_ServiceDate >= fecha && A.d_ServiceDate <= fechaFin
                             && A.i_IsDeleted == 0
                             && E.v_ComponentId == Constants.COVID_ID
                             && C.v_CustomerOrganizationId == organizationId
                             && A.v_ServiceId.Contains("N" + nodoId)
                             //&& A.v_Sede == Sede
                             && A.ClinicaExternad == ClinicaId
                             select new ListaCovid19
                             {
                                 Nombres = B.v_FirstName,
                                 Estado = "",
                                 Apellidos = B.v_FirstLastName + " " + B.v_SecondLastName,
                                 Protocolo = C.v_Name,
                                 Empresa = D.v_Name,
                                 Puesto = B.v_CurrentOccupation,
                                 ServiceId = A.v_ServiceId,
                                 PersonId = A.v_PersonId,
                                 ComponentId = E.v_ComponentId,
                                 ServiceComponentId = E.v_ServiceComponentId,
                                 Index = F.i_UIIndex,
                                 OrganizationId = C.v_CustomerOrganizationId,
                                 Telefono = B.v_TelephoneNumber
                             }
                             ).ToList();

                query.OrderBy(o1 => o1.ServiceComponentId).ThenBy(o2 => o2.Index).ToList();

                query = query.GroupBy(g => g.ServiceId).Select(s => s.First()).ToList();

                var listServiceComponentsId = query.Select(s => s.ServiceComponentId).ToList();
                var ListServiceId = query.Select(s => s.ServiceId).ToList();

                var valoresEncuesta = EstadoEncuesta(listServiceComponentsId);
                var valoresLab = EstadoEncuestaLaboratorio(listServiceComponentsId);
                var listResult = new List<ListaCovid19>();
                foreach (var item in query)
                {
                    var oListaCovid19 = new ListaCovid19();

                    oListaCovid19.Nombres = item.Nombres;

                    oListaCovid19.Apellidos = item.Apellidos;
                    oListaCovid19.Protocolo = item.Protocolo;
                    oListaCovid19.Empresa = item.Empresa;
                    oListaCovid19.Puesto = item.Puesto;
                    oListaCovid19.ServiceId = item.ServiceId;
                    oListaCovid19.PersonId = item.PersonId;
                    oListaCovid19.ComponentId = item.ComponentId;
                    oListaCovid19.ServiceComponentId = item.ServiceComponentId;
                    oListaCovid19.Estado = valoresEncuesta.Find(p => p.ServiceComponentId == item.ServiceComponentId) == null ? "pendiente" : "realizado";
                    oListaCovid19.EstadoLaboratorio = valoresLab.Find(p => p.ServiceComponentId == item.ServiceComponentId) == null ? "pendiente" : "realizado";
                    oListaCovid19.OrganizationId = item.OrganizationId;
                    oListaCovid19.Telefono = item.Telefono;
                    listResult.Add(oListaCovid19);
                }

                var listaResultadosLab = valoresLab.FindAll(p => p.ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID);
                if (listResult.Count > 0)
                {
                    listResult[0].CountNegativo = listaResultadosLab.FindAll(p => p.Value1 == "0").Count;
                    listResult[0].CountNoValido = listaResultadosLab.FindAll(p => p.Value1 == "1").Count;
                    listResult[0].CountIgMPositivo = listaResultadosLab.FindAll(p => p.Value1 == "2").Count;
                    listResult[0].CountIgGPositivo = listaResultadosLab.FindAll(p => p.Value1 == "3").Count;
                    listResult[0].CountIgMIgGPositivo = listaResultadosLab.FindAll(p => p.Value1 == "4").Count;
                }


                return listResult;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DatosObtenidosCovid GetHcByDni(string dni)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                var result = new DatosObtenidosCovid();
                var query = new DatosObtenidosCovid();

                //Consultar HeadCount
                 query = (from A in dbContext.hcactualizado 
                          join B in dbContext.location on A.Sede equals B.v_Name

                          where A.Dni == dni && B.v_OrganizationId == Constants.EMPRESA_BACKUS_ID

                             select new DatosObtenidosCovid
                             {
                                 v_Apellido = A.ApellidoPaterno,
                                 v_SegundoApellido = A.ApellidoMaterno,
                                 v_NombrePila = A.Nombres,
                                 d_FechaNacimiento = A.FechaNacimiento,
                                 v_ClaveSexo = A.Sexo,
                                 v_PosicionNombre = A.Puesto,
                                 v_Soc = A.HC,
                                v_Mail = "",
                                v_Telefon = "",
                                Empleadora = A.EmpresaEmpleadora,
                                 Sede = A.Sede,
                                 SedeId = B.ValueSede.Value
                                }).FirstOrDefault();

                 if (query == null)
                 {
                     //Consultar BD
                     query = (from A in dbContext.person
                              where A.v_DocNumber == dni
                              select new DatosObtenidosCovid
                              {
                                  v_Apellido = A.v_FirstLastName,
                                  v_SegundoApellido = A.v_SecondLastName,
                                  v_NombrePila = A.v_FirstName,
                                  d_FechaNacimiento = A.d_Birthdate,
                                  v_PosicionNombre = A.v_CurrentOccupation,
                                  v_Mail = A.v_Mail,
                                  v_Telefon = A.v_TelephoneNumber,
                                  Empleadora= "",
                                  Sede="",
                                  SedeId = -1

                              }).FirstOrDefault();
                 }


                 var ultimoServicio = ObtenerUltimoResultadoPruebaCovid19(dni);        
                  result.v_Apellido = query.v_Apellido;
                  result.v_SegundoApellido = query.v_SegundoApellido;
                  result.v_NombrePila = query.v_NombrePila;
                  result.d_FechaNacimiento = query.d_FechaNacimiento;
                  result.v_ClaveSexo = query.v_ClaveSexo;
                  result.v_PosicionNombre = query.v_PosicionNombre;
                  result.v_Soc = query.v_Soc;
                  result.v_Mail = query.v_Mail;
                  result.v_Telefon = query.v_Telefon;
                  result.Empleadora = query.Empleadora;
                  result.Sede = query.Sede;
                  result.SedeId = query.SedeId;
                  result.FechaUltimaCovid = ultimoServicio.FechaServicio.ToString();
                  result.ResultadpUltimaCovid = ultimoServicio.Resultado;       

                return result;           
            }
             catch (Exception)
            {

                throw;
            }
        }

        public UltimoServicioBE ObtenerUltimoResultadoPruebaCovid19(string dni)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                var query = (from A in dbContext.person
                             join B in dbContext.service on A.v_PersonId equals B.v_PersonId
                             where A.v_DocNumber == dni && B.i_IsDeleted ==0
                             orderby B.d_ServiceDate descending
                             select new UltimoServicioBE
                             {
                                ServiceId =  B.v_ServiceId,
                                FechaServicio =  B.d_ServiceDate.Value
                             }
                             ).ToList();

                if (query.Count == 0)
                {
                    return new UltimoServicioBE { Resultado = "sin datos", FechaServicio = null };
                }


                var ultimoServicio = query[0];

                var resultados = new ServiceBL().ValoresComponente(ultimoServicio.ServiceId, Constants.COVID_ID);
                var resultado1raPrueba = resultados.Find(p => p.v_ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID) == null ? "" : resultados.Find(p => p.v_ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID).v_Value1Name;
                return new UltimoServicioBE { Resultado = resultado1raPrueba, FechaServicio = ultimoServicio.FechaServicio };

            }
            catch (Exception)
            {

                throw;
            }
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

        public List<UltimoServicioBE> ObtenerUltimosResultadoPruebaCovid19(string personId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var ultimosServicios = new List<UltimoServicioBE>();
            try
            {
                var query = (from A in dbContext.person
                             join B in dbContext.service on A.v_PersonId equals B.v_PersonId
                             where A.v_PersonId == personId && B.i_IsDeleted == 0
                             orderby B.d_ServiceDate descending
                             select new UltimoServicioBE
                             {
                                 ServiceId = B.v_ServiceId,
                                 FechaServicio = B.d_ServiceDate.Value
                             }
                             ).ToList();

                if (query.Count == 0)
                {
                    return new List<UltimoServicioBE>();
                }


                foreach (var item in query)
                {
                    var resultados = new ServiceBL().ValoresComponente(item.ServiceId, Constants.COVID_ID);
                    var resultado1raPrueba = resultados.Find(p => p.v_ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID) == null ? "" : resultados.Find(p => p.v_ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID).v_Value1Name;    

                    ultimosServicios.Add(new UltimoServicioBE { Resultado = resultado1raPrueba, FechaServicio = item.FechaServicio });                    
                }

                return ultimosServicios; 

            }
            catch (Exception)
            {

                throw;
            }
        }

        public int ObtenerOpcionFormato(string organizationId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                var query = (from A in dbContext.organization
                             where A.v_OrganizationId == organizationId                             
                             select A ).FirstOrDefault();

                if (query.i_TipoFormatoCovid19 == null)
                {
                    return (int)TipoFormatoCovid19.Detallado;    
                }

                return query.i_TipoFormatoCovid19.Value;  

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool EsEditable(string serviceComponentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var service = (from A in dbContext.servicecomponentfields 
                            where A.v_ServiceComponentId == serviceComponentId 
                            && A.v_ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID 
                            select A).FirstOrDefault();

            var rowService = (from Z in dbContext.servicecomponent
                              join X in dbContext.service on Z.v_ServiceId equals X.v_ServiceId
                              where Z.v_ServiceComponentId == serviceComponentId select X).FirstOrDefault();

            if (rowService != null)
            {
                if (rowService.CorreoEnviado == 1)
                {
                    return false;
                }
            }

            if (service == null) return true;
            
            DateTime fecharegistro = service.d_InsertDate.Value;
            var minutes = (DateTime.Now - fecharegistro).TotalMinutes;

            if (minutes > 30)
            {
                return false;
            }

            return true;
        }

        public List<SedesBackus> ObtenerSedesBackus(string EmpresaId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var ultimosServicios = new List<UltimoServicioBE>();
            try
            {
                var query = (from A in dbContext.location
                             where A.v_OrganizationId == EmpresaId && A.i_IsDeleted == 0 && A.ValueSede != null
                             select new SedesBackus
                             {
                                v_LocationId = A.v_LocationId,
                                v_LocationName = A.v_Name,
                                ValueSede = A.ValueSede.Value
                             }
                             ).ToList();

                

                return query;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<UsuarioRegCovid> ListarNodosCovid()
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var nodos = (from A in dbContext.usuarioregcovid where A.i_IsDeleted == 0 
                         select new UsuarioRegCovid { 
                             i_UsuarioRegcovidId = A.i_UsuarioRegcovidId,
                             i_NodeId = A.i_NodeId,
                             v_NodeName = A.v_NodeName,
                             v_UserName = A.v_UserName
                        }).ToList();

            return nodos.GroupBy(g => g.i_NodeId).Select(s => s.First()).ToList();
        }

        public UsuarioRegCovidBE ValidarUSuarioRegCovid(string usuario, string pass, int nodoId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            return (from A in dbContext.usuarioregcovid
                    join B in dbContext.organization on A.v_OrganizationId equals B.v_OrganizationId
                    join C in dbContext.protocol on A.v_OrganizationId equals C.v_CustomerOrganizationId
                    where A.v_UserName == usuario && A.v_Password == pass && A.i_NodeId == nodoId
                    select new UsuarioRegCovidBE
                    {
                        i_UsuarioRegcovidId = A.i_UsuarioRegcovidId,
                        i_NodeId = A.i_NodeId.Value,
                        v_NodeName = A.v_NodeName, 
                        v_OrganizationId  = A.v_OrganizationId,
                        v_OrganizationName = B.v_Name,
                        v_ProtocolId = A.v_ProtocolId,
                        v_ProtocolName = C.v_Name,
                        v_UserName = A.v_UserName
                    }).FirstOrDefault();
            
        }
        
    }

    public class Valores
    {
        public string ServiceId { get; set; }
        public string ServiceComponentId { get; set; }
        public string Value1 { get; set; }
        public string ComponentFieldId { get; set; }
    }

    public class SedesBackus
    {
        public string v_LocationId { get; set; }
        public string v_LocationName { get; set; }
        public int ValueSede { get; set; }

    }

    public class UsuarioRegCovid 
    { 
        public int i_UsuarioRegcovidId { get; set; }
        public int? i_NodeId { get; set; }
        public string v_NodeName { get; set; }
        public string v_UserName { get; set; }
        public string v_Password { get; set; }
    }
        
}

using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsolaRegCovid19
{
   public class Incidencias
    {
       SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        public string _dni { get; set; }

        public Incidencias(string dni)
        {
            _dni = dni;
        }

       public DatosPersonales ObtenerDatosPersonales(){
           var query = (from A in dbContext.person 
                        where A.v_DocNumber == _dni
                        select new DatosPersonales { 
                            NombresTrabajador =  A.v_FirstName + " "+ A.v_FirstLastName + " " + A.v_SecondLastName 
                        }).FirstOrDefault();

           return query;
       }

       public List<Servicios> DatosServicios()
       {
           var fecha = DateTime.Parse("20-08-2020");
           var servicios = (from A in dbContext.service
                        join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                        join C in dbContext.systemuser on A.i_InsertUserId equals C.i_SystemUserId
                        where B.v_DocNumber == _dni 
                        && A.d_ServiceDate >= fecha
                        select new Servicios
                        {
                            ServiceId = A.v_ServiceId,
                            FechaServicio = A.d_ServiceDate,
                            Tecnico = A.TecnicoCovid,
                            FechaInsert = A.d_InsertDate,
                            FechaUpdate = A.d_UpdateDate,
                            Usuario = C.v_UserName,
                            CorreoEnviado = A.CorreoEnviado.Value,
                            CorreoTrabajador =  B.v_Mail
                        }).ToList();

           return servicios;
       }

       public string ObtenerResultadoLaboratorio(string serviceId)
       {
           var llenoEncuesta = "NO";
           var llenoLaboratorio = "NO";
           var ResultadoLab = "";
           var FechaInsert = "";
           var FechaUpdate = "";

           var valoresExamen = ValoresComponente(serviceId, Constants.COVID_ID);
           if (valoresExamen == null)
           {
               return "SIN DATOS";
           }

           var dataEnc = valoresExamen.Find(p => p.v_ComponentFieldId == Constants.COVID_DOMICILIO_ID);
           if (dataEnc != null)
           {
               llenoEncuesta = "SI";
           }

           var dataLab = valoresExamen.Find(p => p.v_ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID);
           if (dataLab != null)
           {
               llenoLaboratorio = "SI";
               ResultadoLab = dataLab.v_Value1Name;
               FechaInsert = dataLab.d_InsertDate == null ? "" : dataLab.d_InsertDate.Value.ToString();
               FechaUpdate = dataLab.d_UpdateDate == null ? "" : dataLab.d_UpdateDate.Value.ToString();
           }
           else
           {
               Console.BackgroundColor = ConsoleColor.Red;
               Console.WriteLine("NO HAY DATOS");
               Console.BackgroundColor = ConsoleColor.Black;
           }

           var pdf = PDFCreado(serviceId);

           return string.Format("Encuesta:{0}, Laboratorio:{1} {2}  / FI:{3} / FU:{4} / PDF:{5} ", llenoEncuesta, llenoLaboratorio, ResultadoLab, FechaInsert, FechaUpdate, pdf);
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
                                                      i_GroupId = G.i_GroupId.Value,
                                                      d_InsertDate = D.d_InsertDate,
                                                      d_UpdateDate = D.d_UpdateDate
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
                                     v_Value1Name = sp == null ? "" : sp.v_Value1,
                                     d_InsertDate = a.d_InsertDate,
                                     d_UpdateDate = a.d_UpdateDate
                                 }).ToList();


               return finalQuery;
           }
           catch (Exception)
           {

               throw;
           }

       }

       public string PDFCreado(string serviceId)
       {
           string _rutaFichasCovid = Sigesoft.Common.Utils.GetApplicationConfigValue("rutaFichasCovid");

           string[] file = Directory.GetFiles(_rutaFichasCovid, serviceId + ".pdf");
           if (file.Count() > 0)
           {
               return "PDF ENCONTRADO EN SERVER";
           }
           else
           {
               return "SIN RESULTADOS";
           }
       }
       

    }
}

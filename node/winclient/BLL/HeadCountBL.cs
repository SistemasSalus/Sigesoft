using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BLL
{
   public class HeadCountBL
    {
       public List<headcountDto> GetListHeadCount()
       {
           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
               var query = (from A in dbContext.headcount
                            where A.v_Nif != ""
                           select new headcountDto
                           {
                              v_Nif = A.v_Nif,
                              v_NombreCompleto = A.v_NombreCompleto,
                              v_Apellido = A.v_Apellido,
                              v_SegundoApellido = A.v_SegundoApellido,
                              v_ClaveSexo =   A.v_ClaveSexo,
                              d_FechaNacimiento =A.d_FechaNacimiento,
                              v_Soc = A.v_Soc,
                              v_SubDivPersona =A.v_SubDivPersona
                           }).ToList();

            
               return query;

           }
           catch (Exception ex)
           {              
               return null;
           }
       }

       public List<TrabajadoresServicioBE> GetListTrabajadores()
       {
           try
           {
               var fechaInicio = DateTime.Parse("01/01/2020");
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
               var query = (from A in dbContext.person
                            join B in dbContext.service on A.v_PersonId equals B.v_PersonId
                            where A.v_DocNumber !=""  && B.d_ServiceDate > fechaInicio
                            select new TrabajadoresServicioBE
                            {
                                v_DocNumber = A.v_DocNumber,
                                d_Birthdate = A.d_Birthdate.Value,
                                i_SexTypeId = A.i_SexTypeId.Value,
                                v_ServiceId = B.v_ServiceId
                            }).ToList();

               return query;

           }
           catch (Exception ex)
           {
               return null;
           }
       }

       public bool  ActualizarSede(string serviceId, string sede)
       {
           try
           {
               SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

               // Obtener la entidad fuente
               var objEntitySource = (from a in dbContext.service
                                      where a.v_ServiceId == serviceId
                                      select a).FirstOrDefault();
               objEntitySource.v_Sede = sede;

               // Guardar los cambios
               dbContext.SaveChanges();
               return true;
           }
           catch (Exception ex)
           {               
               return false;
           }
       }
    }
}

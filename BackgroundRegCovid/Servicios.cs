using Sigesoft.Common;
using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundRegCovid
{
    public class Servicios{   

        public List<DatosServicioDto> ListarServicios(DateTime fecha)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            
            var servicios = (from A in dbContext.service
                             join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                             join C in dbContext.servicecomponent on A.v_ServiceId equals C.v_ServiceId
                             join D in dbContext.protocol on A.v_ProtocolId equals D.v_ProtocolId
                             join E in dbContext.organization on D.v_CustomerOrganizationId equals E.v_OrganizationId
                             join F in dbContext.location on new { a = E.v_OrganizationId, b = A.v_Sede } equals new {a = F.v_OrganizationId, b = F.v_Name} into F_join
                             from F in F_join.DefaultIfEmpty()                             
                             where A.i_IsDeleted == 0 
                             && (A.d_ServiceDate >= fecha)
                             && (A.CorreoEnviado == null)
                             select new DatosServicioDto
                             {
                                 ServiceId = A.v_ServiceId,
                                 OrganizationId = D.v_CustomerOrganizationId,
                                 ApellidoPaterno = B.v_FirstLastName,
                                 ApellidoMaterno = B.v_SecondLastName,
                                 Nombres = B.v_FirstName,
                                 Documento = B.v_DocNumber,
                                 ComponentId = C.v_ComponentId,
                                 FechaServicio = A.d_ServiceDate.Value,
                                 FechaInsert = A.d_InsertDate.Value,
                                 TipoFormato = E.i_TipoFormatoCovid19,
                                 Sede = A.v_Sede,
                                 SedeId = F.ValueSede.Value,
                                 Tecnico = A.TecnicoCovid,
                                 TipoEmpresa = A.TipoEmpresaCovidId,
                                 NombresCompleto = B.v_FirstLastName + " " + B.v_SecondLastName + ", " + B.v_FirstName,
                                 EmpresaEmpleadora = A.EmpresaEmpleadora
                             }).ToList();

           servicios = servicios.FindAll(p => p.ComponentId == Constants.COVID_ID || p.ComponentId == Constants.ANTIGENOS_ID).ToList();
           //servicios = servicios.GroupBy(p => p.ServiceId).Select(s => s.First()).OrderBy(f => f.FechaInsert).ToList().ToList();

           return servicios;
        }
    }
}

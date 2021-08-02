using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sigesoft.Node.WinClient.DAL;

namespace ReporteSisCovid
{
    public class Services
    {
        public string ServiceId { get; set; }
        public string ComponentId { get; set; }

        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string FirstName { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string SexType { get; set; }
        public string Sede { get; set; }
        public string Empleador { get; set; }
        public string EmpresaPrincipal { get; set; }
        public string Tecnico { get; set; }
        public string EmpresaCliente { get; set; }

        public Services(){}

        public List<Services> ServiceList(DateTime startDate, DateTime endDate)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            startDate = startDate.Date;
            endDate = endDate.AddDays(1).Date;

            var services = (from a in dbContext.service

                            join b in dbContext.person on a.v_PersonId equals b.v_PersonId

                            join c in dbContext.datahierarchy on new { a = b.i_DocTypeId.Value, b = 106 } equals new { a = c.i_ItemId, b = c.i_GroupId } into c_join
                            from c in c_join.DefaultIfEmpty()

                            join d in dbContext.systemparameter on new { a = b.i_SexTypeId.Value, b = 100 } equals new { a = d.i_ParameterId, b = d.i_GroupId } into d_join
                            from d in d_join.DefaultIfEmpty()

                            join e in dbContext.protocolcomponent on a.v_ProtocolId equals e.v_ProtocolId

                            join f in dbContext.hcactualizado on b.v_DocNumber equals f.Dni into f_join
                            from f in f_join.DefaultIfEmpty()

                            join g in dbContext.protocol on a.v_ProtocolId equals g.v_ProtocolId into g_join
                            from g in g_join.DefaultIfEmpty()

                            join h in dbContext.organization on g.v_CustomerOrganizationId equals h.v_OrganizationId into h_join
                            from h in h_join.DefaultIfEmpty()

                            where a.d_ServiceDate >= startDate && a.d_ServiceDate <= endDate
                                    //&& a.i_IsDeleted == 0 && g.v_CustomerOrganizationId == "N003-OO000001651"
                                    && a.i_IsDeleted == 0 && g.v_CustomerOrganizationId == "N003-OO000000425"

                            select new Services { 
                                ServiceId = a.v_ServiceId,
                                ComponentId = e.v_ComponentId,
                                DocumentType = c.v_Value1,
                                DocumentNumber = b.v_DocNumber,
                                FirstName = b.v_FirstName,
                                FirstLastName = b.v_FirstLastName,
                                SecondLastName = b.v_SecondLastName,
                                BirthDate = b.d_Birthdate,
                                SexType = d.v_Value1,
                                Sede = a.v_Sede,
                                Tecnico = a.TecnicoCovid,
                                Empleador = f.EmpresaEmpleadora,
                                EmpresaPrincipal = f.HC,
                                EmpresaCliente = h.v_Name
                            }).ToList();

            var servicesOnlyCovid19 = services.FindAll(p => p.ComponentId == Sigesoft.Common.Constants.CERTIFICADO_COVID_ID || p.ComponentId == Sigesoft.Common.Constants.COVID_ID || p.ComponentId == Sigesoft.Common.Constants.ANTIGENOS_ID).ToList();

            var groupServices = servicesOnlyCovid19.GroupBy(g => g.ServiceId).Select(s => s.First()).ToList();

            return groupServices;
        }
    }
}

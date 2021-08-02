using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReporteSisCovid
{
    public class Covid19ExamValues
    {
        public string ComponentFieldId { get; set; }
        public string ComponentFielName { get; set; }
        public string ServiceComponentFieldsId { get; set; }
        public string Value1 { get; set; }
        public string Value1Name { get; set; }

        public List<Covid19ExamValues> ListServiceValues(string serviceId, string componentId)
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

                                                   where A.v_ServiceId == serviceId
                                                           && H.v_ComponentId == componentId
                                                           && B.i_IsDeleted == 0
                                                           && C.i_IsDeleted == 0

                                                   select new 
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

                                  select new Covid19ExamValues
                                  {
                                      ComponentFieldId = a.v_ComponentFieldId,
                                      ComponentFielName = a.v_ComponentFielName,
                                      ServiceComponentFieldsId = a.v_ServiceComponentFieldsId,
                                      Value1 = a.v_Value1,
                                      Value1Name = sp == null ? "" : sp.v_Value1
                                  }).ToList();


                return finalQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

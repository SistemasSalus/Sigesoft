using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.DAL;
using Sigesoft.Common;

namespace Sigesoft.Node.WinClient.BLL
{
    public class ProfessionalBL
    {
        public SystemUserList GetSystemUserName(ref OperationResult pobjOperationResult, int SystemUserId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from su1 in dbContext.systemuser
                             join A in dbContext.person on su1.v_PersonId equals A.v_PersonId
                             where su1.i_IsDeleted == 0 && su1.i_SystemUserId == SystemUserId
                             
                             select new SystemUserList
                             {
                                 v_PersonName = A.v_FirstName + " " + A.v_FirstLastName + " " + A.v_SecondLastName

                             }
                            ).FirstOrDefault();



              
                //List<SystemUserList> objData = query.ToList();
                pobjOperationResult.Success = 1;
                return query;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }


        }

    }
}

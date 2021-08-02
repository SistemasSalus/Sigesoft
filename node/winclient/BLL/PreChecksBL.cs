using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BLL
{
   public class PreChecksBL
    {

       public bool EsBDProduccion()
       {
           SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
           var query = (from A in dbContext.service where A.i_IsDeleted == 0  select A).Count();

           if (query < 5000)
               return false; 
           return true;
       }

    }
}

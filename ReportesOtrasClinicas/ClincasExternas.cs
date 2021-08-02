using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesOtrasClinicas
{
    public class ClincasExternas
    {
        private DateTime _fecha  { get; set; }

        public ClincasExternas(DateTime fecha)
        {
            _fecha = fecha;
        }

        public List<ClinicaBE> Listar()
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var query = (from A in dbContext.systemparameter 
                         where A.i_GroupId == 280 select A).ToList();

            var lista = new List<ClinicaBE>();
            foreach (var item in query)
            {
                lista.Add(new ClinicaBE { ClincaExternaId = item.i_ParameterId, NombreProvincia = item.v_Value1 });
            }
            return lista;
        }

        public List<ServicioBE> Servicios(int clinicaExternaId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var query = (from A in dbContext.service
                         join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                         where A.d_ServiceDate == _fecha
                         && A.ClinicaExternad == clinicaExternaId
                         select new ServicioBE
                         {
                             ServiceId =A.v_ServiceId,
                             FechaServicio =  A.d_ServiceDate.Value,
                             Trabajador = B.v_FirstName + " " + B.v_FirstLastName + " " +B.v_SecondLastName,
                             Sede = A.v_Sede,
                             ClinicaExterna = A.ClinicaExternad.Value
                         }).ToList();


            return query;

        }

    }
}

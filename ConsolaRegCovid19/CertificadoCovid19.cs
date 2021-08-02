using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolaRegCovid19
{
    public class CertificadoCovid19
    {
        private string _dni { get; set; }

        public CertificadoCovid19(string dni)
        {
            _dni = dni;
        }

        public List<string> ServicesCovid19() {

            var services = GetServices();
            var list = new List<string>();

            foreach (var item in services)
            {
                list.Add(string.Format("Fecha servicio: {0}, Trabajador: {1}, ServiceId: {2} ", item.FechaServicio.Value, item.Trabajador, item.ServiceId));
            }
            //services.Add(string.Format("Fecha servicio: {0}, Trabajador: {1}, ServiceId: {2} ", "01 Julio 2020", "Alberto Merchan Cosme", "00001"));
            //services.Add(string.Format("Fecha servicio: {0}, Trabajador: {1}, ServiceId: {2} ", "11 Julio 2020", "Alberto Merchan Cosme", "00002"));
            //services.Add(string.Format("Fecha servicio: {0}, Trabajador: {1}, ServiceId: {2} ", "21 Julio 2020", "Alberto Merchan Cosme", "00003"));
            return list;
        }

        private List<listService> GetServices() {

            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            try
            {
                var query = (from ser in dbContext.service
                             join per in dbContext.person on ser.v_PersonId equals per.v_PersonId
                             where per.v_DocNumber == _dni 
                                && ser.i_IsDeleted == 0
                                orderby ser.d_ServiceDate descending
                             select new listService { 
                                FechaServicio = ser.d_ServiceDate,
                                Trabajador = per.v_SecondLastName + " " + per.v_FirstLastName + " " + per.v_FirstName,
                                ServiceId = ser.v_ServiceId
                             }).ToList();
                return query;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        class listService {
            public DateTime? FechaServicio { get; set; }
            public string Trabajador { get; set; }
            public string ServiceId { get; set; }        
        }
    }
}

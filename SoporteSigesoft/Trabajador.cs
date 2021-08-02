using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoporteSigesoft
{
    public class Trabajador
    {
        public List<DatosTrabajador> BuscarTrabajador(string filtro)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var query = (from A in dbContext.pacient
                         join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                         where B.v_FirstName.Contains(filtro)
                         select new DatosTrabajador
                         {
                             ApellidoPaterno =  B.v_FirstLastName,
                             ApellidoMaterno = B.v_SecondLastName,
                             Nombres = B.v_FirstName,
                             Documento = B.v_DocNumber
                         }).ToList();

            return query;
        }
    }

    public class DatosTrabajador
    {
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombres { get; set; }
        public string Documento { get; set; }
    }
}

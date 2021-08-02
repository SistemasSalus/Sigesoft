using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsolaRegCovid19
{
   public class DatosPersonales
    {
        public string NombresTrabajador { get; set; }
    }

   public class Servicios
   {
       public string ServiceId { get; set; }
       public DateTime? FechaServicio { get; set; }
       public string Tecnico { get; set; }
       public DateTime? FechaInsert { get; set; }
       public DateTime? FechaUpdate { get; set; }
       public string Usuario { get; set; }
       public int? CorreoEnviado { get; set; }
       public string CorreoTrabajador { get; set; }
   }
}

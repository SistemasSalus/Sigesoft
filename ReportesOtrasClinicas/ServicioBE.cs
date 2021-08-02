using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportesOtrasClinicas
{
   public class ServicioBE
    {
        public string ServiceId { get; set; }
        public DateTime FechaServicio { get; set; }
        public string Trabajador { get; set; }
        public string Sede { get; set; }
        public int ClinicaExterna { get; set; }
    }
}

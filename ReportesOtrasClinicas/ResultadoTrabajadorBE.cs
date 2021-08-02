using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportesOtrasClinicas
{
   public class ResultadoTrabajadorBE
    {
        public string ServicioId { get; set; }           
        public string Nodo { get; set; }
        public DateTime FechaExamen  { get; set; }
        public string CentroMedico { get; set; }
        public string EmpresaEmpleadora { get; set; }
        public string EmpresaContratista { get; set; }
        public string NombresApellidos { get; set; }
        public string Dni { get; set; }
        public string Sexo { get; set; }
        public string Sede { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Edad { get; set; }
        public string Tecnico { get; set; }
        public string Celular { get; set; }

        public string Sintomas { get; set; }

       public string ResultadoIgM { get; set; }
       public string ResultadoIgG { get; set; }
    }
}

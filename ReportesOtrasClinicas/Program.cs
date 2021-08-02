using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportesOtrasClinicas
{
    class Program
    {
        static void Main(string[] args)
        {
            var fechaActual = DateTime.Now.Date.AddDays(-1);
            var oClinicas = new ClincasExternas(fechaActual);

            var clinicasExternas = oClinicas.Listar();

            foreach (var item in clinicasExternas)
            {
               var servicios = oClinicas.Servicios(item.ClincaExternaId);

               var resultados = new List<ResultadoTrabajadorBE>();
               foreach (var servicio in servicios)
               {
                   Console.ForegroundColor = ConsoleColor.White;
                   Console.WriteLine(string.Format("{0}, {1}", servicio.ServiceId, servicio.Trabajador));
                   var oResultadoTrabajador = new ResultadoTrabajador(servicio.ServiceId);
                   resultados.Add(oResultadoTrabajador.ObtenerResultadoTrabajador());
               }

               if (resultados.Count > 0)
               {
                   Console.ForegroundColor = ConsoleColor.Blue;
                   Console.WriteLine(string.Format("Nro. Servicios: {0}, Clínica:{1}", resultados.Count, resultados[0].CentroMedico));
                   var excel = new GenerarExcel();
                   var archivo = excel.CrearExcel(resultados);  
                
                   var correo = new EnviarCorreo(archivo,resultados[0].Sede);

                   if (correo.EnviarExcel())
                   {
                       Console.ForegroundColor = ConsoleColor.Cyan;
                       Console.WriteLine("OK EMAIL");
                   }
                   else
                   {
                       Console.ForegroundColor = ConsoleColor.Red;
                       Console.WriteLine("FAIL EMAIL");
                   }
                   
               }
               
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--FIN--");
            Console.ReadLine();
        }
    }
}

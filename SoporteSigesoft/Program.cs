using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoporteSigesoft
{
    class Program
    {
        static int opcionSeleccionada = -1;

        static void Main(string[] args)
        {
            ImprimirTitulo();
            var opciones = ListarOpcionesMenu();
            ImprimirMenu(opciones);
            opcionSeleccionada = ValidarOpcionSeleccionada(opciones);

            if (opcionSeleccionada == 3)
            {
                var fecha = DateTime.Parse("10-12-2020");
                var x = new ProcesoEnvioCorreos(fecha);

                Impresiones.ImprimirLinea(x.Regularizar());
            }
            
            Console.ReadLine();
        }

        private static void ImprimirMenu(List<OpcionMenu> lista)
        {
            Impresiones.ImprimirLinea("Lista de opciones");
            Impresiones.ImprimirLinea("*****************");            
            foreach (var item in lista)
            {
                Console.WriteLine("[{0}] {1} ", item.NumeroOpcion, item.NombreOpcion); 
            }
            Impresiones.ImprimirLinea("\n");
            
        }

        private static void ImprimirTitulo(){ 
            string titulo = "   SOPORTE SIGESOFT  ";
            string tituloLinea = "=====================\n";
            Impresiones.ImprimirLinea(ConsoleColor.Blue, ConsoleColor.White, titulo, true);
            Impresiones.ImprimirLinea(ConsoleColor.Blue, ConsoleColor.White, tituloLinea, true);
        }

        private static List<OpcionMenu> ListarOpcionesMenu()
        {
            var opcion1 = new OpcionMenu { NumeroOpcion = 1, NombreOpcion = "Buscar Trabajador" };
            var opcion2 = new OpcionMenu { NumeroOpcion = 2, NombreOpcion = "Listar Servicios de un trabajador" };
            var opcion3 = new OpcionMenu { NumeroOpcion = 3, NombreOpcion = "Regularizar envío de correos Covid" };
            var lista = new List<OpcionMenu>();
            lista.Add(opcion1);
            lista.Add(opcion2);
            lista.Add(opcion3);
            return lista;
        }

        

        #region VALIDACIONES
        private static int ValidarOpcionSeleccionada(List<OpcionMenu> opciones)
        {
            var esnumero = false;
            string option = string.Empty;
            do
            {
                Impresiones.ImprimirTexto(ConsoleColor.Black, ConsoleColor.Yellow, "Elegir opción: ", false);
                option = Console.ReadLine();
                esnumero = EsNumero(option);
                if (esnumero)
                {
                    var existeOpcion = opciones.Find(p => p.NumeroOpcion == int.Parse(option));
                    if (existeOpcion == null)
                        esnumero = false;
                }
            } while (!esnumero);
           
            return int.Parse(option);          
        }
        #endregion

        #region UTILIDADES
        private static bool EsNumero(string option)
        {
            long number1 = 0;
            bool canConvert = long.TryParse(option, out number1);

            return canConvert;
        }
        #endregion
    }
}

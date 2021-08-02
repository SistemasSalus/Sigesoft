using Sigesoft.Node.WinClient.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruceHeadCount
{
    class Program
    {
        static void Main(string[] args)
        {
            HeadCountBL oHeadCountBL = new HeadCountBL();

            var listaHeadCount = oHeadCountBL.GetListHeadCount();
            var listaTrabajadores = oHeadCountBL.GetListTrabajadores().ToList();

            Console.WriteLine("Cantidad de registros para analizar:" + listaHeadCount.Count());

            var contador = 1;
            var actualizados = 0;
            var masunacoincidencia = 0;
            var noexiste = 0;
            var registroextraño = 0;
            var DNIDuplicados = "";
            foreach (var item in listaHeadCount)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("***************ANALIZAR***************");

                var mensaje = string.Format("Item:{0}, Trabajador: {1}, DNI: {2}", contador.ToString(), item.v_NombreCompleto, item.v_Nif);
                Console.WriteLine(mensaje);

                var existeTrabajador = listaTrabajadores.FindAll(p => p.v_DocNumber.Contains(item.v_Nif.Trim()));
                if (existeTrabajador.Count == 1)
                {
                    Console.BackgroundColor = ConsoleColor.Black;

                    //if (oHeadCountBL.ActualizarSede(existeTrabajador[0].v_ServiceId, item.v_SubDivPersona))
                    //{
                    //    Console.WriteLine("ESTATUS: ACTUALIZADO");
                    //}
                    //else
                    //{
                    //    Console.BackgroundColor = ConsoleColor.Red;
                    //    Console.WriteLine("ESTATUS: NO ACTUALIZADO");
                    //    Console.BackgroundColor = ConsoleColor.Black;
                    //}
                    
                    actualizados++;
                }
                else if (existeTrabajador.Count > 1)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write("ESTATUS: ");
                    Console.WriteLine("MÁS DE UNA COINCIDENCIA");
                    Console.BackgroundColor = ConsoleColor.Black;
                    masunacoincidencia++;
                    DNIDuplicados += "; " + item.v_Nif;
                }
                else if (existeTrabajador.Count == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("ESTATUS: ");
                    Console.WriteLine("NO EXISTE");
                    Console.BackgroundColor = ConsoleColor.Black;
                    noexiste++;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("ESTATUS: ");
                    Console.WriteLine("REGISTRO EXTRAÑO");
                    Console.BackgroundColor = ConsoleColor.Black;
                    registroextraño++;
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("");
                
                contador++;

            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(string.Format("Actualizados: {0}, Mas de una Coincidencia: {1}, No existe: {2}, Registros Extraño {3}, SUMA:{4}", actualizados, masunacoincidencia, noexiste, registroextraño, actualizados + masunacoincidencia + noexiste + registroextraño));
            Console.WriteLine("Duplicados:" + DNIDuplicados);
            Console.ReadLine();

        }
    }
}

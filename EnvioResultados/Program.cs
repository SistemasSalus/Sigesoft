using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvioResultados
{
    class Program
    {
        static void Main(string[] args)
        {
            var fecha = DateTime.Now.Date;
            string ruta = Sigesoft.Common.Utils.GetApplicationConfigValue("ruta");
            string rutaLog = Sigesoft.Common.Utils.GetApplicationConfigValue("rutaLog") + DateTime.Now.ToString("MMddyyyy_hh_mm")+".txt";
            Resultados oResultados = new Resultados(fecha, ruta);

            var serviciosPendientes = oResultados.BuscarServiciosPendientesEmail();

            //var xx = serviciosPendientes.FindAll(p => p.ServiceId == "N105-SR000000432").ToList();
            using (StreamWriter outputFile = new StreamWriter(rutaLog))
            {
                Console.WriteLine("PENDIENTES POR ENVIAR: " + serviciosPendientes.Count);
                foreach (var item in serviciosPendientes)
                {
                    try
                    {
                        var resultados = oResultados.ObtenerDatos(item.ServiceId);
                        if (resultados.Count > 0)
                        {
                            

                            var resultadoCovid19 = "";
                            if (resultados[0].ResultadoPrueba.ToString() == ((int)ResultadoCovid.Negativo).ToString())
                            {
                                resultadoCovid19 = "Negativo";
                                Console.WriteLine("RESULTADO COVID: " + resultadoCovid19 + " NO SE ENVÍA");
                            }
                            else if (resultados[0].ResultadoPrueba.ToString() == ((int)ResultadoCovid.No_valido).ToString())
                            {
                                resultadoCovid19 = "No Válido";
                                Console.WriteLine("RESULTADO COVID: " + resultadoCovid19 + " NO SE ENVÍA");
                            }
                            else if (resultados[0].ResultadoPrueba.ToString() == ((int)ResultadoCovid.IgM_Positivo).ToString())
                            {
                                resultadoCovid19 = "IgM Positivo";
                                Console.WriteLine("RESULTADO COVID: " + resultadoCovid19 + " SÍ SE ENVÍA");
                            }
                            else if (resultados[0].ResultadoPrueba.ToString() == ((int)ResultadoCovid.IgG_Positivo).ToString())
                            {
                                resultadoCovid19 = "IgG Positivo";
                                Console.WriteLine("RESULTADO COVID: " + resultadoCovid19 + " SÍ SE ENVÍA");
                            }
                            else if (resultados[0].ResultadoPrueba.ToString() == ((int)ResultadoCovid.IgM_e_IgG_positivo).ToString())
                            {
                                resultadoCovid19 = "IgM e IgG Positivo";
                                Console.WriteLine("RESULTADO COVID: " + resultadoCovid19 + " SÍ SE ENVÍA");
                            }
                            else if (resultados[0].ResultadoPrueba.ToString() == ((int)ResultadoCovid.No_se_realizo).ToString())
                            {
                                resultadoCovid19 = "No se realizó";
                                Console.WriteLine("RESULTADO COVID: " + resultadoCovid19 + " NO SE ENVÍA");
                            }

                            if (resultadoCovid19 != "" && (resultadoCovid19 == "IgM Positivo" || resultadoCovid19 == "IgG Positivo" || resultadoCovid19 == "IgM e IgG Positivo"))
                            {
                                var result = oResultados.EnviarPdfTrabajador(item.ServiceId, item.OrganizationId, resultadoCovid19, resultados[0].Sede, item.ClinicaExterna);

                                var mensaje = string.Format("Trabajador: {0}, Servicio: {1}, resulato:{2}", resultados[0].Nombres + " " + resultados[0].ApellidoPaterno + " " + resultados[0].ApellidoMaterno, item.ServiceId, resultadoCovid19);
                                if (result)
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine(mensaje);
                                    outputFile.WriteLine(mensaje + "(OK)");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(mensaje);
                                    outputFile.WriteLine(mensaje + "(FAIL)");
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("EX ERROR!", ex.Message + " " + ex.StackTrace);
                    }           
                }
            }

        


            Console.WriteLine("--FIN--");            
        }

        enum ResultadoCovid
        {
            Negativo = 0,
            No_valido = 1,
            IgM_Positivo = 2,
            IgG_Positivo = 3,
            IgM_e_IgG_positivo = 4,
            No_se_realizo = 5,
        }
    }
}

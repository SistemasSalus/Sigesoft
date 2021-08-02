using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundRegCovid
{
    class Program
    {
        static void Main(string[] args)
        {
            //var fecha = DateTime.Now.Date;
            var fecha = DateTime.Now.Date.AddDays(-3);
            //var fecha = DateTime.Parse("01/02/2021");

            string rutaFichas = Utils.GetApplicationConfigValue("rutaFichasCovid");
            string rutaLog = Utils.GetApplicationConfigValue("rutaLog") + DateTime.Now.ToString("MMddyyyy_hh_mm") + ".txt";

            GenerarPdf oGenerarPdf = new GenerarPdf();
            Servicios oServicios = new Servicios();
            EnviarCorreo oEnviarCorreo = new EnviarCorreo();
            TransferenciaVigcovid oTransferenciaVigcovid = new TransferenciaVigcovid();
            StringBuilder mensaje = new StringBuilder("");

            using (StreamWriter outputFile = new StreamWriter(rutaLog))
            {
                //var listaServiciosPivot = oServicios.ListarServiciosPivot(fechai;fechaf);
                var listaServicios = oServicios.ListarServicios(fecha);
                //listaServicios = listaServicios.FindAll(p => p.ServiceId == "N110-SR000001354" || p.ServiceId == "N264-SR000000421");
                //listaServicios = listaServicios.FindAll(p => p.ServiceId == "N206-SR000000572" || p.ServiceId == "N205-SR000000291" || p.ServiceId == "N206-SR000000571" || p.ServiceId == "N205-SR000000289" || p.ServiceId == "N205-SR000000288" || p.ServiceId == "N205-SR000000293" || p.ServiceId == "N206-SR000000570" || p.ServiceId == "N206-SR000000576" || p.ServiceId == "N205-SR000000287" || p.ServiceId == "N206-SR000000569");
                //listaServicios = listaServicios.FindAll(p => p.ServiceId == "N264-SR000000431");
                //listaServicios = listaServicios.FindAll(p => p.Sede == "CD CALLAO");
                //listaServicios = listaServicios.FindAll(p => p.ServiceId.Substring(0, 4) == "N206");

                foreach (var item in listaServicios)
                {
                    try
                    {
                        if (item.NombresCompleto.ToLower().Contains("sistema"))
                        {
                            outputFile.WriteLine("SERVICIO DE PRUEBA: " + item.ServiceId);
                        }
                        else
                        {
                            mensaje.Append("TRABAJADOR: " + item.NombresCompleto);
                            var resultadoCovid = oGenerarPdf.GenerarReporte(item.ServiceId, item.TipoFormato == null ? 1 : item.TipoFormato.Value, rutaFichas, item.ComponentId);

                            mensaje.Append("\n \t PDF: " + resultadoCovid);

                            if (string.IsNullOrEmpty(resultadoCovid) || resultadoCovid == "----")
                            {
                                mensaje.Append("\n \t EMAIL: NO TIENE RESULTADO");
                            }
                            else
                            {
                                mensaje.Append("\n \t EMAIL: " + oEnviarCorreo.SendEmailToWorkerAndAnalyst(rutaFichas, item.ServiceId, item.OrganizationId, resultadoCovid, item.Sede, item.EmpresaEmpleadora,item.ComponentId).ToString());
                                oEnviarCorreo.ActualizarFlagEnvioCorreo(item.ServiceId);
                            }


                            if (resultadoCovid.ToUpper() == "IGM POSITIVO" || resultadoCovid.ToUpper() == "IGG POSITIVO" || resultadoCovid.ToUpper() == "IGM E IGG POSITIVO" || resultadoCovid.ToUpper() == "POSITIVO")
                            {
                                if (item.SedeId == null)
                                {
                                    mensaje.Append("\n \t VIGCOVID: " + "SEDE NO EXISTE EN BACKUS");
                                }
                                else if (item.TipoEmpresa != 1)
                                {
                                    mensaje.Append("\n \t VIGCOVID: " + "NO ES TIPO EMPRESA BACKUS");
                                }
                                else if (item.OrganizationId == Sigesoft.Common.Constants.EMPRESA_BACKUS_ID)
                                {
                                    var resultadoId = -1;
                                    if (resultadoCovid.ToUpper() == "IGM POSITIVO")
                                    {
                                        resultadoId = 2;
                                    }
                                    else if (resultadoCovid.ToUpper() == "IGG POSITIVO")
                                    {
                                        resultadoId = 3;
                                    }
                                    else if (resultadoCovid.ToUpper() == "IGM E IGG POSITIVO")
                                    {
                                        resultadoId = 4;
                                    }
                                    else if (resultadoCovid.ToUpper() == "POSITIVO")
                                    {
                                        resultadoId = 6;
                                    }
                                    oTransferenciaVigcovid.TransferirVigcovid(item.Documento, resultadoId, item.FechaServicio, item.SedeId.Value, item.ComponentId);
                                    mensaje.Append("\n \t VIGCOVID: " + "ENVIADO");
                                }
                                else
                                {
                                    mensaje.Append("\n \t VIGCOVID: " + "NO ES BACKUS - NO ENVIADO");
                                }

                            }
                            else
                            {
                                mensaje.Append("\n \t VIGCOVID: " + "NO");
                            }

                            mensaje.Append("\n \t SERVICIO: " + item.ServiceId);

                            outputFile.WriteLine(mensaje);
                            Console.WriteLine(mensaje);

                        }
                        mensaje = new StringBuilder("");

                    }
                    catch (Exception ex)
                    {
                        outputFile.Write(ex.Message);
                        throw;
                    }

                }
            }


            //ENVÍO DE NOTIFICACIÓN A VIGCOVID
            var listaIngresosVigilancia = new NotificacionVigcovid().ListarIngresosVigcovid();

            using (StreamWriter outputFile = new StreamWriter(rutaLog))
            {
                foreach (var item in listaIngresosVigilancia)
                {
                    var trabajador = new NotificacionVigcovid().EnviarNotificacionIngresoVigcovid(item);

                    new NotificacionVigcovid().CambiarFlagEnvio(item.Id);

                    new ReporteHojaReferencia().GenerarHojaReferencia(item.Trabajador.ToString(), item.Dni.ToString(), item.Edad.ToString(), item.Empleadora, item.PuestoTrabajo, item.EmailTrabajador, item.MedicoVigila, rutaFichas);

                    //outputFile.WriteLine("INGRESO A VIGCOVID: " + trabajador);                   
                }
            }           
            
        }
    }
}

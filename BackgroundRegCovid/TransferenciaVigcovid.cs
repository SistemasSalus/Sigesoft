using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundRegCovid
{
    public class TransferenciaVigcovid
    {
        public bool TransferirVigcovid( string dni, int resultado, DateTime fechaExamen, int sedeId, string componentId)
        {
            try
            {
                //-- 0 Negativo 
                //-- 1 No válido
                //-- 2 IgM Positivo
                //-- 3 IgG Positivo
                //-- 4 IgM e IgG positivo
                //-- 5 No se realizó
                //-- 6 Positivo
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                
                var enviarAVigCovid = true;
                var accionTransferenciaVigcovid = "";
                var registroVigcovidID = -1;

                #region Validar registro activo en VIGCOVID

                var registrosVigcovidActivos = dbContext.background_consultar_registros_vigcovid(dni).ToList();

                if (registrosVigcovidActivos.Count > 0)
                {
                    //var ultimoRegistro = registrosVigcovidActivos[0];
                    //var fechaIngresoUltimoRegistro = ultimoRegistro.FechaIngreso;
                    //registroVigcovidID = ultimoRegistro.Id;

                    //if (fechaIngresoUltimoRegistro <= fechaExamen.AddDays(-21))
                    //{
                    //16-01-21 <= 01-01-21 ACTULIZA REGISTRO EN VIGCOVID
                    accionTransferenciaVigcovid = "INSERTAR EXAMEN";

                    //}
                    //else 
                    //{
                    //01-01-21 <= 02-01-21 CREA UN NUEVO REGISTRO
                    //}
                }
                else
                {
                    accionTransferenciaVigcovid = "INSERTAR REGISTRO";
                }


                #endregion

                //if (componentId == Sigesoft.Common.Constants.COVID_ID)
                //{
                //    var serviciosAnteriores = dbContext.regcovid_background_servicios_anteriores(dni);
                //    foreach (var servicio in serviciosAnteriores)
                //    {
                //        if (servicio.Resultado.ToUpper() == "IGM POSITIVO" || servicio.Resultado.ToUpper() == "IGG POSITIVO" || servicio.Resultado.ToUpper() == "IGM E IGG POSITIVO")
                //        {
                //            enviarAVigCovid = false;
                //            break;
                //        }
                //    }

                //    if (enviarAVigCovid)
                //    {
                //        if(accionTransferenciaVigcovid == "INSERTAR REGISTRO")
                //        {
                //            dbContext.usp_enviar_vigcovid(dni, resultado, fechaExamen, sedeId, componentId);
                //        }else{
                //            dbContext.background_enviar_examen_vigcovid(registroVigcovidID, resultado, fechaExamen, componentId);
                //        }
                        
                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}
                //else {//SI ES ANTÍGENOS
                    if (resultado.ToString() == "6")
                    {
                        if (accionTransferenciaVigcovid == "INSERTAR REGISTRO")
                        {
                            dbContext.usp_enviar_vigcovid(dni, resultado, fechaExamen, sedeId, componentId);
                        }
                        else
                        {
                            dbContext.background_enviar_examen_vigcovid(registroVigcovidID, resultado, fechaExamen, componentId);
                        }

                        
                        return true;
                    }

                        return false;
                //}              
                
                
            }
            catch (Exception ex)
            {                
                throw;
            }
            
        }
    }
}

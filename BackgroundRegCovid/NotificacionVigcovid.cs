using Sigesoft.Common;
using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackgroundRegCovid
{
    public class NotificacionVigcovid
    {
        public List<usp_notificacion_vigcovidResult> ListarIngresosVigcovid()
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var lista = dbContext.usp_notificacion_vigcovid().ToList();

            return lista;
        }

        public string EnviarNotificacionIngresoVigcovid(usp_notificacion_vigcovidResult ousp_notificacion_vigcovidResult)
        {
            OperationResult objOperationResult = new OperationResult();
            try
            {
                var configEmail = Sigesoft.Node.WinClient.BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 161, "i_ParameterId");

                var correoTrabajador = ousp_notificacion_vigcovidResult.EmailTrabajador;
                var correoAnalista = ousp_notificacion_vigcovidResult.EmailAnalista;
                var correoBP = ousp_notificacion_vigcovidResult.EmailBP;
                var correoChampion = ousp_notificacion_vigcovidResult.EmailChampion;
                var correoSeguridadFisica = ousp_notificacion_vigcovidResult.EmailSeguridadFisica;
                var nombreTrabajador = ousp_notificacion_vigcovidResult.Trabajador;                

                string smtp = configEmail[0].Value1.ToLower();
                int port = int.Parse(configEmail[1].Value1);
                string from = configEmail[2].Value1.ToLower();
                string fromPassword = configEmail[4].Value1;
                string subject = "INGRESO A VIGILANCIA";
                bool enableSsl = Convert.ToBoolean(int.Parse(configEmail[3].Value1));
                string message = string.Format("Buenos días, el trabajador {0} , ha sido ingresado a vigilancia",nombreTrabajador);

                var enviarA = correoTrabajador;
                var copiaCc = correoAnalista + ";" + correoBP + ";" + correoChampion + ";" + correoSeguridadFisica;

                var copiaBcc = "alberto.merchan@saluslaboris.com.pe;luis.delacruz@saluslaboris.com.pe";
                Sigesoft.Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, enviarA, copiaCc, copiaBcc, subject, message, null);

                return nombreTrabajador;
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }

        public void CambiarFlagEnvio(int Idtrabajador)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            dbContext.usp_cambiarestadoenvionotificacion(Idtrabajador);
        }

    }
}

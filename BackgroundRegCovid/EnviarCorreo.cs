using Sigesoft.Common;
using Sigesoft.Node.WinClient.BLL;
using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundRegCovid
{
    public class EnviarCorreo
    {
        public bool EnviarPdfTrabajador_(string ruta, string serviceId, string organizationId, string resultadoCovid, string sedeEmpresa)
        {
            OperationResult objOperationResult = new OperationResult();
            try
            {                
                var configEmail = Sigesoft.Node.WinClient.BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 161, "i_ParameterId");

                

                var trabajadorData = new ServiceBL().GetWorkerData(serviceId);
                if (string.IsNullOrEmpty(trabajadorData.Email)) return false;

                var correoTrabajador = trabajadorData.Email;
                string smtp = configEmail[0].Value1.ToLower();
                int port = int.Parse(configEmail[1].Value1);
                string from = configEmail[2].Value1.ToLower();
                string fromPassword = configEmail[4].Value1;
                string subject = "Resultado Covid-19";
                bool enableSsl = Convert.ToBoolean(int.Parse(configEmail[3].Value1));
                string message = string.Format("Buenos días, el resultado del trabajador {0} , es: {1}", trabajadorData.Trabajador, resultadoCovid);

                #region Buscar Archivo
                
                var atachFiles = new List<string>();
                var atachFile = "";

                DirectoryInfo di = new DirectoryInfo(ruta);
                foreach (var fi in di.GetFiles(serviceId + ".pdf"))
                    atachFile = ruta + serviceId + ".pdf";

                if (atachFile == "")
                    atachFile = ruta + serviceId + "-N003-ME000000060" + ".pdf";

                atachFiles.Add(atachFile);
                #endregion

                var copiaCc = "";
                if (resultadoCovid == "IgM Positivo" || resultadoCovid == "IgG Positivo" || resultadoCovid == "IgM e IgG positivo")
                {
                    //copiaCc = ObtenerCorreosPorSede(sedeEmpresa); 
                }             

                var enviarA = correoTrabajador;
                var copiaBcc = "luis.delacruz@saluslaboris.com.pe, saul.ramos@saluslaboris.com.pe";
                Sigesoft.Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, enviarA, copiaCc, copiaBcc, subject, message, atachFiles);
                //Sigesoft.Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, "", "albertomerchanc@hotmail.com", "", subject, message, atachFiles);

                if (ActualizarFlagEnvioCorreo(serviceId))
                    return true;
                else
                    return false;            

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string SendEmailToWorkerAndAnalyst(string ruta, string serviceId, string organizationId, string resultadoCovid, string sedeEmpresa, string empresaEmpleadora, string componentId)
        {
            OperationResult objOperationResult = new OperationResult();
            try
            {
                var workerData = new ServiceBL().GetWorkerData(serviceId);
                if (string.IsNullOrEmpty(workerData.Email))                
                    return "NO TIENE CORREO";

                var to = workerData.Email;
                var cc = ObtenerCorreosPorSede(sedeEmpresa, resultadoCovid, organizationId);
                if (organizationId == Constants.EMPRESA_PERULNG_ID)
                {
                    //cc = ObtenerCorreosPorSede(sedeEmpresa, resultadoCovid, organizationId);

                    var correosContratista = ObtenerCorreosPorEmpleadora(empresaEmpleadora);
                    if (!string.IsNullOrEmpty(correosContratista))
                    {
                        cc += ";" + correosContratista;
                    }                    
                }
                var bcc = "saul.ramos@saluslaboris.com.pe;luis.delacruz@saluslaboris.com.pe";
                string body = string.Format("Buenos días, el resultado del trabajador {0} , es: {1}", workerData.Trabajador, resultadoCovid);
                var attachmentFileList = GetPruebaRapidaFilePathList(serviceId, ruta, componentId);

                SendEmail oSendEmail = new SendEmail("Resultado Covid-19", to, cc, bcc, body, attachmentFileList);
                oSendEmail.Send();
                return "ENVÍO CORRECTO";
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex;
            }
        }

        public bool ActualizarFlagEnvioCorreo(string serviceId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objServicio = (from A in dbContext.service where A.v_ServiceId == serviceId select A).FirstOrDefault();

                objServicio.CorreoEnviado = 1;
                dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private string ObtenerCorreosPorSede(string sede, string resultadoExamen, string organizationId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var resultados = (from a in dbContext.location where a.v_OrganizationId == organizationId && a.v_Name.ToUpper().Trim() == sede.ToUpper().Trim() select a).ToList();

            var correos = "";
            foreach (var item in resultados)
            {
                if (item.v_Email != null)
                {
                    correos += item.v_Email + ";";
                }

            }

            //if (organizationId == "N003-OO000001651")
            //{
            //    return correos;
            //}
            //else
            //{
                //if (resultadoExamen == "IgM Positivo" || resultadoExamen == "IgG Positivo" || resultadoExamen == "IgM e IgG positivo" || resultadoExamen == "Positivo")
                //{
                    return correos;
                //}
            //}

            //return "";
        }

        private string ObtenerCorreosPorEmpleadora(string empresaEmpleadora)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var resultados = (from a in dbContext.empresasempleadoras where a.Nombre.ToUpper().Trim() == empresaEmpleadora.ToUpper().Trim() select a).ToList();

            var correos = "";
            foreach (var item in resultados)
            {
                if (item.Correo != null)
                {
                    correos += item.Correo + ";";
                }

            }

            return correos;
        }


        private List<string> GetPruebaRapidaFilePathList(string serviceId, string pathDirectory, string componentId)
        {
            var attachFiles = new List<string>();
            var atachFile = "";

            DirectoryInfo di = new DirectoryInfo(pathDirectory);

            if (componentId == Constants.COVID_ID)
            {
                foreach (var fi in di.GetFiles(serviceId + " PR.pdf"))
                    atachFile = pathDirectory + serviceId + " PR.pdf";
            }
            if (componentId == Constants.ANTIGENOS_ID)
            {
                foreach (var fi in di.GetFiles(serviceId + " ANT.pdf"))
                    atachFile = pathDirectory + serviceId + " ANT.pdf";
            }

            //foreach (var fi in di.GetFiles(serviceId + ".pdf"))
            //    atachFile = pathDirectory + serviceId + ".pdf";

            if (atachFile == "")
                atachFile = pathDirectory + serviceId + "-N003-ME000000060" + ".pdf";

            attachFiles.Add(atachFile);

            return attachFiles;
            
        }

    }
}

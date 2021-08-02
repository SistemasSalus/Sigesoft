using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using NetPdf;
using Sigesoft.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace BackgroundRegCovid
{
    public class ReporteHojaReferencia
    {

        public string GenerarHojaReferencia(string trabajador, string dni, string edad, string empresa, string puestoTrabajo,string correoTrabajador, string medicoVigila, string ruta)
        {

            MergeExPDF _mergeExPDF = new MergeExPDF();
            ReportDocument rp = new ReportDocument();
            List<string> _filesNameToMerge = new List<string>();

            var listaEntidad = new List<HojaReferencia>();
            var entidad = new HojaReferencia();
            entidad.Trabajador = trabajador;
            entidad.Dni = dni;
            entidad.Edad = edad;
            entidad.Empleadora = empresa;
            entidad.PuestoTrabajo = puestoTrabajo;
            entidad.MedicoVigila = medicoVigila;
            listaEntidad.Add(entidad);

            var dsGetRepo = new DataSet();
            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(listaEntidad);
            dt.TableName = "dtHojaReferencia";
            dsGetRepo.Tables.Add(dt);
            rp = new Reportes.crHojaReferencia();
            rp.SetDataSource(dsGetRepo);
            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            var objDiskOpt = new DiskFileDestinationOptions();
            objDiskOpt.DiskFileName = ruta + trabajador + "-" + dni+ ".pdf";
            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
            rp.ExportOptions.DestinationOptions = objDiskOpt;
            rp.Export();
            rp.Close();

            var rutaDirectorio = AppDomain.CurrentDomain.BaseDirectory;
            _filesNameToMerge.Add(rutaDirectorio + "hojaReferencia2.pdf");

            var x = _filesNameToMerge.ToList();
            _mergeExPDF.FilesName = x;
            _mergeExPDF.DestinationFile = ruta + dni + ".pdf";
            _mergeExPDF.Execute();

            EnviarNotificacionIngresoVigcovid(dni, correoTrabajador, trabajador, ruta);

            return "";
        }



        public string EnviarNotificacionIngresoVigcovid(string dni, string correoTrabajador, string nombreTrabajador, string rutaArchivo)
        {
            OperationResult objOperationResult = new OperationResult();
            try
            {
                var configEmail = Sigesoft.Node.WinClient.BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 161, "i_ParameterId");              

                string smtp = configEmail[0].Value1.ToLower();
                int port = int.Parse(configEmail[1].Value1);
                string from = configEmail[2].Value1.ToLower();
                string fromPassword = configEmail[4].Value1;
                string subject = "INGRESO A VIGILANCIA";
                bool enableSsl = Convert.ToBoolean(int.Parse(configEmail[3].Value1));
                string message = string.Format("Buenos días, {0} , se adjunta la hoja de referencia", nombreTrabajador);

                var enviarA = correoTrabajador;

                #region Buscar Archivo

                string ruta = rutaArchivo;
                var atachFiles = new List<string>();
                var atachFile = "";

                DirectoryInfo di = new DirectoryInfo(rutaArchivo);
                foreach (var fi in di.GetFiles(dni + ".pdf"))
                {
                    atachFile = ruta + dni + ".pdf";

                }

                if (atachFile == "")
                {
                    return "NO SE ENVIÓ";
                }

                atachFiles.Add(atachFile);
                #endregion               


                var copiaBcc = "saul.ramos@saluslabois.com.pe; luis.delacruz@gmail.com";
                //Sigesoft.Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, enviarA, "", copiaBcc, subject, message, atachFiles);

                return nombreTrabajador;
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }
    }




    public class HojaReferencia
    {
        public string Trabajador { get; set; }
        public string Dni { get; set; }
        public string Edad { get; set; }
        public string Empleadora { get; set; }
        public string PuestoTrabajo { get; set; }
        public string MedicoVigila { get; set; }

    }
}

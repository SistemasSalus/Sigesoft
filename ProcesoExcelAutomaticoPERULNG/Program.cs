using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;

namespace ProcesoExcelAutomaticoPERULNG
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBL oServiceBL = new ServiceBL();
            OperationResult objOperationResult = new OperationResult();

            Console.WriteLine("==============================================");
            Console.WriteLine("**REPORTE CONSOLIDADO PERULNG**");
            Console.WriteLine("==============================================");
            string ruta = Sigesoft.Common.Utils.GetApplicationConfigValue("rutaReportes").ToString();
            Console.WriteLine("Ruta del reporte: " + ruta);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");

            //var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //var startDate = DateTime.Parse("01/05/2020");
            var startDate = DateTime.Now.Date;
            var EndDate = DateTime.Now.Date;
            //var EndDate = DateTime.Parse("31/03/2021");
            
            Console.WriteLine("Generando Excel...");

            var servicesHoy = oServiceBL.GetServicesPagedAndFilteredFullNodeJessicaOblitas(ref objOperationResult, 0, null, "", "", startDate, EndDate);

            //servicesHoy = servicesHoy.FindAll(p => p.v_OrganizationId == Constants.EMPRESA_PERULNG_ID).ToList();
            servicesHoy = servicesHoy.FindAll(p => p.v_OrganizationId == Constants.EMPRESA_TRANSPORTES77_ID).ToList();

            var ListaServicios = servicesHoy;
            var total = ListaServicios.Count();
            var lista = new List<ReporteExcelAutomaticoCovid19>();
            foreach (var servicio in ListaServicios)
            {
                var oReporteExcelCovid19 = oServiceBL.ReporteExcelCovid19Backus(servicio.v_ServiceId);

                if (oReporteExcelCovid19 != null)
                {
                    if (oReporteExcelCovid19.Resultado != "SIN DATOS")
                    {
                        lista.Add(oReporteExcelCovid19);
                    }
                    Console.WriteLine("SERVICVIOID:" + oReporteExcelCovid19.ServicioId + " " + oReporteExcelCovid19.FechaExamen + " " + oReporteExcelCovid19.Dni + " " + total--);
                }
            }

            #region CREACIÓN DE EXCEL CONSOLIDADO Y ENVIÓ DE CORREO A EMPRESA PRINCIPAL
            var rutaExcel = CrearExcel(lista, ruta, "T77");

            if (!string.IsNullOrEmpty(rutaExcel))
            {
                Console.WriteLine("Envío de correo a EMPRESA PRINCIPAL");
                //var to = "SRamirez2@huntloc.pe";
                var to = "Melisa.Fernandez@ab-inbev.com;miguel.maticorena@saluslaboris.com.pe";
                var cco = "rafael.carrasco@saluslaboris.com.pe;saul.ramos@saluslaboris.com.pe;luis.delacruz@saluslaboris.com.pe;johana.vergara@saluslaboris.com.pe; hreeves@saluslaboris.com.pe; liliana.rizo@saluslaboris.com.pe";

                EnviarExcelEmpresa(rutaExcel, to, cco);
            }
            #endregion

            #region CREACIÓN DE EXCEL Y ENVÍO DE CORREO POR EMPRESA EMPLEADORA

            var empresasEmpleadoras = lista.GroupBy(p => p.EmpresaEmpleadora).Select(s => s.First()).ToList();

            foreach (var empleadora in empresasEmpleadoras)
            {
                Console.WriteLine("Envío de correo a EMPRESA EMPLEADORA: " + empleadora.EmpresaEmpleadora);

                if (empleadora.EmpresaPrincipal != empleadora.EmpresaEmpleadora)
                {
                    var rutaExcelEmpleadora = CrearExcel(lista.FindAll(p => p.EmpresaEmpleadora == empleadora.EmpresaEmpleadora), ruta, empleadora.EmpresaEmpleadora);

                    var correosEmpleadora = oServiceBL.CorreosPorEmpresaEmpleadora(empleadora.EmpresaEmpleadora);

                    if (!string.IsNullOrEmpty(correosEmpleadora))
                    {
                        EnviarExcelEmpresa(rutaExcelEmpleadora, correosEmpleadora, "");
                    }
                }

            }

            #endregion
        }

        private static string CrearExcel(List<ReporteExcelAutomaticoCovid19> lista, string ruta, string empresa)
        {
            var fila = 1;
            string fileName;
            fileName = string.Format("{1} {0}-{2}", DateTime.Now.ToString("ddMMyyy hhmm"), empresa, "T77");

            Excel.Application xlSamp = new Microsoft.Office.Interop.Excel.Application();

            if (xlSamp == null)
            {
                Console.WriteLine("Excel is not Installed");
                Console.ReadKey();
                return "";
            }

            Excel.Workbook xlWorkbook;
            Excel.Worksheet xlWorksheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkbook = xlSamp.Workbooks.Add(misValue);
            xlWorksheet = (Excel.Worksheet)xlWorkbook.Worksheets.get_Item(1);



            xlWorksheet.Cells[fila, 1] = "FECHA DE PRUEBA";
            xlWorksheet.Cells[fila, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 1].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 1].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 2] = "DNI";
            xlWorksheet.Cells[fila, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 2].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 2].ColumnWidth = 20;
            xlWorksheet.Cells[fila, 2].NumberFormat = "@";

            xlWorksheet.Cells[fila, 3] = "APELLIDO PATERNO";
            xlWorksheet.Cells[fila, 3].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 3].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 3].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 4] = "APELLIDO MATERNO";
            xlWorksheet.Cells[fila, 4].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 4].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 4].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 5] = "NOMBRES";
            xlWorksheet.Cells[fila, 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 5].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 5].ColumnWidth = 20;

            //xlWorksheet.Cells[fila, 6] = "TIPO DE EMPRESA";
            //xlWorksheet.Cells[fila, 6].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            //xlWorksheet.Cells[fila, 6].EntireRow.Font.Bold = true;
            //xlWorksheet.Cells[fila, 6].ColumnWidth = 40;

            xlWorksheet.Cells[fila, 6] = "EMPRESA PRINCIPAL";
            xlWorksheet.Cells[fila, 6].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 6].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 6].ColumnWidth = 40;

            xlWorksheet.Cells[fila, 7] = "EMPRESA EMPLEADORA";
            xlWorksheet.Cells[fila, 7].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 7].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 7].ColumnWidth = 40;

            xlWorksheet.Cells[fila, 8] = "CD(LUGAR)";
            xlWorksheet.Cells[fila, 8].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 8].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 8].ColumnWidth = 20;

            //xlWorksheet.Cells[fila, 9] = "ÁREA";
            //xlWorksheet.Cells[fila, 9].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            //xlWorksheet.Cells[fila, 9].EntireRow.Font.Bold = true;
            //xlWorksheet.Cells[fila, 9].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 9] = "PUESTO";
            xlWorksheet.Cells[fila, 9].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 9].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 9].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 10] = "EDAD";
            xlWorksheet.Cells[fila, 10].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 10].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 10].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 11] = "SEXO";
            xlWorksheet.Cells[fila, 11].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 11].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 11].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 12] = "CELULAR";
            xlWorksheet.Cells[fila, 12].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 12].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 12].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 13] = "RESULTADO ACTUAL";
            xlWorksheet.Cells[fila, 13].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 13].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 13].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 14] = "RESULTADO ANTERIOR";
            xlWorksheet.Cells[fila, 14].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 14].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 14].ColumnWidth = 21;

            xlWorksheet.Cells[fila, 15] = "FECHA DE RESULTADO ANTERIOR";
            xlWorksheet.Cells[fila, 15].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 15].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 15].ColumnWidth = 21;

            xlWorksheet.Cells[fila, 16] = "(servicio id)";
            xlWorksheet.Cells[fila, 16].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 16].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 16].ColumnWidth = 21;

            xlWorksheet.Cells[fila, 17] = "(técnico)";
            xlWorksheet.Cells[fila, 17].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 17].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 17].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 18] = "(centro médico)";
            xlWorksheet.Cells[fila, 18].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 18].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 18].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 19] = "TIPO EXAMEN";
            xlWorksheet.Cells[fila, 19].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 19].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 19].ColumnWidth = 21;
            
            xlWorksheet.Cells[fila, 20] = "RAZÓN EXAMEN";
            xlWorksheet.Cells[fila, 20].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 20].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 20].ColumnWidth = 22;

            xlWorksheet.Cells[fila, 21] = "LUGAR EXAMEN";
            xlWorksheet.Cells[fila, 21].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 21].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 21].ColumnWidth = 23;

            fila++;
            foreach (var item in lista)
            {
                xlWorksheet.Cells[fila, 1] = item.FechaExamen;
                xlWorksheet.Cells[fila, 2].NumberFormat = "@";
                xlWorksheet.Cells[fila, 2] = item.Dni;
                xlWorksheet.Cells[fila, 3] = item.ApellidoPaterno;
                xlWorksheet.Cells[fila, 4] = item.ApellidoMaterno;
                xlWorksheet.Cells[fila, 5] = item.Nombres;
                xlWorksheet.Cells[fila, 6] = item.EmpresaPrincipal;
                xlWorksheet.Cells[fila, 7] = item.EmpresaEmpleadora;
                xlWorksheet.Cells[fila, 8] = item.Sede;
                xlWorksheet.Cells[fila, 9] = item.Puesto;
                xlWorksheet.Cells[fila, 10] = item.Edad;
                xlWorksheet.Cells[fila, 11] = item.Sexo;
                xlWorksheet.Cells[fila, 12] = item.Celular;
                xlWorksheet.Cells[fila, 13] = item.Resultado;
                xlWorksheet.Cells[fila, 14] = item.AntecedenteResultado;
                xlWorksheet.Cells[fila, 15] = item.AntecedenteFechaResultado;
                xlWorksheet.Cells[fila, 16] = item.ServicioId;
                xlWorksheet.Cells[fila, 17] = item.Tecnico;
                xlWorksheet.Cells[fila, 18] = item.CentroMedico;
                xlWorksheet.Cells[fila, 19] = item.TipoExamen;
                xlWorksheet.Cells[fila, 20] = item.RazonExamen;
                xlWorksheet.Cells[fila, 21] = item.LugarExamen;

                fila++;
            }

            string location = ruta + fileName + " " + DateTime.Now.Date.ToString("dd MM") + ".xls";
            xlWorkbook.SaveAs(location, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkbook.Close(true, misValue, misValue);
            xlSamp.Quit();

            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSamp);
                xlSamp = null;
                return location;
            }
            catch (Exception ex)
            {
                xlSamp = null;
                Console.Write("Error " + ex.ToString());
                return "";
            }
            finally
            {
                GC.Collect();
            }
        }

        private static void EnviarExcelEmpresa(string rutaExcel, string to, string cco)
        {
            OperationResult objOperationResult = new OperationResult();
            try
            {
                var configEmail = Sigesoft.Node.WinClient.BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 161, "i_ParameterId");

                string smtp = configEmail[0].Value1.ToLower();
                int port = int.Parse(configEmail[1].Value1);
                string from = configEmail[2].Value1.ToLower();
                string fromPassword = configEmail[4].Value1;
                bool enableSsl = Convert.ToBoolean(int.Parse(configEmail[3].Value1));

                //string smtp = "mail.saluslaboris.pe";
                //int port_ = int.Parse("465");
                //string from = "resultadoscovid@saluslaboris.pe";
                //string fromPassword = "SalusLaboris";
                //bool enableSsl = true;

                string subject = "Resultados Covid-19";
                string message = string.Format("Buenos tardes, se adjunta excel con resultados COVID19");
                var atachFiles = new List<string>();
                string atachFile = rutaExcel;
                atachFiles.Add(atachFile);
                Sigesoft.Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, to, cco, subject, message, atachFiles);

                //Sigesoft.Common.Utils.SendMessage(smtp, port_, enableSsl, true, from, fromPassword, "luis.delacruz@saluslaboris.com.pe", "", subject, message, atachFiles);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

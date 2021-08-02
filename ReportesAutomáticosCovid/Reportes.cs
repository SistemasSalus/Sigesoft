using SendGrid;
using SendGrid.Helpers.Mail;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
namespace ReportesAutomáticosCovid
{
    public class Reportes
    {
        public void GenerateExcel()
        {

            ServiceBL oServiceBL = new ServiceBL();
            OperationResult objOperationResult = new OperationResult();


            try
            {

                //DateTime? pdatBeginDate = DateTime.Now.Date;
                //DateTime? pdatEndDate = DateTime.Now.Date.AddDays(1);

                //DateTime? pdatBeginDate = DateTime.Parse("21/11/2020");
                //DateTime? pdatEndDate = DateTime.Parse("30/11/2020");

                //var servicesHoy = oServiceBL.GetServicesPagedAndFilteredFullNode(ref objOperationResult, 0, null, "", "", pdatBeginDate, pdatEndDate);

                //servicesHoy = servicesHoy.FindAll(p => p.v_OrganizationId == "N003-OO000001651").ToList();

                //var listaEmpresas = servicesHoy.GroupBy(g => g.v_CustomerOrganizationId).Select(s => s.First()).ToList();

                //foreach (var empresa in listaEmpresas)
                //{
                //    var servicios = servicesHoy.FindAll(p => p.v_CustomerOrganizationId == empresa.v_CustomerOrganizationId).Select(s => s.v_ServiceId).ToList();

                //    var lista = new List<ReporteExcelCovid19>();
                //    foreach (var servicio in servicios)
                //    {
                //        var oReporteExcelCovid19 = oServiceBL.ReporteExcelCovid19(servicio);
                //        if (oReporteExcelCovid19 != null)
                //        {
                //            lista.Add(oReporteExcelCovid19);
                //        }
                //    }
                //    var rutaExcel = CrearExcel(lista);

                //    if (!string.IsNullOrEmpty(rutaExcel))
                //    {
                //        if (empresa.Email == "")
                //        {

                //        }
                //        else
                //        {
                //            //EnviarExcelEmpresa(rutaExcel, empresa.Email);
                //        }

                //    }
                //}


                //#region Backus

                //DateTime? pdatBeginDate = DateTime.Parse("09/09/2020");
                //DateTime? pdatEndDate = DateTime.Parse("10/09/2020");

                //var servicesHoy = oServiceBL.GetServicesPagedAndFilteredFullNode(ref objOperationResult, 0, null, "", "", pdatBeginDate, pdatEndDate);
                //var aristri = servicesHoy.Find(p => p.v_DocNumber == "10612483");
             
                //Console.WriteLine(servicesHoy.Count);
                //servicesHoy = servicesHoy.FindAll(p => p.v_OrganizationId == "N003-OO000000425").ToList();
                //Console.WriteLine(servicesHoy.Count);

                //var listaEmpresas = servicesHoy.GroupBy(g => g.v_CustomerOrganizationId).Select(s => s.First()).ToList();

                //foreach (var empresa in listaEmpresas)
                //{
                //    var servicios = servicesHoy.FindAll(p => p.v_CustomerOrganizationId == empresa.v_CustomerOrganizationId).Select(s => s.v_ServiceId).ToList();

                //    var lista = new List<ReporteExcelCovid19>();
                //    foreach (var servicio in servicios)
                //    {
                //        if (servicio =="N102-SR000002789")
                //        {
                            
                //        }
                //        var oReporteExcelCovid19 = oServiceBL.ReporteExcelCovid19(servicio);
                //        if (oReporteExcelCovid19 != null)
                //        {
                //            lista.Add(oReporteExcelCovid19);
                //        }
                //    }
                //    var rutaExcel = CrearExcel(lista);
                //    if (!string.IsNullOrEmpty(rutaExcel))
                //    {
                //        //EnviarExcelEmpresa(rutaExcel, empresa.Email);
                //    }


                //}
                //#endregion

                //DateTime pdatBeginDate = DateTime.Parse("30/09/2020");
                //DateTime pdatEndDate = DateTime.Parse("01/10/2020");

                //var response = oServiceBL.ReporteFichasCovid19(pdatBeginDate, pdatEndDate);
                //var demo = response;


                #region Jessica Oblitas
                Console.WriteLine("==============================================");
                Console.WriteLine("**REPORTE CONSOLIDADO BACKUS - PRUEBA RÁPIDA**");
                Console.WriteLine("==============================================");
                string ruta = Sigesoft.Common.Utils.GetApplicationConfigValue("rutaReportes").ToString();

                Console.WriteLine("Ruta del reporte: " + ruta);
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");

                var startDate = DateTime.Now.Date;
                var EndDate = DateTime.Now.Date;

                Console.WriteLine("Ingresar Fecha de inicio (YYYY-MM-DD)");
                var readLine1 = Console.ReadLine();
                if (!string.IsNullOrEmpty(readLine1))
                    startDate = DateTime.Parse(readLine1);

                Console.WriteLine("Ingresar Fecha de fin (YYYY-MM-DD)");
                var readLine2 = Console.ReadLine();
                if (!string.IsNullOrEmpty(readLine2))
                    EndDate = DateTime.Parse(readLine2);

                Console.WriteLine("procesando...");           

                var servicesHoy = oServiceBL.GetServicesPagedAndFilteredFullNodeJessicaOblitas(ref objOperationResult, 0, null, "", "", startDate, EndDate);

                servicesHoy = servicesHoy.FindAll(p => p.v_OrganizationId == "N003-OO000000425").ToList();

                var ListaServicios = servicesHoy;
                var total = ListaServicios.Count();
                var lista = new List<ReporteExcelCovid19JessicaOblitas>();
                foreach (var servicio in ListaServicios)
                {
                    
                        var oReporteExcelCovid19 = oServiceBL.ReporteExcelCovid19JesicaOblitas(servicio.v_ServiceId);

                        if (oReporteExcelCovid19 != null)
                        {
                            if (oReporteExcelCovid19.Resultado != "SIN DATOS")
                            {
                                lista.Add(oReporteExcelCovid19);
                            }
                            Console.WriteLine("SERVICVIOID:" + oReporteExcelCovid19.FechaExamen + " " + oReporteExcelCovid19.Dni + " " + total--);
                        }
                                     
                    
                }
                var rutaExcel = CrearExcelJesicaOblitas(lista, ruta);
                //if (!string.IsNullOrEmpty(rutaExcel))
                //{
                //    var to_emails = new List<EmailAddress>
                //    {
                //        //new EmailAddress("francisco.collantes@saluslaboris.com.pe"),
                //        new EmailAddress("luis.delacruz@saluslaboris.com.pe"),
                //        ////new EmailAddress("SRamirez2@huntloc.pe"),
                //        //// new EmailAddress("juan.bazarte@saluslaboris.com.pe")
                //    };
                //    //ExecuteEmail(rutaExcel, to_emails).Wait();
                //    //SRamirez2@huntloc.pe; juan.bazarte@saluslaboris.com.pe
                //    //EnviarExcelEmpresa(rutaExcel, "marlene.pachas@saluslaboris.com.pe;usebackuslima.tecnica@saluslaboris.com.pe;magali.reyes@ab-inbev.com;jessica.oblitas@saluslaboris.com.pe;claudia.castaneda@saluslaboris.com.pe;GLADYS.FLORES@ab-inbev.com;Flor.Vargas@ab-inbev.com;Graciela.Ferrel@ab-inbev.com;ANTOINETTE.LOSTAUNAUFELIX@ab-inbev.com;Sandy.Jauregui@ab-inbev.com");
                //}

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
                Console.ReadLine();
                throw;
            }



        }

        private string CrearExcel(List<ReporteExcelCovid19> lista, string ruta)
        {
            var fila = 4;
            
            string fileName;
            fileName = string.Format("{0} {1}", lista[0].EmpresaEmpleadora, DateTime.Now.Date.ToString("ddMMyyy"));



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

            xlWorksheet.Cells[fila, 1] = "Fecha Examen";
            xlWorksheet.Cells[fila, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 1].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 1].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 2] = "Centro Médico";
            xlWorksheet.Cells[fila, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 2].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 2].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 3] = "Unidad";
            xlWorksheet.Cells[fila, 3].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 3].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 3].ColumnWidth = 30;

            xlWorksheet.Cells[fila, 4] = "Empresa Empleadora";
            xlWorksheet.Cells[fila, 4].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 4].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 4].ColumnWidth = 40;

            xlWorksheet.Cells[fila, 5] = "Nombres Completos";
            xlWorksheet.Cells[fila, 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 5].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 5].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 6] = "Dni";
            xlWorksheet.Cells[fila, 6].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 6].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 6].ColumnWidth = 20;
            xlWorksheet.Cells[fila, 6].NumberFormat = "@";

            xlWorksheet.Cells[fila, 7] = "Edad";
            xlWorksheet.Cells[fila, 7].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 7].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 7].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 8] = "Síntomas";
            xlWorksheet.Cells[fila, 8].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 8].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 8].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 9] = "Resultado";
            xlWorksheet.Cells[fila, 9].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 9].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 9].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 10] = "";
            xlWorksheet.Cells[fila, 10].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 10].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 10].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 10] = "VALOR EXAMEN";
            xlWorksheet.Cells[fila, 10].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 10].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 10].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 11] = "Aptitud";
            xlWorksheet.Cells[fila, 11].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 11].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 11].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 12] = "Motivo";
            xlWorksheet.Cells[fila, 12].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 12].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 12].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 13] = "Técnico";
            xlWorksheet.Cells[fila, 13].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 13].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 13].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 14] = "Celular";
            xlWorksheet.Cells[fila, 14].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 14].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 14].ColumnWidth = 20;

            fila++;
            foreach (var item in lista)
            {
                xlWorksheet.Cells[fila, 1] = item.FechaExamen;
                xlWorksheet.Cells[fila, 2] = item.CentroMedico;
                xlWorksheet.Cells[fila, 3] = item.Unidad;
                xlWorksheet.Cells[fila, 4] = item.EmpresaEmpleadora;
                xlWorksheet.Cells[fila, 5] = item.NombresApellidos;
                xlWorksheet.Cells[fila, 6] = item.Dni;
                xlWorksheet.Cells[fila, 7] = item.Edad;
                xlWorksheet.Cells[fila, 8] = item.Sintomas;
                xlWorksheet.Cells[fila, 9] = item.Resultado;
                xlWorksheet.Cells[fila, 10] = item.ResultadoIgG;
                xlWorksheet.Cells[fila, 10] = item.ValorExamen;
                xlWorksheet.Cells[fila, 11] = item.Aptitud;
                xlWorksheet.Cells[fila, 12] = item.Motivo;
                xlWorksheet.Cells[fila, 13] = item.Tecnico;
                xlWorksheet.Cells[fila, 14] = item.Celular;
                fila++;
            }

            string location = ruta + fileName + ".xls";
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

        private string CrearExcelJesicaOblitas(List<ReporteExcelCovid19JessicaOblitas> lista, string ruta)
        {
            var fila = 4;
            //string ruta = Sigesoft.Common.Utils.GetApplicationConfigValue("rutaReportes").ToString();
            string fileName;
            fileName = string.Format("Backus {0}", DateTime.Now.Date.ToString("ddMMyyy"));

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

            xlWorksheet.Cells[fila, 6] = "TIPO DE EMPRESA";
            xlWorksheet.Cells[fila, 6].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 6].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 6].ColumnWidth = 40;

            xlWorksheet.Cells[fila, 7] = "EMPRESA PRINCIPAL";
            xlWorksheet.Cells[fila, 7].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 7].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 7].ColumnWidth = 40;

            xlWorksheet.Cells[fila, 8] = "EMPRESA EMPLEADORA";
            xlWorksheet.Cells[fila, 8].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 8].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 8].ColumnWidth = 40;

            xlWorksheet.Cells[fila, 9] = "CD(LUGAR)";
            xlWorksheet.Cells[fila, 9].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 9].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 9].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 10] = "ÁREA";
            xlWorksheet.Cells[fila, 10].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 10].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 10].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 11] = "PUESTO";
            xlWorksheet.Cells[fila, 11].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 11].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 11].ColumnWidth = 20;
            
            xlWorksheet.Cells[fila, 12] = "EDAD";
            xlWorksheet.Cells[fila, 12].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 12].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 12].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 13] = "SEXO";
            xlWorksheet.Cells[fila, 13].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 13].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 13].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 14] = "CELULAR";
            xlWorksheet.Cells[fila, 14].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 14].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 14].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 15] = "RESULTADO ACTUAL";
            xlWorksheet.Cells[fila, 15].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 15].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 15].ColumnWidth = 20;
            
            xlWorksheet.Cells[fila, 16] = "RESULTADO ANTERIOR";
            xlWorksheet.Cells[fila, 16].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 16].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 16].ColumnWidth = 21;

            xlWorksheet.Cells[fila, 17] = "FECHA DE RESULTADO ANTERIOR";
            xlWorksheet.Cells[fila, 17].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 17].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 17].ColumnWidth = 21;

            xlWorksheet.Cells[fila, 18] = "(servicio id)";
            xlWorksheet.Cells[fila, 18].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 18].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 18].ColumnWidth = 21;

            xlWorksheet.Cells[fila, 19] = "(técnico)";
            xlWorksheet.Cells[fila, 19].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 19].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 19].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 20] = "(centro médico)";
            xlWorksheet.Cells[fila, 20].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 20].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 20].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 21] = "TIPO EXAMEN";
            xlWorksheet.Cells[fila, 21].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 21].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 21].ColumnWidth = 21;

            fila++;
            foreach (var item in lista)
            {
                xlWorksheet.Cells[fila, 1] = item.FechaExamen;
                xlWorksheet.Cells[fila, 2].NumberFormat = "@";
                xlWorksheet.Cells[fila, 2] = item.Dni;
                xlWorksheet.Cells[fila, 3] = item.ApellidoPaterno;
                xlWorksheet.Cells[fila, 4] = item.ApellidoMaterno;
                xlWorksheet.Cells[fila, 5] = item.Nombres;
                xlWorksheet.Cells[fila, 6] = item.TipoEmpresa;
                xlWorksheet.Cells[fila, 7] = item.EmpresaPrincipal;
                xlWorksheet.Cells[fila, 8] = item.EmpresaEmpleadora;
                xlWorksheet.Cells[fila, 9] = item.Sede;
                xlWorksheet.Cells[fila, 10] = item.Area;
                xlWorksheet.Cells[fila, 11] = item.Puesto;
                xlWorksheet.Cells[fila, 12] = item.Edad;
                xlWorksheet.Cells[fila, 13] = item.Sexo;
                xlWorksheet.Cells[fila, 14] = item.Celular;
                xlWorksheet.Cells[fila, 15] = item.Resultado;
                xlWorksheet.Cells[fila, 16] = item.AntecedenteResultado;
                xlWorksheet.Cells[fila, 17] = item.AntecedenteFechaResultado;
                xlWorksheet.Cells[fila, 18] = item.ServicioId;
                xlWorksheet.Cells[fila, 19] = item.Tecnico;
                xlWorksheet.Cells[fila, 20] = item.CentroMedico;
                xlWorksheet.Cells[fila, 21] = item.TipoExamen;

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

        private void EnviarExcelEmpresa(string rutaExcel, string correoEmpresa)
        {
            OperationResult objOperationResult = new OperationResult();
            try
            {
                var configEmail = Sigesoft.Node.WinClient.BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 161, "i_ParameterId");

                string smtp = configEmail[0].Value1.ToLower();
                int port = int.Parse(configEmail[1].Value1);
                string from = configEmail[2].Value1.ToLower();
                string fromPassword = configEmail[4].Value1;
                string subject = "Resultados Covid-19";
                bool enableSsl = Convert.ToBoolean(int.Parse(configEmail[3].Value1));
                string message = string.Format("Buenos tardes, se adjunta excel con resultados COVID19");
                var atachFiles = new List<string>();
                string atachFile = rutaExcel;
                atachFiles.Add(atachFile);
                Sigesoft.Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, correoEmpresa, "francisco.collantes@saluslaboris.com.pe;luis.delacruz@saluslaboris.com.pe", subject, message, atachFiles);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        static async Task ExecuteEmail(string rutaExcel, List<EmailAddress> tos)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("administrador2@saluslaboris.com.pe", "Administrador");
            var subject = "Resultados Covid-19";
            //var subjects = new List<string>() { "Resultados Covid-19" };

            //var to = new EmailAddress(correoEmpresa);

            var plainTextContent = "Buenas tardes, se adjunta excel con resultados COVID19";
            var htmlContent = "<strong>Buenos tardes, se adjunta excel con resultados COVID19</strong>";
            var showAllRecipients = false;
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, plainTextContent, htmlContent, showAllRecipients);
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //con copia
            var cc_emails = new List<EmailAddress>
            {
                new EmailAddress("luis.delacruz@saluslaboris.com.pe"),
                new EmailAddress("pool.camacho@saluslaboris.com.pe")
            };
            msg.AddCcs(cc_emails);
            ////con copia oculta
            //var bcc_emails = new List<EmailAddress>
            //{
            //   new EmailAddress("test5@example.com"),
            //   new EmailAddress("test6@example.com")
            //};
            //msg.AddBccs(bcc_emails);
            
            //var bytes = File.ReadAllBytes(rutaExcel);
            //var file = Convert.ToBase64String(bytes);
            //msg.AddAttachment("reporte.xls", file, "text/csv", "attachment", "banner");

            byte[] byteData = Encoding.ASCII.GetBytes(File.ReadAllText(rutaExcel));
            msg.Attachments = new List<SendGrid.Helpers.Mail.Attachment>
            {
                new SendGrid.Helpers.Mail.Attachment
                {
                    Content = Convert.ToBase64String(byteData),
                    Filename = "reporte.xls",
                    Type = "application/vnd.ms-excel",
                    Disposition = "attachment"
                }
            };

            //using (var fileStream = File.OpenRead(rutaExcel))
            //{
            //    await msg.AddAttachmentAsync("reporte.xls", fileStream);
            //    var response = await client.SendEmailAsync(msg);
            //    var resp = response;
            //}

            var response = await client.SendEmailAsync(msg);
            var resp = response;
        }

      
    }
}

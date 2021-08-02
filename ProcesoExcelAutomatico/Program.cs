using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ProcesoExcelAutomatico
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBL oServiceBL = new ServiceBL();
            OperationResult objOperationResult = new OperationResult();

            Console.WriteLine("==============================================");
            Console.WriteLine("**REPORTE CONSOLIDADO BACKUS**");
            Console.WriteLine("==============================================");
            string ruta = Sigesoft.Common.Utils.GetApplicationConfigValue("rutaReportes").ToString();
            Console.WriteLine("Ruta del reporte: " + ruta);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");

            //var startDate = new DateTime(Datetime.Now.Year, DateTime.Now.Month, 1);
            //var startDate = DateTime.Now.Date;
            //var EndDate = DateTime.Now.Date;

            var startDate = DateTime.Parse("01/06/2020");
            var EndDate = DateTime.Parse("07/06/2020");

            Console.WriteLine("Generando Excel...");

            var servicesHoy = oServiceBL.GetServicesPagedAndFilteredFullNodeJessicaOblitas(ref objOperationResult, 0, null, "", "", startDate, EndDate);

            servicesHoy = servicesHoy.FindAll(p => p.v_OrganizationId == Constants.EMPRESA_BACKUS_ID).ToList();
            //servicesHoy = servicesHoy.FindAll(p => p.Sede == "CD CAJAMARCA").ToList();

            //#region AMC
            //List<string> dnis = new List<string>();
            // dnis.Add("24486391");
            //dnis.Add("23925371");
            //dnis.Add("45410458");
            //dnis.Add("71517151");
            //dnis.Add("46674184");

            
            //var servicesDnis = servicesHoy.FindAll(p => dnis.Contains(p.v_DocNumber));
            //#endregion 

            //servicesHoy = servicesHoy.FindAll(p => p.Sede == "PTA SAN MATEO");
            //servicesHoy = servicesHoy.FindAll(p => p.v_ComponentId == Constants.ANTIGENOS_ID);

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
                    Console.WriteLine("SERVICVIOID:" + oReporteExcelCovid19.FechaExamen + " " + oReporteExcelCovid19.Dni + " " + total--);
                }
            }

            #region CREACIÓN DE EXCEL CONSOLIDADO Y ENVIÓ DE CORREO A EMPRESA PRINCIPAL
            var rutaExcel = CrearExcel(lista, ruta, "BACKUS PRINCIPAL");

            if (!string.IsNullOrEmpty(rutaExcel))
            {
                Console.WriteLine("Envío de correo a EMPRESA PRINCIPAL");
                //var to = "Sandy.Jauregui@ab-inbev.com;Graciela.Ferrel@ab-inbev.com; GLADYS.FLORES@ab-inbev.com;ANTOINETTE.LOSTAUNAUFELIX@ab-inbev.com;Flor.Vargas@ab-inbev.com;Fernanda.Bravo@ab-inbev.com;Katia.Cardenas@ab-inbev.com;Samantha.Gomez@ab-inbev.com; emiko.mori@ab-inbev.com; Nelly.Chavez@ab-inbev.com; Gloria.Lock@ab-inbev.com; Giuliana.Fiocco@ab-inbev.com; Evelyn.Alania@ab-inbev.com;Deborah.Infantas@ab-inbev.com;Eliana.Ayala2@ab-inbev.com;Alejandra.Cateriano@ab-inbev.com;Elvia.Cabeza@ab-inbev.com;Elvira.Alban@ab-inbev.com;Javier.Orellana@ab-inbev.com;Catherine.Alva@ab-inbev.com;Melisa.Fernandez@ab-inbev.com;ALESSANDRA.ARBAIZA@ab-inbev.com;VANESSAPAULINA.LANDAVERY@gmodelo.com.mx";
                var to = "GLADYS.FLORES@ab-inbev.com;Sandy.Jauregui@ab-inbev.com;Flor.Vargas@ab-inbev.com;ANTOINETTE.LOSTAUNAUFELIX@ab-inbev.com;Jessica.Quicanon-ext@ab-inbev.com;Graciela.Ferrel@ab-inbev.com;Gabriela.ruiz2@ab-inbev.com;VANESSAPAULINA.LANDAVERY@gmodelo.com.mx;BRUNELLA.NACARINON@ab-inbev.com;Nelly.Chavez@ab-inbev.com;Nelly.Chavez@ab-inbev.com;Samantha.Gomez@ab-inbev.com;Fernanda.Bravo@ab-inbev.com;Katia.Cardenas@ab-inbev.com;Mery.Gone@ab-inbev.com;Magali.Reyes@ab-inbev.com;Rosa.Portocarrero@ab-inbev.com;Catherine.Alva@ab-inbev.com;Javier.Orellana@ab-inbev.com;Carla.Silva@pe.ab-inbev.com;Elizabeth.Kiyan@ab-inbev.com;bruno.peralta2@ab-inbev.com";
                var cco = "gladys.tisza@saluslaboris.com.pe;francisco.pinto@saluslaboris.com.pe;programaciones.pruebasrapidas@saluslaboris.com.pe;hreeves@saluslaboris.com.pe;rafael.carrasco@saluslaboris.com.pe;veronika.gutierrez@saluslaboris.com.pe; luis.delacruz@saluslaboris.com.pe;saul.ramos@saluslaboris.com.pe;liliana.rizo@saluslaboris.com.pe;Melisa.Fernandez@ab-inbev.com;miguel.maticorena@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;monica.perleche@saluslaboris.com.pe;citas.csolima@saluslaboris.com.pe";
                //var cco = "gladys.tisza@saluslaboris.com.pe;francisco.pinto@saluslaboris.com.pe;programaciones.pruebasrapidas@saluslaboris.com.pe;hreeves@saluslaboris.com.pe;rafael.carrasco@saluslaboris.com.pe;veronika.gutierrez@saluslaboris.com.pe; luis.delacruz@saluslaboris.com.pe;saul.ramos@saluslaboris.com.pe";

                EnviarExcelEmpresa(rutaExcel, to, cco);
            }
            #endregion

            #region CREACIÓN DE EXCEL Y ENVÍO DE CORREO POR EMPRESA EMPLEADORA

            //var empresasEmpleadoras = lista.GroupBy(p => p.EmpresaEmpleadora).Select(s => s.First()).ToList();

            //foreach (var empleadora in empresasEmpleadoras)
            //{
            //    Console.WriteLine("Envío de correo a EMPRESA EMPLEADORA: " + empleadora.EmpresaEmpleadora);

            //    var rutaExcelEmpleadora = CrearExcel(lista.FindAll(p => p.EmpresaEmpleadora == empleadora.EmpresaEmpleadora), ruta, empleadora.EmpresaEmpleadora);

            //    var correosEmpleadora = oServiceBL.CorreosPorEmpresaEmpleadora(empleadora.EmpresaEmpleadora);

            //    if (!string.IsNullOrEmpty(correosEmpleadora))
            //    {
            //        //EnviarExcelEmpresa(rutaExcel, correosEmpleadora, "");
            //    }
            //}

            #endregion
        }

        private static string CrearExcel(List<ReporteExcelAutomaticoCovid19> lista, string ruta, string empresa)
        {
            var fila = 1;
            string fileName;
            fileName = string.Format("{1} {0}-{2}", DateTime.Now.ToString("ddMMyyyy hhmm"), empresa, "backus");

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

            xlWorksheet.Cells[fila, 22] = "RAZÓN EXAMEN";
            xlWorksheet.Cells[fila, 22].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 22].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 22].ColumnWidth = 22;

            xlWorksheet.Cells[fila, 23] = "LUGAR EXAMEN";
            xlWorksheet.Cells[fila, 23].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 23].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 23].ColumnWidth = 23;

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
                xlWorksheet.Cells[fila, 22] = item.RazonExamen;
                xlWorksheet.Cells[fila, 23] = item.LugarExamen;

                fila++;
            }

            string location = ruta + fileName /*+ " " + DateTime.Now.Date.ToString("hhss")*/ + ".xls";
            xlWorkbook.SaveAs(location, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, false, misValue, misValue, misValue, misValue);
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
                string subject = "Resultados Covid-19";
                bool enableSsl = Convert.ToBoolean(int.Parse(configEmail[3].Value1));
                string message = string.Format("Buenos tardes, se adjunta excel con resultados COVID19");
                //string message = string.Format("Buenos dias, tomar en cuenta este excel con resultados COVID19, omitir el anterior");
                var atachFiles = new List<string>();
                string atachFile = rutaExcel;
                atachFiles.Add(atachFile);
                Sigesoft.Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, to, cco , subject, message, atachFiles);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

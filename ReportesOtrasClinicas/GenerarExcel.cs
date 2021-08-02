using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportesOtrasClinicas
{
   public class GenerarExcel
    {    

        public string CrearExcel(List<ResultadoTrabajadorBE> lista)
        {
            var fila = 4;
            string ruta = Sigesoft.Common.Utils.GetApplicationConfigValue("rutaReportes").ToString();
            string fileName;
            fileName = string.Format("{0} {1}", lista[0].CentroMedico, DateTime.Now.Date.ToString("ddMMyyy"));

            Microsoft.Office.Interop.Excel.Application xlSamp = new Microsoft.Office.Interop.Excel.Application();

            if (xlSamp == null)
            {
                Console.WriteLine("Excel is not Installed");
                Console.ReadKey();
                return "";
            }

            Microsoft.Office.Interop.Excel.Workbook xlWorkbook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkbook = xlSamp.Workbooks.Add(misValue);
            xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.get_Item(1);



            xlWorksheet.Cells[fila, 1] = "Fecha Examen";
            xlWorksheet.Cells[fila, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 1].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 1].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 2] = "Centro Médico";
            xlWorksheet.Cells[fila, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 2].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 2].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 3] = "Empresa Empleadora";
            xlWorksheet.Cells[fila, 3].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 3].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 3].ColumnWidth = 40;

            xlWorksheet.Cells[fila, 4] = "CONTRATISTA";
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

            xlWorksheet.Cells[fila, 7] = "Lugar(SEDE)";
            xlWorksheet.Cells[fila, 7].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 7].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 7].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 8] = "Edad";
            xlWorksheet.Cells[fila, 8].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 8].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 8].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 9] = "Sexo";
            xlWorksheet.Cells[fila, 9].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 9].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 9].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 10] = "Síntomas";
            xlWorksheet.Cells[fila, 10].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 10].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 10].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 11] = "Resultado IgM";
            xlWorksheet.Cells[fila, 11].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 11].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 11].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 12] = "Resultado IgG";
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

            xlWorksheet.Cells[fila, 15] = "NODO";
            xlWorksheet.Cells[fila, 15].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 15].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 15].ColumnWidth = 20;

            xlWorksheet.Cells[fila, 16] = "SERVICIO ID";
            xlWorksheet.Cells[fila, 16].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[fila, 16].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[fila, 16].ColumnWidth = 21;

            fila++;
            foreach (var item in lista)
            {
                xlWorksheet.Cells[fila, 1] = item.FechaExamen;
                xlWorksheet.Cells[fila, 2] = item.CentroMedico;
                xlWorksheet.Cells[fila, 3] = item.EmpresaEmpleadora;
                xlWorksheet.Cells[fila, 4] = item.EmpresaContratista;
                xlWorksheet.Cells[fila, 5] = item.NombresApellidos;
                xlWorksheet.Cells[fila, 6] = "_" + item.Dni;
                xlWorksheet.Cells[fila, 7] = item.Sede;
                xlWorksheet.Cells[fila, 8] = item.Edad;
                xlWorksheet.Cells[fila, 9] = item.Sexo;
                xlWorksheet.Cells[fila, 10] = item.Sintomas;
                xlWorksheet.Cells[fila, 11] = item.ResultadoIgM;
                xlWorksheet.Cells[fila, 12] = item.ResultadoIgG;
                xlWorksheet.Cells[fila, 13] = item.Tecnico;
                xlWorksheet.Cells[fila, 14] = item.Celular;
                xlWorksheet.Cells[fila, 15] = item.Nodo;
                xlWorksheet.Cells[fila, 16] = item.ServicioId;
                fila++;
            }

            string location = ruta + fileName + " " + DateTime.Now.Date.ToString("dd MM") + ".xls";
            xlWorkbook.SaveAs(location, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
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
    }
}

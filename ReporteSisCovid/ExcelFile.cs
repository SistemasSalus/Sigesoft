using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReporteSisCovid
{
    public class ExcelFile
    {

        public string GenerateExcel(string path, List<ListServiceValuesForExcel> list)
        {
            var row = 4;            
            string fileName;
            fileName = string.Format("Backus {0}", DateTime.Now.ToString("ddMMyyy hhmm"));

            Microsoft.Office.Interop.Excel.Application xlSamp = new Microsoft.Office.Interop.Excel.Application();

            if (xlSamp == null)
            {
                Console.WriteLine("Excel is not Installed");
                Console.ReadKey();
                return " ERROR";
            }

            Microsoft.Office.Interop.Excel.Workbook xlWorkbook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkbook = xlSamp.Workbooks.Add(misValue);
            xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.get_Item(1);

            xlWorksheet.Cells[row, 1] = "TIPO DOCUMENTO";
            xlWorksheet.Cells[row, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 1].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 1].ColumnWidth = 20;

            xlWorksheet.Cells[row, 2] = "NRO DOCUMENTO";
            xlWorksheet.Cells[row, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 2].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 2].ColumnWidth = 20;
            
            xlWorksheet.Cells[row, 3] = "APE PATERNO";
            xlWorksheet.Cells[row, 3].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 3].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 3].ColumnWidth = 20;

            xlWorksheet.Cells[row, 4] = "APE MATERNO";
            xlWorksheet.Cells[row, 4].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 4].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 4].ColumnWidth = 20;

            xlWorksheet.Cells[row, 5] = "NOMBRES";
            xlWorksheet.Cells[row, 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 5].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 5].ColumnWidth = 20;

            xlWorksheet.Cells[row, 6] = "FECHA NACIMIENTO";
            xlWorksheet.Cells[row, 6].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 6].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 6].ColumnWidth = 20;

            xlWorksheet.Cells[row, 7] = "";
            xlWorksheet.Cells[row, 7].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 7].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 7].ColumnWidth = 20;

            xlWorksheet.Cells[row, 8] = "";
            xlWorksheet.Cells[row, 8].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 8].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 8].ColumnWidth = 20;

            xlWorksheet.Cells[row, 9] = "SEXO";
            xlWorksheet.Cells[row, 9].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 9].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 9].ColumnWidth = 20;

            xlWorksheet.Cells[row, 10] = "TIPO SEGURO";
            xlWorksheet.Cells[row, 10].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 10].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 10].ColumnWidth = 20;

            xlWorksheet.Cells[row, 11] = "OTRO TIPO SEGURO";
            xlWorksheet.Cells[row, 11].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 11].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 11].ColumnWidth = 20;

            xlWorksheet.Cells[row, 12] = "UBIGEO";
            xlWorksheet.Cells[row, 12].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 12].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 12].ColumnWidth = 20;

            xlWorksheet.Cells[row, 13] = "PERSONAL SALUD";
            xlWorksheet.Cells[row, 13].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 13].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 13].ColumnWidth = 20;

            xlWorksheet.Cells[row, 14] = "FECHA EJECUCIÓN";
            xlWorksheet.Cells[row, 14].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 14].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 14].ColumnWidth = 20;

            xlWorksheet.Cells[row, 15] = "PROCEDENCIA";
            xlWorksheet.Cells[row, 15].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 15].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 15].ColumnWidth = 20;

            xlWorksheet.Cells[row, 16] = "TIPO RESULTADO 1";
            xlWorksheet.Cells[row, 16].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 16].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 16].ColumnWidth = 20;

            xlWorksheet.Cells[row, 17] = "TIPO RESULTADO 2";
            xlWorksheet.Cells[row, 17].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 17].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 17].ColumnWidth = 20;

            xlWorksheet.Cells[row, 18] = "MAYOR 60";
            xlWorksheet.Cells[row, 18].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 18].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 18].ColumnWidth = 20;

            xlWorksheet.Cells[row, 19] = "HIPERTENSIÓN ARTERIAL";
            xlWorksheet.Cells[row, 19].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 19].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 19].ColumnWidth = 20;

            xlWorksheet.Cells[row, 20] = "ENFERMEDAD CARDIOVASCULAR";
            xlWorksheet.Cells[row, 20].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 20].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 20].ColumnWidth = 20;

            xlWorksheet.Cells[row, 21] = "DIABETES";
            xlWorksheet.Cells[row, 21].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 21].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 21].ColumnWidth = 20;

            xlWorksheet.Cells[row, 22] = "OBESIDAD";
            xlWorksheet.Cells[row, 22].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 22].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 22].ColumnWidth = 20;

            xlWorksheet.Cells[row, 23] = "ASMA";
            xlWorksheet.Cells[row, 23].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 23].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 23].ColumnWidth = 20;

            xlWorksheet.Cells[row, 24] = "ENF. PULMONAR CRÓNICA";
            xlWorksheet.Cells[row, 24].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 24].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 24].ColumnWidth = 20;

            xlWorksheet.Cells[row, 25] = "INSUFICIENCIA RENAL CRÓNICA";
            xlWorksheet.Cells[row, 25].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 25].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 25].ColumnWidth = 20;

            xlWorksheet.Cells[row, 26] = "ENF. TRATAMIENTO INMUNOSUPRESOR";
            xlWorksheet.Cells[row, 26].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 26].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 26].ColumnWidth = 20;

            xlWorksheet.Cells[row, 27] = "CÁNCER";
            xlWorksheet.Cells[row, 27].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 27].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 27].ColumnWidth = 20;

            xlWorksheet.Cells[row, 28] = "EMBARAZO O PUERPERIO";
            xlWorksheet.Cells[row, 28].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 28].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 28].ColumnWidth = 20;

            xlWorksheet.Cells[row, 29] = "PERSONAL DE SALUD";
            xlWorksheet.Cells[row, 29].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 29].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 29].ColumnWidth = 20;

            xlWorksheet.Cells[row, 30] = "EMPLEADOR";
            xlWorksheet.Cells[row, 30].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 30].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 30].ColumnWidth = 20;

            xlWorksheet.Cells[row, 31] = "EMPRESA PRINCIPAL";
            xlWorksheet.Cells[row, 31].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 31].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 31].ColumnWidth = 20;

            xlWorksheet.Cells[row, 32] = "SEDE";
            xlWorksheet.Cells[row, 32].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 32].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 32].ColumnWidth = 20;

            xlWorksheet.Cells[row, 33] = "TÉCNICO";
            xlWorksheet.Cells[row, 33].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 33].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 33].ColumnWidth = 20;

            xlWorksheet.Cells[row, 34] = "TIPO DE PRUEBA";
            xlWorksheet.Cells[row, 34].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
            xlWorksheet.Cells[row, 34].EntireRow.Font.Bold = true;
            xlWorksheet.Cells[row, 34].ColumnWidth = 20;

            row++;
            foreach (var item in list)
            {
                xlWorksheet.Cells[row, 1] = item.TipoDocumento;
                xlWorksheet.Cells[row, 2] = item.NumeroDocumento;
                xlWorksheet.Cells[row, 3] = item.ApellidoPaterno;
                xlWorksheet.Cells[row, 4] = item.ApellidoMaterno;
                xlWorksheet.Cells[row, 5] = item.Nombres;
                xlWorksheet.Cells[row, 6] = item.FechaNacimiento;
                xlWorksheet.Cells[row, 7] = "";
                xlWorksheet.Cells[row, 8] = "";
                xlWorksheet.Cells[row, 9] = item.Sexo;
                xlWorksheet.Cells[row, 10] = item.TipoSeguro;
                xlWorksheet.Cells[row, 11] = item.OtroTipoSeguro;
                xlWorksheet.Cells[row, 12] = item.Ubigueo;
                xlWorksheet.Cells[row, 13] = item.PersonalSalud;
                xlWorksheet.Cells[row, 14] = item.FechaEjecucion;
                xlWorksheet.Cells[row, 15] = item.Procedencia;
                xlWorksheet.Cells[row, 16] = item.TipoResultado1;
                xlWorksheet.Cells[row, 17] = item.TipoResultado2;
                xlWorksheet.Cells[row, 18] = item.Mayor60;
                xlWorksheet.Cells[row, 19] = item.HipertensionArterial;
                xlWorksheet.Cells[row, 20] = item.EnfermedadCardiovascular;
                xlWorksheet.Cells[row, 21] = item.Diabetes;
                xlWorksheet.Cells[row, 22] = item.Obesidad;
                xlWorksheet.Cells[row, 23] = item.Asma;
                xlWorksheet.Cells[row, 24] = item.EnfPulmonarCronica;
                xlWorksheet.Cells[row, 25] = item.InsuficienciaRenalCronica;
                xlWorksheet.Cells[row, 26] = item.EnfTratamientoInmunosupresor;
                xlWorksheet.Cells[row, 27] = item.Cancer;
                xlWorksheet.Cells[row, 28] = item.EmbarazoPuerperio;
                xlWorksheet.Cells[row, 29] = item.PersonalSalud;
                xlWorksheet.Cells[row, 30] = item.Empleador;
                xlWorksheet.Cells[row, 31] = item.EmpresaPrincipal;
                xlWorksheet.Cells[row, 32] = item.Sede;
                xlWorksheet.Cells[row, 33] = item.Tecnico;
                xlWorksheet.Cells[row, 34] = item.TipoExamen;
                row++;
            }

            string location = path + fileName + ".xls";
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

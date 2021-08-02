using Sigesoft.Node.WinClient.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
namespace ReportesAutomáticosCovid
{
    class Program
    {
        static void Main(string[] args)
        {
            Reportes oReportes = new Reportes();
            oReportes.GenerateExcel();           
        }
    }
}

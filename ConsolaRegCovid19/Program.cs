using Sigesoft.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolaRegCovid19
{
    class Program
    {
        static int optionSelected;

        static void Main(string[] args)
        {

            Iniciar();
            Console.WriteLine("Desea Seguir");

            var seguir = Console.ReadLine();

            do
            {
                Iniciar();       
            } while (seguir == "SI");

            
        }



        static void Iniciar()
        {
            PrintTitle();

            PrintMainMenu();

            optionSelected = CheckSelectedOption();

            if (optionSelected == (int)Enums.mainMenuoptions.PrintServices)
            {
                PrintCertificatesCovid19();

                PrintOptionCertificatesCovid19();

                if (optionSelected == (int)Enums.menuServiceOptions.PrintResultService)
                {
                    PrintResultService();
                }
            }
            else if (optionSelected == (int)Enums.mainMenuoptions.UpLoadExcel)
            {
                UpLoadExcel();
            }
            else if (optionSelected == (int)Enums.mainMenuoptions.GeneratePDFCovid19)
            {
                GenerateCovid19();
            }
            else if (optionSelected == (int)Enums.mainMenuoptions.GenerateCovid19PorDni)
            {
                GenerateCovid19PorDni();
            }
            else if (optionSelected == (int)Enums.mainMenuoptions.Incidencia)
            {
                MostrarIncidencias();
            }
        }
        

        private static void MostrarIncidencias()
        {
            Console.Write("Ingresar DNI(s), separados por (,): ");
            var dnis = Console.ReadLine();
            //dnis = "24390728,40611585,72433165,24964625,47415930,73537554,23990713,10313249,47856393,47988729,42171141,44599607,44999686,23864315,23943999,02413566,23943882,47173898,46684240,43478843,72476711,44726848,23854661,46689585,46121606,73786435,41659372,45537184,45343197,48413932,23984308,48130365,47965673,70418002,72545180,47310806,25752410,47678223,71945795,46840388,47496685,71517993,46610671,47435967,71244891,74300578,40541068,43906480,45990375,75494499,41207223,75002011,45149994,42793096,23963941,77575410,40295650,73967840";
            var arrDnis = dnis.Split(',').ToList();

            for (int i = 0; i < arrDnis.Count; i++)
            {
                Incidencias oIncidencias = new Incidencias(arrDnis[i]);

                var NombresTrabajador = oIncidencias.ObtenerDatosPersonales().NombresTrabajador;
                var Servicios = oIncidencias.DatosServicios().ToList();

                Console.WriteLine(string.Format("Trabajador: {0}", NombresTrabajador));
                Console.WriteLine(string.Format("==========SERVICIOS========="));
                if (Servicios.Count == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("=========================="));
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {                   

                    foreach (var item in Servicios)
                    {
                        Console.WriteLine(string.Format("ServicioId: {0}, FechaServicio: {1}, Técnico: {2}, F.Insert: {3}, F.Update: {4}, Usuario: {5}, EnvioCorreo {6}, Correo:{7} ", item.ServiceId, item.FechaServicio, item.Tecnico, item.FechaInsert, item.FechaUpdate, item.Usuario, item.CorreoEnviado, item.CorreoTrabajador));
                        Console.WriteLine(string.Format("Resultado: {0}", oIncidencias.ObtenerResultadoLaboratorio(item.ServiceId)));
                    }
                }
                

                Console.WriteLine(string.Format("============================"));
            }

          
            


        }

        private static void GenerateCovid19PorDni()
        {
            Console.Write("Ingresar DNI(s), separados por (,): ");
            var dnis = Console.ReadLine();
            var arrDnis = dnis.Split(',').ToList() ;

            Console.Write("Tipo Formato:(D-R)");
            var tipoFormato = Console.ReadLine();

            Console.Write("Ingresar Ruta: ");
            var ruta = Console.ReadLine();

            var oGenerateCerticateCovid19 = new GenerateCerticateCovid19(arrDnis, ruta);
            int contador = 1;
            var servicos = oGenerateCerticateCovid19.ServicesCovid19PorDnis();
            foreach (var item in servicos)
            {
                 if (tipoFormato == "R")
                {
                    if (oGenerateCerticateCovid19.GeneratePdfResumido(item.ServiceId))
                    {
                        var msg = string.Format("Item:{4} .-Nodo: {0}, ServiceId: {1}, PersonId: {2}, Trabajador:{3}\n", item.NodoId, item.ServiceId, item.PersonId, item.Trabajador, contador);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(msg);
                    }
                    else
                    {
                        var msg = string.Format("Item:{4} .-Nodo: {0}, ServiceId: {1}, PersonId: {2}, Trabajador:{3}\n", item.NodoId, item.ServiceId, item.PersonId, item.Trabajador, contador);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(msg);
                    }    
                }

                 contador++;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("----FIN PRUEBA RÁPIDA----");

        }

        private static void PrintResultService()
        {
            Console.WriteLine("Mostrando resultados... ");
        }

        private static void GenerateCovid19()
        {
            Console.Write("Ingresar Fecha Inicio: ");
            var fechaInicio = Console.ReadLine();

            Console.Write("Ingresar Fecha Fin: ");
            var fechaFin = Console.ReadLine();

            Console.Write("Ingresar ID Empresa: ");
            var organizationId = Console.ReadLine();

            Console.Write("Ingresar Nodo: ");
            var nodoId = Console.ReadLine();

            Console.Write("Ingresar Ruta: ");
            var ruta = Console.ReadLine();

            Console.Write("Tipo Formato:(D-R)");
            var tipoFormato = Console.ReadLine();

            var oGenerateCerticateCovid19 = new GenerateCerticateCovid19(fechaInicio, fechaFin, organizationId, nodoId, ruta);

            var servicios = oGenerateCerticateCovid19.ServicesCovid19();
            int contador = 1;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Total:"+  servicios.Count());
            foreach (var item in servicios)
            {
                if (tipoFormato == "D")
                {
                    if (oGenerateCerticateCovid19.GeneratePdfDetallado(item.ServiceId))
                    {
                        var msg = string.Format("Item:{4} .-Nodo: {0}, ServiceId: {1}, PersonId: {2}, Trabajador:{3}\n", item.NodoId, item.ServiceId, item.PersonId, item.Trabajador, contador);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(msg);
                    }
                    else
                    {
                        var msg = string.Format("Item:{4} .-Nodo: {0}, ServiceId: {1}, PersonId: {2}, Trabajador:{3}\n", item.NodoId, item.ServiceId, item.PersonId, item.Trabajador, contador);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(msg);
                    }    
                }
                else if (tipoFormato == "R")
                {
                    if (oGenerateCerticateCovid19.GeneratePdfResumido(item.ServiceId))
                    {
                        var msg = string.Format("Item:{4} .-Nodo: {0}, ServiceId: {1}, PersonId: {2}, Trabajador:{3}\n", item.NodoId, item.ServiceId, item.PersonId, item.Trabajador, contador);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(msg);
                    }
                    else
                    {
                        var msg = string.Format("Item:{4} .-Nodo: {0}, ServiceId: {1}, PersonId: {2}, Trabajador:{3}\n", item.NodoId, item.ServiceId, item.PersonId, item.Trabajador, contador);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(msg);
                    }    
                }
                contador++;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("----FIN PRUEBA RÁPIDA----");

            var serviciosCertificados = oGenerateCerticateCovid19.ServicesCertificadoCovid19();
            foreach (var item in serviciosCertificados)
            {
                if (tipoFormato == "D")
                {
                    if (oGenerateCerticateCovid19.GeneratePdfDetallado(item.ServiceId))
                    {
                        var msg = string.Format("Item:{4} .-Nodo: {0}, ServiceId: {1}, PersonId: {2}, Trabajador:{3}\n", item.NodoId, item.ServiceId, item.PersonId, item.Trabajador, contador);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(msg);
                    }
                    else
                    {
                        var msg = string.Format("Item:{4} .-Nodo: {0}, ServiceId: {1}, PersonId: {2}, Trabajador:{3}\n", item.NodoId, item.ServiceId, item.PersonId, item.Trabajador, contador);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(msg);
                    }
                }
                else if (tipoFormato == "R")
                {
                    if (oGenerateCerticateCovid19.GeneratePdfResumido(item.ServiceId))
                    {
                        var msg = string.Format("Item:{4} .-Nodo: {0}, ServiceId: {1}, PersonId: {2}, Trabajador:{3}\n", item.NodoId, item.ServiceId, item.PersonId, item.Trabajador, contador);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(msg);
                    }
                    else
                    {
                        var msg = string.Format("Item:{4} .-Nodo: {0}, ServiceId: {1}, PersonId: {2}, Trabajador:{3}\n", item.NodoId, item.ServiceId, item.PersonId, item.Trabajador, contador);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(msg);
                    }
                }
                contador++;
            }

            Console.Write("----FIN PRUEBA CERTIFICADOS----");
        }        

        private static void UpLoadExcel()
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            Microsoft.Office.Interop.Excel.Range range;

            string str;
            int rCnt;
            int cCnt;
            int rw = 0;
            int cl = 0;

            Console.Write("Ingresar Ruta Excel: ");
            var pathExcel = Console.ReadLine();
            Console.Write("Nro. Filas: ");
            rw = int.Parse(Console.ReadLine());

            Console.Write("Ingresar Nodo: ");
            var nodoId = int.Parse(Console.ReadLine());            

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(pathExcel, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;
            //rw = 37;// range.Rows.Count;
            cl = 71;// range.Columns.Count;


            for (rCnt = 3; rCnt  <= rw; rCnt++)
            {
                //REGISTRO Y AGENDA
                 var FechaServicio = "";
                    var Nombres  = "";
                    var ApePaterno  = "";
                    var ApeMaterno  = "";
                    var TipoDocumento  = "";
                    var NroDocumento  = "";
                    var Genero  = "";
                    var FechaNacimiento = ""; 
                    var Email  = "";
                    var Celulares  = "";
                    var Puesto  = "";
                    var Direccion  = "";
                    var NombreSede  = "";
                    var TipoEmpresaCovidId = -1;
                    var Tecnico ="";

                //ENCUESTA
                 int NodeId  = nodoId;
                string CalendarId = "INVALIDO";
                string ServiceComponentId  = "";
                string PersonId  = "";
                string SystemUserId  = "";
                string ComponentId  = Constants.COVID_ID;

                string TipoDomicilioId  = "";
                string Geolocalizacion  = "";
                string PersonalSaludId  = "";
                string ProfesionId  = "-1";
                string SintomasId  = "";
                string InicioSintomas  = "";

                bool TosId  = false;
                bool DolorGargantaId  = false;
                bool CongestionNasalId  = false;
                bool DificultadRespiratoriaId  = false;

                bool FiebreEscalofrioId  = false;
                bool MalestarGeneralId  = false;
                bool DiarreaId  = false;
                bool NauseasVomitosId  = false;

                bool CefaleaId  = false;
                bool IrritabilidadConfusionId  = false;
                bool DolorId  = false;
                bool ExpectoracionId  = false;

                bool MuscularId  = false;
                bool AbdominalId  = false;
                bool PechoId  = false;
                bool ArticulacionesId  = false;

                string OtrosSintomas  = "";

                bool DiabetesId  = false;
                bool PulmonarCronicaId  = false;
                bool CancerId  = false;
                bool HipertensionArterialId  =false;

                bool ObesidadId  = false;
                bool Mayor65Id  = false;
                bool InsuficienciaRenalId  =false;
                bool EmbarazoPuerperioId  = false;

                bool EnfCardioVascularId  = false;
                bool AsmaId  = false;
                bool InmunosupresorId  = false;
                bool RiesgoPersonalSaludId  = false;

                bool CertificacionId = false;

                //LABORATORIO

                string FechaEjecucion = "";
                string ProcedenciaSolicitudId ="";
                string ResultadoPrimeraPruebaId ="-1";
                string ResultadoSegundaPruebaId = "-1";
                string ClasificacionClinicaId  ="-1";


                for (cCnt = 1; cCnt  <= cl; cCnt++)
                {
                    var valueCell = (range.Cells[rCnt, cCnt] as Microsoft.Office.Interop.Excel.Range).Value2;

                    if (cCnt == 1)
                    {
                        str = (DateTime.FromOADate(valueCell)).ToString();
                        FechaServicio = str;
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (cCnt == 2)
                    {
                        
                    }
                    else if (cCnt == 3)
                    {

                    }
                    else if (cCnt == 4)
                    {

                    }
                    else if (cCnt == 5)
                    {
                         str = valueCell.ToString();
                         if (str == "BACKUS")
                         {
                             TipoEmpresaCovidId = 1;
                         }
                         else
                         {
                             TipoEmpresaCovidId = -1;
                         }
                         
                    }
                    else if (cCnt == 6)
                    {
                        str = valueCell.ToString();
                        NombreSede = str;      
                    }
                    else if (cCnt == 7)
                    {
                        str = valueCell.ToString();
                        TipoDocumento = str == "DNI" ? "1" : "-1";
                    }
                    else if (cCnt == 8)
                    {
                        str = valueCell.ToString();
                        NroDocumento = str;                        
                    }
                    else if (cCnt == 9)
                    {
                        str = valueCell.ToString();
                        ApePaterno = str;
                    }
                    else if (cCnt == 10)
                    {
                        str = valueCell.ToString();
                        ApeMaterno = str;
                    }
                    else if (cCnt == 11)
                    {
                        str = valueCell.ToString();
                        Nombres = str;
                    }
                    else if (cCnt == 12)
                    {
                        str = valueCell.ToString();
                        var edad = int.Parse(str);
                        FechaNacimiento = (DateTime.Now.Date.AddYears(-edad)).ToString();
                    }
                    else if (cCnt == 13)
                    {
                        str = valueCell.ToString();
                        Genero = str == "M" ? "1" : "2";
                    }
                    else if (cCnt == 14)
                    {
                        str = valueCell.ToString();
                        Celulares = str;                        
                    }
                    else if (cCnt == 15)
                    {

                    }
                    else if (cCnt == 16)
                    {

                    }
                    else if (cCnt == 17)
                    {
                        str = valueCell.ToString();
                        Direccion = str;
                    }
                    else if (cCnt == 18)
                    {

                    }
                    else if (cCnt == 19)
                    {

                    }
                    else if (cCnt == 20)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            Mayor65Id = str == "X" ? true : false;                           
                        }
                        
                    }
                    else if (cCnt == 21)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            DiabetesId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 22)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            PulmonarCronicaId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 23)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            CancerId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 24)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            HipertensionArterialId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 25)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            ObesidadId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 26)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            InsuficienciaRenalId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 27)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            EmbarazoPuerperioId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 28)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            EnfCardioVascularId = str == "X" ? true : false;
                        }                        
                    }
                    else if (cCnt == 29)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            AsmaId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 30)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            InmunosupresorId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 31)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            PersonalSaludId = str == "SI" ? "1" : "0";
                        }
                    }
                    else if (cCnt == 32)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            ProfesionId = str == "X" ? "1" : "-1";
                        }
                    }
                    else if (cCnt == 33)
                    {
                        if (valueCell != null)
                        {
                            if (ProfesionId == "-1")
                            {
                                str = (valueCell.ToString()).Trim().ToUpper();
                                ProfesionId = str == "X" ? "4" : "-1";
                            }
                        }
                    }
                    else if (cCnt == 34)
                    {
                        if (valueCell != null)
                        {
                            if (ProfesionId == "-1")
                            {
                                str = (valueCell.ToString()).Trim().ToUpper();
                                ProfesionId = str == "X" ? "1" : "-1";
                            }
                        }
                    }
                    else if (cCnt == 35)
                    {
                        if (valueCell != null)
                        {
                            if (ProfesionId == "-1")
                            {
                                str = (valueCell.ToString()).Trim().ToUpper();
                                ProfesionId = str == "X" ? "5" : "-1";
                            }
                        }
                    }
                    else if (cCnt == 36)
                    {
                        if (valueCell != null)
                        {
                            if (ProfesionId == "-1")
                            {
                                str = (valueCell.ToString()).Trim().ToUpper();
                                ProfesionId = str == "X" ? "2" : "-1";
                            }
                        }
                    }
                    else if (cCnt == 37)
                    {
                        if (valueCell != null)
                        {
                            if (ProfesionId == "-1")
                            {
                                str = (valueCell.ToString()).Trim().ToUpper();
                                ProfesionId = str == "X" ? "3" : "-1";
                            }
                        }
                    }
                    else if (cCnt == 38)
                    {
                        if (valueCell != null)
                        {
                            if (ProfesionId == "-1")
                            {
                                str = (valueCell.ToString()).Trim().ToUpper();
                                ProfesionId = str == "X" ? "6" : "-1";
                            }
                        }
                    }
                    else if (cCnt == 39)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            SintomasId = str == "NO" ? "0" : "1";
                        }
                    }
                    else if (cCnt == 40)
                    {
                        if (valueCell != null)
                        {
                            //Validar fecha formato
                            str = valueCell.ToString();
                            InicioSintomas = str;
                        }
                    }
                    else if (cCnt == 41)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            TosId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 42)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            DolorGargantaId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 43)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            CongestionNasalId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 44)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            DificultadRespiratoriaId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 45)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            FiebreEscalofrioId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 46)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            MalestarGeneralId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 47)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            DiarreaId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 48)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            NauseasVomitosId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 49)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            CefaleaId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 50)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            IrritabilidadConfusionId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 51)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            DolorId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 52)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            ExpectoracionId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 53)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            MuscularId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 54)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            AbdominalId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 55)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            PechoId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 56)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            ArticulacionesId = str == "X" ? true : false;
                        }
                    }
                    else if (cCnt == 57)
                    {
                        if (valueCell != null)
                        {
                            str = valueCell.ToString();
                            OtrosSintomas = str;
                        }
                    }
                    else if (cCnt == 58)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            if (str == "LEVE")
                            {
                                str = "1";
                            }
                            else if (str == "MODERADO")
                            {
                                str = "2";
                            }
                            else if (str == "SEVERA")
                            {
                                str = "3";
                            }
                            else
                            {
                                str = "-1";
                            }
                            ClasificacionClinicaId = str;

                        }
                    }
                    else if (cCnt == 59)
                    {
                        if (valueCell != null)
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            if (str == "LLAMADA AL 113")
                            {
                                str = "0";
                            }
                            else if (str == "PERSONAL DE SALUD")
                            {
                                str = "1";
                            }
                            else if (str == "PRUEBA EN ESTABLECIMIENTO DE SALUD")
                            {
                                str = "2";
                            }
                            else if (str == "CONTACTO EN CASO CONFIRMADO")
                            {
                                str = "3";
                            }
                            else if (str == "CONTACTO CON CASO SOSPECHOSO")
                            {
                                str = "4";
                            }
                            else if (str == "PERSONA PROVENIENTE DEL EXTRANJERO")
                            {
                                str = "5";
                            }
                            else if (str == "OTRO PRIORIZADO")
                            {
                                str = "6";
                            }
                            ProcedenciaSolicitudId = str;
                        }
                    }
                    else if (cCnt == 60)
                    {
                        if (valueCell != null)
                        {
                            FechaEjecucion = FechaServicio.ToString();
                        }
                    }
                    else if (cCnt == 61)
                    {
                        if (valueCell != null)
                        {
                            if (ResultadoPrimeraPruebaId == "-1")
                            {
                                str = (valueCell.ToString()).Trim().ToUpper();
                                ResultadoPrimeraPruebaId = str == "X" ? "2" : "-1";
                            }
                        }
                    }
                    else if (cCnt == 62)
                    {
                        if (valueCell != null)
                        {
                        if (ResultadoPrimeraPruebaId == "-1")
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            ResultadoPrimeraPruebaId = str == "X" ? "3" : "-1";
                        }
                        }
                    }
                    else if (cCnt == 63)
                    {
                        if (valueCell != null)
                        {
                        if (ResultadoPrimeraPruebaId == "-1")
                        {
                            str = (valueCell.ToString()).Trim().ToUpper();
                            ResultadoPrimeraPruebaId = str == "X" ? "4" : "-1";
                        }
                        }
                    }
                    else if (cCnt == 64)
                    {
                        if (valueCell != null)
                        {
                            if (ResultadoPrimeraPruebaId == "-1")
                            {
                                str = (valueCell.ToString()).Trim().ToUpper();
                                ResultadoPrimeraPruebaId = str == "X" ? "0" : "-1";
                            }
                        }
                    }
                    else if (cCnt == 65)
                    {
                        if (valueCell != null)
                        {
                            if (ResultadoPrimeraPruebaId == "-1")
                            {
                                str = (valueCell.ToString()).Trim().ToUpper();
                                ResultadoPrimeraPruebaId = str == "X" ? "5" : "-1";
                            }
                        }
                    }
                    else if (cCnt == 66)
                    {

                    }
                    else if (cCnt == 67)
                    {

                    }
                    else if (cCnt == 68)
                    {

                    }
                    else if (cCnt == 69)
                    {

                    }
                    else if (cCnt == 70)
                    {

                    }
                    else if (cCnt == 71)
                    {
                        if (valueCell != null)
                        {
                            str = valueCell.ToString();
                            Tecnico = str;
                        }
                    }
                    

                    //Console.WriteLine(string.Format("{0} -- {1}, {2} ", rCnt,cCnt, str));
                   
                    
                }

                //Agendar
                var oAgendarTrabajador = new AgendarTrabajador(FechaServicio,Nombres,ApePaterno,ApeMaterno,TipoDocumento,NroDocumento,Genero,FechaNacimiento,Email,Celulares,Puesto,Direccion,NombreSede,TipoEmpresaCovidId,nodoId,Tecnico);


                CalendarId =oAgendarTrabajador.Agendar();

                if (CalendarId != "INVALIDO")
                {
                    var oEncuesta = new RegistrarEncuestaCovid19(nodoId, CalendarId, ServiceComponentId, 
                        TipoDomicilioId, Geolocalizacion, PersonalSaludId, 
                        ProfesionId, SintomasId, InicioSintomas, TosId, DolorGargantaId, CongestionNasalId, 
                        DificultadRespiratoriaId, FiebreEscalofrioId, MalestarGeneralId, DiarreaId, NauseasVomitosId, 
                        CefaleaId, IrritabilidadConfusionId, DolorId, ExpectoracionId, MuscularId, AbdominalId,
                        PechoId, ArticulacionesId, OtrosSintomas, DiabetesId, PulmonarCronicaId, 
                        CancerId, HipertensionArterialId, ObesidadId, Mayor65Id, InsuficienciaRenalId, 
                        EmbarazoPuerperioId, EnfCardioVascularId, AsmaId, InmunosupresorId, RiesgoPersonalSaludId, CertificacionId);

                    oEncuesta.GrabarEncuesta();

                    FechaEjecucion = FechaServicio.ToString();
                    var oLab = new RegistrarLaboratorioCovid19(nodoId,CalendarId, FechaEjecucion, ProcedenciaSolicitudId, ResultadoPrimeraPruebaId, ResultadoSegundaPruebaId, ClasificacionClinicaId);

                    oLab.GrabarLaboratorio();

                    Console.WriteLine("ok - " + CalendarId);
                }

                
            }


        }

        private static void PrintMainMenu()
        {
            Console.WriteLine("Lista de opciones:");
            Console.WriteLine("\t 1.-Obtener Certificados");
            Console.WriteLine("\t 2.-Cargar Excel");
            Console.WriteLine("\t 3.-Generar Pdf Covid19");
            Console.WriteLine("\t 4.-Generar Pdf Covid19 por DNI(s)");
            Console.WriteLine("\t 5.-Incidencias");
        }

        private static void PrintTitle()
        {
            Console.WriteLine("----CONSOLA DE REGCOVID19----");
            Console.WriteLine("-----------------------------\n");
        }

        private static void PrintOptionCertificatesCovid19()
        {
            Console.WriteLine("\n¿Qué desea hacer?\n");
            Console.WriteLine("\t 1.-Mostrar resultados");
            Console.WriteLine("\t 2.-Enviar Certificado");

            optionSelected = CheckSelectedOption();

        }

        private static void PrintCertificatesCovid19()
        {
            Console.Write("Ingresar DNI trabajador: ");

            var dni = Console.ReadLine();

            var oCertificadoCovid19 = new CertificadoCovid19(dni);

            var servicesFound = oCertificadoCovid19.ServicesCovid19();

            Console.Write("\nLista de servicios: \n");

            var count = 1;
            for (int i = 0; i < servicesFound.Count; i++)
            {
                var item = string.Format("\t {0} {1}\n",count, servicesFound[i]);
                Console.Write(item);
                count++;
            }

        }

        private static int CheckSelectedOption()
        {
            var esnumero = false;
            string option = string.Empty;
            do
            {
                Console.Write("Elegir opción: ");
                option = Console.ReadLine();
                esnumero = EsNumero(option);

            } while (!esnumero);

            return int.Parse(option);
            
        }

        private static bool EsNumero(string option)
        {
            long number1 = 0;
            bool canConvert = long.TryParse(option, out number1);

            return canConvert;
        }
    }
}

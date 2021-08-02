using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Sigesoft.Node.WinClient.BE;
using System.Web.Configuration;


namespace NetPdf
{
    public class FichaMedicaOcupacional312
    {
        private static void RunFile(string filePDF)
        {
            Process proceso = Process.Start(filePDF);
            proceso.WaitForExit();
            proceso.Close();
        }

        public static void CreateFichaMedicalOcupacional312Report(ServiceList DataService,
                                                                    PacientList filiationData,
                                                                    List<HistoryList> listAtecedentesOcupacionales,
                                                                    List<FamilyMedicalAntecedentsList> listaPatologicosFamiliares,
                                                                    List<PersonMedicalHistoryList> listMedicoPersonales,
                                                                    List<NoxiousHabitsList> listaHabitoNocivos,
                                                                    List<ServiceComponentFieldValuesList> Antropometria,
                                                                    List<ServiceComponentFieldValuesList> FuncionesVitales,
                                                                    List<ServiceComponentFieldValuesList> ExamenFisco,
                                                                    List<ServiceComponentFieldValuesList> Oftalmologia,
                                                                    List<ServiceComponentFieldValuesList> Oftalmologia_UC,
                                                                    List<ServiceComponentFieldValuesList> Psicologia,
                                                                    List<ServiceComponentFieldValuesList> OIT,
                                                                    List<ServiceComponentFieldValuesList> RX,
                                                                    List<ServiceComponentFieldValuesList> Laboratorio,
                                                                    string Audiometria,
                                                                    List<ServiceComponentFieldValuesList> Espirometria,
                                                                    List<DiagnosticRepositoryList> ListDiagnosticRepository,
                                                                    List<RecomendationList> ListRecomendation,
                                                                    List<RestrictionList> ListRestricciones,
                                                                    List<ServiceComponentList> ExamenesServicio,
                                                                    organizationDto infoEmpresaPropietaria,
                                                                    List<ServiceComponentFieldValuesList> VisionEstero,
                                                                    List<ServiceComponentFieldValuesList> VisionColor,
                                                                    string filePDF)
        {
            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);

            //try
            //{NO_BORDER
            // step 2: we create aPA writer that listens to the document
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePDF, FileMode.Create));

            //create an instance of your PDFpage class. This is the class we generated above.
            pdfPage page = new pdfPage();
            //set the PageEvent of the pdfWriter instance to the instance of our PDFPage class
            writer.PageEvent = page;

            // step 3: we open the document
            document.Open();
            // step 4: we Add content to the document
            // we define some fonts
            #region Declaration Tables

            var subTitleBackGroundColor = new BaseColor(System.Drawing.Color.White);
            string include = string.Empty;
            List<PdfPCell> cells = null;
            float[] columnWidths = null;
            //string[] columnValues = null;
            string[] columnHeaders = null;

            //PdfPTable header1 = new PdfPTable(2);
            //header1.HorizontalAlignment = Element.ALIGN_CENTER;
            //header1.WidthPercentage = 100;
            ////header1.TotalWidth = 500;
            ////header1.LockedWidth = true;    // Esto funciona con TotalWidth
            //float[] widths = new float[] { 150f, 200f };
            //header1.SetWidths(widths);


            //Rectangle rec = document.PageSize;
            //PdfPTable header2 = new PdfPTable(6);
            //header2.HorizontalAlignment = Element.ALIGN_CENTER;
            //header2.WidthPercentage = 100;
            //header1.TotalWidth = 500;
            //header1.LockedWidth = true;    // Esto funciona con TotalWidth
            //float[] widths1 = new float[] { 16.6f, 18.6f, 16.6f, 16.6f, 16.6f, 16.6f };
            //header2.SetWidths(widths1);
            //header2.SetWidthPercentage(widths1, rec);

            //PdfPTable companyData = new PdfPTable(6);
            //companyData.HorizontalAlignment = Element.ALIGN_CENTER;
            //companyData.WidthPercentage = 100;
            //header1.TotalWidth = 500;
            //header1.LockedWidth = true;    // Esto funciona con TotalWidth
            //float[] widthscolumnsCompanyData = new float[] { 16.6f, 16.6f, 16.6f, 16.6f, 16.6f, 16.6f };
            //companyData.SetWidths(widthscolumnsCompanyData);

            PdfPTable filiationWorker = new PdfPTable(8);

            PdfPTable table = null;

            PdfPTable whiteline = null;

            PdfPCell cell = null;

            #endregion

            #region Fonts

            Font fontTitle1 = FontFactory.GetFont(FontFactory.HELVETICA, 14, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
            Font fontTitle2 = FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
            Font fontTitleTable = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));
            Font fontTitleTableNegro = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));
            Font fontSubTitle = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.White));
            Font fontSubTitleNegroNegrita = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));

            Font fontColumnValue = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
            Font fontColumnValueNegrita = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));

            Font fontAptitud = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));

            //Font fontTitleTableNegro = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));

            #endregion

            //document.Add(new Paragraph("\r\n"));

            #region Title
         
            PdfPCell CellLogo = null;
            cells = new List<PdfPCell>();
            PdfPCell cellPhoto1 = null;

            if (filiationData.b_Photo != null)
                cellPhoto1 = new PdfPCell(HandlingItextSharp.GetImage(filiationData.b_Photo, null,null,40,40)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT };
            else
                cellPhoto1 = new PdfPCell(new Phrase("Sin Foto Trabjador", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT };

            if (infoEmpresaPropietaria.b_Image != null)
            {
                CellLogo = new PdfPCell(HandlingItextSharp.GetImage(infoEmpresaPropietaria.b_Image, null,null,120,30)) 
                                { HorizontalAlignment = PdfPCell.ALIGN_LEFT };
            }
            else
            {
                CellLogo = new PdfPCell(new Phrase("Sin Logo", fontColumnValue)) 
                                { HorizontalAlignment = PdfPCell.ALIGN_LEFT };
            }

            columnWidths = new float[] { 100f };

            var cellsTit = new List<PdfPCell>()
            { 
                new PdfPCell(new Phrase("Ficha Médico Ocupacional", fontTitle1))
                                { HorizontalAlignment = PdfPCell.ALIGN_LEFT },
                new PdfPCell(new Phrase("(ANEXO N° 02 - RM. N° 312-2011/MINSA)", fontSubTitleNegroNegrita)) 
                                { HorizontalAlignment = PdfPCell.ALIGN_LEFT },
            };

            table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, PdfPCell.NO_BORDER, null, fontTitleTable);

            cells.Add(CellLogo);
            cells.Add(new PdfPCell(table));
            cells.Add(cellPhoto1);

            columnWidths = new float[] { 30f, 50f, 20f };

            table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, PdfPCell.NO_BORDER, null, fontTitleTable);
            document.Add(table);

            var cellsWhite = new List<PdfPCell>()
            { 
                new PdfPCell(new Phrase("   ", fontTitle1))
                                { HorizontalAlignment = PdfPCell.ALIGN_LEFT },
                //new PdfPCell(new Phrase("(ANEXO N° 02 - RM. N° 312-2011/MINSA)", fontSubTitleNegroNegrita)) 
                //                { HorizontalAlignment = PdfPCell.ALIGN_LEFT },
            };
            columnWidths = new float[] { 100f };
            whiteline = HandlingItextSharp.GenerateTableFromCells(cellsWhite, columnWidths, PdfPCell.NO_BORDER, null, fontTitleTable);
            
            //whiteline = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, PdfPCell.NO_BORDER, null, fontTitleTable);
            document.Add(whiteline);

            #endregion

                         
            #region Cabecera del Reporte

            string PreOcupacional = "", Periodica = "", Retiro = "", Otros = "";

            if (DataService.i_EsoTypeId == (int)Sigesoft.Common.TypeESO.PreOcupacional)
            {
                PreOcupacional = "X";
            }
            else if (DataService.i_EsoTypeId == (int)Sigesoft.Common.TypeESO.PeriodicoAnual)
            {
                Periodica = "X";
            }
            else if (DataService.i_EsoTypeId == (int)Sigesoft.Common.TypeESO.Retiro)
            {
                Retiro = "X";
            }
            else
            {
                Otros = "X";
            }

            string Mes = "";
            Mes = Sigesoft.Common.Utils.Getmouth(DataService.i_MesV);


            cells = new List<PdfPCell>()
                 {
                    //fila
                    new PdfPCell(new Phrase("N° de Atención: ", fontColumnValue)),                                   
                    new PdfPCell(new Phrase(DataService.v_ServiceId, fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_LEFT},                                    
                    new PdfPCell(new Phrase("Fecha de Atención", fontColumnValue))
                                             { Colspan=2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase("Día", fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(DataService.i_DiaV.ToString(), fontColumnValue))
                                             { HorizontalAlignment = PdfPCell.ALIGN_LEFT},                    
                    new PdfPCell(new Phrase("Mes", fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(Mes, fontColumnValue))
                                          { HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase("Año ", fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(DataService.i_AnioV.ToString(), fontColumnValue))
                                              { HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    
                     //fila
                    new PdfPCell(new Phrase("Tipo de Evaluación: ", fontColumnValue)),                                                       
                    new PdfPCell(new Phrase("Preocupacional", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT},  
                    new PdfPCell(new Phrase(PreOcupacional, fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase("Periodica", fontColumnValue))
                                 { Colspan=2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(Periodica, fontColumnValue))
                                          { HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase("Retiro", fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(Retiro, fontColumnValue))
                                          { HorizontalAlignment = PdfPCell.ALIGN_LEFT},                 
                    new PdfPCell(new Phrase("Otros", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(Otros, fontColumnValue))
                                          { HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                     //fila
                    new PdfPCell(new Phrase("Lugar de Examen: ", fontColumnValue)),                                                       
                    new PdfPCell(new Phrase(DataService.v_OwnerOrganizationName, fontColumnValue)) { Colspan = 9},   
                 
                 
                 };

            columnWidths = new float[] { 15f, 15f, 7f, 5f, 5f, 5f, 5f, 10f, 7f, 6f };

            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, null, fontTitleTable);

            document.Add(filiationWorker);

            #endregion

            #region Datos de la Empresa

            String PuestoPostula = "";
            if (DataService.i_EsoTypeId == (int)Sigesoft.Common.TypeESO.PreOcupacional)
            {
                PuestoPostula = DataService.v_CurrentOccupation;
            }
            else
            {
                PuestoPostula = "";
            }
            cells = new List<PdfPCell>()
                {
                     //fila
                    new PdfPCell(new Phrase("Razón Social: ", fontColumnValue)),                                   
                    new PdfPCell(new Phrase(DataService.EmpresaTrabajo, fontColumnValue))
                                 { Colspan=9, HorizontalAlignment = PdfPCell.ALIGN_LEFT},  

                    //fila
                    new PdfPCell(new Phrase("Actividad Económica: ", fontColumnValue)),
                                new PdfPCell(new Phrase(DataService.RubroEmpresaTrabajo, fontColumnValue))
                                 { Colspan=9, HorizontalAlignment = PdfPCell.ALIGN_LEFT},  

                    //fila
                    new PdfPCell(new Phrase("Lugar de Trabajo: ", fontColumnValue)),
                                new PdfPCell(new Phrase(DataService.DireccionEmpresaTrabajo, fontColumnValue)) 
                                    { Colspan=9, HorizontalAlignment = PdfPCell.ALIGN_LEFT},  

                     //fila
                    new PdfPCell(new Phrase("Puesto de Trabajo: ", fontColumnValue)),
                                new PdfPCell(new Phrase(DataService.v_CurrentOccupation, fontColumnValue)) 
                                    { Colspan=9, HorizontalAlignment = PdfPCell.ALIGN_LEFT},  

                    //fila
                    new PdfPCell(new Phrase("Puesto al que Postula (Solo Pre Ocupacional): ", fontColumnValue))
                                    { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT}, 
                    new PdfPCell(new Phrase(PuestoPostula, fontColumnValue))
                                    { Colspan=5, HorizontalAlignment = PdfPCell.ALIGN_LEFT}, 
                };
            columnWidths = new float[] { 15f, 10f, 7f, 5f, 5f, 5f, 5f, 5f, 8f, 9f };

            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "I. DATOS DE LA EMPRESA", fontTitleTable);

            document.Add(filiationWorker);

            #endregion

            #region Filiación del trabajador

            //PdfPCell cellPhoto = null;

            //if (filiationData.b_Photo != null)
            //    cellPhoto = new PdfPCell(HandlingItextSharp.GetImage(filiationData.b_Photo, 15F));
            //else
            //    cellPhoto = new PdfPCell(new Phrase("Sin Foto", fontColumnValue));

            //cellPhoto.Rowspan = 5;
            //cellPhoto.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //cellPhoto.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

            string ResidenciaLugarTrabajoSI = "", ResidenciaLugarTrabajoNO = "";

            if (DataService.i_ResidenceInWorkplaceId == (int)Sigesoft.Common.SiNo.SI)
            {
                ResidenciaLugarTrabajoSI = "X";
            }
            else
            {
                ResidenciaLugarTrabajoNO = "X";
            }


            string ESSALUD = "", EPS = "", SCTR = "", OTRO = "";

            if (DataService.i_TypeOfInsuranceId == (int)Sigesoft.Common.TypeOfInsurance.ESSALUD)
            {
                ESSALUD = "X";
            }
            else if (DataService.i_TypeOfInsuranceId == (int)Sigesoft.Common.TypeOfInsurance.EPS)
            {
                EPS = "X";
            }
            else
            {
                OTRO = "X";
            }

            string DNI = "", Pass = "", Carnet = "";

            if (DataService.i_DocTypeId == 1)
            {
                DNI = "X";
            }
            else if (DataService.i_DocTypeId == 2)
            {
                Pass = "X";
            }
            else if (DataService.i_DocTypeId == 4)
            {
                Carnet = "X";
            }
            string Mes1 = "";
            Mes1 = Sigesoft.Common.Utils.Getmouth(DataService.i_MesN);

            cells = new List<PdfPCell>()
                {
                    //fila
                    new PdfPCell(new Phrase("Nombres y Apellidos: ", fontColumnValue)), 
                    new PdfPCell(new Phrase(DataService.v_Pacient, fontColumnValue))
                                                                        { Colspan = 9, HorizontalAlignment = PdfPCell.ALIGN_LEFT}, 
                    //new PdfPCell(new PdfPCell(HandlingItextSharp.GetImage(filiationData.b_Photo, 15F)) )
                    //                                                   { Rowspan = 5, HorizontalAlignment = PdfPCell.ALIGN_CENTER, 
                    //                                                     VerticalAlignment = PdfPCell.ALIGN_MIDDLE 
                    //                                                   },
                     //new PdfPCell(cellPhoto),
                    //fila
                    new PdfPCell(new Phrase("Fecha de Nacimiento: ", fontColumnValue)),                                   
                    new PdfPCell(new Phrase("Día: ", fontColumnValue))
                                                     { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(DataService.i_DiaN.ToString(), fontColumnValue)), 
                    new PdfPCell(new Phrase("Mes: ", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(Mes1, fontColumnValue))
                                                     { Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase("Año: ", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(DataService.i_AnioN.ToString(), fontColumnValue)),
                 
                    //fila
                    new PdfPCell(new Phrase("Edad: ", fontColumnValue)), 
                    new PdfPCell(new Phrase(DataService.i_Edad.ToString(), fontColumnValue))
                                                    { Colspan = 9, HorizontalAlignment = PdfPCell.ALIGN_LEFT},                   
           

                    //fila
                    new PdfPCell(new Phrase("Documento de Identidad Carnet de Extranjeria (  " + Carnet +"  ), DNI (  " + DNI+"  ), Pasaporte (  " + Pass + "  ): ", fontColumnValue))
                                                { Colspan = 6, HorizontalAlignment = PdfPCell.ALIGN_LEFT}, 
                    new PdfPCell(new Phrase(DataService.v_DocNumber, fontColumnValue))
                                                                        { Colspan = 4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},   
                  
                    //fila
                    new PdfPCell(new Phrase("Dirección Fiscal: ", fontColumnValue)) {  HorizontalAlignment = PdfPCell.ALIGN_RIGHT},  
                    new PdfPCell(new Phrase(DataService.v_AdressLocation, fontColumnValue))
                                                                        { Colspan = 9, HorizontalAlignment = PdfPCell.ALIGN_LEFT},  

                    //fila
                    new PdfPCell(new Phrase("Distrito: ", fontColumnValue)){  HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(DataService.DistritoPaciente, fontColumnValue)) 
                                              { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT},   
                    new PdfPCell(new Phrase("Provincia: ", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT}, 
                    new PdfPCell(new Phrase(DataService.ProvinciaPaciente, fontColumnValue)),
                    new PdfPCell(new Phrase("Departamento ", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT}, 
                    new PdfPCell(new Phrase(DataService.DepartamentoPaciente, fontColumnValue)) 
                                                         { Colspan = 4, HorizontalAlignment = PdfPCell.ALIGN_LEFT}, 


                    //fila
                    new PdfPCell(new Phrase("Residencia en lugar de Trabajo: ", fontColumnValue)){  HorizontalAlignment = PdfPCell.ALIGN_RIGHT},                                  
                    new PdfPCell(new Phrase("SI: ", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT},                                 
                    new PdfPCell(new Phrase(ResidenciaLugarTrabajoSI, fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_LEFT}, 
                    new PdfPCell(new Phrase("NO: ", fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(ResidenciaLugarTrabajoNO, fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase("Tiempo de Residencia en lugar de Trabajo: ", fontColumnValue))
                                         { Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_LEFT}, 
                    new PdfPCell(new Phrase(DataService.v_ResidenceTimeInWorkplace, fontColumnValue)),
                    new PdfPCell(new Phrase("Años ", fontColumnValue)),
                 

                    //fila
                    new PdfPCell(new Phrase("ESSALUD", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT},                                  
                    new PdfPCell(new Phrase(ESSALUD, fontColumnValue)),                                    
                    new PdfPCell(new Phrase("EPS", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT},   
                    new PdfPCell(new Phrase(EPS, fontColumnValue)),
                    new PdfPCell(new Phrase("OTRO", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT},  
                     new PdfPCell(new Phrase("", fontColumnValue)),                                   
                    new PdfPCell(new Phrase("SCTR", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT},                                      
                    new PdfPCell(new Phrase(" ", fontColumnValue)), 
                    new PdfPCell(new Phrase("OTRO", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT},  
                    new PdfPCell(new Phrase(OTRO, fontColumnValue)),

                     //fila
                    new PdfPCell(new Phrase("Correo Electrónico: ", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT},                                  
                    new PdfPCell(new Phrase(DataService.Email, fontColumnValue))       
                                    { Colspan = 5, HorizontalAlignment = PdfPCell.ALIGN_LEFT},        
                    new PdfPCell(new Phrase("Teléfono: ", fontColumnValue))
                                    { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(DataService.Telefono, fontColumnValue))
                                    { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_LEFT}, 

                    //fila
                    new PdfPCell(new Phrase("Estado Civil: ", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT},                                
                    new PdfPCell(new Phrase(DataService.EstadoCivil, fontColumnValue))       
                                    { Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_LEFT},        
                    new PdfPCell(new Phrase("Grado de Instrucción: ", fontColumnValue))
                                    { Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(DataService.GradoInstruccion, fontColumnValue))
                                    { Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_LEFT}, 

                     //fila
                    new PdfPCell(new Phrase("N° Total de Hijos Vivos: ", fontColumnValue))
                                    { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT},               
                    new PdfPCell(new Phrase(filiationData.i_NumberLivingChildren.ToString(), fontColumnValue))       
                                    { Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_LEFT},        
                    new PdfPCell(new Phrase("N° Dependiente: ", fontColumnValue))
                                    { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase(filiationData.i_NumberDependentChildren.ToString(), fontColumnValue))
                                    { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_LEFT},                                                                
                                                   
                };

            columnWidths = new float[] { 15f, 5f, 5f, 10f, 8f, 8f, 4f, 4f, 8f, 9f };

            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "II. FILIACIÓN DEL TRABAJADOR", fontTitleTable);

            document.Add(filiationWorker);

            #endregion

            #region Antecedentes Ocupacionales

            cells = new List<PdfPCell>();

            if (listAtecedentesOcupacionales != null && listAtecedentesOcupacionales.Count > 0)
            {
                columnWidths = new float[] { 5f, 7f };

                foreach (var item in listAtecedentesOcupacionales)
                {
                    //Columna EMPRESA
                    cell = new PdfPCell(new Phrase(item.v_Organization, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                    cells.Add(cell);

                    //Columna ÁREA
                    cell = new PdfPCell(new Phrase(item.v_TypeActivity, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                    cells.Add(cell);

                    //Columna OCUPACIÓN
                    cell = new PdfPCell(new Phrase(item.v_workstation, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                    cells.Add(cell);

                    //Crear Tabla en duro FECHA
                    List<PdfPCell> cells1 = new List<PdfPCell>();
                    cells1.Add(new PdfPCell(new Phrase("Fecha Inicio: ", fontColumnValue)));
                    cells1.Add(new PdfPCell(new Phrase(item.d_StartDate.Value.ToString("MM/yyyy"), fontColumnValue)));

                    cells1.Add(new PdfPCell(new Phrase("Fecha Fin: ", fontColumnValue)));
                    cells1.Add(new PdfPCell(new Phrase(item.d_EndDate.Value.ToString("MM/yyyy"), fontColumnValue)));

                    table = HandlingItextSharp.GenerateTableFromCells(cells1, columnWidths, "", fontTitleTable);

                    cell = new PdfPCell(table);
                    cells.Add(cell);

                    //Columna EXPOSICIÓN
                    cell = new PdfPCell(new Phrase(item.Exposicion == "" ? "No refiere peligros en el puesto" : item.Exposicion, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                    cells.Add(cell);
                    
                    //Columna TIPO OPERACIÓN
                    cell = new PdfPCell(new Phrase(item.TiempoLabor, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                    cells.Add(cell);

                    //Columna EPPS
                    cell = new PdfPCell(new Phrase(item.Epps == "" ? "No usó EPP" : item.Epps, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                    cells.Add(cell);
                }

                columnWidths = new float[] { 15f, 15f, 15f, 15f, 15f, 15f, 15f };

            }
            else
            {
                cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)) { Colspan = 8, HorizontalAlignment = PdfPCell.ALIGN_LEFT });
                columnWidths = new float[] { 15f, 15f, 15f, 15f, 15f, 15f, 15f };
            }

            columnHeaders = new string[] { "Empresa", "Área", "Ocupación", "Fecha", "Exposición", "Tiempo de Trabajo", "EPP" };

            table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "III. ANTECEDENTES OCUPACIONALES", fontTitleTable, columnHeaders);

            document.Add(table);

            #endregion

            //// Salto de linea
            //document.Add(new Paragraph("\r\n"));

            #region Antecedentes Patológicos Personales

            string AlergiaX = "", DiabetesX = "", HepatitisBX = "", TBCX = "", AsmaX = "", HTAX = "", ITSX = "", TifoideaX = "", BronquitisX = "", NeoplasiasX = "", ConvulsionesX = "", QuemadurasX = "", CirugiasX = "", IntoxicacionesX = "";

            var Alergia = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.ALERGIA_NO_ESPECIFICADA);
            var Diabetess = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.DIABETES_MELLITUS);
            var TBC = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.TUBERCULOSIS);
            var Hepatitis = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.HEPATITISB);
            var Asma = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.ASMA);
            var HTA = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.HTA);
            var ITS = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.ITS);
            var Tifoidea = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.TIFOIDEA);
            var Bronquitis = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.BRONQUITIS);
            // Alejandro
            var Neoplasias = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.NEOPLASIAS);
            var Convulsiones = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.CONVULSIONES);
            var Quemaduras = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.QUEMADURAS);
            var Cirugias = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.CIRUGIAS);
            var Intoxicaciones = listMedicoPersonales.FindAll(p => p.v_DiseasesId == Sigesoft.Common.Constants.INTOXICACIONES);

            #region Marcar con X

            if (Bronquitis.Count() != 0)
            {
                if (Bronquitis != null)
                {
                    BronquitisX = "X";
                }
                else
                {
                    BronquitisX = "";
                }
            }
            else
            {
                BronquitisX = "";
            }

            if (Tifoidea.Count() != 0)
            {
                if (Tifoidea != null)
                {
                    TifoideaX = "X";
                }
                else
                {
                    TifoideaX = "";
                }
            }
            else
            {
                TifoideaX = "";
            }
            if (ITS.Count() != 0)
            {
                if (ITS != null)
                {
                    ITSX = "X";
                }
                else
                {
                    ITSX = "";
                }
            }
            else
            {
                ITSX = "";
            }

            if (HTA.Count() != 0)
            {
                if (HTA != null)
                {
                    HTAX = "X";
                }
                else
                {
                    HTAX = "";
                }
            }
            else
            {
                HTAX = "";
            }
            if (Asma.Count() != 0)
            {
                if (Asma != null)
                {
                    AsmaX = "X";
                }
                else
                {
                    AsmaX = "";
                }
            }
            else
            {
                AsmaX = "";
            }
            if (Alergia.Count() != 0)
            {
                if (Alergia != null)
                {
                    AlergiaX = "X";
                }
                else
                {
                    AlergiaX = "";
                }
            }
            else
            {
                AlergiaX = "";
            }

            if (Diabetess.Count() != 0)
            {
                if (Diabetess != null)
                {
                    DiabetesX = "X";
                }
                else
                {
                    DiabetesX = "";
                }
            }
            else
            {
                DiabetesX = "";
            }

            if (TBC.Count() != 0)
            {
                if (TBC != null)
                {
                    TBCX = "X";
                }
                else
                {
                    TBCX = "";
                }
            }
            else
            {
                TBCX = "";
            }

            if (Hepatitis.Count() != 0)
            {
                if (Hepatitis != null)
                {
                    HepatitisBX = "X";
                }
                else
                {
                    HepatitisBX = "";
                }
            }
            else
            {
                HepatitisBX = "";
            }

            // Alejandro
            if (Neoplasias.Count() != 0)
            {
                if (Neoplasias != null)
                {
                    NeoplasiasX = "X";
                }
                else
                {
                    NeoplasiasX = "";
                }
            }
            else
            {
                NeoplasiasX = "";
            }

            if (Convulsiones.Count() != 0)
            {
                if (Convulsiones != null)
                {
                    ConvulsionesX = "X";
                }
                else
                {
                    ConvulsionesX = "";
                }
            }
            else
            {
                ConvulsionesX = "";
            }

            if (Quemaduras.Count() != 0)
            {
                if (Quemaduras != null)
                {
                    QuemadurasX = "X";
                }
                else
                {
                    QuemadurasX = "";
                }
            }
            else
            {
                QuemadurasX = "";
            }

            if (Cirugias.Count() != 0)
            {
                if (Cirugias != null)
                {
                    CirugiasX = "X";
                }
                else
                {
                    CirugiasX = "";
                }
            }
            else
            {
                CirugiasX = "";
            }

            if (Intoxicaciones.Count() != 0)
            {
                if (Intoxicaciones != null)
                {
                    IntoxicacionesX = "X";
                }
                else
                {
                    IntoxicacionesX = "";
                }
            }
            else
            {
                IntoxicacionesX = "";
            }


            #endregion

            // Alejandro
            #region No Refiere

            var noRefiereAP = string.Empty;

            if (Alergia.Count == 0 && Diabetess.Count == 0 && TBC.Count == 0 && Hepatitis.Count == 0 && Hepatitis.Count == 0
                && Asma.Count == 0 && HTA.Count == 0 && ITS.Count == 0 && Tifoidea.Count == 0
                && Bronquitis.Count == 0 && Neoplasias.Count == 0 && Convulsiones.Count == 0 && Quemaduras.Count == 0
                && Cirugias.Count == 0 && Intoxicaciones.Count == 0)
            {
                noRefiereAP = ": No refiere";
            }

            #endregion

            cells = new List<PdfPCell>()
                {

                    //fila
                    new PdfPCell(new Phrase("Alergias ", fontColumnValue)),                                   
                    new PdfPCell(new Phrase(AlergiaX, fontColumnValue)),                                    
                    new PdfPCell(new Phrase("Diabetes", fontColumnValue)), 
                    new PdfPCell(new Phrase(DiabetesX, fontColumnValue)),
                    new PdfPCell(new Phrase("TBC", fontColumnValue)),
                    new PdfPCell(new Phrase(TBCX, fontColumnValue)),                                        
                    new PdfPCell(new Phrase("Hepatitis B", fontColumnValue)),
                    new PdfPCell(new Phrase(HepatitisBX, fontColumnValue)),    

                    //fila
                    new PdfPCell(new Phrase("Asma ", fontColumnValue)),                                   
                    new PdfPCell(new Phrase(AsmaX, fontColumnValue)),                                    
                    new PdfPCell(new Phrase("HTA", fontColumnValue)), 
                    new PdfPCell(new Phrase(HTAX, fontColumnValue)),
                    new PdfPCell(new Phrase("ITS", fontColumnValue)),
                    new PdfPCell(new Phrase(ITSX, fontColumnValue)),                                        
                    new PdfPCell(new Phrase("Tifoidea", fontColumnValue)),
                    new PdfPCell(new Phrase(TifoideaX, fontColumnValue)),    

                    //fila
                    new PdfPCell(new Phrase("Bronquitis ", fontColumnValue)),                                   
                    new PdfPCell(new Phrase(BronquitisX, fontColumnValue)),                                    
                    new PdfPCell(new Phrase("Neoplasia", fontColumnValue)), 
                    new PdfPCell(new Phrase(NeoplasiasX, fontColumnValue)),
                    new PdfPCell(new Phrase("Convulsiones", fontColumnValue)),
                    new PdfPCell(new Phrase(ConvulsionesX, fontColumnValue)),                                        
                    new PdfPCell(new Phrase("Otros", fontColumnValue)),
                    new PdfPCell(new Phrase(" ", fontColumnValue)), 
 
                    //fila
                    new PdfPCell(new Phrase("Quemaduras ", fontColumnValue)),                                   
                    new PdfPCell(new Phrase(QuemadurasX, fontColumnValue))
                                     { Colspan=7, HorizontalAlignment = PdfPCell.ALIGN_LEFT },

                    //fila
                    new PdfPCell(new Phrase("Cirugias ", fontColumnValue)),                                   
                    new PdfPCell(new Phrase(CirugiasX, fontColumnValue))
                                     { Colspan=3, HorizontalAlignment = PdfPCell.ALIGN_LEFT },

                    new PdfPCell(new Phrase("Intoxicaciones ", fontColumnValue)),                                   
                    new PdfPCell(new Phrase(IntoxicacionesX, fontColumnValue))
                                     { Colspan=3, HorizontalAlignment = PdfPCell.ALIGN_LEFT },

                };

            columnWidths = new float[] { 20f, 5f, 20f, 5f, 20f, 5f, 20f, 5f, };

            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "IV. ANTECEDENTES PATOLÓGICOS PERSONALES" + noRefiereAP, fontTitleTable);

            document.Add(filiationWorker);

            #endregion

            //// Salto de linea
            //document.Add(new Paragraph("\r\n"));

            #region Hábitos Nocivos

            if (listaHabitoNocivos == null)
                listaHabitoNocivos = new List<NoxiousHabitsList>();

            var Alcohol = listaHabitoNocivos.FindAll(p => p.i_TypeHabitsId == (int)Sigesoft.Common.TypeHabit.Alcohol);
            var Tabaco = listaHabitoNocivos.FindAll(p => p.i_TypeHabitsId == (int)Sigesoft.Common.TypeHabit.Tabaco);
            var Drogas = listaHabitoNocivos.FindAll(p => p.i_TypeHabitsId == (int)Sigesoft.Common.TypeHabit.Drogas);

            cells = new List<PdfPCell>()
                {
                     //fila
                    new PdfPCell(new Phrase("Hábitos Nocivos ", fontColumnValue)), 
                    //new PdfPCell(new Phrase("Tipo ", fontColumnValue)),
                    //new PdfPCell(new Phrase("Cantidad ", fontColumnValue)),
                    new PdfPCell(new Phrase("Frecuencia ", fontColumnValue)),

                    //fila
                    new PdfPCell(new Phrase("Alcohol ", fontColumnValue)), 
                    //new PdfPCell(new Phrase(Alcohol.Count == 0 ? string.Empty : Alcohol[0].v_DescriptionHabit, fontColumnValue)),
                    //new PdfPCell(new Phrase(Alcohol.Count == 0 ? string.Empty :Alcohol[0].v_DescriptionQuantity, fontColumnValue)),
                    new PdfPCell(new Phrase(Alcohol.Count == 0 ? string.Empty :Alcohol[0].v_Frequency, fontColumnValue)),

                    //fila
                    new PdfPCell(new Phrase("Tabaco ", fontColumnValue)), 
                    //new PdfPCell(new Phrase(Tabaco.Count == 0 ? string.Empty :Tabaco[0].v_DescriptionHabit, fontColumnValue)),
                    //new PdfPCell(new Phrase(Tabaco.Count == 0 ? string.Empty :Tabaco[0].v_DescriptionQuantity, fontColumnValue)),
                    new PdfPCell(new Phrase(Tabaco.Count == 0 ? string.Empty :Tabaco[0].v_Frequency, fontColumnValue)),

                    //fila
                    new PdfPCell(new Phrase("Drogas ", fontColumnValue)), 
                    //new PdfPCell(new Phrase(Drogas.Count == 0 ? string.Empty :Drogas[0].v_DescriptionHabit, fontColumnValue)),
                    //new PdfPCell(new Phrase(Drogas.Count == 0 ? string.Empty :Drogas[0].v_DescriptionQuantity, fontColumnValue)),
                    new PdfPCell(new Phrase(Drogas.Count == 0 ? string.Empty :Drogas[0].v_Frequency, fontColumnValue)),

                    //fila
                    //new PdfPCell(new Phrase("Medicamentos ", fontColumnValue)), 
                    //new PdfPCell(new Phrase(" ", fontColumnValue))
                    //              { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                };

            columnWidths = new float[] { 20f, 80f };

            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "", fontTitleTable);

            document.Add(filiationWorker);

            #endregion

            #region Antecedentes Patológicos Familiares

            cells = new List<PdfPCell>();

            var noRefiere = string.Empty;

            if (listaPatologicosFamiliares != null && listaPatologicosFamiliares.Count > 0)
            {
                columnWidths = new float[] { 7f };
                include = "DxAndComment";

           
                //Columna FAMILIAR
                cell = new PdfPCell(new Phrase("Padre", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                cells.Add(cell);
                var PadreDx = listaPatologicosFamiliares.FindAll(p => p.i_TypeFamilyId == (int)Sigesoft.Common.TypeFamily.PADRE_OK);
                // Crear tabla de recomendaciones para insertarla en la celda que corresponde
                table = HandlingItextSharp.GenerateTableFromList(PadreDx, columnWidths, include, fontColumnValue);
                cell = new PdfPCell(table);
                cells.Add(cell);


                //Columna FAMILIAR
                cell = new PdfPCell(new Phrase("Madre", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                cells.Add(cell);
                var MadreDx = listaPatologicosFamiliares.FindAll(p => p.i_TypeFamilyId == (int)Sigesoft.Common.TypeFamily.MADRE_OK);
                // Crear tabla de recomendaciones para insertarla en la celda que corresponde
                table = HandlingItextSharp.GenerateTableFromList(MadreDx, columnWidths, include, fontColumnValue);
                cell = new PdfPCell(table);
                cells.Add(cell);


                //Columna FAMILIAR
                cell = new PdfPCell(new Phrase("Hermanos", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                cells.Add(cell);
                var HermanosDx = listaPatologicosFamiliares.FindAll(p => p.i_TypeFamilyId == (int)Sigesoft.Common.TypeFamily.HERMANOS_OK);
                // Crear tabla de recomendaciones para insertarla en la celda que corresponde
                table = HandlingItextSharp.GenerateTableFromList(HermanosDx, columnWidths, include, fontColumnValue);
                cell = new PdfPCell(table);
                cells.Add(cell);

                //Columna FAMILIAR
                cell = new PdfPCell(new Phrase("Esposo(a)", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                cells.Add(cell);
                var EspososDx = listaPatologicosFamiliares.FindAll(p => p.i_TypeFamilyId == (int)Sigesoft.Common.TypeFamily.ABUELOS_OK);
                // Crear tabla de recomendaciones para insertarla en la celda que corresponde
                table = HandlingItextSharp.GenerateTableFromList(EspososDx, columnWidths, include, fontColumnValue);
                cell = new PdfPCell(table);
                cells.Add(cell);


                cell = new PdfPCell(table);
                cells.Add(cell);

                columnWidths = new float[] { 10, 60f };

            }
            else
            {
                noRefiere = ": No refiere";
            }

            table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "V. ANTECEDENTES PATOLÓGICOS FAMILIARES" + noRefiere, fontTitleTable, null);

            document.Add(table);

            cells = new List<PdfPCell>()
                {
                   //fila
                    new PdfPCell(new Phrase("Hijos Vivos: ", fontColumnValue)),                                   
                    new PdfPCell(new Phrase(" ", fontColumnValue)),                                    
                    new PdfPCell(new Phrase("N°", fontColumnValue)), 
                    new PdfPCell(new Phrase(DataService.HijosVivos == null ? "" : DataService.HijosVivos.ToString(), fontColumnValue)),
                    new PdfPCell(new Phrase("Hijos Muertos", fontColumnValue)),
                    new PdfPCell(new Phrase(" ", fontColumnValue)),                                        
                    new PdfPCell(new Phrase("N°", fontColumnValue)),
                    new PdfPCell(new Phrase(DataService.HijosDependientes == null ? "" : DataService.HijosDependientes.ToString(), fontColumnValue)){  HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                   
                };

            columnWidths = new float[] { 15f, 5f, 5f, 5f, 15f, 5f, 5f, 5f };

            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "", fontTitleTable);

            document.Add(filiationWorker);

            cells = new List<PdfPCell>()
               {
                     //fila
                    new PdfPCell(new Phrase("Otros Antecedentes y Absentismo: Enfermedades y Accidentes (asociados a Trabajo o no) ", fontColumnValueNegrita))
                                    { Colspan = 5, HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                                                                      
                    //fila
                    new PdfPCell(new Phrase("Enfermedad, Accidente ", fontColumnValue))
                                    { Rowspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase("Asociado al Trabajo", fontColumnValue))
                                    { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase("Año", fontColumnValue))
                                     { Rowspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase("Días de Descanso", fontColumnValue))
                                     { Rowspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER},

                    //fila
              
                    new PdfPCell(new Phrase("Si", fontColumnValue))
                                    { HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase("No", fontColumnValue))
                                    { HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    //new PdfPCell(new Phrase("", fontColumnValue)),
                    //new PdfPCell(new Phrase("", fontColumnValue)),
                    
               };

            foreach (var item in listMedicoPersonales)
            {
                cell = new PdfPCell(new Phrase(item.v_DiseasesName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_LEFT };
                cells.Add(cell);

                if (item.i_TypeDiagnosticId == (int)Sigesoft.Common.TipoDx.Accidente_Ocupacional || item.i_TypeDiagnosticId == (int)Sigesoft.Common.TipoDx.Enfermedad_Ocupacional)
                {
                    cell = new PdfPCell(new Phrase("X", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                    cells.Add(cell);

                    cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                    cells.Add(cell);
                }
                else
                {
                    cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                    cells.Add(cell);

                    cell = new PdfPCell(new Phrase("X", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                    cells.Add(cell);
                }

                cell = new PdfPCell(new Phrase(item.d_StartDate == null ? string.Empty : item.d_StartDate.Value.ToString("yyyy").ToString(), fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                cells.Add(cell);

                cell = new PdfPCell(new Phrase(item.v_TreatmentSite, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                cells.Add(cell);

            }

            columnWidths = new float[] { 40f, 10f, 10f, 10f, 30f };

            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "", fontTitleTable);

            document.Add(filiationWorker);

            #endregion

            document.NewPage();

            #region Evaluación Médica

            //Antropometria
            string Talla = Antropometria.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)Antropometria.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.ANTROPOMETRIA_TALLA_ID)).v_Value1;
            string Peso = Antropometria.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)Antropometria.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.ANTROPOMETRIA_PESO_ID)).v_Value1;
            string IMC = Antropometria.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)Antropometria.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.ANTROPOMETRIA_IMC_ID)).v_Value1;
            string ICC = Antropometria.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)Antropometria.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.ANTROPOMETRIA_INDICE_CINTURA_ID)).v_Value1;
            string PerimetroCadera = Antropometria.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)Antropometria.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.ANTROPOMETRIA_PERIMETRO_CADERA_ID)).v_Value1;
            string PerimetroAbdominal = Antropometria.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)Antropometria.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.ANTROPOMETRIA_PERIMETRO_ABDOMINAL_ID)).v_Value1;
            string PorcentajeGrasaCorporal = Antropometria.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)Antropometria.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.ANTROPOMETRIA_PORCENTAJE_GRASA_CORPORAL_ID)).v_Value1;

            //Funciones Vitales
            string FrecResp = FuncionesVitales.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)FuncionesVitales.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.FUNCIONES_VITALES_FREC_RESPIRATORIA_ID)).v_Value1;
            string frecCard = FuncionesVitales.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)FuncionesVitales.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.FUNCIONES_VITALES_FREC_CARDIACA_ID)).v_Value1;
            string PAD = FuncionesVitales.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)FuncionesVitales.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.FUNCIONES_VITALES_PAD_ID)).v_Value1;
            string PAS = FuncionesVitales.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)FuncionesVitales.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.FUNCIONES_VITALES_PAS_ID)).v_Value1;
            string Temperatura = FuncionesVitales.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)FuncionesVitales.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.FUNCIONES_VITALES_TEMPERATURA_ID)).v_Value1;
            string SaturacionOxigeno = FuncionesVitales.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)FuncionesVitales.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.FUNCIONES_VITALES_SAT_O2_ID)).v_Value1;

            string ConcatenadoOtros = "";

            if (PerimetroCadera != "0" && PerimetroCadera != string.Empty)
            {
                ConcatenadoOtros = ConcatenadoOtros + " Permimetro Cadera : " + PerimetroCadera + "; ";
            }

            if (PerimetroAbdominal != "0" && PerimetroAbdominal != "0,00" && PerimetroAbdominal != string.Empty)
            {
                ConcatenadoOtros = ConcatenadoOtros + "Perímetro Cintura : " + PerimetroAbdominal + "; ";
            }

            if (PorcentajeGrasaCorporal != "0.00" && PorcentajeGrasaCorporal != "0,00" && PorcentajeGrasaCorporal != string.Empty)
            {
                ConcatenadoOtros = ConcatenadoOtros + "% Grasa Corporal : " + PorcentajeGrasaCorporal + "; ";
            }

            if (SaturacionOxigeno != "0" && SaturacionOxigeno != string.Empty)
            {
                ConcatenadoOtros = ConcatenadoOtros + "Saturación de Oxígeno : " + SaturacionOxigeno + "; ";
            }


            ConcatenadoOtros = ConcatenadoOtros == "" ? string.Empty : ConcatenadoOtros.Substring(0, ConcatenadoOtros.Length - 2);

            string Estoscopia = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_ECTOSCOPIA_GENERAL_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_ECTOSCOPIA_GENERAL_DESCRIPCION_ID)).v_Value1;
            string Estado_Mental = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_ESTADO_METAL_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_ESTADO_METAL_DESCRIPCION_ID)).v_Value1;

            string PielX = "";
            string Piel = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_PIEL_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_PIEL_DESCRIPCION_ID)).v_Value1;
            string PielHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_PIEL_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_PIEL_ID)).v_Value1;
            if (PielHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) PielX = "X";

            string CabelloX = "";
            string Cabello = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_CABELLO_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_CABELLO_DESCRIPCION_ID)).v_Value1;
            string CabelloHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_CABELLO_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_CABELLO_ID)).v_Value1;
            if (CabelloHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) CabelloX = "X";


            string OidoX = "";
            string Oido = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_OIDOS_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_OIDOS_DESCRIPCION_ID)).v_Value1;
            string OidoHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_OIDOS_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_OIDOS_ID)).v_Value1;
            if (OidoHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) OidoX = "X";


            string NarizX = "";
            string Nariz = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_NARIZ_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_NARIZ_DESCRIPCION_ID)).v_Value1;
            string NarizHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_NARIZ_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_NARIZ_ID)).v_Value1;
            if (NarizHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) NarizX = "X";


            string BocaX = "";
            string Boca = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_BOCA_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_BOCA_DESCRIPCION_ID)).v_Value1;
            string BocaHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_BOCA_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_BOCA_ID)).v_Value1;
            if (BocaHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) BocaX = "X";


            string FaringeX = "";
            string Faringe = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_FARINGE_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_FARINGE_DESCRIPCION_ID)).v_Value1;
            string FaringeHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_FARINGE_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_FARINGE_ID)).v_Value1;
            if (FaringeHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) FaringeX = "X";


            string CuelloX = "";
            string Cuello = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_CUELLO_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_CUELLO_DESCRIPCION_ID)).v_Value1;
            string CuelloHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_CUELLO_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_CUELLO_ID)).v_Value1;
            if (CuelloHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) CuelloX = "X";


            string ApaRespiratorioX = "";
            string ApaRespiratorio = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATO_RESPIRATORIO_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATO_RESPIRATORIO_DESCRIPCION_ID)).v_Value1;
            string ApaRespiratorioHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATORESPIRATORIO_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATORESPIRATORIO_ID)).v_Value1;
            if (ApaRespiratorioHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) ApaRespiratorioX = "X";


            string ApaCardioVascularX = "";
            string ApaCardioVascular = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_CARDIO_VASCULAR_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_CARDIO_VASCULAR_DESCRIPCION_ID)).v_Value1;
            string ApaCardioVascularHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_CARDIO_VASCULAR_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_CARDIO_VASCULAR_ID)).v_Value1;
            if (ApaCardioVascularHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) ApaCardioVascularX = "X";


            string ApaDigestivoX = "";
            string ApaDigestivo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATO_DIGESTIVO_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATO_DIGESTIVO_DESCRIPCION_ID)).v_Value1;
            string ApaDigestivoHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATO_DIGESTIVO_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATO_DIGESTIVO_ID)).v_Value1;
            if (ApaDigestivoHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) ApaDigestivoX = "X";


            string ApaGenitoUrinarioX = "";
            //string ApaGenitoUrinario = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATO_GENITOURINARIO_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATO_GENITOURINARIO_DESCRIPCION_ID)).v_Value1;
            string ApaGenitoUrinario = "DIFERIDO";
            string ApaGenitoUrinarioHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_GENITOURINARIO_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_GENITOURINARIO_ID)).v_Value1;
            if (ApaGenitoUrinarioHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) ApaGenitoUrinarioX = "X";

            // Alejandro
            // si es mujer el trabajador mostrar sus antecedentes

            int? sex = DataService.i_SexTypeId;

            //if (sex == (int?)Sigesoft.Common.Gender.FEMENINO)
            //{
            //    ApaGenitoUrinario = string.Format("Menarquia: {0} ," +
            //                                       "FUM: {1} ," +
            //                                       "Régimen Catamenial: {2} ," +
            //                                       "Gestación y Paridad: {3} ," +
            //                                       "MAC: {4} ," +
            //                                       "Cirugía Ginecológica: {5}", string.IsNullOrEmpty(DataService.v_Menarquia) ? "No refiere" : DataService.v_Menarquia,
            //                                                                    DataService.d_Fur == null ? "No refiere" : DataService.d_Fur.Value.ToShortDateString(),
            //                                                                    string.IsNullOrEmpty(DataService.v_CatemenialRegime) ? "No refiere" : DataService.v_CatemenialRegime,
            //                                                                    DataService.v_Gestapara,  
            //                                                                    DataService.v_Mac,
            //                                                                    string.IsNullOrEmpty(DataService.v_CiruGine) ? "No refiere" : DataService.v_CiruGine);
                                                                                                                                                                                                                                                                                                                        
            //}

            string ApaLocomotorX = "";
            string ApaLocomotor = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATO_LOCOMOTOR_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATO_LOCOMOTOR_DESCRIPCION_ID)).v_Value1;
            string ApaLocomotorHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATO_LOCOMOTOR_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_APARATO_LOCOMOTOR_ID)).v_Value1;
            if (ApaLocomotorHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) ApaLocomotorX = "X";


            string MarchaX = "";
            string Marcha = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_MARCHA_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_MARCHA_DESCRIPCION_ID)).v_Value1;
            string MarchaHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_MARCHA_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_MARCHA_ID)).v_Value1;
            if (MarchaHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) MarchaX = "X";


            string ColumnaX = "";
            string Columna = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_COLUMNA_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_COLUMNA_DESCRIPCION_ID)).v_Value1;
            string ColumnaHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_COLMNA_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_COLMNA_ID)).v_Value1;
            if (ColumnaHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) ColumnaX = "X";


            string SuperioresX = "";
            string Superiores = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_EXTREMIDADES_SUPERIORES_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_EXTREMIDADES_SUPERIORES_DESCRIPCION_ID)).v_Value1;
            string SuperioresHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_EXTREMIDADE_SUPERIORES_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_EXTREMIDADE_SUPERIORES_ID)).v_Value1;
            if (SuperioresHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) SuperioresX = "X";


            string InferioresX = "";
            string Inferiores = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_EXTREMIDADES_INFERIORES_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_EXTREMIDADES_INFERIORES_DESCRIPCION_ID)).v_Value1;
            string InferioresHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_EXTREMIDADES_INFERIORES_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_EXTREMIDADES_INFERIORES_ID)).v_Value1;
            if (InferioresHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) InferioresX = "X";


            string SistemaLinfaticoX = "";
            string SistemaLinfatico = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_LINFATICOS_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_LINFATICOS_DESCRIPCION_ID)).v_Value1;
            string SistemaLinfaticoHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_LINFATICOS_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_LINFATICOS_ID)).v_Value1;
            if (SistemaLinfaticoHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) SistemaLinfaticoX = "X";


            string SistemaNerviosoX = "";
            string SistemaNervioso = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_SISTEMA_NERVIOSO_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_SISTEMA_NERVIOSO_DESCRIPCION_ID)).v_Value1;
            string SistemaNerviosoHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_SISTEMA_NERVIOSO_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_SISTEMA_NERVIOSO_ID)).v_Value1;
            if (SistemaNerviosoHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) SistemaNerviosoX = "X";



            string Hallazgos = Oftalmologia.Count() == 0 || ((ServiceComponentFieldValuesList)Oftalmologia.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.OFTALMOLOGIA_HALLAZGOS_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)Oftalmologia.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.OFTALMOLOGIA_HALLAZGOS_ID)).v_Value1;
         
            //TEST DE ESTEREOPSIS:Frec. 10 seg/arc, Normal.
            string TestIshi = VisionColor.Count() == 0 || ((ServiceComponentFieldValuesList)VisionColor.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.VISION_DE_COLORES_TEST_DE_ISHIHARA_SELECCIONAR)) == null ? string.Empty : ((ServiceComponentFieldValuesList)VisionColor.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.VISION_DE_COLORES_TEST_DE_ISHIHARA_SELECCIONAR)).v_Value1Name;

            string TestEstereopsis = VisionEstero.Count() == 0 || ((ServiceComponentFieldValuesList)VisionEstero.Find(p => p.v_ComponentFieldId == "N002-MF000000685")) == null ? string.Empty : ((ServiceComponentFieldValuesList)VisionEstero.Find(p => p.v_ComponentFieldId == "N002-MF000000685")).v_Value1Name;
            string TestEstereopsis1 = VisionEstero.Count() == 0 || ((ServiceComponentFieldValuesList)VisionEstero.Find(p => p.v_ComponentFieldId == "N009-MF000001580")) == null ? string.Empty : ((ServiceComponentFieldValuesList)VisionEstero.Find(p => p.v_ComponentFieldId == "N009-MF000001580")).v_Value1Name;
           

            string OjoAnexoX = "";
            string OjoAnexo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_OJOSANEXOS_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_OJOSANEXOS_DESCRIPCION_ID)).v_Value1;
            string OjoAnexoHallazgo = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_OJOSANEXOS_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_OJOSANEXOS_ID)).v_Value1;

            //ServiceComponentList findOftalmologia = ExamenesServicio.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.OFTALMOLOGIA_ID);
           
            string ValorOD_VC_SC = "", ValorOI_VC_SC = "", ValorOD_VC_CC = "", ValorOI_VC_CC = "";
            string ValorOD_VL_SC = "", ValorOI_VL_SC = "", ValorOD_VL_CC = "", ValorOI_VL_CC = "";

            if (Oftalmologia_UC.Count !=0)
            {
                ValorOD_VC_SC = (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_SC_ODC)) == null ? string.Empty : (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_SC_ODC).v_Value1);
                ValorOI_VC_SC = (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_SC_OIC)) == null ? string.Empty : (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_SC_OIC).v_Value1);
                ValorOD_VC_CC = (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_CC_ODC)) == null ? string.Empty : (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_CC_ODC).v_Value1);
                ValorOI_VC_CC = (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_CC_OIC)) == null ? string.Empty : (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_CC_OIC).v_Value1);

                ValorOD_VL_SC = (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_SC_OD)) == null ? string.Empty : (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_SC_OD).v_Value1);
                ValorOI_VL_SC = (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_SC_OI)) == null ? string.Empty : (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_SC_OI).v_Value1);
                ValorOD_VL_CC = (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_CC_OD)) == null ? string.Empty : (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_CC_OD).v_Value1);
                ValorOI_VL_CC = (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_CC_OI)) == null ? string.Empty : (Oftalmologia_UC.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_CC_OI).v_Value1);
            }
          
       
            if (OjoAnexoHallazgo == ((int)Sigesoft.Common.NormalAlteradoHallazgo.SinHallazgos).ToString()) OjoAnexoX = "X";

            string HallazgosExFisico = ExamenFisco.Count() == 0 || ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_HALLAZGOS_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)ExamenFisco.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.EXAMEN_FISICO_HALLAZGOS_ID)).v_Value1;

            cells = new List<PdfPCell>()
               {
                     //fila
                    new PdfPCell(new Phrase("Síntomas Act.", fontColumnValue)),
                    new PdfPCell(new Phrase(DataService.v_Story, fontColumnValue))
                                    { Colspan=11, HorizontalAlignment = PdfPCell.ALIGN_LEFT},                     
                    //fila
                    new PdfPCell(new Phrase("Examen Clínico", fontColumnValue))
                                    { Rowspan = 3, HorizontalAlignment = PdfPCell.ALIGN_LEFT},             
                    new PdfPCell(new Phrase("Talla(m)", fontColumnValue)),                                        
                    new PdfPCell(new Phrase(Talla, fontColumnValue))
                                    { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase("Peso (kg.)", fontColumnValue)){ Colspan = 2},
                    new PdfPCell(new Phrase(Peso, fontColumnValue)){ Colspan = 2},
                     new PdfPCell(new Phrase("IMC", fontColumnValue)),                                   
                    new PdfPCell(new Phrase(IMC == "0.00" ? " " :IMC, fontColumnValue)),                                               
                    new PdfPCell(new Phrase("ICC", fontColumnValue))
                                     { HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase(ICC == "0.00" ? " " : ICC, fontColumnValue)),

                     //fila                                                
                    new PdfPCell(new Phrase("Frecuencia Respiratoria            (resp. x min)", fontColumnValue))
                                             { Colspan=2, HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase(FrecResp, fontColumnValue)),
                                  
                    new PdfPCell(new Phrase("Frecuencia Cardiaca (lat x min)", fontColumnValue))
                                    { Colspan=2, HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase(frecCard, fontColumnValue)),
                                    
                     new PdfPCell(new Phrase("Presión Arterial (mmHg)", fontColumnValue))
                                  { Colspan=2, HorizontalAlignment = PdfPCell.ALIGN_LEFT},         
                    new PdfPCell(new Phrase(PAS + " / " + PAD , fontColumnValue)), 
                                              
                    new PdfPCell(new Phrase("Saturación Oxígeno", fontColumnValue)),
                    new PdfPCell(new Phrase(SaturacionOxigeno, fontColumnValue)),
                    //new PdfPCell(new Phrase(SaturacionOxigeno == "0.00" || SaturacionOxigeno == "0,00" || string.IsNullOrEmpty(SaturacionOxigeno) ? "" : double.Parse(SaturacionOxigeno).ToString("#.#"), fontColumnValue)),


                     //fila                                                
                    new PdfPCell(new Phrase("Otros", fontColumnValue)),         
                    new PdfPCell(new Phrase(ConcatenadoOtros, fontColumnValue))
                                     { Colspan = 10, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                    //fila                                                
                    new PdfPCell(new Phrase("Examen Físico", fontColumnValue))
                                      { Colspan=12, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                    //fila    
                    new PdfPCell(new Phrase("Estoscopia", fontColumnValue)),                        
                    new PdfPCell(new Phrase(Estoscopia == "" || string.IsNullOrEmpty(Estoscopia) ? "ABEG ( X )    ABEH ( X )    ABEN ( X )" : Estoscopia, fontColumnValue))
                                      { Colspan=11, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                    //fila                                                
                     new PdfPCell(new Phrase("Estado Mental", fontColumnValue)),                      
                    new PdfPCell(new Phrase(Estado_Mental == "" || string.IsNullOrEmpty(Estado_Mental) ? "LUCIDO, ORIENTADO EN TIEMPO ESPACIO Y PERSONA" : Estado_Mental, fontColumnValue))
                                      { Colspan=11, HorizontalAlignment = PdfPCell.ALIGN_LEFT},


                    //fila                                                
                     new PdfPCell(new Phrase("Organo o Sistema", fontColumnValue)),                        
                    new PdfPCell(new Phrase("Sin Hallaz.", fontColumnValue)),
                    new PdfPCell(new Phrase("Hallazgos", fontColumnValue))
                                      { Colspan=10, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                    //fila                                                
                     new PdfPCell(new Phrase("Piel", fontColumnValue)),                       
                    new PdfPCell(new Phrase(PielX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(Piel, fontColumnValue))
                                      { Colspan=10, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                    //fila                                                
                     new PdfPCell(new Phrase("Cabellos", fontColumnValue)),                       
                    new PdfPCell(new Phrase(CabelloX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(Cabello, fontColumnValue))
                                      { Colspan=10, HorizontalAlignment = PdfPCell.ALIGN_LEFT}, 

                                         //fila                                                
                     new PdfPCell(new Phrase("Ojos y Anexos", fontColumnValue))
                                { Rowspan=7, HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase(OjoAnexoX, fontColumnValue))
                                { Rowspan=7, HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase("Hallazgos", fontColumnValue))
                                { Colspan=2, HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase(Hallazgos, fontColumnValue))
                                      { Colspan=9, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                    //fila

                    new PdfPCell(new Phrase("AGUDEZA VISUAL", fontColumnValue)){Rowspan=2, Colspan=2, HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase("Sin corregir", fontColumnValue)){Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase("Corregida", fontColumnValue)){Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                

                    //Linea
                    //linea en blanco
                    new PdfPCell(new Phrase("O.D", fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase("O.I", fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase("O.D", fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase("O.I", fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_CENTER},

                 

                    //Linea
                    new PdfPCell(new Phrase("VISIÓN DE CERCA",fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(DEvolver262string(ValorOD_VC_SC), fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(DEvolver262string(ValorOI_VC_SC), fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(DEvolver262string(ValorOD_VC_CC), fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(DEvolver262string(ValorOI_VC_CC), fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_CENTER},

                    //Linea
                    new PdfPCell(new Phrase("VISIÓN DE LEJOS", fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_RIGHT},
                    new PdfPCell(new Phrase(DEvolver237string(ValorOD_VL_SC), fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(DEvolver237string(ValorOI_VL_SC), fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(DEvolver237string(ValorOD_VL_CC), fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(DEvolver237string(ValorOI_VL_CC), fontColumnValue)){Colspan=2,HorizontalAlignment = PdfPCell.ALIGN_CENTER},

                        //fila
                    new PdfPCell(new Phrase("Visión de Profundidad", fontColumnValue))
                                { Colspan=2, HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase("Test de Estereopsis:   " + TestEstereopsis + " / " +TestEstereopsis1 , fontColumnValue))
                                { Colspan=3, HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase("Visión de Colores", fontColumnValue))
                                    { Colspan=2, HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                    new PdfPCell(new Phrase("Test de ISHIHARA: " + TestIshi, fontColumnValue))
                                { Colspan=3, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                    //fila
                    new PdfPCell(new Phrase("Hallazgos", fontColumnValue))
                                { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase("", fontColumnValue))
                                { HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase("Sin Hallaz.", fontColumnValue))
                                { HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase("Hallazgos", fontColumnValue))
                                { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_CENTER},

                    //fila                                                
                     new PdfPCell(new Phrase("Oido", fontColumnValue)),                   
                    new PdfPCell(new Phrase(OidoX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(Oido, fontColumnValue))
                                      { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},       
                     new PdfPCell(new Phrase("Nariz", fontColumnValue)),  
                    new PdfPCell(new Phrase(NarizX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(Nariz, fontColumnValue))
                                      { Colspan=4708, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                                         //fila                                                
                     new PdfPCell(new Phrase("Boca", fontColumnValue)),                        
                    new PdfPCell(new Phrase(BocaX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(Boca, fontColumnValue))
                                      { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},                                               
                     new PdfPCell(new Phrase("Faringe", fontColumnValue)),                        
                    new PdfPCell(new Phrase(FaringeX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(Faringe, fontColumnValue))
                                      { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                                         //fila                                                
                     new PdfPCell(new Phrase("Cuello", fontColumnValue)),                       
                    new PdfPCell(new Phrase(CuelloX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(Cuello, fontColumnValue))
                                      { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},                                           
                     new PdfPCell(new Phrase("Aparato Respiratorio", fontColumnValue)),                  
                    new PdfPCell(new Phrase(ApaRespiratorioX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(ApaRespiratorio, fontColumnValue))
                                      { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                                         //fila                                                
                     new PdfPCell(new Phrase("Aparato Cardiovascular", fontColumnValue)),                        
                    new PdfPCell(new Phrase(ApaCardioVascularX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(ApaCardioVascular, fontColumnValue))
                                      { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},                                           
                     new PdfPCell(new Phrase("Aparato Digestivo", fontColumnValue)),                  
                    new PdfPCell(new Phrase(ApaDigestivoX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(ApaDigestivo, fontColumnValue))
                                      { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                                       //fila                                                
                     new PdfPCell(new Phrase("Aparato Genitourinario", fontColumnValue)),                      
                    new PdfPCell(new Phrase(ApaGenitoUrinarioX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(ApaGenitoUrinario, fontColumnValue))
                                      { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},                                               
                     new PdfPCell(new Phrase("Aparato Locomotor", fontColumnValue)),                     
                    new PdfPCell(new Phrase(ApaLocomotorX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(ApaLocomotor, fontColumnValue))
                                      { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                                       //fila                                                
                     new PdfPCell(new Phrase("Marcha", fontColumnValue)),                        
                    new PdfPCell(new Phrase(MarchaX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(Marcha, fontColumnValue))
                                      { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},                                               
                     new PdfPCell(new Phrase("Columna", fontColumnValue)),                      
                    new PdfPCell(new Phrase(ColumnaX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(Columna, fontColumnValue))
                                      { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                                       //fila                                                
                     new PdfPCell(new Phrase("Miembros Superiores", fontColumnValue)),                        
                    new PdfPCell(new Phrase(SuperioresX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(Superiores, fontColumnValue))
                                      { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},                                                
                     new PdfPCell(new Phrase("Miembros Inferiores", fontColumnValue)),                        
                    new PdfPCell(new Phrase(InferioresX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    new PdfPCell(new Phrase(Inferiores, fontColumnValue))
                                      { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

                                       //fila                                                
                    // new PdfPCell(new Phrase("Sistema Linfático", fontColumnValue)),
                    // new PdfPCell(new Phrase(SistemaLinfaticoX, fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_CENTER },
                    //new PdfPCell(new Phrase(SistemaLinfatico, fontColumnValue))
                    //                  { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT},                                               
                    // new PdfPCell(new Phrase("Sistema Nervioso", fontColumnValue)),                        
                    //new PdfPCell(new Phrase(SistemaNerviosoX, fontColumnValue)){HorizontalAlignment = PdfPCell.ALIGN_CENTER},
                    //new PdfPCell(new Phrase(SistemaNervioso, fontColumnValue))
                    //                  { Colspan=4, HorizontalAlignment = PdfPCell.ALIGN_LEFT}

                    //fila    
                    new PdfPCell(new Phrase("Resumen de Hallazgos", fontColumnValue)),                        
                    new PdfPCell(new Phrase(HallazgosExFisico == "" || string.IsNullOrEmpty(HallazgosExFisico) ? "Evaluación sin Hallazgos significativos" : HallazgosExFisico, fontColumnValue))
                                      { Colspan=11, HorizontalAlignment = PdfPCell.ALIGN_LEFT},

               };

            columnWidths = new float[] { 14f, 11f, 9f, 9f, 9f, 9f, 12f, 12f, 9f, 10f, 13f, 7f };

            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "VI. EVALUACIÓN MÉDICA (llenar con letra clara o marcar con una 'X')", fontTitleTable);

            document.Add(filiationWorker);
            #endregion

            #region Evaluación Psicológicas

            //PSICOLOGIA
            string conclusionPsico = Psicologia.Count() == 0 || ((ServiceComponentFieldValuesList)Psicologia.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.PSICOLOGIA_CELIMA_CONCLUSIONES)) == null ? string.Empty : ((ServiceComponentFieldValuesList)Psicologia.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.PSICOLOGIA_CELIMA_CONCLUSIONES)).v_Value1;
            //string AreaEmocional = Psicologia.Count() == 0 || ((ServiceComponentFieldValuesList)Psicologia.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.PSICOLOGIA_AREA_EMOCIONAL_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)Psicologia.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.PSICOLOGIA_AREA_EMOCIONAL_ID)).v_Value1Name;
            //string PsicologiaConclusiones = Psicologia.Count() == 0 || ((ServiceComponentFieldValuesList)Psicologia.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.PSICOLOGIA_AREA_CONCLUSIONES_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)Psicologia.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.PSICOLOGIA_AREA_CONCLUSIONES_ID)).v_Value1;

            //List<ServiceComponentList> ListaExamenesPsicologicos = ExamenesServicio.FindAll(p => p.i_CategoryId == 7).ToList();
            //string DiagnosticosPsicologicos = "";

            //foreach (var item in ListaExamenesPsicologicos)
            //{
            //    foreach (var item1 in ListDiagnosticRepository)
            //    {
            //        if (item1.v_ComponentId == item.v_ComponentId)
            //        {
            //            DiagnosticosPsicologicos = item1.v_DiseasesName + ";";
            //        }
            //    }
            //}

            //DiagnosticosPsicologicos = DiagnosticosPsicologicos == "" ? string.Empty : DiagnosticosPsicologicos.Substring(0, DiagnosticosPsicologicos.Length - 1);
            //cells = new List<PdfPCell>();

            //if (Psicologia.Count() != 0)
            //{
                cells = new List<PdfPCell>()
                  {
                       //fila
                        new PdfPCell(new Phrase(conclusionPsico , fontColumnValue)), 
                         //fila
                        //new PdfPCell(new Phrase("Hallazgos: " + DiagnosticosPsicologicos , fontColumnValue)), 

                  };

                columnWidths = new float[] { 100f };
            //}
            //else
            //{
            //    if (ListaExamenesPsicologicos != null)
            //    {
            //        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
            //    }
            //    else
            //    {
            //        cells.Add(new PdfPCell(new Phrase("Este examen No aplica al protocolo de atención.", fontColumnValue)));
            //    }

            //    //cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
            //    columnWidths = new float[] { 100f };
            //}


            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "VII. CONCLUSIONES DE EVALUACIÓN PSICOLÓGICA", fontTitleTable);

            document.Add(filiationWorker);

            #endregion

            #region Conclusiones Radiográficas

            //RX
            string ConclusionesOIT = OIT.Count() == 0 || OIT.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.RX_CONCLUSIONES_OIT_ID) == null ? string.Empty : OIT.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.RX_CONCLUSIONES_OIT_ID).v_Value1Name;
            string ConclusionesOITDescripcion = OIT.Count() == 0 || ((ServiceComponentFieldValuesList)OIT.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.RX_CONCLUSIONES_OIT_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)OIT.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.RX_CONCLUSIONES_OIT_DESCRIPCION_ID)).v_Value1;
            
            string ConclusionesRadiografica = RX.Count() == 0 || ((ServiceComponentFieldValuesList)RX.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.RX_CONCLUSIONES_RADIOGRAFICAS_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)RX.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.RX_CONCLUSIONES_RADIOGRAFICAS_ID)).v_Value1Name;
            string ConclusionesRadiograficaDescripcion = RX.Count() == 0 || ((ServiceComponentFieldValuesList)RX.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.RX_CONCLUSIONES_RADIOGRAFICAS_DESCRIPCION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)RX.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.RX_CONCLUSIONES_RADIOGRAFICAS_DESCRIPCION_ID)).v_Value1;
            string ExposicionPolvo = OIT.Count() == 0 || OIT.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.RX_EXPOSICION_POLVO_ID) == null ? string.Empty : OIT.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.RX_EXPOSICION_POLVO_ID).v_Value1Name;
            string ExposicionPolvoDescripcion = OIT.Count() == 0 || OIT.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.RX_EXPOSICION_POLVO_DESCRIPCION_ID) == null ? string.Empty : OIT.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.RX_EXPOSICION_POLVO_DESCRIPCION_ID).v_Value1;


            List<ServiceComponentList> ListaExamenesRX = ExamenesServicio.FindAll(p => p.i_CategoryId == 6).ToList();
            string DiagnosticosRx = "";

            foreach (var item in ListaExamenesRX)
            {
                foreach (var item1 in ListDiagnosticRepository)
                {
                    if (item1.v_ComponentId == item.v_ComponentId)
                    {
                        DiagnosticosRx = item1.v_DiseasesName + ";";
                    }
                }
            }

            DiagnosticosRx = DiagnosticosRx == "" ? string.Empty : DiagnosticosRx.Substring(0, DiagnosticosRx.Length - 1);

            // Alejandro
            cells = new List<PdfPCell>();

            var xConcluOIT = string.Empty;
         
            if (!string.IsNullOrEmpty(ConclusionesOIT))
            {              
                xConcluOIT = ConclusionesOIT;
            }

            if (!string.IsNullOrEmpty(ConclusionesOITDescripcion))
            {               
                xConcluOIT += ", " + ConclusionesOITDescripcion;               
            }
        
            if (!string.IsNullOrEmpty(xConcluOIT))
            {
                 cells.Add(new PdfPCell(new Phrase("Conclusiones OIT: " + xConcluOIT, fontColumnValue)));
            }

            if (!string.IsNullOrEmpty(ExposicionPolvoDescripcion))
            {
                cells.Add(new PdfPCell(new Phrase("Exposicion al polvo: " + ExposicionPolvoDescripcion, fontColumnValue)));
            }

            if (!string.IsNullOrEmpty(ConclusionesRadiograficaDescripcion))
            {
                cells.Add(new PdfPCell(new Phrase(ConclusionesRadiograficaDescripcion, fontColumnValue)));
            }

            if (string.IsNullOrEmpty(xConcluOIT) 
                && string.IsNullOrEmpty(ExposicionPolvoDescripcion)
                && string.IsNullOrEmpty(ConclusionesRadiograficaDescripcion))
            {
                cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
            }

            ////if (OIT.Count() != 0 && RX.Count() != 0)
            //if (RX.Count() != 0)
            //{
            //    //cells = new List<PdfPCell>()
            //    //    {
            //    //        //fila
            //    //        new PdfPCell(new Phrase("Conclusiones Radiograficas : " + ConclusionesRadiograficaDescripcion, fontColumnValue)), 
                    
            //    //        //fila                    
            //    //        //new PdfPCell(new Phrase("Hallazgos : " + DiagnosticosRx, fontColumnValue)), 	                                           
            //    //    };

            //    cells.Add(new PdfPCell(new Phrase("(RX) Torax : " + ConclusionesRadiograficaDescripcion, fontColumnValue)));

            //    //columnWidths = new float[] { 100f };
            //}
            //else
            //{
            //    if (ListaExamenesRX != null)
            //    {
            //        cells.Add(new PdfPCell(new Phrase("(RX) Torax : No se han registrado datos.", fontColumnValue)));
            //    }
            //    else
            //    {
            //        cells.Add(new PdfPCell(new Phrase("Este examen no aplica al protocolo de atención.", fontColumnValue)));
            //    }

            //    //columnWidths = new float[] { 100f };
            //}

            columnWidths = new float[] { 100f };
            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "VIII. CONCLUSIONES RADIOGRÁFICAS", fontTitleTable);

            document.Add(filiationWorker);

            #endregion

            #region Hallazgos Patológicos de Laboratorio

            //RX
            //string ConclusionesLaboratorio = Laboratorio.Count() == 0 ? string.Empty : Laboratorio.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.LABORATORIO_HALLAZGO_PATOLOGICO_LABORATORIO_ID).v_Value1;
            //var  __ConclusionesLaboratorio = Laboratorio.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.LABORATORIO_HALLAZGO_PATOLOGICO_LABORATORIO_ID);


            List<ServiceComponentList> ListaExamenesLaboratorio = ExamenesServicio.FindAll(p => p.i_CategoryId == 1).ToList();
            string DiagnosticosLaboratorio = "";

            foreach (var item in ListaExamenesLaboratorio)
            {
                foreach (var item1 in ListDiagnosticRepository)
                {
                    if (item1.v_ComponentId == item.v_ComponentId)
                    {
                        DiagnosticosLaboratorio = item1.v_DiseasesName + " ";
                    }
                }
            }

            DiagnosticosLaboratorio = DiagnosticosLaboratorio.Trim() == "" ? "Hallazgos dentro de rangos Normales" : DiagnosticosLaboratorio.Substring(0, DiagnosticosLaboratorio.Length - 1);


            cells = new List<PdfPCell>()
                    {
                        //fila
                        new PdfPCell(new Phrase(DiagnosticosLaboratorio, fontColumnValue)),
                      
                    };

            columnWidths = new float[] { 100f };


            //string vvv = "";

            //if (!string.IsNullOrEmpty(filiationData.v_BloodGroupName))
            //{
            //    vvv = string.Format("Grupo y factor sanguineo : \"{0}\" - {1}", filiationData.v_BloodGroupName, filiationData.v_BloodFactorName);
            //}

            //if (Laboratorio.Count() != 0)
            //{
            //    cells = new List<PdfPCell>()
            //        {
            //            //fila
            //            new PdfPCell(new Phrase(vvv, fontColumnValue)),
            //            //fila
            //            new PdfPCell(new Phrase(ConclusionesLaboratorio, fontColumnValue)), 
            //            //fila
            //            //new PdfPCell(new Phrase("Hallazgos : " + DiagnosticosLaboratorio, fontColumnValue)), 
            //        };

            //    columnWidths = new float[] { 100f };
            //}
            //else
            //{
            //    if (ListaExamenesLaboratorio != null)
            //    {
            //        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
            //    }
            //    else
            //    {
            //        cells.Add(new PdfPCell(new Phrase("Este examen no aplica al protocolo de atención.", fontColumnValue)));
            //    }
            //    columnWidths = new float[] { 100f };
            //}


            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "IX. HALLAZGOS DE LABORATORIO", fontTitleTable);

            document.Add(filiationWorker);

            #endregion

            #region Conclusión Audiometría

            // Verificar si el examen esta contenida en el protocolo
            var existeAudio = ExamenesServicio.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.AUDIOMETRIA_ID);
            cells = new List<PdfPCell>();

            if (existeAudio != null) // El examen esta contemplado en el protocolo del paciente
            {
                //Audiometria
                //string ConclusionesAudiometria = Audiometria.Count() == 0 || ((ServiceComponentFieldValuesList)Audiometria.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.AUDIOMETRIA_CONCLUSIONES_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)Audiometria.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.AUDIOMETRIA_CONCLUSIONES_ID)).v_Value1;
                string ConclusionesAudiometria = Audiometria;
                var ListaAudioMetriaDx = ListDiagnosticRepository.FindAll(p => p.v_ComponentId == Sigesoft.Common.Constants.AUDIOMETRIA_ID);
                string DiagnosticoAudiometria = "";

                foreach (var item in ListaAudioMetriaDx)
                {
                    DiagnosticoAudiometria = item.v_DiseasesName + ";";
                }

                DiagnosticoAudiometria = DiagnosticoAudiometria == "" ? string.Empty : DiagnosticoAudiometria.Substring(0, DiagnosticoAudiometria.Length - 1);
                //cells = new List<PdfPCell>();

                if (ConclusionesAudiometria != "")
                {
                    cells = new List<PdfPCell>()
                        {
                            //fila
                            new PdfPCell(new Phrase(ConclusionesAudiometria, fontColumnValue)), 
                            //fila
                            //new PdfPCell(new Phrase("Hallazgos : " + DiagnosticoAudiometria, fontColumnValue)), 
                        };

                    columnWidths = new float[] { 100f };
                }
                else
                {
                    cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                    columnWidths = new float[] { 100f };
                }
            }
            else
            {
                cells.Add(new PdfPCell(new Phrase("Este examen No aplica al protocolo de atención.", fontColumnValue)));
            }

            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "X. CONCLUSIONES AUDIOMETRÍA", fontTitleTable);

            document.Add(filiationWorker);

            #endregion

            #region Conclusión Espirometría

            // Verificar si el examen esta contenida en el protocolo
            var existeEspiro = ExamenesServicio.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.ESPIROMETRIA_ID);
            cells = new List<PdfPCell>();

            if (existeEspiro != null) // El examen esta contemplado en el protocolo del paciente
            {
                //ESPIROMETRIA
                string ResultadoEspirometria = Espirometria.Count() == 0 || ((ServiceComponentFieldValuesList)Espirometria.Find(p => p.v_ComponentFieldId == "N003-MF000000016")) == null ? string.Empty : ((ServiceComponentFieldValuesList)Espirometria.Find(p => p.v_ComponentFieldId == "N003-MF000000016")).v_Value1;
                //string ObservacionEspirometria = Espirometria.Count() == 0 || ((ServiceComponentFieldValuesList)Espirometria.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.ESPIROMETRIA_OBSERVACION_ID)) == null ? string.Empty : ((ServiceComponentFieldValuesList)Espirometria.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.ESPIROMETRIA_OBSERVACION_ID)).v_Value1;

                //var ListaEspirometriaDx = ListDiagnosticRepository.FindAll(p => p.v_ComponentId == Sigesoft.Common.Constants.ESPIROMETRIA_ID);
                //string DiagnosticoEspirometria = "";

                //foreach (var item in ListaEspirometriaDx)
                //{
                //    DiagnosticoEspirometria = item.v_DiseasesName + ";";
                //}

                //DiagnosticoEspirometria = DiagnosticoEspirometria == "" ? string.Empty : DiagnosticoEspirometria.Substring(0, DiagnosticoEspirometria.Length - 1);
                //cells = new List<PdfPCell>();

                if (Espirometria.Count() != 0)
                {
                    cells = new List<PdfPCell>()
                        {
                           //fila
                            new PdfPCell(new Phrase(ResultadoEspirometria , fontColumnValue)), 
                               //fila
                            //new PdfPCell(new Phrase("Hallazgos: " + DiagnosticoEspirometria, fontColumnValue)), 
                        };

                    columnWidths = new float[] { 100f };
                }
                else
                {
                    cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                    columnWidths = new float[] { 100f };
                }
            }
            else
            {
                cells.Add(new PdfPCell(new Phrase("Este examen No aplica al protocolo de atención.", fontColumnValue)));
            }

            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "XI. CONCLUSIONES DE ESPIROMETRÍA", fontTitleTable);

            document.Add(filiationWorker);
            #endregion

            #region Otros

            string[] excludeComponents = {   Sigesoft.Common.Constants.PSICOLOGIA_ID,
                                                 Sigesoft.Common.Constants.RX_ID,
                                                 Sigesoft.Common.Constants.LABORATORIO_ID ,
                                                 Sigesoft.Common.Constants.AUDIOMETRIA_ID,
                                                 Sigesoft.Common.Constants.ESPIROMETRIA_ID,
                                                 Sigesoft.Common.Constants.ANTROPOMETRIA_ID,
                                                 Sigesoft.Common.Constants.FUNCIONES_VITALES_ID,
                                                 Sigesoft.Common.Constants.EXAMEN_FISICO_ID,
                                                 Sigesoft.Common.Constants.OFTALMOLOGIA_ID,
                                                 Sigesoft.Common.Constants.OIT_ID
                                             };

            int?[] excludeCategoryTypeExam = {  (int)Sigesoft.Common.CategoryTypeExam.Laboratorio,
                                                   (int)Sigesoft.Common.CategoryTypeExam.Psicologia,
                                                
                                                };
            //

            var otherExams = ExamenesServicio.FindAll(p => !excludeComponents.Contains(p.v_ComponentId) &&
                                                           !excludeCategoryTypeExam.Contains(p.i_CategoryId));

            // Utilizado Solo para mostrar titulo <OTROS>
            cells = new List<PdfPCell>()
            {

            };

            columnWidths = new float[] { 100f };

            table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "XII. OTROS", fontTitleTable);
            document.Add(table);

            // Otros Examenes

            foreach (var oe in otherExams)
            {
                table = TableBuilderReportFor312(oe, fontTitleTable, fontSubTitleNegroNegrita, fontColumnValue, subTitleBackGroundColor);

                if (table != null)
                    document.Add(table);
            }

            #endregion

            #region DIAGNÓSTICO MÉDICO OCUPACIONAL

            var DxOcupacionales = ListDiagnosticRepository.FindAll(p => p.i_DiagnosticTypeId == (int)Sigesoft.Common.TipoDx.Enfermedad_Ocupacional || p.i_DiagnosticTypeId == (int)Sigesoft.Common.TipoDx.Accidente_Ocupacional);

            cells = new List<PdfPCell>();

            cells = new List<PdfPCell>()
                  {
                       //fila
                        new PdfPCell(new Phrase(" ", fontTitleTable)){ HorizontalAlignment = PdfPCell.ALIGN_CENTER}, 
                        new PdfPCell(new Phrase("P", fontColumnValue)), 
                        new PdfPCell(new Phrase("D", fontColumnValue)), 
                        new PdfPCell(new Phrase("R", fontColumnValue)), 
                        new PdfPCell(new Phrase("CIE-10", fontTitleTable)) { HorizontalAlignment = PdfPCell.ALIGN_CENTER}, 
                    
                  };


            if (DxOcupacionales != null && DxOcupacionales.Count > 0)
            {
                columnWidths = new float[] { 5f, 7f };

                foreach (var item in DxOcupacionales)
                {
                    cell = new PdfPCell(new Phrase(item.v_DiseasesName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_LEFT };
                    cells.Add(cell);


                    if (item.i_FinalQualificationId == (int)Sigesoft.Common.FinalQualification.Presuntivo)
                    {
                        cell = new PdfPCell(new Phrase("X", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);

                        cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);


                        cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);


                        cell = new PdfPCell(new Phrase(item.v_Cie10, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);
                    }
                    else if (item.i_FinalQualificationId == (int)Sigesoft.Common.FinalQualification.Definitivo)
                    {
                        cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);

                        cell = new PdfPCell(new Phrase("X", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);


                        cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);


                        cell = new PdfPCell(new Phrase(item.v_Cie10, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);
                    }

                    else if (item.i_FinalQualificationId == (int)Sigesoft.Common.FinalQualification.Descartado)
                    {
                        cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);

                        cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);


                        cell = new PdfPCell(new Phrase("X", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);


                        cell = new PdfPCell(new Phrase(item.v_Cie10, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_LEFT };
                        cells.Add(cell);
                    }
                }
            }
            else
            {
                cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)) { Colspan = 5, HorizontalAlignment = PdfPCell.ALIGN_LEFT });
                columnWidths = new float[] { 50f, 3f, 3f, 3f, 20f };
            }
            columnWidths = new float[] { 50f, 3f, 3f, 3f, 20f };

            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "XIII. DIAGNÓSTICOS MÉDICO OCUPACIONALES", fontTitleTable);

            document.Add(filiationWorker);
            #endregion

            document.NewPage();

            #region Otros Dx

            var DxOtros = ListDiagnosticRepository.FindAll(p => p.i_DiagnosticTypeId != (int)Sigesoft.Common.TipoDx.Accidente_Ocupacional &&
                                                            p.i_DiagnosticTypeId != (int)Sigesoft.Common.TipoDx.Enfermedad_Ocupacional &&
                                                            p.i_DiagnosticTypeId != null);
         
            cells = new List<PdfPCell>()
            {

                new PdfPCell(new Phrase("Otros Diagnósticos", fontTitleTable)) { HorizontalAlignment = PdfPCell.ALIGN_CENTER}, 
                new PdfPCell(new Phrase("P", fontColumnValue)), 
                new PdfPCell(new Phrase("D", fontColumnValue)), 
                new PdfPCell(new Phrase("R", fontColumnValue)), 
                new PdfPCell(new Phrase("CIE-10", fontTitleTable)) {  HorizontalAlignment = PdfPCell.ALIGN_CENTER}, 

            };

            columnWidths = new float[] { 50f, 3f, 3f, 3f, 20f };
            table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "", fontTitleTable);
            document.Add(table);

            cells = new List<PdfPCell>();

            if (DxOtros != null && DxOtros.Count > 0)
            {
                //columnWidths = new float[] { 5f, 7f };
                columnWidths = new float[] { 50f, 3f, 3f, 3f, 20f };

                // Alejandro
                // arreglo de Dx normales
                string[] dxExclude = {  Sigesoft.Common.Constants.NORMOACUSIA_OIDO_DERECHO,
                                        Sigesoft.Common.Constants.NORMOACUSIA_OIDO_IZQUIERDO,
                                        Sigesoft.Common.Constants.EMETROPE,
                                        Sigesoft.Common.Constants.NORMOPESO,
                                        Sigesoft.Common.Constants.NORMOACUSIA,
                                        //Sigesoft.Common.Constants.NORMOACUSIA_UC_AUDIOMETRIA_OIDO_IZQUIERDO,
                                        //Sigesoft.Common.Constants.NORMOACUSIA_UC_AUDIOMETRIA_OIDO_DERECHO
                                     };

                var rowCountDxOtros = DxOtros.Count;

                foreach (var item in DxOtros)
                {
                    var indDxOtros = DxOtros.IndexOf(item) + 1;

                    if (indDxOtros == rowCountDxOtros) // Ultimo registro de la lista pintar una linea debajo
                    {
                        cell = new PdfPCell(new Phrase(indDxOtros + "  " + item.v_DiseasesName, fontColumnValue)) { Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_LEFT };
                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase(indDxOtros + "  " + item.v_DiseasesName, fontColumnValue)) { Border = PdfPCell.LEFT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_LEFT };
                    }
                
                    cells.Add(cell);

                    // Alejandro
                    // Verificar si es un dx de normalidad
                    string newDx = string.Empty;

                    if (!(dxExclude.Contains(item.v_DiseasesId)))
                    {
                        
                        newDx = item.v_Cie10;
                    }

                    if (item.i_FinalQualificationId == (int)Sigesoft.Common.FinalQualification.Presuntivo)
                    {
                        cell = new PdfPCell(new Phrase("X", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);

                        cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);

                        cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);

                        cell = new PdfPCell(new Phrase(newDx, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);
                    }
                    else if (item.i_FinalQualificationId == (int)Sigesoft.Common.FinalQualification.Definitivo)
                    {
                        cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);

                        cell = new PdfPCell(new Phrase("X", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);


                        cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);


                        cell = new PdfPCell(new Phrase(newDx, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);
                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);

                        cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);


                        cell = new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);


                        cell = new PdfPCell(new Phrase(newDx, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                        cells.Add(cell);
                    }
                }
            }
            else
            {
                cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                columnWidths = new float[] { 100f };
            }


            table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths,"", fontTitleTable,null);
            document.Add(table);

            #endregion

            

            #region Apto

            string Apto = "", AptoRestricciones = "", NoApto = "";

            var esoType = (Sigesoft.Common.TypeESO)DataService.i_EsoTypeId;

            if (esoType != Sigesoft.Common.TypeESO.Retiro)
            {

                if (DataService.i_AptitudeStatusId == (int)Sigesoft.Common.AptitudeStatus.Apto)
                {
                    Apto = "X";
                }
                else if (DataService.i_AptitudeStatusId == (int)Sigesoft.Common.AptitudeStatus.AptRestriccion)
                {
                    AptoRestricciones = "X";
                }
                else if (DataService.i_AptitudeStatusId == (int)Sigesoft.Common.AptitudeStatus.NoApto)
                {
                    NoApto = "X";
                }

                cells = new List<PdfPCell>()
                  {
                       //fila
                        new PdfPCell(new Phrase("Apto", fontAptitud)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT}, 
                        new PdfPCell(new Phrase(Apto, fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                        new PdfPCell(new Phrase("Apto con Restricciones", fontAptitud)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT}, 
                        new PdfPCell(new Phrase(AptoRestricciones, fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT}, 
                        new PdfPCell(new Phrase("No Apto", fontAptitud)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT}, 
                        new PdfPCell(new Phrase(NoApto, fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                  };

                columnWidths = new float[] { 20f, 20f, 20f, 20f, 20f, 20f };
            }
            else   // es de retiro
            {
                cells = new List<PdfPCell>()
                {
                    //fila
                    new PdfPCell(new Phrase("Paciente evaluado", fontAptitud)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT}, 
                    new PdfPCell(new Phrase("X", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT},
                      
                };

                columnWidths = new float[] { 20f, 20f};
            }

            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "", fontTitleTable);

            document.Add(filiationWorker);

            #endregion

            #region RECOMENDACIONES

            columnHeaders = null;

            if (ListRecomendation.Count == 0)
            {
                ListRecomendation.Add(new RecomendationList { v_RecommendationName = "No se han registrado datos." });
                columnWidths = new float[] { 100f };
                include = "v_RecommendationName";
            }
            else
            {
                columnWidths = new float[] { 3f,97f };
                include = "i_Item,v_RecommendationName";
                columnHeaders = new string[] { "RECOMENDACIONES" };
            }

            ListRecomendation = ListRecomendation                          
                               .GroupBy(x => x.v_RecommendationName)
                               .Select(group => group.First())
                               .ToList();

            var Recomendations = HandlingItextSharp.GenerateTableFromList(ListRecomendation, columnWidths, include, fontColumnValue, "XIV. RECOMENDACIONES ", fontTitleTable);

            //var habitoNocivo = HandlingItextSharp.GenerateTableFromList(noxiousHabit, columnWidths, include, fontColumnValue, "III. HÁBITOS NOCIVOS", fontTitleTable);

            document.Add(Recomendations);

            #endregion

            #region RESTRICCIONES

            columnHeaders = null;

            if (ListRestricciones.Count == 0)
            {
                ListRestricciones.Add(new RestrictionList { v_RestrictionName = "No se han registrado datos." });
                columnWidths = new float[] { 100f };
                include = "v_RestrictionName";
            }
            else
            {
                columnWidths = new float[] { 3f, 97f };
                include = "i_Item,v_RestrictionName";
                columnHeaders = new string[] { "RESTRICCIONES" };
            }

            ListRestricciones = ListRestricciones
                               .GroupBy(x => x.v_RestrictionName)
                               .Select(group => group.First())
                               .ToList();

            var Restricciones = HandlingItextSharp.GenerateTableFromList(ListRestricciones, columnWidths, include, fontColumnValue, "XV. RESTRICCIONES ", fontTitleTable);

            //var habitoNocivo = HandlingItextSharp.GenerateTableFromList(noxiousHabit, columnWidths, include, fontColumnValue, "III. HÁBITOS NOCIVOS", fontTitleTable);

            document.Add(Restricciones);

            #endregion

            #region Firma

            #region Creando celdas de tipo Imagen y validando nulls

            // Firma del trabajador ***************************************************
            PdfPCell cellFirmaTrabajador = null;
            DirectoryInfo rutaFirma = null;
            rutaFirma = new DirectoryInfo(WebConfigurationManager.AppSettings["FirmaHuella"].ToString());
            //iTextSharp.text.Image Firmajpg = iTextSharp.text.Image.GetInstance(rutaFirma + DataService.v_DocNumber + "_F.jpg");

            if (filiationData.FirmaTrabajador != null)
                cellFirmaTrabajador = new PdfPCell(HandlingItextSharp.GetImage(filiationData.FirmaTrabajador, 20, 20));
            else
                cellFirmaTrabajador = new PdfPCell(new Phrase("Sin Firma Trabajador", fontColumnValue));
                //cellFirmaTrabajador = new PdfPCell(Firmajpg);

            // Huella del trabajador **************************************************
            PdfPCell cellHuellaTrabajador = null;
            DirectoryInfo rutaHuella = null;
            rutaHuella = new DirectoryInfo(WebConfigurationManager.AppSettings["FirmaHuella"].ToString());
            //iTextSharp.text.Image Huellajpg = iTextSharp.text.Image.GetInstance(rutaHuella + DataService.v_DocNumber + "_H.jpg");
            if (filiationData.HuellaTrabajador != null)
                cellHuellaTrabajador = new PdfPCell(HandlingItextSharp.GetImage(filiationData.HuellaTrabajador, 15, 15));
            else
                cellHuellaTrabajador = new PdfPCell(new Phrase("Sin Huella", fontColumnValue));
                //cellHuellaTrabajador = new PdfPCell(Huellajpg);

            // Firma del doctor Auditor **************************************************

            PdfPCell cellFirma = null;

            if (DataService.FirmaDoctor != null)
                cellFirma = new PdfPCell(HandlingItextSharp.GetImage(DataService.FirmaDoctor, 45, 45));
            else
                cellFirma = new PdfPCell(new Phrase("Sin Firma", fontColumnValue));

            #endregion

            #region Crear tablas en duro (para la Firma y huella del trabajador)

            cells = new List<PdfPCell>();

            cellFirmaTrabajador.HorizontalAlignment = Element.ALIGN_CENTER;
            cellFirmaTrabajador.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellFirmaTrabajador.FixedHeight = 70F;
            cells.Add(cellFirmaTrabajador);
            cells.Add(new PdfPCell(new Phrase("Firma del Examinado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

            columnWidths = new float[] { 100f };

            var tableFirmaTrabajador = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "", fontTitleTable);

            //***********************************************

            cells = new List<PdfPCell>();

            cellHuellaTrabajador.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHuellaTrabajador.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellHuellaTrabajador.FixedHeight = 70F;
            cells.Add(cellHuellaTrabajador);
            cells.Add(new PdfPCell(new Phrase("Huella del Examinado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

            columnWidths = new float[] { 100f };

            var tableHuellaTrabajador = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, "", fontTitleTable);

            #endregion

            cells = new List<PdfPCell>();

            // 1 celda vacia              
            cells.Add(new PdfPCell(tableFirmaTrabajador));

            // 1 celda vacia
            cells.Add(new PdfPCell(tableHuellaTrabajador));

            // 2 celda
            cell = new PdfPCell(new Phrase("Firma y Sello Médico", fontColumnValue)) { Rowspan=2};
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cells.Add(cell);

            // 3 celda (Imagen)
            cellFirma.HorizontalAlignment = Element.ALIGN_CENTER;
            cellFirma.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellFirma.FixedHeight = 70F;
            cells.Add(cellFirma);

            cells.Add(new PdfPCell(new Phrase("Con la cual declara que la información declarada es veraz", fontColumnValue)) { Colspan=2, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
            cells.Add(new PdfPCell(new Phrase("", fontColumnValue)) { Colspan = 2});

            columnWidths = new float[] { 25f, 25f, 20f, 30F };
            table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, " ", fontTitleTable);

            document.Add(table);

            #endregion


            document.Close();
            writer.Close();
            writer.Dispose();
        }

        private static PdfPTable TableBuilderReportFor312(ServiceComponentList serviceComponent, Font fontTitle, Font fontSubTitle, Font fontColumnValue, BaseColor SubtitleBackgroundColor)
        {
            PdfPTable table = null;
            List<PdfPCell> cells = null;
            PdfPCell cell = null;
            float[] columnWidths = null;

            switch (serviceComponent.v_ComponentId)
            {

                case Sigesoft.Common.Constants.ELECTROCARDIOGRAMA_ID:

                    #region ELECTROCARDIOGRAMA

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName + ": ", fontSubTitle))
                    {
                        Colspan = 2,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                           //Conclusiones = ekg.Count == 0 ? string.Empty : ekg.Find(p => p.v_ComponentFieldId == Constants.ELECTROCARDIOGRAMA_DESCRIPCION_INTERPRETACION_ID).v_Value1,

                        var conclusion = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.ELECTROCARDIOGRAMA_DESCRIPCION_INTERPRETACION_ID);
                        //var hallazgos = serviceComponent.DiagnosticRepository;
                        //var join = string.Join(",", hallazgos.Select(p => p.v_DiseasesName));

                        cells.Add(new PdfPCell(new Phrase(conclusion == null || string.IsNullOrEmpty(conclusion.v_Value1) ? "ELECROCARDIOGRAMA DENTRO DE LA NORMALIDAD" : "" + conclusion.v_Value1, fontColumnValue)));
                        //cells.Add(new PdfPCell(new Phrase(string.IsNullOrEmpty(join) ? "Hallazgos: -----" : "Hallazgos: " + join, fontColumnValue)));
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("NO REQUERIDO", fontColumnValue)));
                    }

                    columnWidths = new float[] { 100f };
                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.EVALUACION_ERGONOMICA_ID:

                    #region EVALUACION_ERGONOMICA

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName + ": ", fontSubTitle))
                    {
                        Colspan = 2,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var conclusion = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EVALUACION_ERGONOMICA_CONCLUSION_ID);
                        var hallazgos = serviceComponent.DiagnosticRepository;
                        var join = string.Join(",", hallazgos.Select(p => p.v_DiseasesName));

                        cells.Add(new PdfPCell(new Phrase(string.IsNullOrEmpty(conclusion.v_Value1) ? "No se han registrado datos." : "Conclusiones: " + conclusion.v_Value1, fontColumnValue)));
                        //cells.Add(new PdfPCell(new Phrase(string.IsNullOrEmpty(join) ? "Hallazgos: -----" : "Hallazgos: " + join, fontColumnValue)));
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                    }

                    columnWidths = new float[] { 100f };
                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;
                case Sigesoft.Common.Constants.ALTURA_ESTRUCTURAL_ID:

                    #region ALTURA_ESTRUCTURAL

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName + ": ", fontSubTitle))
                    {
                        Colspan = 2,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var conclusion = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.ALTURA_ESTRUCTURAL_DESCRIPCION_ID);
                        var hallazgos = serviceComponent.DiagnosticRepository;
                        var join = string.Join(",", hallazgos.Select(p => p.v_DiseasesName));

                        cells.Add(new PdfPCell(new Phrase(string.IsNullOrEmpty(conclusion.v_Value1) ? "No se han registrado datos." : "Conclusiones: " + conclusion.v_Value1, fontColumnValue)));
                        //cells.Add(new PdfPCell(new Phrase(string.IsNullOrEmpty(join) ? "Hallazgos: -----" : "Hallazgos: " + join, fontColumnValue)));
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                    }

                    columnWidths = new float[] { 100f };
                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                //case Sigesoft.Common.Constants.ALTURA_GEOGRAFICA_ID:

                //    #region ALTURA_GEOGRAFICA

                //    cells = new List<PdfPCell>();

                //    // Subtitulo  ******************
                //    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName + ": ", fontSubTitle))
                //    {
                //        Colspan = 2,
                //        BackgroundColor = SubtitleBackgroundColor,
                //        HorizontalAlignment = Element.ALIGN_LEFT,
                //    };

                //    cells.Add(cell);
                //    //*****************************************

                //    if (serviceComponent.ServiceComponentFields.Count > 0)
                //    {
                //        var conclusion = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.ALTURA_GEOGRAFICA_DESCRIPCION_ID);
                //        var hallazgos = serviceComponent.DiagnosticRepository;
                //        var join = string.Join(",", hallazgos.Select(p => p.v_DiseasesName));

                //        cells.Add(new PdfPCell(new Phrase(conclusion == null || string.IsNullOrEmpty(conclusion.v_Value1) ? "No se han registrado datos." : "Conclusiones: " + conclusion.v_Value1, fontColumnValue)));
                //        //cells.Add(new PdfPCell(new Phrase(string.IsNullOrEmpty(join) ? "Hallazgos: -----" : "Hallazgos: " + join, fontColumnValue)));
                //    }
                //    else
                //    {
                //        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                //    }

                //    columnWidths = new float[] { 100f };
                //    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                //    #endregion

                    //break;
                case Sigesoft.Common.Constants.MUSCULO_ESQUELETICO_1_ID:

                    #region OSTEO_MUSCULAR

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName + ": ", fontSubTitle))
                    {
                        Colspan = 2,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var conclusion = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.MÚSCULO_ESQUELÉTICO_1_DETALLE_DE_ANTECEDENTES_ENCONTRADOS);
                      
                        cells.Add(new PdfPCell(new Phrase(string.IsNullOrEmpty(conclusion.v_Value1) ? "CONCLISIÓN SIN ALTERACIONES" : "Hallazgos: " + conclusion.v_Value1, fontColumnValue)));
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("NO REQUERIDO", fontColumnValue)));
                    }

                    columnWidths = new float[] { 100f };
                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.PRUEBA_ESFUERZO_ID:

                    #region PRUEBA_ESFUERZO

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName + ": ", fontSubTitle))
                    {
                        Colspan = 2,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var conclusion = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PRUEBA_ESFUERZO_DESCRIPCION_ID);
                        var hallazgos = serviceComponent.DiagnosticRepository;
                        var join = string.Join(",", hallazgos.Select(p => p.v_DiseasesName));

                        cells.Add(new PdfPCell(new Phrase(conclusion == null || string.IsNullOrEmpty(conclusion.v_Value1) ? "No se han registrado datos." : "Conclusiones: " + conclusion.v_Value1, fontColumnValue)));
                        //cells.Add(new PdfPCell(new Phrase(string.IsNullOrEmpty(join) ? "Hallazgos: -----" : "Hallazgos: " + join, fontColumnValue)));
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                    }

                    columnWidths = new float[] { 100f };
                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;
                case Sigesoft.Common.Constants.TAMIZAJE_DERMATOLOGIO_ID:

                    #region TAMIZAJE_DERMATOLOGICO

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName + ": ", fontSubTitle))
                    {
                        Colspan = 2,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var conclusion = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.TAMIZAJE_DERMATOLOGIO_DESCRIPCION1_ID);
                        var hallazgos = serviceComponent.DiagnosticRepository;
                        var join = string.Join(",", hallazgos.Select(p => p.v_DiseasesName));

                        cells.Add(new PdfPCell(new Phrase(string.IsNullOrEmpty(conclusion.v_Value1) ? "No se han registrado datos." : "Conclusiones: " + conclusion.v_Value1, fontColumnValue)));
                        //cells.Add(new PdfPCell(new Phrase(string.IsNullOrEmpty(join) ? "Hallazgos: -----" : "Hallazgos: " + join, fontColumnValue)));
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                    }

                    columnWidths = new float[] { 100f };
                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.ODONTOGRAMA_ID:

                    #region ODONTOGRAMA

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName + ": ", fontSubTitle))
                    {
                        Colspan = 2,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var conclusion = string.Join(", ", serviceComponent.DiagnosticRepository.Select(p => p.v_DiseasesName));
                        cells.Add(new PdfPCell(new Phrase(string.IsNullOrEmpty(conclusion) ? "No se han registrado datos." : conclusion, fontColumnValue)));
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("NO REQUERIDO.", fontColumnValue)));
                    }

                    columnWidths = new float[] { 100f };
                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;
                case Sigesoft.Common.Constants.GINECOLOGIA_ID:

                    #region EVALUACION_GINECOLOGICA

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName + ": ", fontSubTitle))
                    {
                        Colspan = 2,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var descripcion = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.GINECOLOGIA_HALLAZGOS_ID);

                        cells.Add(new PdfPCell(new Phrase(string.IsNullOrEmpty(descripcion.v_Value1) ? "No se han registrado datos." : descripcion.v_Value1, fontColumnValue)));

                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                    }

                    columnWidths = new float[] { 100f };
                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;
                case Sigesoft.Common.Constants.EXAMEN_MAMA_ID:

                    #region EXAMEN_MAMA

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName + ": ", fontSubTitle))
                    {
                        Colspan = 2,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var descripcion = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_MAMA_HALLAZGOS_ID);

                        cells.Add(new PdfPCell(new Phrase(string.IsNullOrEmpty(descripcion.v_Value1) ? "No se han registrado datos." : descripcion.v_Value1, fontColumnValue)));

                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                    }

                    columnWidths = new float[] { 100f };
                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                default:
                    break;
            }

            return table;

        }


        private static string DEvolver237string (string pstrIdParameter)
        {
            if (pstrIdParameter == "1")
            {
                return "20/20";

            }
            else if (pstrIdParameter == "2")
            {
                  return "20/25";

            }
            else if (pstrIdParameter == "3")
            {
                return "20/30";

            }
            else if (pstrIdParameter == "4")
            {
                return "20/40";

            }
            else if (pstrIdParameter == "5")
            {
                return "20/50";

            }
            else if (pstrIdParameter == "6")
            {
                return "20/60";

            }
            else if (pstrIdParameter == "7")
            {
                return "20/80";

            }
            else if (pstrIdParameter == "8")
            {
                return "20/100";

            }
            else if (pstrIdParameter == "9")
            {
                return "20/140";

            }
            else if (pstrIdParameter == "10")
            {
                return "20/200";

            }
            else if (pstrIdParameter == "11")
            {
                return "20/400";

            }
            else if (pstrIdParameter == "12")
            {
                return "CD 3M";

            }
            else if (pstrIdParameter == "13")
            {
                return "CD 1M";

            }
            else if (pstrIdParameter == "14")
            {
                return "CD 0.3M";

            }
            else if (pstrIdParameter == "15")
            {
                return "MM";

            }
            else if (pstrIdParameter == "16")
            {
                return "PL";

            }
            else if (pstrIdParameter == "17")
            {
                return"NPL";

            }
            else if (pstrIdParameter == "18")
            {
                return "---";
            }
            else
            { return ""; }
        }
        private static string DEvolver262string(string pstrIdParameter)
        {
            if (pstrIdParameter == "1")
            {
                return "20/20";
            }
            else if (pstrIdParameter == "2")
            {
                return "20/30";

            }
            else if (pstrIdParameter == "3")
            {
                return "20/40";
                
            }
            else if (pstrIdParameter == "4")
            {
                return "20/50";

            }
            else if (pstrIdParameter == "5")
            {
                return "20/60";

            }
            else if (pstrIdParameter == "6")
            {
                return "20/70";

            }
            else if (pstrIdParameter == "7")
            {
                return "20/80";

            }
            else if (pstrIdParameter == "8")
            {
                return "20/100";

            }
            else if (pstrIdParameter == "9")
            {
                return "20/160";

            }
            else if (pstrIdParameter == "10")
            {
                return "20/200";

            }
            else if (pstrIdParameter == "11")
            {
                return "> 20/200";

            }
            else
            { return ""; }
        }


    }

}

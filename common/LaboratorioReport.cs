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


namespace NetPdf
{
    public class LaboratorioReport
    {
        #region Report de Laboratorio

        public static void CreateLaboratorioReport(PacientList filiationData, List<ServiceComponentList> serviceComponent, organizationDto infoEmpresaPropietaria, string filePDF)
        {
            //
            // step 1: creation of a document-object
            Document document = new Document();
            //Document document = new Document(new Rectangle(500f, 300f), 10, 10, 10, 10);
            //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            //Document document = new Document(PageSize.A4, 0, 0, 20, 20);
            //
            try
            {
                // step 2: we create a writer that listens to the document
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePDF, FileMode.Create));
                //
                //create an instance of your PDFpage class. This is the class we generated above.
                pdfPage page = new pdfPage();
                //set the PageEvent of the pdfWriter instance to the instance of our PDFPage class
                writer.PageEvent = page;

                // step 3: we open the document
                document.Open();
                // step 4: we Add content to the document
                // we define some fonts

                #region Fonts

                Font fontTitle1 = FontFactory.GetFont(FontFactory.HELVETICA, 18, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
                Font fontTitle2 = FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
                Font fontTitleTable = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.White));
                Font fontTitleTableNegro = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));
                Font fontSubTitle = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.White));
                Font fontSubTitleNegroNegrita = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));

                Font fontColumnValue = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
               

                #endregion

                //#region Logo

                //PdfPCell CellLogo = null;

                //if (infoEmpresaPropietaria.b_Image != null)

                //    CellLogo = new PdfPCell(HandlingItextSharp.GetImage(infoEmpresaPropietaria.b_Image, 20F));
                //else
                //    CellLogo = new PdfPCell(new Phrase("Sin Logo", fontColumnValue));

            
                //PdfPTable headerTbl = new PdfPTable(1);
                //headerTbl.TotalWidth = writer.PageSize.Width;
                //PdfPCell cellLogo = new PdfPCell(CellLogo);

                //cellLogo.VerticalAlignment = Element.ALIGN_TOP;
                //cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;

                //cellLogo.Border = PdfPCell.NO_BORDER;
                //headerTbl.AddCell(cellLogo);
                //document.Add(headerTbl);

                //#endregion

                //#region Title

                //Paragraph cTitle = new Paragraph("LABORATORIO CLÍNICO", fontTitle2);
                ////Paragraph cTitle2 = new Paragraph(customerOrganizationName, fontTitle2);
                //cTitle.Alignment = Element.ALIGN_CENTER;
                ////cTitle2.Alignment = Element.ALIGN_CENTER;

                //document.Add(cTitle);
                ////document.Add(cTitle2);

                //#endregion

                #region Title

                PdfPCell CellLogo = null;
                List<PdfPCell> cells = null;
                cells = new List<PdfPCell>();
                PdfPCell cellPhoto1 = null;
                float[] columnWidths = null;
                PdfPTable table = null;

                if (filiationData.b_Photo != null)
                    cellPhoto1 = new PdfPCell(HandlingItextSharp.GetImage(filiationData.b_Photo, null, null, 40, 40)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT };
                else
                    cellPhoto1 = new PdfPCell(new Phrase("Sin Foto Trabjador", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT };

                if (infoEmpresaPropietaria.b_Image != null)
                {
                    CellLogo = new PdfPCell(HandlingItextSharp.GetImage(infoEmpresaPropietaria.b_Image, null, null, 90, 40)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT };
                }
                else
                {
                    CellLogo = new PdfPCell(new Phrase("Sin Logo", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_LEFT };
                }

                columnWidths = new float[] { 100f };

                var cellsTit = new List<PdfPCell>()
                { 
                new PdfPCell(new Phrase("LABORATORIO CLÍNICO", fontTitle1)) { HorizontalAlignment = PdfPCell.ALIGN_CENTER },
                };

                table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, PdfPCell.NO_BORDER, null, fontTitleTable);

                cells.Add(CellLogo);
                cells.Add(new PdfPCell(table));
                cells.Add(cellPhoto1);

                columnWidths = new float[] { 20f, 60f, 20f };

                table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, PdfPCell.NO_BORDER, null, fontTitleTable);
                document.Add(table);

                #endregion
                #region Declaration Tables
                var subTitleBackGroundColor = new BaseColor(System.Drawing.Color.White);
                string include = string.Empty;
                //List<PdfPCell> cells = null;
                //float[] columnWidths = null;
                string[] columnValues = null;
                string[] columnHeaders = null;

                //PdfPTable header1 = new PdfPTable(2);
                //header1.HorizontalAlignment = Element.ALIGN_CENTER;
                //header1.WidthPercentage = 100;
                ////header1.TotalWidth = 500;
                ////header1.LockedWidth = true;    // Esto funciona con TotalWidth
                //float[] widths = new float[] { 150f, 200f};
                //header1.SetWidths(widths);


                //Rectangle rec = document.PageSize;
                PdfPTable header2 = new PdfPTable(6);
                header2.HorizontalAlignment = Element.ALIGN_CENTER;
                header2.WidthPercentage = 100;
                //header1.TotalWidth = 500;
                //header1.LockedWidth = true;    // Esto funciona con TotalWidth
                float[] widths1 = new float[] { 16.6f, 18.6f, 16.6f, 16.6f, 16.6f, 16.6f };
                header2.SetWidths(widths1);
                //header2.SetWidthPercentage(widths1, rec);

                PdfPTable companyData = new PdfPTable(6);
                companyData.HorizontalAlignment = Element.ALIGN_CENTER;
                companyData.WidthPercentage = 100;
                //header1.TotalWidth = 500;
                //header1.LockedWidth = true;    // Esto funciona con TotalWidth
                float[] widthscolumnsCompanyData = new float[] { 16.6f, 16.6f, 16.6f, 16.6f, 16.6f, 16.6f };
                companyData.SetWidths(widthscolumnsCompanyData);

                PdfPTable filiationWorker = new PdfPTable(4);

                //PdfPTable table = null;

                PdfPCell cell = null;

                #endregion

                // Salto de linea
                //document.Add(new Paragraph("\r\n"));

                #region Datos personales del trabajador

                cells = new List<PdfPCell>()
                {
                    new PdfPCell(new Phrase("Nombres: ", fontColumnValue)), new PdfPCell(new Phrase(filiationData.v_FirstName, fontColumnValue)),                   
                    new PdfPCell(new Phrase("Apellidos: ", fontColumnValue)), new PdfPCell(new Phrase(filiationData.v_FirstLastName + " " + filiationData.v_SecondLastName , fontColumnValue)),                   
                    //new PdfPCell(new Phrase("Foto:", fontColumnValue)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT }, cellPhoto,
                                                                                        
                    new PdfPCell(new Phrase("Edad: ", fontColumnValue)), new PdfPCell(new Phrase(filiationData.i_Age.ToString(), fontColumnValue)),                   
                    //new PdfPCell(new Phrase("Seguro: ", fontColumnValue)), new PdfPCell(new Phrase(filiationData.v_TypeOfInsuranceName, fontColumnValue)),                   
                    //new PdfPCell(new Phrase(" ", fontColumnValue)), 

                    new PdfPCell(new Phrase("Empresa: ", fontColumnValue)), new PdfPCell(new Phrase(filiationData.v_FullWorkingOrganizationName, fontColumnValue)),                   
                    new PdfPCell(new Phrase("Centro Médico: ", fontColumnValue)), new PdfPCell(new Phrase(filiationData.v_OwnerOrganizationName, fontColumnValue)),                   
                    //new PdfPCell(new Phrase(" ", fontColumnValue)),      
                                         
                    //new PdfPCell(new Phrase("Médico: ", fontColumnValue)), new PdfPCell(new Phrase("Dr(a)." + filiationData.v_DoctorPhysicalExamName, fontColumnValue)),                   
                    new PdfPCell(new Phrase("Fecha Atención: ", fontColumnValue)), new PdfPCell(new Phrase(filiationData.d_ServiceDate.Value.ToShortDateString(), fontColumnValue)),                   
                    //new PdfPCell(new Phrase(" ", fontColumnValue)),                  
                                                   
                };

                columnWidths = new float[] { 20.6f, 40.6f, 16.6f, 30.6f, };

                filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, PdfPCell.NO_BORDER, "I. DATOS PERSONALES", fontTitleTableNegro, null);

                document.Add(filiationWorker);

                #endregion

                #region BIOQUIMICA

                cells = new List<PdfPCell>();

                // Subtitulo  ******************
                cell = new PdfPCell(new Phrase("BIOQUIMICA", fontSubTitleNegroNegrita))
                {
                    Colspan = 4,
                    BackgroundColor = subTitleBackGroundColor,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                };

                cells.Add(cell);
                //*****************************************

                // titulo
                cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                var xGlucosa = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.GLUCOSA_ID);
                var xColesterol = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.COLESTEROL_ID);
                var xColesterolHdl = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.COLESTEROL_HDL_ID);
                var xColesterolLdl = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.COLESTEROL_LDL_ID);
                var xColesterolVldl = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.COLESTEROL_VLDL_ID);
                var xTrigliceridos = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.TRIGLICERIDOS_ID);
                var xCreatinina = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.CREATININA_ID);
                var xAcidoUrico = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.ACIDO_URICO_ID);
                var xAntigenoProstatico = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.ANTIGENO_PROSTATICO_ID);
                var xPlomoSangre = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.PLOMO_SANGRE_ID);
                var xTgo = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.TGO_ID);
                var xTgp = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.TGP_ID);
                var xUrea = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.UREA_ID);
             
                if (xGlucosa != null)
                {                   
                    var glucosa = xGlucosa.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.GLUCOSA_GLUCOSA_ID);
                    var glucosaValord = Sigesoft.Common.Constants.GLUCOSA_GLUCOSA_VALOR_DESEABLE_ID;

                    //cells.Add(new PdfPCell(new Phrase("LEUCOCITOS", fontColumnValue)));
                    //cells.Add(new PdfPCell(new Phrase(leucocitos == null ? string.Empty : leucocitos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    //cells.Add(new PdfPCell(new Phrase("4.0 - 10.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    //cells.Add(new PdfPCell(new Phrase(leucocitos == null ? string.Empty : leucocitos.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });


                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("GLUCOSA", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(glucosa == null ? string.Empty : glucosa.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("70 - 110", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                   
                }

                if (xColesterol != null)
                {
                    var colesterol = xColesterol.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_COLESTEROL_TOTAL_ID);
                    var colesterolValord = Sigesoft.Common.Constants.COLESTEROL_COLESTEROL_TOTAL_DESEABLE_ID;

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("COLESTEROL", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(colesterol == null ? string.Empty : colesterol.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("< a 200", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xColesterolHdl != null)
                {
                    var colesterolHdl = xColesterolHdl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_HDL_BIOQUIMICA_COLESTEROL_HDL);
                    var colesterolHdlValord =  Sigesoft.Common.Constants.COLESTEROL_HDL_BIOQUIMICA_COLESTEROL_HDL_DESEABLE;

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("COLESTEROL TOTAL / HDL", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(colesterolHdl == null ? string.Empty : colesterolHdl.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("< a 200", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xColesterolLdl != null)
                {
                    var colesterolLdl = xColesterolLdl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_LDL_BIOQUIMICA_COLESTEROL_LDL);
                    var colesterolLdlValord = Sigesoft.Common.Constants.COLESTEROL_LDL_BIOQUIMICA_COLESTEROL_LDL_DESEABLE;

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("LDL", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(colesterolLdl == null ? string.Empty : colesterolLdl.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(colesterolLdlValord == null ? string.Empty : colesterolLdlValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xColesterolVldl != null)
                {
                    var colesterolVldl = xColesterolVldl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COLESTEROL_VLDL_BIOQUIMICA_COLESTEROL_VLDL);
                    var colesterolVldlValord =  Sigesoft.Common.Constants.COLESTEROL_VLDL_BIOQUIMICA_COLESTEROL_VLDL_DESEABLE;

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("VLDL", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(colesterolVldl == null ? string.Empty : colesterolVldl.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(colesterolVldlValord == null ? string.Empty : colesterolVldlValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xTrigliceridos != null)
                {
                    var trigliceridos = xTrigliceridos.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.TRIGLICERIDOS_BIOQUIMICA_TRIGLICERIDOS);
                    var trigliceridosValord = Sigesoft.Common.Constants.TRIGLICERIDOS_BIOQUIMICA_TRIGLICERIDOS_DESEABLE;

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("TRIGLICERIDOS", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(trigliceridos == null ? string.Empty : trigliceridos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("< a 200", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xCreatinina != null)
                {
                    var creatinina = xCreatinina.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.CREATININA_BIOQUIMICA_CREATININA);
                    var creatininaValord = Sigesoft.Common.Constants.CREATININA_BIOQUIMICA_CREATININA_DESEABLE;

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("CREATININA", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(creatinina == null ? string.Empty : creatinina.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(creatininaValord == null ? string.Empty : creatininaValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xAcidoUrico != null)
                {
                    var acidourico = xAcidoUrico.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.ACIDO_URICO_BIOQUIMICA_ACIDO_URICO);
                    var acidouricoValord = Sigesoft.Common.Constants.ACIDO_URICO_BIOQUIMICA_ACIDO_URICO_DESEABLE;

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("ÁCIDO ÚRICO", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(acidourico == null ? string.Empty : acidourico.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(acidouricoValord == null ? string.Empty : acidouricoValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xAntigenoProstatico != null)
                {
                    var antigenoprostatico = xAntigenoProstatico.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.ANTIGENO_PROSTATICO_ANTIGENO_PROSTATICO_VALOR);
                    var antigenoprostaticoValord = Sigesoft.Common.Constants.ANTIGENO_PROSTATICO_VALOR_DESEABLE;

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("ANTÍGENO PROSTÁTICO", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(antigenoprostatico == null ? string.Empty : antigenoprostatico.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(antigenoprostaticoValord == null ? string.Empty : antigenoprostaticoValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xPlomoSangre != null)
                {
                    var plomoensangre = xPlomoSangre.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PLOMO_SANGRE_BIOQUIMICA_PLOMO_SANGRE);
                    var plomoensangreValord = Sigesoft.Common.Constants.PLOMO_SANGRE_BIOQUIMICA_PLOMO_SANGRE_DESEABLE;

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("PLOMO EN SANGRE", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(plomoensangre == null ? string.Empty : plomoensangre.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(plomoensangreValord == null ? string.Empty : plomoensangreValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xTgo != null)
                {
                    var tgo = xTgo.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.TGO_BIOQUIMICA_TGO);
                    var tgoValord = Sigesoft.Common.Constants.TGO_BIOQUIMICA_TGO_DESEABLE;

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("TGO", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(tgo == null ? string.Empty : tgo.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(tgoValord == null ? string.Empty : tgoValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xTgp != null)
                {
                    var tgp = xTgp.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.TGP_BIOQUIMICA_TGP);
                    var tgpValord = Sigesoft.Common.Constants.TGP_BIOQUIMICA_TGP_DESEABLE;

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("TGP", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(tgp == null ? string.Empty : tgp.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(tgpValord == null ? string.Empty : tgpValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xUrea != null)
                {
                    var urea = xUrea.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.UREA_BIOQUIMICA_UREA);
                    var ureaValord = Sigesoft.Common.Constants.UREA_BIOQUIMICA_UREA_DESEABLE;

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("UREA", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(urea == null ? string.Empty : urea.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(ureaValord == null ? string.Empty : ureaValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase("mg/dl", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                var xAglutinaciones = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.AGLUTINACIONES_LAMINA_ID);
                var xExamenElisa = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.EXAMEN_ELISA_ID);
                var xHepatitisA = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.HEPATITIS_A_ID);
                var xHepatitisC = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.HEPATITIS_C_ID);
                var xVdrl = serviceComponent.Find(p => p.v_ComponentId == Sigesoft.Common.Constants.VDRL_ID);

                if (xAglutinaciones != null)
                {
                    cells.Add(new PdfPCell(new Phrase("REACTIVOS", fontSubTitleNegroNegrita)) { Colspan = 4 });

                    var tificoO = xAglutinaciones.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.AGLUTINACIONES_LAMINA_REACTIVOS_TIFICO_O);
                    var tificoH = xAglutinaciones.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.AGLUTINACIONES_LAMINA_REACTIVOS_TIFICO_H);
                    var paratificoA = xAglutinaciones.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.AGLUTINACIONES_LAMINA_REACTIVOS_PARATIFICO_A);
                    var paratificoB = xAglutinaciones.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.AGLUTINACIONES_LAMINA_REACTIVOS_PARATIFICO_B);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("TÍFICO O", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(tificoO == null ? string.Empty : tificoO.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("TÍFICO H", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(tificoH == null ? string.Empty : tificoH.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("PARATÍFICO O", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(paratificoA == null ? string.Empty : paratificoA.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("PARATÍFICO H", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(paratificoB == null ? string.Empty : paratificoB.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }


                if (xExamenElisa != null)
                {
                    var ExamenElisa = xExamenElisa.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_ELISA_REACTIVOS_EXAMEN_ELISA);
                    
                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("EXAMEN DE ELISA", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(ExamenElisa == null ? string.Empty : ExamenElisa.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xHepatitisC != null)
                {
                    var HepatitisC = xHepatitisC.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEPATITIS_C_REACTIVOS_HEPATITIS_C);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("HEPATITIS C", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(HepatitisC == null ? string.Empty : HepatitisC.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xHepatitisA != null)
                {
                    var HepatitisA = xHepatitisA.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HEPATITIS_A_REACTIVOS_HEPATITIS_A);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("HEPATITIS A", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(HepatitisA == null ? string.Empty : HepatitisA.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }

                if (xVdrl != null)
                {
                    var Vdrl = xVdrl.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.VDRL_REACTIVOS_VDRL);

                    // 1era fila
                    cells.Add(new PdfPCell(new Phrase("VDRL", fontColumnValue)));
                    cells.Add(new PdfPCell(new Phrase(Vdrl == null ? string.Empty : Vdrl.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                }
                columnWidths = new float[] { 25f, 25f, 25f, 25f };

                table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);
                document.Add(table);

                #endregion
          
                #region Imprimir todos los examenes de laboratorio

                string[] orderPrint = new string[]
                { 
                    Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_ID, 
                    Sigesoft.Common.Constants.EXAMEN_COMPLETO_DE_ORINA_ID,                  
                    Sigesoft.Common.Constants.GRUPO_Y_FACTOR_SANGUINEO_ID,
                    Sigesoft.Common.Constants.GLUCOSA_ID,                
                    Sigesoft.Common.Constants.COLESTEROL_ID,
                    Sigesoft.Common.Constants.COLESTEROL_HDL_ID,
                    Sigesoft.Common.Constants.COLESTEROL_LDL_ID,
                    Sigesoft.Common.Constants.COLESTEROL_VLDL_ID,
                    Sigesoft.Common.Constants.TRIGLICERIDOS_ID,
                    Sigesoft.Common.Constants.CREATININA_ID,
                    Sigesoft.Common.Constants.ACIDO_URICO_ID,
                    Sigesoft.Common.Constants.ANTIGENO_PROSTATICO_ID,
                    Sigesoft.Common.Constants.PLOMO_SANGRE_ID,
                    Sigesoft.Common.Constants.TGO_ID,
                    Sigesoft.Common.Constants.TGP_ID,
                    Sigesoft.Common.Constants.UREA_ID,
                    Sigesoft.Common.Constants.AGLUTINACIONES_LAMINA_ID,
                    Sigesoft.Common.Constants.EXAMEN_ELISA_ID,
                    Sigesoft.Common.Constants.HEPATITIS_A_ID,
                    Sigesoft.Common.Constants.HEPATITIS_C_ID,
                    Sigesoft.Common.Constants.VDRL_ID,
                    Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_ID,
                    Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_ID,
                    Sigesoft.Common.Constants.BK_DIRECTO_ID,
                    Sigesoft.Common.Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID,

                };
           
                #endregion

                //Capturar la firma del medico que grabo en laboratorio
                var lab = serviceComponent.Find(p => p.i_CategoryId == (int)Sigesoft.Common.Consultorio.Laboratorio);

                ReportBuilderReportForLaboratorioReport(serviceComponent, orderPrint, fontTitleTable, fontSubTitleNegroNegrita, fontColumnValue, subTitleBackGroundColor, document);
                    
                #region Firma y sello Médico
              
                table = new PdfPTable(2);
                table.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.WidthPercentage = 40;

                columnWidths = new float[] { 15f, 25f };
                table.SetWidths(columnWidths);

                PdfPCell cellFirma = null;
                        
                if (lab != null)
                {
                    if (lab.FirmaMedico != null)
                        cellFirma = new PdfPCell(HandlingItextSharp.GetImage(lab.FirmaMedico, 35, 35));
                    else
                        cellFirma = new PdfPCell(new Phrase("Sin Foto", fontColumnValue));
                }
                else
                {
                    cellFirma = new PdfPCell();
                }
                
                cellFirma.HorizontalAlignment = Element.ALIGN_CENTER;
                cellFirma.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellFirma.FixedHeight = 70F;

                cell = new PdfPCell(new Phrase("Firma y Sello Médico", fontColumnValue));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                table.AddCell(cell);
                table.AddCell(cellFirma);

                document.Add(table);

                #endregion
    
                // step 5: we close the document
                document.Close();
                writer.Close();
                writer.Dispose();
                //RunFile(filePDF);

            }
            catch (DocumentException)
            {
                throw;
            }
            catch (IOException)
            {
                throw;
            }

        }

        private static void ReportBuilderReportForLaboratorioReport(List<ServiceComponentList> serviceComponent, string[] order, Font fontTitle, Font fontSubTitle, Font fontColumnValue, BaseColor SubtitleBackgroundColor, Document document)
        {
            if (order != null)
            {
                var sortEntity = GetSortEntity(order, serviceComponent);

                if (sortEntity != null)
                {
                    foreach (var ent in sortEntity)
                    {
                        var table = TableBuilderReportForLaboratorioReport(ent, fontTitle, fontSubTitle, fontColumnValue, SubtitleBackgroundColor);

                        if (table != null)
                        {
                            if (ent.v_ComponentId == Sigesoft.Common.Constants.GRUPO_Y_FACTOR_SANGUINEO_ID)
                            {
                                document.NewPage();
                            }

                            document.Add(table);
                        }
                    }
                }
            }
        }

        private static PdfPTable TableBuilderReportForLaboratorioReport(ServiceComponentList serviceComponent, Font fontTitle, Font fontSubTitle, Font fontColumnValue, BaseColor SubtitleBackgroundColor)
        {
            PdfPTable table = null;
            List<PdfPCell> cells = null;
            PdfPCell cell = null;
            float[] columnWidths = null;
            Font fontColumnValueNegrita = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));


            switch (serviceComponent.v_ComponentId)
            {
                case Sigesoft.Common.Constants.HEMOGRAMA_COMPLETO_ID:

                    #region HEMOGRAMA_COMPLETO

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var leucocitos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000145");
                        
                        var linfocitos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000154");
                      
                        var Mixtos_M = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000155");
                       
                        var Neutrofilos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000150");
                       
                        var linfocitos_P = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000152");
                      
                        var Mixtos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000149");
                       
                        var Neutrofilos_P = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000148");
                      
                        var Hematies = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000139");
                      
                        var hemoglobina = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000140");
                       
                        var hematocritos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000141");
                       
                        var VCM = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000142");
                      

                        var HCM = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000143");

                        var CHCM = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000144");


                        var RDW_CV = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000153");
                      
                        var Plaquetas = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000146");
                     

                        var VPM = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N002-MF000000147");
                       
                        //var PDW = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N003-MF000000003");
                        //var PDWValord = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N003-MF000000003");

                        var Plaquetocrito = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == "N003-MF000000004");
                       



                      

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("LEUCOCITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(leucocitos == null ? string.Empty : leucocitos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("4.0 - 10.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(leucocitos == null ? string.Empty : leucocitos.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                       
                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("LINFOCITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(linfocitos == null ? string.Empty : linfocitos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("0.8 - 4.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(linfocitos == null ? string.Empty : linfocitos.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("MIXTOS #", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(Mixtos_M == null ? string.Empty : Mixtos_M.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("0.1 - 0.9", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(Mixtos_M == null ? string.Empty : Mixtos_M.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("NEUTRÓFILOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(Neutrofilos == null ? string.Empty : Neutrofilos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("2.0 - 7.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(Neutrofilos == null ? string.Empty : Neutrofilos.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                                
                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("LINFOCITOS %", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(linfocitos_P == null ? string.Empty : linfocitos_P.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("20.0 - 40.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(linfocitos_P == null ? string.Empty : linfocitos_P.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("MIXTOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(Mixtos == null ? string.Empty : Mixtos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("1.0 - 9.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(Mixtos == null ? string.Empty : Mixtos.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("NEUTRÓFILOS %", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(Neutrofilos_P == null ? string.Empty : Neutrofilos_P.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("50.0 - 70.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(Neutrofilos_P == null ? string.Empty : Neutrofilos_P.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("HEMATIES %", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(Hematies == null ? string.Empty : Hematies.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("3.50 - 5.50", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(Hematies == null ? string.Empty : Hematies.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("HEMOGLOBINA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(hemoglobina == null ? string.Empty : hemoglobina.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("H:>13.0 M:>12.0 ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(hemoglobina == null ? string.Empty : hemoglobina.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        
                              // 1era fila
                        cells.Add(new PdfPCell(new Phrase("HEMATOCRITO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(hematocritos == null ? string.Empty : hematocritos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("37.0 - 54.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(hematocritos == null ? string.Empty : hematocritos.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("VCM", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(VCM == null ? string.Empty : VCM.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("80.0 - 100.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(VCM == null ? string.Empty : VCM.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        
                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("HCM", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(HCM == null ? string.Empty : HCM.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("27.0 - 34.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(HCM == null ? string.Empty : HCM.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("CHCM", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(CHCM == null ? string.Empty : CHCM.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("32.0 - 36.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(CHCM == null ? string.Empty : CHCM.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        
 // 1era fila
                        cells.Add(new PdfPCell(new Phrase("RDW-CV", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(RDW_CV == null ? string.Empty : RDW_CV.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("12.0 - 16.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(RDW_CV == null ? string.Empty : RDW_CV.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        cells.Add(new PdfPCell(new Phrase("PLAQUETAS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(Plaquetas == null ? string.Empty : Plaquetas.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("150 - 450", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(Plaquetas == null ? string.Empty : Plaquetas.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        cells.Add(new PdfPCell(new Phrase("VPM", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(VPM == null ? string.Empty : VPM.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("6.5 - 12.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(VPM == null ? string.Empty : VPM.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        //cells.Add(new PdfPCell(new Phrase("PDW", fontColumnValue)));
                        //cells.Add(new PdfPCell(new Phrase(PDW == null ? string.Empty : PDW.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase("9.0 - 17.0", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(PDW == null ? string.Empty : PDW.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        cells.Add(new PdfPCell(new Phrase("PLAQUETOCRITO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(Plaquetocrito == null ? string.Empty : Plaquetocrito.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("0.108 - 0.282", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(Plaquetocrito == null ? string.Empty : Plaquetocrito.v_MeasurementUnitName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                      

                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;
                case Sigesoft.Common.Constants.EXAMEN_COMPLETO_DE_ORINA_ID:

                    #region EXAMEN_COMPLETO_DE_ORINA

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var color = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_COLOR);
                        var colorValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_COLOR_DESEABLE;

                        var aspecto = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_ASPECTO);
                        var aspectoValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_ASPECTO_DESEABLE;

                        var densidad = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_DENSIDAD);
                        var densidadValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_DENSIDAD_DESEABLE;

                        var ph = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_PH);
                        var phValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MACROSCOPICO_PH_DESEABLE;

                        var celulasEpiteleales = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CELULAS_EPITELIALES);
                        var celulasEpitelealesValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CELULAS_EPITELIALES_DESEABLE;

                        var celulasEpitelealesA = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CELULAS_EPITELIALES_A);                     

                        var leucocitos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_LEUCOCITOS);
                        var leucocitosValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_LEUCOCITOS_DESEABLE;

                        var hematies = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_HEMATIES);
                        var hematiesValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_HEMATIES_DESEABLE;

                        var germenes = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_GERMENES);
                        var germenesValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_GERMENES_DESEABLE;

                        var cilindros = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CILINDROS);
                        var cilindrosValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CILINDROS_DESEABLE;

                        var cristales = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CRISTALES);
                        var cristalesValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_CRISTALES_DESEABLE;

                        var filamentoMucoide = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_FILAMENTO_MUCOIDE);
                        var filamentoMucoideValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_MICROSCOPICO_FILAMENTO_MUCOIDE_DESEABLE;

                        var nitrittos = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_NITRITOS);
                        var nitrittosValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_NITRITOS_DESEABLE;

                        var proteinas = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_PROTEINAS);
                        var proteinasValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_PROTEINAS_DESEABLE;

                        var glucosa = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_GLUCOSA);
                        var glucosaValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_GLUCOSA_DESEABLE;

                        var cetonas = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_CETONAS);
                        var cetonasValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_CETONAS_DESEABLE;

                        var urobilinogeno = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_UROBILINOGENO);
                        var urobilinogenoValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_UROBILINOGENO_DESEABLE;

                        var bilirrubinas = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_BILIRRUBINA);
                        var bilirrubinasValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_BILIRRUBINA_DESEABLE;

                        var sangre = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_SANGRE);
                        var sangreValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_SANGRE_DESEABLE;

                        var hemoglobina = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_HEMOGLOBINA);
                        var hemoglobinaValord = Sigesoft.Common.Constants.EXAMEN_COMPLETO_ORINA_BIOQUIMICO_HEMOGLOBINA_DESEABLE;
              

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        cells.Add(new PdfPCell(new Phrase("EXAMEN FISICO", fontColumnValueNegrita)) { Colspan = 4 });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("COLOR", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(color == null ? string.Empty : color.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(colorValord == null ? string.Empty : colorValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("ASPECTO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(aspecto == null ? string.Empty : aspecto.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(aspectoValord == null ? string.Empty : aspectoValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 3era fila
                        cells.Add(new PdfPCell(new Phrase("DENSIDAD", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(densidad == null ? string.Empty : densidad.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(densidadValord == null ? string.Empty : densidadValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 4ta fila
                        cells.Add(new PdfPCell(new Phrase("PH", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(ph == null ? string.Empty : ph.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(phValord == null ? string.Empty : phValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        cells.Add(new PdfPCell(new Phrase("SEDIMENTO URINARIO", fontColumnValueNegrita)) { Colspan = 4 });
                        
                        // 5ta fila
                        cells.Add(new PdfPCell(new Phrase("CELULAS EPITELEALES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(celulasEpiteleales == null || celulasEpitelealesA == null  ? string.Empty : "De: " + celulasEpiteleales.v_Value1 + " A: " + celulasEpitelealesA.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(celulasEpitelealesValord == null ? string.Empty : celulasEpitelealesValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                       
                        // 6xta fila
                        cells.Add(new PdfPCell(new Phrase("LEUCOCITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(leucocitos == null ? string.Empty : leucocitos.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(leucocitosValord == null ? string.Empty : leucocitosValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 7ma fila
                        cells.Add(new PdfPCell(new Phrase("HEMATIES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(hematies == null ? string.Empty : hematies.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(hematiesValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 8tavo fila
                        cells.Add(new PdfPCell(new Phrase("GERMENES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(germenes == null ? string.Empty : germenes.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(germenesValord == null ? string.Empty : germenesValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        //// 9na fila
                        //cells.Add(new PdfPCell(new Phrase("CILINDROS", fontColumnValue)));
                        //cells.Add(new PdfPCell(new Phrase(cilindros == null ? string.Empty : cilindros.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(cilindrosValord == null ? string.Empty : cilindrosValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 10ma fila
                        cells.Add(new PdfPCell(new Phrase("CRISTALES", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(cristales == null ? string.Empty : cristales.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(cristalesValord == null ? string.Empty : cristalesValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("FILAMENTO MUCOIDE", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(filamentoMucoide == null ? string.Empty : filamentoMucoide.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(filamentoMucoideValord == null ? string.Empty : filamentoMucoideValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        cells.Add(new PdfPCell(new Phrase("EXAMEN BIOQUIMICO", fontColumnValueNegrita)) { Colspan = 4 });
                        
                        // 12va fila
                        cells.Add(new PdfPCell(new Phrase("NITRITOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(nitrittos == null ? string.Empty : nitrittos.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(nitrittosValord == null ? string.Empty : nitrittosValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("PROTEINAS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(proteinas == null ? string.Empty : proteinas.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(proteinasValord == null ? string.Empty : proteinasValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("GLUCOSA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(glucosa == null ? string.Empty : glucosa.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(glucosaValord == null ? string.Empty : glucosaValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("CETONAS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(cetonas == null ? string.Empty : cetonas.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(cetonasValord == null ? string.Empty : cetonasValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("UROBILINOGENO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(urobilinogeno == null ? string.Empty : urobilinogeno.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(urobilinogenoValord == null ? string.Empty : urobilinogenoValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila
                        cells.Add(new PdfPCell(new Phrase("BILIRRUBINAS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(bilirrubinas == null ? string.Empty : bilirrubinas.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(bilirrubinasValord == null ? string.Empty : bilirrubinasValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        //// 11va fila
                        //cells.Add(new PdfPCell(new Phrase("SANGRE", fontColumnValue)));
                        //cells.Add(new PdfPCell(new Phrase(sangre == null ? string.Empty : sangre.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(sangreValord == null ? string.Empty : sangreValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 11va fila NO HAY EN CELIMA
                        //cells.Add(new PdfPCell(new Phrase("HEMOGLOBINA", fontColumnValue)));
                        //cells.Add(new PdfPCell(new Phrase(hemoglobina == null ? string.Empty : hemoglobina.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(hemoglobinaValord == null ? string.Empty : hemoglobinaValord, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.GRUPO_Y_FACTOR_SANGUINEO_ID:

                    #region GRUPO_Y_FACTOR_SANGUINEO

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var grupoSanguineo = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.GRUPO_SANGUINEO_ID);                  
                        var factorSanguineo = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.FACTOR_SANGUINEO_ID);
                                          
                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("GRUPO SANGUINEO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(grupoSanguineo == null ? string.Empty : grupoSanguineo.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("FACTOR SANGUINEO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(factorSanguineo == null ? string.Empty : factorSanguineo.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    
                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_ID:

                    #region PARASITOLOGICO_SIMPLE

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var macroscopico = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_MACROSCOPICO);
                        var microscopico = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_MICROSCOPICO);
                        var parasitoloSimple = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_PARASITOLOGICO_SIMPLE);
                       
                        //var resultado = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SIMPLE_EXAMEN_HECES_RESULTADOS);

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 3ERa fila
                        cells.Add(new PdfPCell(new Phrase("PARASITOLOGICO SIMPLE", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(parasitoloSimple == null ? string.Empty : parasitoloSimple.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        var msgMacros = string.Empty;

                        if (parasitoloSimple != null && Convert.ToInt32(parasitoloSimple.v_Value1) == (int)Sigesoft.Common.PositivoNegativoNoSeRealizo.Positivo)
                        {
                            if (macroscopico != null)
                                msgMacros = macroscopico.v_Value1;
                        }
                       
                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("MACROSCOPICO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(macroscopico == null ? string.Empty : msgMacros, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("MICROSCOPICO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(microscopico == null ? string.Empty : microscopico.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        //// 11va fila
                        //cells.Add(new PdfPCell(new Phrase("RESULTADO", fontColumnValue)));
                        //cells.Add(new PdfPCell(new Phrase(resultado == null ? string.Empty : resultado.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });                      

                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_ID:

                    #region PARASITOLOGICO_SERIADO

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        #region PRIMERA MUESTRA

                        var parasitolo1 = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_PARASITOLOGICO_I);                                        
                        var macros_primera = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_MACROSCOPICO);
                        var micros_primera = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_MICROSCOPICO);                      

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        cells.Add(new PdfPCell(new Phrase("PRIMERA MUESTRA", fontColumnValueNegrita)) { Colspan = 4 });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("PARASITOLOGICO I", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(parasitolo1 == null ? string.Empty : parasitolo1.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 4ta fila
                        cells.Add(new PdfPCell(new Phrase("MACROSCOPICO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(macros_primera == null ? string.Empty : macros_primera.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 5ta fila
                        cells.Add(new PdfPCell(new Phrase("MICROSCOPICO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(micros_primera == null ? string.Empty : micros_primera.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        #endregion

                        #region SEGUNDA MUESTRA

                        // SEGUNDA MUESTRA   
                        var parasitolo2 = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_PARASITOLOGICO_II);
                        var macros_segunda = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_MACROSCOPICO);
                        var micros_segunda = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_SEGUNDA_MICROSCOPICO);                      

                        cells.Add(new PdfPCell(new Phrase("SEGUNDA MUESTRA", fontColumnValueNegrita)) { Colspan = 4 });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("PARASITOLOGICO II", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(parasitolo2 == null ? string.Empty : parasitolo2.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 4ta fila
                        cells.Add(new PdfPCell(new Phrase("MACROSCOPICO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(macros_segunda == null ? string.Empty : macros_segunda.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 5ta fila
                        cells.Add(new PdfPCell(new Phrase("MICROSCOPICO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(micros_segunda == null ? string.Empty : micros_segunda.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        #endregion

                        #region TERCERA MUESTRA


                        // TERCERA MUESTRA                    
                        var parasitolo3 = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_PRIMERA_PARASITOLOGICO_III);
                        var macros_tercera = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_MACROSCOPICO);
                        var micros_tercera = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.PARASITOLOGICO_SERIADO_EXAMEN_HECES_TERCERA_MICROSCOPICO);

                        cells.Add(new PdfPCell(new Phrase("TERCERA MUESTRA", fontColumnValueNegrita)) { Colspan = 4 });

                        // 3era fila
                        cells.Add(new PdfPCell(new Phrase("PARASITOLOGICO III", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(parasitolo3 == null ? string.Empty : parasitolo3.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 4ta fila
                        cells.Add(new PdfPCell(new Phrase("MACROSCOPICO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(macros_tercera == null ? string.Empty : macros_tercera.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 5ta fila
                        cells.Add(new PdfPCell(new Phrase("MICROSCOPICO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(micros_tercera == null ? string.Empty : micros_tercera.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        #endregion

                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.BK_DIRECTO_ID:

                    #region BK_DIRECTO

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var muestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.BK_DIRECTO_MICROBIOLOGICO_MUESTRA);
                        //var colaboracion = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.BK_DIRECTO_MICROBIOLOGICO_COLORACION);
                        var resultados = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.BK_DIRECTO_MICROBIOLOGICO_RESULTADOS);

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("MUESTRA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(muestra == null ? string.Empty : muestra.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("RESULTADOS", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(resultados == null ? string.Empty : resultados.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("NEGATIVO", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("N/A", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID:

                    #region TOXICOLOGICO_COCAINA_MARIHUANA

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var muestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_MUESTRA);
                        //var metodo = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_METODO);
                        //var resultadosCocaina = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_COCAINA);
                        //var resultadosMarihuana = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_MARIHUANA);

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("MARIHUANA - COCAINA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(muestra == null ? string.Empty : muestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        //// 2da fila
                        //cells.Add(new PdfPCell(new Phrase("METODO", fontColumnValue)));
                        //cells.Add(new PdfPCell(new Phrase(metodo == null ? string.Empty : metodo.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        //// 2da fila
                        //cells.Add(new PdfPCell(new Phrase("RESULTADOS COCAINA", fontColumnValue)));
                        //cells.Add(new PdfPCell(new Phrase(resultadosCocaina == null ? string.Empty : resultadosCocaina.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        //// 2da fila
                        //cells.Add(new PdfPCell(new Phrase("RESULTADOS MARIHUANA", fontColumnValue)));
                        //cells.Add(new PdfPCell(new Phrase(resultadosMarihuana == null ? string.Empty : resultadosMarihuana.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        //cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.KOH_AL10_ID://DAVID

                    #region KOH_AL10

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var muestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.KOH_AL10_RESULTADOS);

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("KOH al 10%", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(muestra == null ? string.Empty : muestra.v_Value1Name, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("NEGATIVO", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("N/A", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.ALCOHOLENSALIVA_ID://

                    #region ALCOHOLENSALIVA

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var muestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.ALCOHOLENSALIVA_RESULTADOS);

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Unidades", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("MUESTRA", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(muestra == null ? string.Empty : muestra.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("NEGATIVO", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("N/A", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.HISOPADO_FARINGEO_ID://

                    #region HISOPADO_FARINGEO

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var muestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HISOPADO_FARINGEO_CULTIVO);
                        var resultHISOP = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HISOPADO_FARINGEO_RESULTADOS);

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("DIRECTO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(muestra == null ? string.Empty : muestra.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("N/A", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("CULTIVO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(resultHISOP == null ? string.Empty : resultHISOP.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("N/A", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                case Sigesoft.Common.Constants.HISOPADO_DE_MANOS_ID://

                    #region HISOPADO_MANOS

                    cells = new List<PdfPCell>();

                    // Subtitulo  ******************
                    cell = new PdfPCell(new Phrase(serviceComponent.v_ComponentName.ToUpper(), fontSubTitle))
                    {
                        Colspan = 4,
                        BackgroundColor = SubtitleBackgroundColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    };

                    cells.Add(cell);
                    //*****************************************

                    if (serviceComponent.ServiceComponentFields.Count > 0)
                    {
                        var muestra = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HISOPADO_DE_MANOS_CULTIVO);
                        var resultHISOP = serviceComponent.ServiceComponentFields.Find(p => p.v_ComponentFieldsId == Sigesoft.Common.Constants.HISOPADO_DE_MANOS_RESULTADOS);

                        // titulo
                        cells.Add(new PdfPCell(new Phrase("Examen", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Resultado", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("Valores Deseables", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 1era fila
                        cells.Add(new PdfPCell(new Phrase("DIRECTO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(muestra == null ? string.Empty : muestra.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("N/A", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        // 2da fila
                        cells.Add(new PdfPCell(new Phrase("CULTIVO", fontColumnValue)));
                        cells.Add(new PdfPCell(new Phrase(resultHISOP == null ? string.Empty : resultHISOP.v_Value1, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase(" ", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        cells.Add(new PdfPCell(new Phrase("N/A", fontColumnValue)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        columnWidths = new float[] { 25f, 25f, 25f, 25f };
                    }
                    else
                    {
                        cells.Add(new PdfPCell(new Phrase("No se han registrado datos.", fontColumnValue)));
                        columnWidths = new float[] { 100f };
                    }

                    table = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths);

                    #endregion

                    break;

                default:            

                    break;
            }

            return table;

        }

        #endregion

        #region Utils

        private static List<ServiceComponentList> GetSortEntity(string[] order, List<ServiceComponentList> serviceComponent)
        {
            List<ServiceComponentList> orderEntities = new List<ServiceComponentList>();

            foreach (var op in order)
            {
                var find = serviceComponent.Find(P => P.v_ComponentId == op);

                if (find != null)
                {
                    orderEntities.Add(find);

                    // Eliminar 
                    serviceComponent.Remove(find);
                }
            }

            // Unir la entidades

            orderEntities.AddRange(serviceComponent);

            return orderEntities;
        }

        private static void RunFile(string filePDF)
        {
            Process proceso = Process.Start(filePDF);
            proceso.WaitForExit();
            proceso.Close();

        }

        #endregion   
    }
}

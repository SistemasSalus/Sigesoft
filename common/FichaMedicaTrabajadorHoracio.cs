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
    public class FichaMedicaTrabajadorHoracio
    {

        public static void CreateFichaMedicaTrabajadorHoracio(ServiceList DataService, organizationDto infoEmpresaPropietaria, string filePDF)
        {
            // step 1: creation of a document-object
            Document document = new Document();
            //Document document = new Document(new Rectangle(500f, 300f), 10, 10, 10, 10);
            //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            //Document document = new Document(PageSize.A4, 0, 0, 20, 20);
            try
            {
                            // step 2: we create a writer that listens to the document
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePDF, FileMode.Create));

                //create an instance of your PDFpage class. This is the class we generated above.
                pdfPage page = new pdfPage();
                //set the PageEvent of the pdfWriter instance to the instance of our PDFPage class
                writer.PageEvent = page;

                // step 3: we open the document
                document.Open();
                // step 4: we Add content to the document
                // we define some fonts

                #region Fonts

                Font fontTitle1 = FontFactory.GetFont(FontFactory.HELVETICA, 14, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
                Font fontTitle2 = FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
                Font fontTitleTable = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.White));
                Font fontTitleTableNegro = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));
                Font fontSubTitle = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.White));

                Font fontColumnValue = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));


                #endregion
                PdfPCell CellLogo = null;
                #region Title
                List<PdfPCell> cells = null;
                //PdfPCell CellLogo = null;
                cells = new List<PdfPCell>();
                PdfPCell cellPhoto1 = null;
                float[] columnWidths = null;
                PdfPTable table = null;
                if (DataService.b_Photo != null)
                    cellPhoto1 = new PdfPCell(HandlingItextSharp.GetImage(DataService.b_Photo, null, null, 40, 40)) { HorizontalAlignment = PdfPCell.ALIGN_RIGHT };
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
                new PdfPCell(new Phrase("INFORME MEDICO OCUPACIONAL PARA EL TRABAJADOR", fontTitle1))
                                { HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border= PdfPCell.TOP_BORDER }            
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
                 var subTitleBackGroundColor = new BaseColor(System.Drawing.Color.Gray);
                 string include = string.Empty;
               
                 string[] columnValues = null;
                 string[] columnHeaders = null;
                
                 PdfPTable header2 = new PdfPTable(6);
                 header2.HorizontalAlignment = Element.ALIGN_CENTER;
                 header2.WidthPercentage = 100;
                
                 float[] widths1 = new float[] { 16.6f, 18.6f, 16.6f, 16.6f, 16.6f, 16.6f };
                 header2.SetWidths(widths1);
                
                 PdfPTable companyData = new PdfPTable(6);
                 companyData.HorizontalAlignment = Element.ALIGN_CENTER;
                 companyData.WidthPercentage = 100;
               
                 float[] widthscolumnsCompanyData = new float[] { 16.6f, 16.6f, 16.6f, 16.6f, 16.6f, 16.6f };
                 companyData.SetWidths(widthscolumnsCompanyData);

                 PdfPTable filiationWorker = new PdfPTable(4);
                 PdfPCell cell = null;

                 #endregion

                 var Fecha = DataService.d_ServiceDate.Value.ToShortDateString();
                var LugarExamen = infoEmpresaPropietaria.v_Address;
                var Nombres = DataService.v_Pacient;
                var Apellidos = "";
                var Edad = DataService.i_Edad.ToString();
                var Dni = DataService.v_DocNumber;
                var infoEmpresa = DataService.v_CustomerOrganizationName;
                var puesto = DataService.v_CurrentOccupation;

                 var Linea1 = "Fecha: " + Fecha;
                 var Linea2 = " Lugar de Examen: " + LugarExamen;
                 var Linea3 = "Estimado señor (a / ita): " + Nombres + ", " + Apellidos + ", " + " de" + Edad + " años de edad, con DNI" + Dni + "trabajador de la empresa: " + infoEmpresa + " ó " + "postulante a la empresa " + infoEmpresa + "en el /al puesto:" + puesto + ", mediante la presente, le hacemos conocer los resultados de su examen médico pre ocupacional / periódico / de retiro/  (según corresponda):";


            cells = new List<PdfPCell>()
                 {
                    new PdfPCell(new Phrase(" ", fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED}, 
                    new PdfPCell(new Phrase(" ", fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED}, 
                    new PdfPCell(new Phrase(Linea1, fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED}, 
                    new PdfPCell(new Phrase(Linea2, fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED},   
                     new PdfPCell(new Phrase(" ", fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED}, 
                    new PdfPCell(new Phrase(Linea3, fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED}, 
                     new PdfPCell(new Phrase(" ", fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED}, 
                    new PdfPCell(new Phrase(" ", fontColumnValue)){ HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED}, 

                     new PdfPCell(new Phrase("Los antecedentes médicos que nos ha referido son los siguientes: ", fontTitleTableNegro)){ HorizontalAlignment = PdfPCell.ALIGN_LEFT}, 
                 };
            columnWidths = new float[] { 100f };

            filiationWorker = HandlingItextSharp.GenerateTableFromCells(cells, columnWidths, PdfPCell.NO_BORDER, "", fontTitleTable);

            document.Add(filiationWorker);

                 // step 5: we close the document
                 document.Close();

                 RunFile(filePDF);

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

        private static void RunFile(string filePDF)
        {
            Process proceso = Process.Start(filePDF);
            proceso.WaitForExit();
            proceso.Close();

        }

    }
}

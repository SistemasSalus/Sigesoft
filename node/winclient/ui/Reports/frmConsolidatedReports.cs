using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CrystalDecisions.CrystalReports.Engine;

using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;

namespace Sigesoft.Node.WinClient.UI.Reports
{
    public partial class frmConsolidatedReports : Form
    {
        private string _serviceId;
        private List<string> _componentId;
        DataSet dsGetRepo = null;
        private ServiceBL serviceBL = new ServiceBL();
        crConsolidatedReports rp = null;
        private int _esoTypeId;

        public frmConsolidatedReports()
        {
            InitializeComponent();
        }

        public frmConsolidatedReports(string serviceId, List<string> componentId, int esoTypeId)
        {
            InitializeComponent();
            _serviceId = serviceId;
            _componentId = componentId;
            _esoTypeId = esoTypeId;

            //_serviceId = "N009-SR000000213";
            //_ComponentId = "N002-ME000000045";
        }

        private void frmConsolidatedReports_Load(object sender, EventArgs e)
        {
            using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
            {
                rp = new Reports.crConsolidatedReports();

                foreach (var com in _componentId)
                {
                    ChooseReport(rp, com);
                }

                crystalReportViewer1.EnableDrillDown = false;
                crystalReportViewer1.ReportSource = rp;
                crystalReportViewer1.Show();
            }
        }

        #region Bind Report
        
        private DataSet GetReportAnexo7D()
        {
                         
            var AscensoAlturas = new ServiceBL().ReportAscensoGrandesAlturas(_serviceId, Constants.ALTURA_7D_ID);
            var FuncionesVitales = new ServiceBL().ReportFuncionesVitales(_serviceId, Constants.FUNCIONES_VITALES_ID);
            var Antropometria = new ServiceBL().ReportAntropometria(_serviceId, Constants.ANTROPOMETRIA_ID);

            dsGetRepo = new DataSet("Anexo7D");
            
            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(AscensoAlturas);
            dt.TableName = "dtAnexo7D";
            dsGetRepo.Tables.Add(dt);

            DataTable dt1 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(FuncionesVitales);
            dt1.TableName = "dtFuncionesVitales";
            dsGetRepo.Tables.Add(dt1);

            DataTable dt2 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(Antropometria);
            dt2.TableName = "dtAntropometria";
            dsGetRepo.Tables.Add(dt2);

            return dsGetRepo;        


        }

        private DataSet GetReportAlturaFisica()
        {

            var dataListForReport = new PacientBL().ReportAlturaFisica(_serviceId, Constants.ALTURA_FISICA_M_18);

            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);

            dt.TableName = "dtAlturaFisica";

            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;

        }

        private DataSet GetReportCertificadoAptitud()
        {
            OperationResult objOperationResult = new OperationResult();        

            var dataListForReport = new ServiceBL().GetAptitudeCertificate(ref objOperationResult, _serviceId);

            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);
            dt.TableName = "AptitudeCertificate";
            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;
        }

        // new 02/07/15
        private DataSet GetReportMusculoEsqueletico1()
        {
            var dataListForReport = new ServiceBL().ReportMusculoEsqueletico1(_serviceId, Constants.MUSCULO_ESQUELETICO_1_ID);

            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);

            dt.TableName = "dtMusculoEsqueletico1";

            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;


        }

        // new 10/07/15
        private DataSet GetReportMusculoEsqueletico2()
        {
            var dataListForReport = new ServiceBL().ReportMusculoEsqueletico2(_serviceId, Constants.MÚSCULO_ESQUELÉTICO_2_ID);

            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);

            dt.TableName = "dtMusculoEsqueletico2";

            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;


        }

        // new 13/07/15
        private DataSet GetReportInformePsicologicoOcupacional()
        {
            var dataListForReport = new ServiceBL().ReportInformePsicologicoOcupacional(_serviceId, Constants.EXAMEN_PSICOLOGICO, _esoTypeId);

            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);

            dt.TableName = "dtOccupationalPsychologicalReport";

            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;


        }

        // old
        private DataSet GetReportOsteomuscular1()
        {
            var dataListForReport = new ServiceBL().ReportOsteoMuscular(_serviceId, Constants.OSTEO_MUSCULAR_ID);

            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);

            dt.TableName = "dtOstomuscular";

            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;
           

        }
        //old
        private DataSet GetReportOsteomuscular2()
        {
            var dataListForReport = new PacientBL().GetMusculoEsqueletico(_serviceId, Constants.OSTEO_MUSCULAR_ID);

            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);

            dt.TableName = "dtMusculoEsqueletico";

            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;

        }
        //old
        private DataSet GetReportOsteo()
        {
            var dataListForReport = new ServiceBL().GetReportOsteo(_serviceId, Constants.OSTEO_MUSCULAR_ID);

            dsGetRepo = new DataSet();
            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);
            dt.TableName = "dtOsteo";
            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;
        }

        private DataSet GetReportHistoriaOcupacional()
        {                   
            var dataListForReport = new ServiceBL().ReportHistoriaOcupacional(_serviceId);

            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);

            dt.TableName = "HistoriaOcupacional";

            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;
          
        }

        private DataSet GetReportElectrocardiograma()
        {                         
            var dataListForReport = new ServiceBL().GetReportEstudioElectrocardiografico(_serviceId, Constants.ELECTROCARDIOGRAMA_ID);

            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);
            dt.TableName = "dtEstudioElectrocardiografico";
            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;
               
        }

        private DataSet GetReportPruebaEsfuerzo()
        {                   
            var fichaErgonometrica = new ServiceBL().GetReportPruebaEsfuerzo(_serviceId, Constants.PRUEBA_ESFUERZO_ID);
           
            dsGetRepo = new DataSet();
            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(fichaErgonometrica);
            dt.TableName = "dtFichaErgonometrica";
            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;

        }
      
        private DataSet GetReportOdontologia()
        {
            var Path = Application.StartupPath;
            var dataListForReport = new ServiceBL().ReportOdontograma(_serviceId, Constants.ODONTOGRAMA_ID, Path);

            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);
            dt.TableName = "dtOdontograma";
            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;
        }

        private DataSet GetReportOftalmologia()
        {
            var dataListForReport = new ServiceBL().ReportOftalmologia(_serviceId, Constants.AGUDEZA_VISUAL);
            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);

            dt.TableName = "dtOftalmologia";
            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;


        }

        private DataSet GetReportPsicologia()
        {
            var dataListForReport = new PacientBL().GetFichaPsicologicaOcupacional(_serviceId);

            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);

            dt.TableName = "InformePsico";

            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;

        }

        private DataSet GetReportRX()
        {         
            var rp = new Reports.crInformeRadiologico();

            var dataListForReport = new ServiceBL().ReportRadiologico(_serviceId, Constants.RX_ID);
            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);

            dt.TableName = "dtRadiologico";

            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;

        }

        private DataSet GetReportInformeRadiograficoOIT()
        {       
            var dataListForReport = new ServiceBL().ReportInformeRadiografico(_serviceId, Constants.OIT_ID);

            dsGetRepo = new DataSet();
            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);
            dt.TableName = "dtInformeRadiografico";
            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;

        }

        private DataSet GetReportTamizajeDermatologico()
        {
            var dataListForReport = new ServiceBL().ReportTamizajeDermatologico(_serviceId, Constants.TAMIZAJE_DERMATOLOGIO_ID);

            dsGetRepo = new DataSet();
            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);
            dt.TableName = "dtTamizajeDermatologico";
            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;
        }

        private DataSet GetReportGinecologia()
        {
            var dataListForReport = new ServiceBL().GetReportEvaluacionGinecologico(_serviceId, Constants.GINECOLOGIA_ID);

            dsGetRepo = new DataSet();
            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);
            dt.TableName = "dtEvaGinecologico";
            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;
        }

        private DataSet GetReportEspirometriaCuestionario()
        {
            var dataListForReport = new ServiceBL().GetReportCuestionarioEspirometria(_serviceId, Constants.ESPIROMETRIA_ID);

            dsGetRepo = new DataSet();
            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);
            dt.TableName = "dtCuestionarioEspirometria";
            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;
        }

        private DataSet GetReportEspirometria()
        {
            var dataListForReport = new ServiceBL().GetReportInformeEspirometria(_serviceId, Constants.ESPIROMETRIA_ID);

            dsGetRepo = new DataSet();
            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);
            dt.TableName = "dtInformeEspirometria";
            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;
        }

        private DataSet GetReportEvaluacionPsicolaboralPersonal()
        {
            var dataListForReport = new ServiceBL().GetReportEvaluacionPsicolaborlaPersonal(_serviceId, Constants.EVALUACION_PSICOLABORAL);

            dsGetRepo = new DataSet();
            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);
            dt.TableName = "dtEvaluacionPsicolaboralPersonal";
            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;
        }

        private DataSet GetReportAudiometria()
        {
            try
            {
                var serviceBL = new ServiceBL();
                DataSet dsAudiometria = new DataSet();

                var dxList = serviceBL.GetDiagnosticRepositoryByComponent(_serviceId, Constants.AUDIOMETRIA_ID);
                var dtDx = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dxList);
                dtDx.TableName = "dtDiagnostic";
                dsAudiometria.Tables.Add(dtDx);

                //// Obtener las recomendaciones (removiendo las duplicadas)
                //var recom = dxList.SelectMany(s => s.Recomendations)
                //                  .GroupBy(x => x.v_RecommendationName)
                //                  .Select(group => group.First())
                //                  .ToList();

                var recom = dxList.SelectMany(s => s.Recomendations).ToList();

                //DataTable dtRecom = new DataTable();
                //dtRecom.TableName = "dtRecomendation";
                //dtRecom.Columns.Add("v_RecommendationName", typeof(string));

                //foreach (var item in recom)
                //{
                //    var row = dtRecom.NewRow();
                //    row["v_RecommendationName"] = item.v_RecommendationName;
                //    dtRecom.Rows.Add(row);
                //}

                //dsAudiometria.Tables.Add(dtRecom);

                var dtReco = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(recom);
                dtReco.TableName = "dtRecomendation";
                dsAudiometria.Tables.Add(dtReco); 
          
                //-------******************************************************************************************

                var audioUserControlList = serviceBL.ReportAudiometriaUserControl(_serviceId, Constants.AUDIOMETRIA_ID);
                //aqui hay error corregir despues del cine
                var audioCabeceraList = serviceBL.ReportAudiometria(_serviceId, Constants.AUDIOMETRIA_ID);

                var dtAudiometriaUserControl = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(audioUserControlList);

                var dtCabecera = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(audioCabeceraList);

               
                dtCabecera.TableName = "dtAudiometria";
                dtAudiometriaUserControl.TableName = "dtAudiometriaUserControl";

                dsAudiometria.Tables.Add(dtCabecera);
                dsAudiometria.Tables.Add(dtAudiometriaUserControl);
             


                return dsAudiometria;
            }
            catch (Exception)
            {

                throw;
            }


        }

        private DataSet GetReportHistoriaOcupacionalAudiometria()
        {
            var dataListForReport = serviceBL.ReportHistoriaOcupacionalAudiometria(_serviceId);

            dsGetRepo = new DataSet();

            DataTable dt = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport);

            dt.TableName = "dtHistoriaOcupacional";

            dsGetRepo.Tables.Add(dt);

            return dsGetRepo;

        }

        #endregion

        private void ChooseReport(crConsolidatedReports rp, string componentId)
        {
            //try
            //{
                DataSet ds = null;

                switch (componentId)
                {
                    case Constants.INFORME_CERTIFICADO_APTITUD:
                        ds = GetReportCertificadoAptitud();
                        rp.Subreports["crOccupationalMedicalAptitudeCertificate.rpt"].SetDataSource(ds);
                        rp.SectionCertificadoAptitud.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.MUS_ESQUE:
                        // 1
                        ds = GetReportMusculoEsqueletico1();
                        rp.Subreports["crMusculoEsqueletico1.rpt"].SetDataSource(ds);
                        rp.SectionOsteomuscular1.SectionFormat.EnableSuppress = false;

                        // 2
                        ds = GetReportMusculoEsqueletico2();
                        rp.Subreports["crMusculoEsqueletico2.rpt"].SetDataSource(ds);
                        rp.SectionOsteomuscular2.SectionFormat.EnableSuppress = false;

                        break;
                    case Constants.INFORME_HISTORIA_OCUPACIONAL:
                        ds = GetReportHistoriaOcupacional();
                        rp.Subreports["crHistoriaOcupacional.rpt"].SetDataSource(ds);
                        rp.SectionHistoriaOcupacional.SectionFormat.EnableSuppress = false;
                        rp.SectionHistoriaOcupacional.SectionFormat.PageOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                        rp.SectionHistoriaOcupacional.ReportObjects["SubReportHistoriaOcupacional"].Width = 15905;
                        break;
                    case Constants.ALTURA_7D_ID:
                        ds = GetReportAnexo7D();
                        rp.Subreports["crAnexo7D.rpt"].SetDataSource(ds);
                        rp.SectionAnexo7D.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.ALTURA_FISICA_M_18:
                        ds = GetReportAlturaFisica();
                        rp.Subreports["crAlturaFisica.rpt"].SetDataSource(ds);
                        rp.SectionAlturaEstructural.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.LABORATORIO_ID:      // Falta implementar
                        //rp.SectionLaboratorio.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.ELECTROCARDIOGRAMA_ID:
                        ds = GetReportElectrocardiograma();
                        rp.Subreports["crEstudioElectrocardiografico.rpt"].SetDataSource(ds);
                        rp.SectionElectrocardiograma.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.PRUEBA_ESFUERZO_ID:
                        ds = GetReportPruebaEsfuerzo();
                        rp.Subreports["crFichaErgonometrica.rpt"].SetDataSource(ds);
                        rp.SectionPruebaEsfuerzo.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.ODONTOGRAMA_ID:
                        ds = GetReportOdontologia();
                        rp.Subreports["crOdontograma2016.rpt"].SetDataSource(ds);
                        rp.SectionOdontologia.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.AUDIOMETRIA_ID:      // Falta implementar
                        ds = GetReportAudiometria();
                        //rp.Subreports["crFichaAudiometria.rpt"].SetDataSource(ds);
                        rp.Subreports["crFichaAudiometria2016.rpt"].SetDataSource(ds);
                        rp.SectionAudiometria.SectionFormat.EnableSuppress = false;

                        // Historia Ocupacional Audiometria
                        ds = GetReportHistoriaOcupacionalAudiometria();
                        rp.Subreports["crHistoriaOcupacionalAudiometria.rpt"].SetDataSource(ds);
                        rp.SectionHistoriaOcupacionalAudiometria.SectionFormat.EnableSuppress = false;
                        rp.SectionHistoriaOcupacionalAudiometria.SectionFormat.PageOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                        rp.SectionHistoriaOcupacionalAudiometria.ReportObjects["SubReportHistoriaOcupacionalAudiometria"].Width = 15905;
                        break;
                    case Constants.GINECOLOGIA_ID:      // Falta implementar
                        ds = GetReportGinecologia();
                        rp.Subreports["crEvaluacionGenecologica.rpt"].SetDataSource(ds);
                        rp.SectionGinecologia.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.OFT:
                        ds = GetReportOftalmologia();
                        rp.Subreports["crInformeOftalmologico.rpt"].SetDataSource(ds);

                        var xExploracionClinicaEstaEnProtolo = (bool)ds.Tables["dtOftalmologia"].Rows[0]["ExploracionClinicaEstaEnProtolo"];
                        var xVisionColoresEstaEnProtolo = (bool)ds.Tables["dtOftalmologia"].Rows[0]["VisionColoresEstaEnProtolo"];
                        var xVisionEstereoscopicaEstaEnProtolo = (bool)ds.Tables["dtOftalmologia"].Rows[0]["VisionEstereoscopicaEstaEnProtolo"];
                        var xCampoVisualEstaEnProtolo = (bool)ds.Tables["dtOftalmologia"].Rows[0]["CampoVisualEstaEnProtolo"];
                        var xPresionIntraocularEstaEnProtolo = (bool)ds.Tables["dtOftalmologia"].Rows[0]["PresionIntraocularEstaEnProtolo"];
                        var xFondoOjoEstaEnProtolo = (bool)ds.Tables["dtOftalmologia"].Rows[0]["FondoOjoEstaEnProtolo"];
                        var xRefraccionEstaEnProtolo = (bool)ds.Tables["dtOftalmologia"].Rows[0]["RefraccionEstaEnProtolo"];
                        var xAgudezaVisualEstaEnProtolo = (bool)ds.Tables["dtOftalmologia"].Rows[0]["AgudezaVisualEstaEnProtolo"];

                        if (xExploracionClinicaEstaEnProtolo == true)
                            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionExploracionClinica"].SectionFormat.EnableSuppress = false;

                        if (xVisionColoresEstaEnProtolo == true)
                            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionVisionColores"].SectionFormat.EnableSuppress = false;

                        if (xVisionEstereoscopicaEstaEnProtolo == true)
                            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionVisionEsteroscopica"].SectionFormat.EnableSuppress = false;

                        if (xCampoVisualEstaEnProtolo == true)
                            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionCampoVisual"].SectionFormat.EnableSuppress = false;

                        if (xPresionIntraocularEstaEnProtolo == true)
                            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionPresionIntraocular"].SectionFormat.EnableSuppress = false;

                        if (xFondoOjoEstaEnProtolo == true)
                            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionFondoOjo"].SectionFormat.EnableSuppress = false;

                        if (xRefraccionEstaEnProtolo == true)
                            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionRefraccion"].SectionFormat.EnableSuppress = false;

                        if (xAgudezaVisualEstaEnProtolo == true)
                            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionAgudezaVisual"].SectionFormat.EnableSuppress = false;

                        rp.SectionOftalmologia.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.EXAMEN_PSICOLOGICO:
                        ds = GetReportInformePsicologicoOcupacional();
                        rp.Subreports["crInformePsicologicoOcupacional.rpt"].SetDataSource(ds);

                        //var esoTypeId = (int)ds.Tables["dtOccupationalPsychologicalReport"].Rows[0]["i_EsoTypeId"];
                        var cb_GrupoOcupacional = Convert.ToInt32(ds.Tables["dtOccupationalPsychologicalReport"].Rows[0]["cb_GrupoOcupacional"]);

                        if (_esoTypeId == (int)TypeESO.PreOcupacional)
                        {
                            rp.Subreports["crInformePsicologicoOcupacional.rpt"].ReportDefinition.Sections["SectionResultadoEvaluacion_EMPO"].SectionFormat.EnableSuppress = false;
                        }
                        else if (_esoTypeId == (int)TypeESO.PeriodicoAnual)
                        {
                            if (cb_GrupoOcupacional == (int)GrupoOcupacional_EMOA_Psicologia.OperariosYTecnicos)
                            {
                                rp.Subreports["crInformePsicologicoOcupacional.rpt"].ReportDefinition.Sections["SectionResultadoEvaluacion_EMOA_OperariosTecnicos"].SectionFormat.EnableSuppress = false;
                            }
                            else if (cb_GrupoOcupacional == (int)GrupoOcupacional_EMOA_Psicologia.Conductores)
                            {
                                rp.Subreports["crInformePsicologicoOcupacional.rpt"].ReportDefinition.Sections["SectionResultadoEvaluacion_EMOA_Conductores"].SectionFormat.EnableSuppress = false;
                            }
                            else if (cb_GrupoOcupacional == (int)GrupoOcupacional_EMOA_Psicologia.Universitarios)
                            {
                                rp.Subreports["crInformePsicologicoOcupacional.rpt"].ReportDefinition.Sections["SectionResultadoEvaluacion_EMOA_UniversitariosAdministrativos"].SectionFormat.EnableSuppress = false;
                            }
                        }
                        else if (_esoTypeId == (int)TypeESO.Retiro)
                        {

                        }

                        rp.SectionPsicologia.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.RX_ID:
                        ds = GetReportRX();
                        rp.Subreports["crInformeRadiologico.rpt"].SetDataSource(ds);
                        rp.SectionRayosX.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.INFORME_RADIOGRAFICO_OIT:
                        ds = GetReportInformeRadiograficoOIT();
                        rp.Subreports["crInformeRadiograficoOIT.rpt"].SetDataSource(ds);
                        rp.SectionRayosXOIT.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.TAMIZAJE_DERMATOLOGIO_ID:
                        ds = GetReportTamizajeDermatologico();
                        rp.Subreports["crTamizajeDermatologico.rpt"].SetDataSource(ds);
                        rp.SectionTamizajeDermatologico.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.ESPIROMETRIA_CUESTIONARIO_ID:
                        ds = GetReportEspirometriaCuestionario();
                        rp.Subreports["crCuestionarioEspirometria2016.rpt"].SetDataSource(ds);
                        rp.SectionEspirometriaCuestionario.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.ESPIROMETRIA_ID:
                        ds = GetReportEspirometria();
                        rp.Subreports["crInformeEspirometria.rpt"].SetDataSource(ds);
                        rp.SectionEspirometria.SectionFormat.EnableSuppress = false;
                        break;
                    case Constants.EVALUACION_PSICOLABORAL:
                        ds = GetReportEvaluacionPsicolaboralPersonal();
                        rp.Subreports["crEvaluacionPsicolaboralPersonal.rpt"].SetDataSource(ds);
                        rp.SectionEvaluacionPsicolaboralPersonal.SectionFormat.EnableSuppress = false;
                        break;
                    default:
                        break;
                }
            //}
            //catch (Exception ex)
            //{
                
            //    throw ex;
            //}
           
        }

        private void frmConsolidatedReports_FormClosing(object sender, FormClosingEventArgs e)
        {
            rp.Close();
            rp.Dispose();
            crystalReportViewer1.Dispose();
        }

        //private void frmConsolidatedReports_Activated(object sender, EventArgs e)
        //{
        //    this.TopMost = true;
        //    this.TopMost = false;
        //}

    }
}

namespace Sigesoft.Node.WinClient.UI
{
    partial class frmMedicalExam
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_MedicalExamId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_Name", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_CategoryName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_ComponentTypeName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_DiagnosableName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_CreationUser");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("d_CreationDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_UpdateUser");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("d_UpdateDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("i_UIIndex");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_MedicalExamFieldsId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_TextLabel", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_IsRequired");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_MeasurementUnitName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_CreationUser");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("d_CreationDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn24 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_UpdateUser");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("d_UpdateDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("i_Order");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_Group");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_DefaultText");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_ComponentFieldId");
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMedicalExam));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ddlCategoryId = new ComboTreeBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grdDataMedicalExam = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuMedicalExam = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuGridNewMedicalExam = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGridEditMedicalExam = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGridDeleteMedicalExam = new System.Windows.Forms.ToolStripMenuItem();
            this.lblRecordCountMedicalExam = new System.Windows.Forms.Label();
            this.lblRecordCountMedicalExamFields = new System.Windows.Forms.Label();
            this.grdDataMedicalExamFields = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuMedicalExamFields = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuGridNewMedicalExamField = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGridEditMedicalExamField = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGridDeleteMedicalExamField = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button7 = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCloneComponent = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDataMedicalExam)).BeginInit();
            this.contextMenuMedicalExam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDataMedicalExamFields)).BeginInit();
            this.contextMenuMedicalExamFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ddlCategoryId);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnFilter);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.MediumBlue;
            this.groupBox1.Location = new System.Drawing.Point(15, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(649, 54);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Búsqueda / Filtro";
            // 
            // ddlCategoryId
            // 
            this.ddlCategoryId.DropDownHeight = 500;
            this.ddlCategoryId.DroppedDown = false;
            this.ddlCategoryId.Location = new System.Drawing.Point(86, 23);
            this.ddlCategoryId.Margin = new System.Windows.Forms.Padding(2);
            this.ddlCategoryId.Name = "ddlCategoryId";
            this.ddlCategoryId.SelectedNode = null;
            this.ddlCategoryId.ShowPath = true;
            this.ddlCategoryId.Size = new System.Drawing.Size(230, 20);
            this.ddlCategoryId.TabIndex = 11;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(397, 23);
            this.txtName.MaxLength = 250;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(148, 20);
            this.txtName.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(342, 23);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 19);
            this.label5.TabIndex = 10;
            this.label5.Text = "Nombre";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnFilter
            // 
            this.btnFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.ForeColor = System.Drawing.Color.Black;
            this.btnFilter.Image = global::Sigesoft.Node.WinClient.UI.Resources.find;
            this.btnFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFilter.Location = new System.Drawing.Point(563, 20);
            this.btnFilter.Margin = new System.Windows.Forms.Padding(2);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(60, 24);
            this.btnFilter.TabIndex = 2;
            this.btnFilter.Text = "Filtrar";
            this.btnFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Categoría";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grdDataMedicalExam);
            this.groupBox2.Controls.Add(this.lblRecordCountMedicalExam);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(797, 178);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Resultados de Componentes";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // grdDataMedicalExam
            // 
            this.grdDataMedicalExam.CausesValidation = false;
            this.grdDataMedicalExam.ContextMenuStrip = this.contextMenuMedicalExam;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.Silver;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdDataMedicalExam.DisplayLayout.Appearance = appearance1;
            ultraGridColumn1.Header.Caption = "Id Examen";
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Width = 147;
            ultraGridColumn2.Header.Caption = "Examen";
            ultraGridColumn2.Header.VisiblePosition = 2;
            ultraGridColumn2.Width = 298;
            ultraGridColumn4.Header.Caption = "Categoría";
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Width = 247;
            ultraGridColumn3.Header.Caption = "Tipo Componente";
            ultraGridColumn3.Header.VisiblePosition = 5;
            ultraGridColumn6.Header.Caption = "Diagnosticable";
            ultraGridColumn6.Header.VisiblePosition = 4;
            ultraGridColumn5.Header.Caption = "Usuario Crea.";
            ultraGridColumn5.Header.VisiblePosition = 6;
            ultraGridColumn5.Width = 125;
            ultraGridColumn10.Format = "dd/MM/yyyy hh:mm tt";
            ultraGridColumn10.Header.Caption = "Fecha Crea.";
            ultraGridColumn10.Header.VisiblePosition = 7;
            ultraGridColumn10.Width = 150;
            ultraGridColumn12.Header.Caption = "Usuario Act.";
            ultraGridColumn12.Header.VisiblePosition = 8;
            ultraGridColumn12.Width = 125;
            ultraGridColumn13.Format = "dd/MM/yyyy hh:mm tt";
            ultraGridColumn13.Header.Caption = "Fecha Act.";
            ultraGridColumn13.Header.VisiblePosition = 9;
            ultraGridColumn13.Width = 150;
            ultraGridColumn11.Header.Caption = "Orden";
            ultraGridColumn11.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn4,
            ultraGridColumn3,
            ultraGridColumn6,
            ultraGridColumn5,
            ultraGridColumn10,
            ultraGridColumn12,
            ultraGridColumn13,
            ultraGridColumn11});
            this.grdDataMedicalExam.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDataMedicalExam.DisplayLayout.InterBandSpacing = 10;
            this.grdDataMedicalExam.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDataMedicalExam.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdDataMedicalExam.DisplayLayout.NewColumnLoadStyle = Infragistics.Win.UltraWinGrid.NewColumnLoadStyle.Hide;
            this.grdDataMedicalExam.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdDataMedicalExam.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdDataMedicalExam.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdDataMedicalExam.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this.grdDataMedicalExam.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdDataMedicalExam.DisplayLayout.Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.grdDataMedicalExam.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.Color.White;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            this.grdDataMedicalExam.DisplayLayout.Override.CellAppearance = appearance3;
            this.grdDataMedicalExam.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.Gainsboro;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.BorderColor = System.Drawing.Color.DarkGray;
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.ForegroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance4.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDataMedicalExam.DisplayLayout.Override.HeaderAppearance = appearance4;
            this.grdDataMedicalExam.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance5.AlphaLevel = ((short)(187));
            appearance5.BackColor = System.Drawing.Color.Gainsboro;
            appearance5.BackColor2 = System.Drawing.Color.Gainsboro;
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.ForegroundAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDataMedicalExam.DisplayLayout.Override.RowAlternateAppearance = appearance5;
            appearance6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdDataMedicalExam.DisplayLayout.Override.RowSelectorAppearance = appearance6;
            this.grdDataMedicalExam.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance7.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            appearance7.BackColor2 = System.Drawing.SystemColors.GradientInactiveCaption;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance7.BorderColor2 = System.Drawing.SystemColors.GradientActiveCaption;
            appearance7.BorderColor3DBase = System.Drawing.SystemColors.GradientActiveCaption;
            appearance7.FontData.BoldAsString = "False";
            appearance7.ForeColor = System.Drawing.Color.Black;
            this.grdDataMedicalExam.DisplayLayout.Override.SelectedRowAppearance = appearance7;
            this.grdDataMedicalExam.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDataMedicalExam.DisplayLayout.RowConnectorColor = System.Drawing.SystemColors.ControlDarkDark;
            this.grdDataMedicalExam.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Dashed;
            this.grdDataMedicalExam.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdDataMedicalExam.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdDataMedicalExam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDataMedicalExam.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdDataMedicalExam.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.grdDataMedicalExam.Location = new System.Drawing.Point(2, 15);
            this.grdDataMedicalExam.Margin = new System.Windows.Forms.Padding(2);
            this.grdDataMedicalExam.Name = "grdDataMedicalExam";
            this.grdDataMedicalExam.Size = new System.Drawing.Size(793, 161);
            this.grdDataMedicalExam.TabIndex = 43;
            this.grdDataMedicalExam.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdDataMedicalExam_InitializeLayout);
            this.grdDataMedicalExam.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdDataMedicalExam_AfterSelectChange);
            this.grdDataMedicalExam.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdDataMedicalExam_MouseDown);
            // 
            // contextMenuMedicalExam
            // 
            this.contextMenuMedicalExam.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGridNewMedicalExam,
            this.mnuGridEditMedicalExam,
            this.mnuGridDeleteMedicalExam});
            this.contextMenuMedicalExam.Name = "contextMenuStrip1";
            this.contextMenuMedicalExam.Size = new System.Drawing.Size(126, 70);
            // 
            // mnuGridNewMedicalExam
            // 
            this.mnuGridNewMedicalExam.Image = global::Sigesoft.Node.WinClient.UI.Resources.add;
            this.mnuGridNewMedicalExam.Name = "mnuGridNewMedicalExam";
            this.mnuGridNewMedicalExam.Size = new System.Drawing.Size(125, 22);
            this.mnuGridNewMedicalExam.Text = "Nuevo";
            this.mnuGridNewMedicalExam.Click += new System.EventHandler(this.mnuGridNewMedicalExam_Click);
            // 
            // mnuGridEditMedicalExam
            // 
            this.mnuGridEditMedicalExam.Image = global::Sigesoft.Node.WinClient.UI.Resources.pencil;
            this.mnuGridEditMedicalExam.Name = "mnuGridEditMedicalExam";
            this.mnuGridEditMedicalExam.Size = new System.Drawing.Size(125, 22);
            this.mnuGridEditMedicalExam.Text = "Modificar";
            this.mnuGridEditMedicalExam.Click += new System.EventHandler(this.mnuGridEditMedicalExam_Click);
            // 
            // mnuGridDeleteMedicalExam
            // 
            this.mnuGridDeleteMedicalExam.Image = global::Sigesoft.Node.WinClient.UI.Resources.system_close;
            this.mnuGridDeleteMedicalExam.Name = "mnuGridDeleteMedicalExam";
            this.mnuGridDeleteMedicalExam.Size = new System.Drawing.Size(125, 22);
            this.mnuGridDeleteMedicalExam.Text = "Eliminar";
            this.mnuGridDeleteMedicalExam.Click += new System.EventHandler(this.mnuGridDeleteMedicalExam_Click);
            // 
            // lblRecordCountMedicalExam
            // 
            this.lblRecordCountMedicalExam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecordCountMedicalExam.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordCountMedicalExam.Location = new System.Drawing.Point(958, 15);
            this.lblRecordCountMedicalExam.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRecordCountMedicalExam.Name = "lblRecordCountMedicalExam";
            this.lblRecordCountMedicalExam.Size = new System.Drawing.Size(231, 19);
            this.lblRecordCountMedicalExam.TabIndex = 1;
            this.lblRecordCountMedicalExam.Text = "No se ha realizado la búsqueda aún.";
            this.lblRecordCountMedicalExam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRecordCountMedicalExamFields
            // 
            this.lblRecordCountMedicalExamFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecordCountMedicalExamFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordCountMedicalExamFields.Location = new System.Drawing.Point(566, 21);
            this.lblRecordCountMedicalExamFields.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRecordCountMedicalExamFields.Name = "lblRecordCountMedicalExamFields";
            this.lblRecordCountMedicalExamFields.Size = new System.Drawing.Size(231, 19);
            this.lblRecordCountMedicalExamFields.TabIndex = 2;
            this.lblRecordCountMedicalExamFields.Text = "No se ha realizado la búsqueda aún.";
            this.lblRecordCountMedicalExamFields.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRecordCountMedicalExamFields.Click += new System.EventHandler(this.lblRecordCountMedicalExamFields_Click);
            // 
            // grdDataMedicalExamFields
            // 
            this.grdDataMedicalExamFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdDataMedicalExamFields.CausesValidation = false;
            this.grdDataMedicalExamFields.ContextMenuStrip = this.contextMenuMedicalExamFields;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.Silver;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdDataMedicalExamFields.DisplayLayout.Appearance = appearance8;
            ultraGridColumn18.Header.Caption = "Id Campo";
            ultraGridColumn18.Header.VisiblePosition = 1;
            ultraGridColumn18.Width = 140;
            ultraGridColumn19.Header.Caption = "Nombre";
            ultraGridColumn19.Header.VisiblePosition = 2;
            ultraGridColumn19.Width = 294;
            ultraGridColumn20.Header.Caption = "Es Obligatorio";
            ultraGridColumn20.Header.VisiblePosition = 6;
            ultraGridColumn21.Header.Caption = "Unidad Medida";
            ultraGridColumn21.Header.VisiblePosition = 7;
            ultraGridColumn22.Header.Caption = "Usuario Crea.";
            ultraGridColumn22.Header.VisiblePosition = 8;
            ultraGridColumn22.Width = 125;
            ultraGridColumn23.Format = "dd/MM/yyyy hh:mm tt";
            ultraGridColumn23.Header.Caption = "Fecha Crea.";
            ultraGridColumn23.Header.VisiblePosition = 9;
            ultraGridColumn23.Width = 150;
            ultraGridColumn24.Header.Caption = "Usuario Act.";
            ultraGridColumn24.Header.VisiblePosition = 10;
            ultraGridColumn24.Width = 125;
            ultraGridColumn25.Format = "dd/MM/yyyy hh:mm tt";
            ultraGridColumn25.Header.Caption = "Fecha Act.";
            ultraGridColumn25.Header.VisiblePosition = 11;
            ultraGridColumn25.Width = 150;
            ultraGridColumn7.Header.Caption = "Orden";
            ultraGridColumn7.Header.VisiblePosition = 4;
            ultraGridColumn8.Header.Caption = "Grupo";
            ultraGridColumn8.Header.VisiblePosition = 3;
            ultraGridColumn8.Width = 246;
            ultraGridColumn9.Header.Caption = "Valor por Defecto";
            ultraGridColumn9.Header.VisiblePosition = 5;
            ultraGridColumn14.Header.VisiblePosition = 0;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn18,
            ultraGridColumn19,
            ultraGridColumn20,
            ultraGridColumn21,
            ultraGridColumn22,
            ultraGridColumn23,
            ultraGridColumn24,
            ultraGridColumn25,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn14});
            this.grdDataMedicalExamFields.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdDataMedicalExamFields.DisplayLayout.InterBandSpacing = 10;
            this.grdDataMedicalExamFields.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDataMedicalExamFields.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdDataMedicalExamFields.DisplayLayout.NewColumnLoadStyle = Infragistics.Win.UltraWinGrid.NewColumnLoadStyle.Hide;
            this.grdDataMedicalExamFields.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdDataMedicalExamFields.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdDataMedicalExamFields.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdDataMedicalExamFields.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this.grdDataMedicalExamFields.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdDataMedicalExamFields.DisplayLayout.Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance9.BackColor = System.Drawing.Color.Transparent;
            this.grdDataMedicalExamFields.DisplayLayout.Override.CardAreaAppearance = appearance9;
            appearance10.BackColor = System.Drawing.SystemColors.Control;
            appearance10.BackColor2 = System.Drawing.SystemColors.ControlLightLight;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            this.grdDataMedicalExamFields.DisplayLayout.Override.CellAppearance = appearance10;
            this.grdDataMedicalExamFields.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.LightGray;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance11.BorderColor = System.Drawing.Color.DarkGray;
            appearance11.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDataMedicalExamFields.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.grdDataMedicalExamFields.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance12.AlphaLevel = ((short)(187));
            appearance12.BackColor = System.Drawing.Color.Gainsboro;
            appearance12.BackColor2 = System.Drawing.Color.Gainsboro;
            appearance12.ForeColor = System.Drawing.Color.Black;
            appearance12.ForegroundAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDataMedicalExamFields.DisplayLayout.Override.RowAlternateAppearance = appearance12;
            appearance13.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdDataMedicalExamFields.DisplayLayout.Override.RowSelectorAppearance = appearance13;
            this.grdDataMedicalExamFields.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance14.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            appearance14.BackColor2 = System.Drawing.SystemColors.GradientInactiveCaption;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance14.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            appearance14.BorderColor2 = System.Drawing.SystemColors.GradientActiveCaption;
            appearance14.FontData.BoldAsString = "False";
            appearance14.ForeColor = System.Drawing.Color.Black;
            this.grdDataMedicalExamFields.DisplayLayout.Override.SelectedRowAppearance = appearance14;
            this.grdDataMedicalExamFields.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDataMedicalExamFields.DisplayLayout.RowConnectorColor = System.Drawing.SystemColors.ControlDarkDark;
            this.grdDataMedicalExamFields.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Dashed;
            this.grdDataMedicalExamFields.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdDataMedicalExamFields.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdDataMedicalExamFields.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdDataMedicalExamFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdDataMedicalExamFields.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.grdDataMedicalExamFields.Location = new System.Drawing.Point(2, 42);
            this.grdDataMedicalExamFields.Margin = new System.Windows.Forms.Padding(2);
            this.grdDataMedicalExamFields.Name = "grdDataMedicalExamFields";
            this.grdDataMedicalExamFields.Size = new System.Drawing.Size(795, 161);
            this.grdDataMedicalExamFields.TabIndex = 44;
            this.grdDataMedicalExamFields.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdDataMedicalExamFields_InitializeLayout);
            this.grdDataMedicalExamFields.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdDataMedicalExamFields_MouseDown);
            // 
            // contextMenuMedicalExamFields
            // 
            this.contextMenuMedicalExamFields.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGridNewMedicalExamField,
            this.mnuGridEditMedicalExamField,
            this.mnuGridDeleteMedicalExamField,
            this.toolStripSeparator1,
            this.importarToolStripMenuItem});
            this.contextMenuMedicalExamFields.Name = "contextMenuStrip1";
            this.contextMenuMedicalExamFields.Size = new System.Drawing.Size(126, 98);
            // 
            // mnuGridNewMedicalExamField
            // 
            this.mnuGridNewMedicalExamField.Image = global::Sigesoft.Node.WinClient.UI.Resources.add;
            this.mnuGridNewMedicalExamField.Name = "mnuGridNewMedicalExamField";
            this.mnuGridNewMedicalExamField.Size = new System.Drawing.Size(125, 22);
            this.mnuGridNewMedicalExamField.Text = "Nuevo";
            this.mnuGridNewMedicalExamField.Click += new System.EventHandler(this.mnuGridNewMedicalExamField_Click);
            // 
            // mnuGridEditMedicalExamField
            // 
            this.mnuGridEditMedicalExamField.Image = global::Sigesoft.Node.WinClient.UI.Resources.pencil;
            this.mnuGridEditMedicalExamField.Name = "mnuGridEditMedicalExamField";
            this.mnuGridEditMedicalExamField.Size = new System.Drawing.Size(125, 22);
            this.mnuGridEditMedicalExamField.Text = "Modificar";
            this.mnuGridEditMedicalExamField.Click += new System.EventHandler(this.mnuGridEditMedicalExamField_Click);
            // 
            // mnuGridDeleteMedicalExamField
            // 
            this.mnuGridDeleteMedicalExamField.Image = global::Sigesoft.Node.WinClient.UI.Resources.system_close;
            this.mnuGridDeleteMedicalExamField.Name = "mnuGridDeleteMedicalExamField";
            this.mnuGridDeleteMedicalExamField.Size = new System.Drawing.Size(125, 22);
            this.mnuGridDeleteMedicalExamField.Text = "Eliminar";
            this.mnuGridDeleteMedicalExamField.Click += new System.EventHandler(this.mnuGridDeleteMedicalExamField_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(122, 6);
            // 
            // importarToolStripMenuItem
            // 
            this.importarToolStripMenuItem.Image = global::Sigesoft.Node.WinClient.UI.Resources.brick_go;
            this.importarToolStripMenuItem.Name = "importarToolStripMenuItem";
            this.importarToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.importarToolStripMenuItem.Text = "Importar";
            this.importarToolStripMenuItem.Click += new System.EventHandler(this.importarToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(16, 72);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblRecordCountMedicalExamFields);
            this.splitContainer1.Panel2.Controls.Add(this.grdDataMedicalExamFields);
            this.splitContainer1.Size = new System.Drawing.Size(797, 387);
            this.splitContainer1.SplitterDistance = 178;
            this.splitContainer1.TabIndex = 2;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button7.Image = global::Sigesoft.Node.WinClient.UI.Resources.delete;
            this.button7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button7.Location = new System.Drawing.Point(828, 143);
            this.button7.Margin = new System.Windows.Forms.Padding(2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 24);
            this.button7.TabIndex = 103;
            this.button7.Text = "Eliminar";
            this.button7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditar.BackColor = System.Drawing.SystemColors.Control;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEditar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditar.ForeColor = System.Drawing.Color.Black;
            this.btnEditar.Image = global::Sigesoft.Node.WinClient.UI.Resources.application_edit;
            this.btnEditar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditar.Location = new System.Drawing.Point(828, 115);
            this.btnEditar.Margin = new System.Windows.Forms.Padding(2);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(75, 24);
            this.btnEditar.TabIndex = 101;
            this.btnEditar.Text = "     Editar";
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnNuevo
            // 
            this.btnNuevo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNuevo.BackColor = System.Drawing.SystemColors.Control;
            this.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNuevo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNuevo.ForeColor = System.Drawing.Color.Black;
            this.btnNuevo.Image = global::Sigesoft.Node.WinClient.UI.Resources.application_form;
            this.btnNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNuevo.Location = new System.Drawing.Point(828, 87);
            this.btnNuevo.Margin = new System.Windows.Forms.Padding(2);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(75, 24);
            this.btnNuevo.TabIndex = 100;
            this.btnNuevo.Text = "     Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = false;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button1.Image = global::Sigesoft.Node.WinClient.UI.Resources.delete;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(828, 352);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 24);
            this.button1.TabIndex = 106;
            this.button1.Text = "Eliminar";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button2.BackColor = System.Drawing.SystemColors.Control;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Image = global::Sigesoft.Node.WinClient.UI.Resources.application_edit;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(828, 324);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 24);
            this.button2.TabIndex = 105;
            this.button2.Text = "     Editar";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button3.BackColor = System.Drawing.SystemColors.Control;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Image = global::Sigesoft.Node.WinClient.UI.Resources.application_form;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(828, 296);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 24);
            this.button3.TabIndex = 104;
            this.button3.Text = "     Nuevo";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button4.BackColor = System.Drawing.SystemColors.Control;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.Black;
            this.button4.Image = global::Sigesoft.Node.WinClient.UI.Resources.brick_go;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(828, 380);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 24);
            this.button4.TabIndex = 107;
            this.button4.Text = "     Importar";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::Sigesoft.Node.WinClient.UI.Resources.system_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(828, 429);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 108;
            this.btnCancel.Text = "Salir";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCloneComponent
            // 
            this.btnCloneComponent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloneComponent.BackColor = System.Drawing.SystemColors.Control;
            this.btnCloneComponent.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCloneComponent.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloneComponent.ForeColor = System.Drawing.Color.Black;
            this.btnCloneComponent.Image = global::Sigesoft.Node.WinClient.UI.Resources.brick_go;
            this.btnCloneComponent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloneComponent.Location = new System.Drawing.Point(828, 171);
            this.btnCloneComponent.Margin = new System.Windows.Forms.Padding(2);
            this.btnCloneComponent.Name = "btnCloneComponent";
            this.btnCloneComponent.Size = new System.Drawing.Size(75, 24);
            this.btnCloneComponent.TabIndex = 109;
            this.btnCloneComponent.Text = "CLONAR";
            this.btnCloneComponent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCloneComponent.UseVisualStyleBackColor = false;
            this.btnCloneComponent.Click += new System.EventHandler(this.btnCloneComponent_Click);
            // 
            // frmMedicalExam
            // 
            this.AcceptButton = this.btnFilter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(919, 482);
            this.Controls.Add(this.btnCloneComponent);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnNuevo);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMedicalExam";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administración de Componentes";
            this.Load += new System.EventHandler(this.frmAdministracion_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDataMedicalExam)).EndInit();
            this.contextMenuMedicalExam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDataMedicalExamFields)).EndInit();
            this.contextMenuMedicalExamFields.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblRecordCountMedicalExam;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ContextMenuStrip contextMenuMedicalExam;
        private System.Windows.Forms.ToolStripMenuItem mnuGridNewMedicalExam;
        private System.Windows.Forms.ToolStripMenuItem mnuGridEditMedicalExam;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDataMedicalExam;
        private System.Windows.Forms.Label lblRecordCountMedicalExamFields;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDataMedicalExamFields;
        private System.Windows.Forms.ContextMenuStrip contextMenuMedicalExamFields;
        private System.Windows.Forms.ToolStripMenuItem mnuGridNewMedicalExamField;
        private System.Windows.Forms.ToolStripMenuItem mnuGridEditMedicalExamField;
        private ComboTreeBox ddlCategoryId;
        private System.Windows.Forms.ToolStripMenuItem mnuGridDeleteMedicalExam;
        private System.Windows.Forms.ToolStripMenuItem mnuGridDeleteMedicalExamField;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem importarToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCloneComponent;
    }
}


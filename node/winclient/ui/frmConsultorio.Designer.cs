namespace Sigesoft.Node.WinClient.UI
{
    partial class frmConsultorio
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("d_CircuitStartDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Paciente");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NroDocum");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Edad");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Empleador");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Cliente");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Protocolo");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_Value1");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_CurrentOccupation");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("i_IsVipId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_ServiceId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_PersonId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("i_NroTickets");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConsultorio));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnLlamar = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.ugListaCola = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.liberarPacienteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.liberarTodosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cambiarEstadoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.porAprobarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.porReevaluarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.culminadoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnLiberar = new System.Windows.Forms.Button();
            this.btnAtender = new System.Windows.Forms.Button();
            this.btnRellamar = new System.Windows.Forms.Button();
            this.ugListaLlamados = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.ugExamenes = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pbFoto = new System.Windows.Forms.PictureBox();
            this.txtTipoESO = new System.Windows.Forms.TextBox();
            this.txtProtocolo = new System.Windows.Forms.TextBox();
            this.txtEmpCliente = new System.Windows.Forms.TextBox();
            this.txtEmpTrabajo = new System.Windows.Forms.TextBox();
            this.txtPuesto = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDocumento = new System.Windows.Forms.TextBox();
            this.txtEdad = new System.Windows.Forms.TextBox();
            this.txtTrabajador = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNameComponent = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugListaCola)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugListaLlamados)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugExamenes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFoto)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.splitContainer1);
            this.groupBox1.Location = new System.Drawing.Point(12, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(960, 620);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 16);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(954, 601);
            this.splitContainer1.SplitterDistance = 606;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.splitContainer2);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(600, 595);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Colas";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 16);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox5);
            this.splitContainer2.Size = new System.Drawing.Size(594, 576);
            this.splitContainer2.SplitterDistance = 334;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.btnImportar);
            this.groupBox4.Controls.Add(this.btnImprimir);
            this.groupBox4.Controls.Add(this.btnLlamar);
            this.groupBox4.Controls.Add(this.btnRefresh);
            this.groupBox4.Controls.Add(this.ugListaCola);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(588, 328);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Listado de COLA";
            // 
            // btnImportar
            // 
            this.btnImportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportar.Image = ((System.Drawing.Image)(resources.GetObject("btnImportar.Image")));
            this.btnImportar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImportar.Location = new System.Drawing.Point(156, 299);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(173, 23);
            this.btnImportar.TabIndex = 4;
            this.btnImportar.Text = "   Importar Datos Laboratorio";
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // btnImprimir
            // 
            this.btnImprimir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImprimir.Image = ((System.Drawing.Image)(resources.GetObject("btnImprimir.Image")));
            this.btnImprimir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImprimir.Location = new System.Drawing.Point(335, 299);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(75, 23);
            this.btnImprimir.TabIndex = 3;
            this.btnImprimir.Text = "     Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // btnLlamar
            // 
            this.btnLlamar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLlamar.Image = ((System.Drawing.Image)(resources.GetObject("btnLlamar.Image")));
            this.btnLlamar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLlamar.Location = new System.Drawing.Point(416, 299);
            this.btnLlamar.Name = "btnLlamar";
            this.btnLlamar.Size = new System.Drawing.Size(75, 23);
            this.btnLlamar.TabIndex = 2;
            this.btnLlamar.Text = "     Llamar";
            this.btnLlamar.UseVisualStyleBackColor = true;
            this.btnLlamar.Click += new System.EventHandler(this.btnLlamar_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRefresh.Location = new System.Drawing.Point(497, 299);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(85, 23);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "     Refrescar";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // ugListaCola
            // 
            this.ugListaCola.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ugListaCola.ContextMenuStrip = this.contextMenuStrip1;
            appearance1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance1.BackColor2 = System.Drawing.SystemColors.ControlLight;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugListaCola.DisplayLayout.Appearance = appearance1;
            ultraGridColumn1.Header.Caption = "Hora de Llegada";
            ultraGridColumn1.Header.VisiblePosition = 1;
            ultraGridColumn2.Header.VisiblePosition = 2;
            ultraGridColumn2.Width = 250;
            ultraGridColumn3.Header.VisiblePosition = 3;
            ultraGridColumn4.Header.VisiblePosition = 4;
            ultraGridColumn5.Header.VisiblePosition = 5;
            ultraGridColumn5.Width = 250;
            ultraGridColumn6.Header.VisiblePosition = 6;
            ultraGridColumn6.Width = 250;
            ultraGridColumn7.Header.VisiblePosition = 7;
            ultraGridColumn7.Width = 250;
            ultraGridColumn8.Header.Caption = "Tipo de EMO";
            ultraGridColumn8.Header.VisiblePosition = 8;
            ultraGridColumn9.Header.Caption = "Puesto";
            ultraGridColumn9.Header.VisiblePosition = 9;
            ultraGridColumn9.Width = 200;
            ultraGridColumn10.Header.Caption = "VIP";
            ultraGridColumn10.Header.VisiblePosition = 10;
            ultraGridColumn11.Header.Caption = "ServiceId";
            ultraGridColumn11.Header.VisiblePosition = 11;
            ultraGridColumn12.Header.Caption = "PersonId";
            ultraGridColumn12.Header.VisiblePosition = 12;
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            appearance2.BackColor2 = System.Drawing.Color.White;
            ultraGridColumn13.Header.Appearance = appearance2;
            ultraGridColumn13.Header.Caption = "Tickets";
            ultraGridColumn13.Header.VisiblePosition = 0;
            ultraGridColumn13.Width = 40;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13});
            this.ugListaCola.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.ugListaCola.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.ugListaCola.DisplayLayout.Override.ColumnHeaderTextOrientation = new Infragistics.Win.TextOrientationInfo(0, Infragistics.Win.TextFlowDirection.Horizontal);
            this.ugListaCola.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ugListaCola.DisplayLayout.Override.RowSelectorAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.Lavender;
            appearance4.BackColor2 = System.Drawing.Color.RoyalBlue;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugListaCola.DisplayLayout.Override.SelectedRowAppearance = appearance4;
            this.ugListaCola.DisplayLayout.RowConnectorColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ugListaCola.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Dashed;
            this.ugListaCola.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugListaCola.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugListaCola.Location = new System.Drawing.Point(6, 19);
            this.ugListaCola.Name = "ugListaCola";
            this.ugListaCola.Size = new System.Drawing.Size(576, 274);
            this.ugListaCola.TabIndex = 0;
            this.ugListaCola.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugListaCola_InitializeRow);
            this.ugListaCola.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ugListaCola_AfterSelectChange);
            this.ugListaCola.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.ugListaCola_ClickCell);
            this.ugListaCola.DoubleClickCell += new Infragistics.Win.UltraWinGrid.DoubleClickCellEventHandler(this.ugListaCola_DoubleClickCell);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.liberarPacienteToolStripMenuItem,
            this.liberarTodosToolStripMenuItem,
            this.toolStripSeparator1,
            this.cambiarEstadoToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(159, 76);
            // 
            // liberarPacienteToolStripMenuItem
            // 
            this.liberarPacienteToolStripMenuItem.Name = "liberarPacienteToolStripMenuItem";
            this.liberarPacienteToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.liberarPacienteToolStripMenuItem.Text = "Liberar Paciente";
            // 
            // liberarTodosToolStripMenuItem
            // 
            this.liberarTodosToolStripMenuItem.Name = "liberarTodosToolStripMenuItem";
            this.liberarTodosToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.liberarTodosToolStripMenuItem.Text = "Liberar Todos";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(155, 6);
            // 
            // cambiarEstadoToolStripMenuItem
            // 
            this.cambiarEstadoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.porAprobarToolStripMenuItem,
            this.porReevaluarToolStripMenuItem,
            this.culminadoToolStripMenuItem});
            this.cambiarEstadoToolStripMenuItem.Name = "cambiarEstadoToolStripMenuItem";
            this.cambiarEstadoToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.cambiarEstadoToolStripMenuItem.Text = "Cambiar Estado";
            // 
            // porAprobarToolStripMenuItem
            // 
            this.porAprobarToolStripMenuItem.Name = "porAprobarToolStripMenuItem";
            this.porAprobarToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.porAprobarToolStripMenuItem.Text = "Por Aprobar";
            this.porAprobarToolStripMenuItem.Click += new System.EventHandler(this.porAprobarToolStripMenuItem_Click);
            // 
            // porReevaluarToolStripMenuItem
            // 
            this.porReevaluarToolStripMenuItem.Name = "porReevaluarToolStripMenuItem";
            this.porReevaluarToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.porReevaluarToolStripMenuItem.Text = "Por Reevaluar";
            this.porReevaluarToolStripMenuItem.Click += new System.EventHandler(this.porReevaluarToolStripMenuItem_Click);
            // 
            // culminadoToolStripMenuItem
            // 
            this.culminadoToolStripMenuItem.Name = "culminadoToolStripMenuItem";
            this.culminadoToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.culminadoToolStripMenuItem.Text = "Culminado";
            this.culminadoToolStripMenuItem.Click += new System.EventHandler(this.culminadoToolStripMenuItem_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.btnLiberar);
            this.groupBox5.Controls.Add(this.btnAtender);
            this.groupBox5.Controls.Add(this.btnRellamar);
            this.groupBox5.Controls.Add(this.ugListaLlamados);
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(588, 232);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Lista de Llamado";
            // 
            // btnLiberar
            // 
            this.btnLiberar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLiberar.Location = new System.Drawing.Point(507, 203);
            this.btnLiberar.Name = "btnLiberar";
            this.btnLiberar.Size = new System.Drawing.Size(75, 23);
            this.btnLiberar.TabIndex = 5;
            this.btnLiberar.Text = "Liberar";
            this.btnLiberar.UseVisualStyleBackColor = true;
            this.btnLiberar.Click += new System.EventHandler(this.btnLiberar_Click);
            // 
            // btnAtender
            // 
            this.btnAtender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtender.Location = new System.Drawing.Point(426, 203);
            this.btnAtender.Name = "btnAtender";
            this.btnAtender.Size = new System.Drawing.Size(75, 23);
            this.btnAtender.TabIndex = 4;
            this.btnAtender.Text = "Atender";
            this.btnAtender.UseVisualStyleBackColor = true;
            this.btnAtender.Click += new System.EventHandler(this.btnAtender_Click);
            // 
            // btnRellamar
            // 
            this.btnRellamar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRellamar.Location = new System.Drawing.Point(345, 203);
            this.btnRellamar.Name = "btnRellamar";
            this.btnRellamar.Size = new System.Drawing.Size(75, 23);
            this.btnRellamar.TabIndex = 3;
            this.btnRellamar.Text = "Rellamar";
            this.btnRellamar.UseVisualStyleBackColor = true;
            this.btnRellamar.Click += new System.EventHandler(this.btnRellamar_Click);
            // 
            // ugListaLlamados
            // 
            this.ugListaLlamados.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.ControlLight;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugListaLlamados.DisplayLayout.Appearance = appearance5;
            this.ugListaLlamados.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.ugListaLlamados.DisplayLayout.Override.ColumnHeaderTextOrientation = new Infragistics.Win.TextOrientationInfo(0, Infragistics.Win.TextFlowDirection.Horizontal);
            this.ugListaLlamados.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ugListaLlamados.DisplayLayout.Override.RowSelectorAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.Lavender;
            appearance7.BackColor2 = System.Drawing.Color.RoyalBlue;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugListaLlamados.DisplayLayout.Override.SelectedRowAppearance = appearance7;
            this.ugListaLlamados.DisplayLayout.RowConnectorColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ugListaLlamados.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Dashed;
            this.ugListaLlamados.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugListaLlamados.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugListaLlamados.Location = new System.Drawing.Point(6, 19);
            this.ugListaLlamados.Name = "ugListaLlamados";
            this.ugListaLlamados.Size = new System.Drawing.Size(576, 178);
            this.ugListaLlamados.TabIndex = 1;
            this.ugListaLlamados.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ugListaLlamados_AfterSelectChange);
            this.ugListaLlamados.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.ugListaLlamados_ClickCell);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.pbFoto);
            this.groupBox3.Controls.Add(this.txtTipoESO);
            this.groupBox3.Controls.Add(this.txtProtocolo);
            this.groupBox3.Controls.Add(this.txtEmpCliente);
            this.groupBox3.Controls.Add(this.txtEmpTrabajo);
            this.groupBox3.Controls.Add(this.txtPuesto);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtDocumento);
            this.groupBox3.Controls.Add(this.txtEdad);
            this.groupBox3.Controls.Add(this.txtTrabajador);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(338, 592);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Paciente";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.ugExamenes);
            this.groupBox6.Location = new System.Drawing.Point(6, 345);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(326, 247);
            this.groupBox6.TabIndex = 17;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Exámenes";
            // 
            // ugExamenes
            // 
            this.ugExamenes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance8.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance8.BackColor2 = System.Drawing.SystemColors.ControlLight;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugExamenes.DisplayLayout.Appearance = appearance8;
            this.ugExamenes.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.ugExamenes.DisplayLayout.Override.ColumnHeaderTextOrientation = new Infragistics.Win.TextOrientationInfo(0, Infragistics.Win.TextFlowDirection.Horizontal);
            this.ugExamenes.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance9.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ugExamenes.DisplayLayout.Override.RowSelectorAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.Lavender;
            appearance10.BackColor2 = System.Drawing.Color.RoyalBlue;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugExamenes.DisplayLayout.Override.SelectedRowAppearance = appearance10;
            this.ugExamenes.DisplayLayout.RowConnectorColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ugExamenes.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Dashed;
            this.ugExamenes.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugExamenes.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugExamenes.Location = new System.Drawing.Point(6, 19);
            this.ugExamenes.Name = "ugExamenes";
            this.ugExamenes.Size = new System.Drawing.Size(314, 222);
            this.ugExamenes.TabIndex = 6;
            // 
            // pbFoto
            // 
            this.pbFoto.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pbFoto.Location = new System.Drawing.Point(85, 16);
            this.pbFoto.Name = "pbFoto";
            this.pbFoto.Size = new System.Drawing.Size(182, 141);
            this.pbFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFoto.TabIndex = 16;
            this.pbFoto.TabStop = false;
            // 
            // txtTipoESO
            // 
            this.txtTipoESO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTipoESO.Location = new System.Drawing.Point(85, 319);
            this.txtTipoESO.Name = "txtTipoESO";
            this.txtTipoESO.Size = new System.Drawing.Size(247, 20);
            this.txtTipoESO.TabIndex = 15;
            // 
            // txtProtocolo
            // 
            this.txtProtocolo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProtocolo.Location = new System.Drawing.Point(67, 293);
            this.txtProtocolo.Name = "txtProtocolo";
            this.txtProtocolo.Size = new System.Drawing.Size(265, 20);
            this.txtProtocolo.TabIndex = 14;
            // 
            // txtEmpCliente
            // 
            this.txtEmpCliente.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmpCliente.Location = new System.Drawing.Point(85, 267);
            this.txtEmpCliente.Name = "txtEmpCliente";
            this.txtEmpCliente.Size = new System.Drawing.Size(247, 20);
            this.txtEmpCliente.TabIndex = 13;
            // 
            // txtEmpTrabajo
            // 
            this.txtEmpTrabajo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmpTrabajo.Location = new System.Drawing.Point(85, 241);
            this.txtEmpTrabajo.Name = "txtEmpTrabajo";
            this.txtEmpTrabajo.Size = new System.Drawing.Size(247, 20);
            this.txtEmpTrabajo.TabIndex = 12;
            // 
            // txtPuesto
            // 
            this.txtPuesto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPuesto.Location = new System.Drawing.Point(55, 215);
            this.txtPuesto.Name = "txtPuesto";
            this.txtPuesto.Size = new System.Drawing.Size(277, 20);
            this.txtPuesto.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 218);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Puesto:";
            // 
            // txtDocumento
            // 
            this.txtDocumento.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocumento.Location = new System.Drawing.Point(137, 189);
            this.txtDocumento.Name = "txtDocumento";
            this.txtDocumento.Size = new System.Drawing.Size(195, 20);
            this.txtDocumento.TabIndex = 9;
            // 
            // txtEdad
            // 
            this.txtEdad.Location = new System.Drawing.Point(47, 189);
            this.txtEdad.Name = "txtEdad";
            this.txtEdad.Size = new System.Drawing.Size(48, 20);
            this.txtEdad.TabIndex = 8;
            // 
            // txtTrabajador
            // 
            this.txtTrabajador.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTrabajador.Location = new System.Drawing.Point(73, 163);
            this.txtTrabajador.Name = "txtTrabajador";
            this.txtTrabajador.Size = new System.Drawing.Size(259, 20);
            this.txtTrabajador.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 322);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Tipo de ESO:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 296);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Protocolo:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 270);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Emp. Cliente:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 244);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Emp. Trabajo:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(101, 192);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Doc:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Edad:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Trabajador:";
            // 
            // lblNameComponent
            // 
            this.lblNameComponent.AutoSize = true;
            this.lblNameComponent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNameComponent.ForeColor = System.Drawing.Color.MediumBlue;
            this.lblNameComponent.Location = new System.Drawing.Point(257, 9);
            this.lblNameComponent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNameComponent.Name = "lblNameComponent";
            this.lblNameComponent.Size = new System.Drawing.Size(124, 20);
            this.lblNameComponent.TabIndex = 63;
            this.lblNameComponent.Text = "EXAMEN XXX";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MediumBlue;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 17);
            this.label1.TabIndex = 62;
            this.label1.Text = "Usted está realizando el examen de :";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(6, 299);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "     CELIMA";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmConsultorio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.lblNameComponent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConsultorio";
            this.Text = "Lista de Atenciones";
            this.Load += new System.EventHandler(this.frmConsultorio_Load);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugListaCola)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugListaLlamados)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugExamenes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label lblNameComponent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox3;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugListaCola;
        private System.Windows.Forms.Button btnLlamar;
        private System.Windows.Forms.Button btnRefresh;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugListaLlamados;
        private System.Windows.Forms.Button btnLiberar;
        private System.Windows.Forms.Button btnAtender;
        private System.Windows.Forms.Button btnRellamar;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDocumento;
        private System.Windows.Forms.TextBox txtEdad;
        private System.Windows.Forms.TextBox txtTrabajador;
        private System.Windows.Forms.TextBox txtTipoESO;
        private System.Windows.Forms.TextBox txtProtocolo;
        private System.Windows.Forms.TextBox txtEmpCliente;
        private System.Windows.Forms.TextBox txtEmpTrabajo;
        private System.Windows.Forms.TextBox txtPuesto;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox6;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugExamenes;
        private System.Windows.Forms.PictureBox pbFoto;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem liberarPacienteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem liberarTodosToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cambiarEstadoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem porAprobarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem porReevaluarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem culminadoToolStripMenuItem;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button button1;
    }
}
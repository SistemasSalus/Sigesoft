namespace Sigesoft.Node.WinClient.UI.Reports
{
    partial class frmManagementReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManagementReports));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblRecordCount1 = new System.Windows.Forms.Label();
            this.chklExamenes = new System.Windows.Forms.CheckedListBox();
            this.rbSeleccioneExamenes = new System.Windows.Forms.RadioButton();
            this.rbTodosExamenes = new System.Windows.Forms.RadioButton();
            this.btnGenerarReporteExamenes = new System.Windows.Forms.Button();
            this.btnGenerarReporteFichasMedicas = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblRecordCountFichaMedica = new System.Windows.Forms.Label();
            this.chklFichasMedicas = new System.Windows.Forms.CheckedListBox();
            this.rbSeleccioneFichaMedica = new System.Windows.Forms.RadioButton();
            this.rbTodosFichaMedica = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lvAdjuntos = new System.Windows.Forms.ListView();
            this.chArchivos = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chMultimediaFileId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblRecordCountAdjuntos = new System.Windows.Forms.Label();
            this.btnDownloadFile = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnConsolidadoReportes = new System.Windows.Forms.Button();
            this.chklConsolidadoReportes = new System.Windows.Forms.CheckedListBox();
            this.chkTodos = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblRecordCount1);
            this.groupBox1.Controls.Add(this.chklExamenes);
            this.groupBox1.Controls.Add(this.rbSeleccioneExamenes);
            this.groupBox1.Controls.Add(this.rbTodosExamenes);
            this.groupBox1.Location = new System.Drawing.Point(68, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1267, 459);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Examenes";
            // 
            // lblRecordCount1
            // 
            this.lblRecordCount1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecordCount1.AutoSize = true;
            this.lblRecordCount1.Location = new System.Drawing.Point(1119, 17);
            this.lblRecordCount1.Name = "lblRecordCount1";
            this.lblRecordCount1.Size = new System.Drawing.Size(142, 13);
            this.lblRecordCount1.TabIndex = 24;
            this.lblRecordCount1.Text = "Se encontraron {0} registros.";
            // 
            // chklExamenes
            // 
            this.chklExamenes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chklExamenes.CheckOnClick = true;
            this.chklExamenes.Enabled = false;
            this.chklExamenes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chklExamenes.FormattingEnabled = true;
            this.chklExamenes.Location = new System.Drawing.Point(100, 34);
            this.chklExamenes.Name = "chklExamenes";
            this.chklExamenes.Size = new System.Drawing.Size(1261, 409);
            this.chklExamenes.TabIndex = 23;
            this.chklExamenes.SelectedValueChanged += new System.EventHandler(this.chklExamenes_SelectedValueChanged);
            // 
            // rbSeleccioneExamenes
            // 
            this.rbSeleccioneExamenes.AutoSize = true;
            this.rbSeleccioneExamenes.Location = new System.Drawing.Point(6, 45);
            this.rbSeleccioneExamenes.Name = "rbSeleccioneExamenes";
            this.rbSeleccioneExamenes.Size = new System.Drawing.Size(78, 17);
            this.rbSeleccioneExamenes.TabIndex = 1;
            this.rbSeleccioneExamenes.Text = "Seleccione";
            this.rbSeleccioneExamenes.UseVisualStyleBackColor = true;
            this.rbSeleccioneExamenes.CheckedChanged += new System.EventHandler(this.rbSeleccioneExamenes_CheckedChanged);
            // 
            // rbTodosExamenes
            // 
            this.rbTodosExamenes.AutoSize = true;
            this.rbTodosExamenes.Checked = true;
            this.rbTodosExamenes.Location = new System.Drawing.Point(6, 22);
            this.rbTodosExamenes.Name = "rbTodosExamenes";
            this.rbTodosExamenes.Size = new System.Drawing.Size(55, 17);
            this.rbTodosExamenes.TabIndex = 0;
            this.rbTodosExamenes.TabStop = true;
            this.rbTodosExamenes.Text = "Todos";
            this.rbTodosExamenes.UseVisualStyleBackColor = true;
            this.rbTodosExamenes.CheckedChanged += new System.EventHandler(this.rbTodosExamenes_CheckedChanged);
            // 
            // btnGenerarReporteExamenes
            // 
            this.btnGenerarReporteExamenes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerarReporteExamenes.BackColor = System.Drawing.SystemColors.Control;
            this.btnGenerarReporteExamenes.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnGenerarReporteExamenes.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnGenerarReporteExamenes.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnGenerarReporteExamenes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerarReporteExamenes.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarReporteExamenes.ForeColor = System.Drawing.Color.Black;
            this.btnGenerarReporteExamenes.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerarReporteExamenes.Image")));
            this.btnGenerarReporteExamenes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerarReporteExamenes.Location = new System.Drawing.Point(1388, 11);
            this.btnGenerarReporteExamenes.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerarReporteExamenes.Name = "btnGenerarReporteExamenes";
            this.btnGenerarReporteExamenes.Size = new System.Drawing.Size(1267, 25);
            this.btnGenerarReporteExamenes.TabIndex = 115;
            this.btnGenerarReporteExamenes.Text = "&Generar";
            this.btnGenerarReporteExamenes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnGenerarReporteExamenes.UseVisualStyleBackColor = false;
            this.btnGenerarReporteExamenes.Click += new System.EventHandler(this.btnGenerarReporteExamenes_Click);
            // 
            // btnGenerarReporteFichasMedicas
            // 
            this.btnGenerarReporteFichasMedicas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerarReporteFichasMedicas.BackColor = System.Drawing.SystemColors.Control;
            this.btnGenerarReporteFichasMedicas.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnGenerarReporteFichasMedicas.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnGenerarReporteFichasMedicas.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnGenerarReporteFichasMedicas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerarReporteFichasMedicas.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarReporteFichasMedicas.ForeColor = System.Drawing.Color.Black;
            this.btnGenerarReporteFichasMedicas.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerarReporteFichasMedicas.Image")));
            this.btnGenerarReporteFichasMedicas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerarReporteFichasMedicas.Location = new System.Drawing.Point(1388, 444);
            this.btnGenerarReporteFichasMedicas.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerarReporteFichasMedicas.Name = "btnGenerarReporteFichasMedicas";
            this.btnGenerarReporteFichasMedicas.Size = new System.Drawing.Size(1267, 408);
            this.btnGenerarReporteFichasMedicas.TabIndex = 131;
            this.btnGenerarReporteFichasMedicas.Text = "&Generar";
            this.btnGenerarReporteFichasMedicas.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnGenerarReporteFichasMedicas.UseVisualStyleBackColor = false;
            this.btnGenerarReporteFichasMedicas.Click += new System.EventHandler(this.btnGenerarReporteFichasMedicas_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblRecordCountFichaMedica);
            this.groupBox2.Controls.Add(this.chklFichasMedicas);
            this.groupBox2.Controls.Add(this.rbSeleccioneFichaMedica);
            this.groupBox2.Controls.Add(this.rbTodosFichaMedica);
            this.groupBox2.Location = new System.Drawing.Point(1104, 438);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1267, 408);
            this.groupBox2.TabIndex = 132;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fichas Médicas";
            // 
            // lblRecordCountFichaMedica
            // 
            this.lblRecordCountFichaMedica.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecordCountFichaMedica.AutoSize = true;
            this.lblRecordCountFichaMedica.Location = new System.Drawing.Point(1119, 18);
            this.lblRecordCountFichaMedica.Name = "lblRecordCountFichaMedica";
            this.lblRecordCountFichaMedica.Size = new System.Drawing.Size(142, 13);
            this.lblRecordCountFichaMedica.TabIndex = 24;
            this.lblRecordCountFichaMedica.Text = "Se encontraron {0} registros.";
            // 
            // chklFichasMedicas
            // 
            this.chklFichasMedicas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chklFichasMedicas.CheckOnClick = true;
            this.chklFichasMedicas.Enabled = false;
            this.chklFichasMedicas.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chklFichasMedicas.FormattingEnabled = true;
            this.chklFichasMedicas.Location = new System.Drawing.Point(100, 37);
            this.chklFichasMedicas.Name = "chklFichasMedicas";
            this.chklFichasMedicas.Size = new System.Drawing.Size(1261, 379);
            this.chklFichasMedicas.TabIndex = 23;
            this.chklFichasMedicas.SelectedValueChanged += new System.EventHandler(this.chklFichasMedicas_SelectedValueChanged);
            // 
            // rbSeleccioneFichaMedica
            // 
            this.rbSeleccioneFichaMedica.AutoSize = true;
            this.rbSeleccioneFichaMedica.Location = new System.Drawing.Point(6, 45);
            this.rbSeleccioneFichaMedica.Name = "rbSeleccioneFichaMedica";
            this.rbSeleccioneFichaMedica.Size = new System.Drawing.Size(78, 17);
            this.rbSeleccioneFichaMedica.TabIndex = 1;
            this.rbSeleccioneFichaMedica.Text = "Seleccione";
            this.rbSeleccioneFichaMedica.UseVisualStyleBackColor = true;
            this.rbSeleccioneFichaMedica.CheckedChanged += new System.EventHandler(this.rbSeleccioneFichaMedica_CheckedChanged);
            // 
            // rbTodosFichaMedica
            // 
            this.rbTodosFichaMedica.AutoSize = true;
            this.rbTodosFichaMedica.Checked = true;
            this.rbTodosFichaMedica.Location = new System.Drawing.Point(6, 22);
            this.rbTodosFichaMedica.Name = "rbTodosFichaMedica";
            this.rbTodosFichaMedica.Size = new System.Drawing.Size(55, 17);
            this.rbTodosFichaMedica.TabIndex = 0;
            this.rbTodosFichaMedica.TabStop = true;
            this.rbTodosFichaMedica.Text = "Todos";
            this.rbTodosFichaMedica.UseVisualStyleBackColor = true;
            this.rbTodosFichaMedica.CheckedChanged += new System.EventHandler(this.rbTodosFichaMedica_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lvAdjuntos);
            this.groupBox3.Controls.Add(this.lblRecordCountAdjuntos);
            this.groupBox3.Location = new System.Drawing.Point(49, 409);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(299, 408);
            this.groupBox3.TabIndex = 133;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Adjuntos";
            // 
            // lvAdjuntos
            // 
            this.lvAdjuntos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chArchivos,
            this.chMultimediaFileId});
            this.lvAdjuntos.FullRowSelect = true;
            this.lvAdjuntos.Location = new System.Drawing.Point(100, 33);
            this.lvAdjuntos.Name = "lvAdjuntos";
            this.lvAdjuntos.Size = new System.Drawing.Size(378, 144);
            this.lvAdjuntos.TabIndex = 25;
            this.lvAdjuntos.UseCompatibleStateImageBehavior = false;
            this.lvAdjuntos.View = System.Windows.Forms.View.Details;
            this.lvAdjuntos.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvAdjuntos_ItemSelectionChanged);
            // 
            // chArchivos
            // 
            this.chArchivos.Text = "Archivo";
            this.chArchivos.Width = 374;
            // 
            // chMultimediaFileId
            // 
            this.chMultimediaFileId.Text = "MultimediaFileId";
            this.chMultimediaFileId.Width = 0;
            // 
            // lblRecordCountAdjuntos
            // 
            this.lblRecordCountAdjuntos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecordCountAdjuntos.AutoSize = true;
            this.lblRecordCountAdjuntos.Location = new System.Drawing.Point(151, 17);
            this.lblRecordCountAdjuntos.Name = "lblRecordCountAdjuntos";
            this.lblRecordCountAdjuntos.Size = new System.Drawing.Size(142, 13);
            this.lblRecordCountAdjuntos.TabIndex = 24;
            this.lblRecordCountAdjuntos.Text = "Se encontraron {0} registros.";
            // 
            // btnDownloadFile
            // 
            this.btnDownloadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownloadFile.BackColor = System.Drawing.SystemColors.Control;
            this.btnDownloadFile.Enabled = false;
            this.btnDownloadFile.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDownloadFile.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnDownloadFile.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnDownloadFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownloadFile.ForeColor = System.Drawing.Color.Black;
            this.btnDownloadFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDownloadFile.Location = new System.Drawing.Point(1388, 254);
            this.btnDownloadFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnDownloadFile.Name = "btnDownloadFile";
            this.btnDownloadFile.Size = new System.Drawing.Size(1267, 408);
            this.btnDownloadFile.TabIndex = 134;
            this.btnDownloadFile.Text = "&Descargar";
            this.btnDownloadFile.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnDownloadFile.UseVisualStyleBackColor = false;
            this.btnDownloadFile.Click += new System.EventHandler(this.btnDownloadFile_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnConsolidadoReportes);
            this.groupBox5.Controls.Add(this.chklConsolidadoReportes);
            this.groupBox5.Controls.Add(this.chkTodos);
            this.groupBox5.Location = new System.Drawing.Point(12, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(483, 397);
            this.groupBox5.TabIndex = 136;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Reportes Seleccionados";
            // 
            // btnConsolidadoReportes
            // 
            this.btnConsolidadoReportes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConsolidadoReportes.AutoSize = true;
            this.btnConsolidadoReportes.BackColor = System.Drawing.SystemColors.Control;
            this.btnConsolidadoReportes.Enabled = false;
            this.btnConsolidadoReportes.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnConsolidadoReportes.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnConsolidadoReportes.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnConsolidadoReportes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsolidadoReportes.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConsolidadoReportes.ForeColor = System.Drawing.Color.Black;
            this.btnConsolidadoReportes.Image = global::Sigesoft.Node.WinClient.UI.Resources.page_white_acrobat;
            this.btnConsolidadoReportes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConsolidadoReportes.Location = new System.Drawing.Point(289, 364);
            this.btnConsolidadoReportes.Margin = new System.Windows.Forms.Padding(2);
            this.btnConsolidadoReportes.Name = "btnConsolidadoReportes";
            this.btnConsolidadoReportes.Size = new System.Drawing.Size(165, 28);
            this.btnConsolidadoReportes.TabIndex = 51;
            this.btnConsolidadoReportes.Text = "  &Generar Reportes en PDF";
            this.btnConsolidadoReportes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnConsolidadoReportes.UseVisualStyleBackColor = false;
            this.btnConsolidadoReportes.Click += new System.EventHandler(this.btnConsolidadoReportes_Click);
            // 
            // chklConsolidadoReportes
            // 
            this.chklConsolidadoReportes.FormattingEnabled = true;
            this.chklConsolidadoReportes.Location = new System.Drawing.Point(20, 42);
            this.chklConsolidadoReportes.Name = "chklConsolidadoReportes";
            this.chklConsolidadoReportes.Size = new System.Drawing.Size(434, 319);
            this.chklConsolidadoReportes.TabIndex = 1;
            // 
            // chkTodos
            // 
            this.chkTodos.AutoSize = true;
            this.chkTodos.Location = new System.Drawing.Point(6, 19);
            this.chkTodos.Name = "chkTodos";
            this.chkTodos.Size = new System.Drawing.Size(115, 17);
            this.chkTodos.TabIndex = 0;
            this.chkTodos.Text = "Seleccionar Todos";
            this.chkTodos.UseVisualStyleBackColor = true;
            // 
            // frmManagementReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 750);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnDownloadFile);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnGenerarReporteFichasMedicas);
            this.Controls.Add(this.btnGenerarReporteExamenes);
            this.Controls.Add(this.groupBox1);
            this.MinimizeBox = false;
            this.Name = "frmManagementReports";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administrador de Reportes";
            this.Load += new System.EventHandler(this.frmManagementReports_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbTodosExamenes;
        private System.Windows.Forms.RadioButton rbSeleccioneExamenes;
        private System.Windows.Forms.CheckedListBox chklExamenes;
        private System.Windows.Forms.Label lblRecordCount1;
        private System.Windows.Forms.Button btnGenerarReporteExamenes;
        private System.Windows.Forms.Button btnGenerarReporteFichasMedicas;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblRecordCountFichaMedica;
        private System.Windows.Forms.CheckedListBox chklFichasMedicas;
        private System.Windows.Forms.RadioButton rbSeleccioneFichaMedica;
        private System.Windows.Forms.RadioButton rbTodosFichaMedica;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblRecordCountAdjuntos;
        private System.Windows.Forms.ListView lvAdjuntos;
        private System.Windows.Forms.ColumnHeader chArchivos;
        private System.Windows.Forms.ColumnHeader chMultimediaFileId;
        private System.Windows.Forms.Button btnDownloadFile;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckedListBox chklConsolidadoReportes;
        private System.Windows.Forms.CheckBox chkTodos;
        private System.Windows.Forms.Button btnConsolidadoReportes;
    }
}
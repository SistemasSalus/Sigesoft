namespace Sigesoft.Node.WinClient.UI.Operations.Popups
{
    partial class frmAddMedicationMedicalConsult
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_ProductId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_ProductName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_GenericName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("r_StockActual");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_CategoryName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn40 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_Brand");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_ProductCode");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn41 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_SerialNumber");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn42 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_CreationUser");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn43 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("d_CreationDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn44 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_UpdateUser");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn45 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("d_UpdateDate");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.gbMedicamentoSeleccionado = new System.Windows.Forms.GroupBox();
            this.lblPresentacion = new System.Windows.Forms.Label();
            this.cbVia = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDosis = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCantidad = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.grdMedicament = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.txtProductSearch = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSearchMedicamento = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAddAndNew = new System.Windows.Forms.Button();
            this.uvAddMedication = new Infragistics.Win.Misc.UltraValidator(this.components);
            this.ttAddMedication = new System.Windows.Forms.ToolTip(this.components);
            this.gbMedicamentoSeleccionado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMedicament)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uvAddMedication)).BeginInit();
            this.SuspendLayout();
            // 
            // gbMedicamentoSeleccionado
            // 
            this.gbMedicamentoSeleccionado.Controls.Add(this.lblPresentacion);
            this.gbMedicamentoSeleccionado.Controls.Add(this.cbVia);
            this.gbMedicamentoSeleccionado.Controls.Add(this.label2);
            this.gbMedicamentoSeleccionado.Controls.Add(this.txtDosis);
            this.gbMedicamentoSeleccionado.Controls.Add(this.label3);
            this.gbMedicamentoSeleccionado.Controls.Add(this.txtCantidad);
            this.gbMedicamentoSeleccionado.Controls.Add(this.label1);
            this.gbMedicamentoSeleccionado.Controls.Add(this.label31);
            this.gbMedicamentoSeleccionado.Location = new System.Drawing.Point(405, 5);
            this.gbMedicamentoSeleccionado.Name = "gbMedicamentoSeleccionado";
            this.gbMedicamentoSeleccionado.Size = new System.Drawing.Size(340, 228);
            this.gbMedicamentoSeleccionado.TabIndex = 1;
            this.gbMedicamentoSeleccionado.TabStop = false;
            this.gbMedicamentoSeleccionado.Text = "Medicamento seleccionado";
            // 
            // lblPresentacion
            // 
            this.lblPresentacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblPresentacion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPresentacion.Location = new System.Drawing.Point(76, 22);
            this.lblPresentacion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPresentacion.Name = "lblPresentacion";
            this.lblPresentacion.Size = new System.Drawing.Size(248, 54);
            this.lblPresentacion.TabIndex = 0;
            // 
            // cbVia
            // 
            this.cbVia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVia.FormattingEnabled = true;
            this.cbVia.Location = new System.Drawing.Point(177, 89);
            this.cbVia.Name = "cbVia";
            this.cbVia.Size = new System.Drawing.Size(147, 21);
            this.cbVia.TabIndex = 2;
            this.uvAddMedication.GetValidationSettings(this.cbVia).Condition = new Infragistics.Win.OperatorCondition(Infragistics.Win.ConditionOperator.NotEquals, "--Seleccionar--", true, typeof(string));
            this.uvAddMedication.GetValidationSettings(this.cbVia).DataType = typeof(string);
            this.uvAddMedication.GetValidationSettings(this.cbVia).EmptyValueCriteria = Infragistics.Win.Misc.EmptyValueCriteria.NullOrEmptyString;
            this.uvAddMedication.GetValidationSettings(this.cbVia).IsRequired = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(140, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Via";
            // 
            // txtDosis
            // 
            this.txtDosis.Location = new System.Drawing.Point(76, 123);
            this.txtDosis.MaxLength = 255;
            this.txtDosis.Multiline = true;
            this.txtDosis.Name = "txtDosis";
            this.txtDosis.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDosis.Size = new System.Drawing.Size(248, 75);
            this.txtDosis.TabIndex = 3;
            this.uvAddMedication.GetValidationSettings(this.txtDosis).EmptyValueCriteria = Infragistics.Win.Misc.EmptyValueCriteria.NullOrEmptyString;
            this.uvAddMedication.GetValidationSettings(this.txtDosis).IsRequired = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Dosis";
            // 
            // txtCantidad
            // 
            this.txtCantidad.Location = new System.Drawing.Point(76, 89);
            this.txtCantidad.Name = "txtCantidad";
            this.txtCantidad.Size = new System.Drawing.Size(54, 20);
            this.txtCantidad.TabIndex = 1;
            this.uvAddMedication.GetValidationSettings(this.txtCantidad).DataType = typeof(string);
            this.uvAddMedication.GetValidationSettings(this.txtCantidad).EmptyValueCriteria = Infragistics.Win.Misc.EmptyValueCriteria.NullOrEmptyString;
            this.uvAddMedication.GetValidationSettings(this.txtCantidad).IsRequired = true;
            this.txtCantidad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCantidad_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cantidad";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(1, 23);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(69, 13);
            this.label31.TabIndex = 0;
            this.label31.Text = "Presentación";
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecordCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordCount.Location = new System.Drawing.Point(202, 206);
            this.lblRecordCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(187, 19);
            this.lblRecordCount.TabIndex = 0;
            this.lblRecordCount.Text = "No se ha realizado la búsqueda aún.";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grdMedicament
            // 
            this.grdMedicament.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdMedicament.CausesValidation = false;
            appearance1.BackColor = System.Drawing.SystemColors.ControlLight;
            appearance1.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdMedicament.DisplayLayout.Appearance = appearance1;
            ultraGridColumn11.Header.Caption = "Id Producto";
            ultraGridColumn11.Header.VisiblePosition = 0;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn11.Width = 139;
            ultraGridColumn1.Header.Caption = "Medicamento";
            ultraGridColumn1.Header.VisiblePosition = 1;
            ultraGridColumn1.Width = 196;
            ultraGridColumn16.Header.Caption = "Nombre Genérico";
            ultraGridColumn16.Header.VisiblePosition = 3;
            ultraGridColumn2.Header.Caption = "Stock Actual";
            ultraGridColumn2.Header.VisiblePosition = 4;
            ultraGridColumn2.Width = 53;
            ultraGridColumn17.Header.Caption = "Categoría";
            ultraGridColumn17.Header.VisiblePosition = 2;
            ultraGridColumn17.Hidden = true;
            ultraGridColumn40.Header.Caption = "Marca";
            ultraGridColumn40.Header.VisiblePosition = 5;
            ultraGridColumn40.Hidden = true;
            ultraGridColumn14.Header.Caption = "Código Producto";
            ultraGridColumn14.Header.VisiblePosition = 7;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn14.Width = 244;
            ultraGridColumn41.Header.Caption = "Nro Serie";
            ultraGridColumn41.Header.VisiblePosition = 6;
            ultraGridColumn41.Hidden = true;
            ultraGridColumn42.Header.Caption = "Usuario Crea.";
            ultraGridColumn42.Header.VisiblePosition = 8;
            ultraGridColumn42.Hidden = true;
            ultraGridColumn42.Width = 125;
            ultraGridColumn43.Format = "dd/MM/yyyy hh:mm tt";
            ultraGridColumn43.Header.Caption = "Fecha Crea.";
            ultraGridColumn43.Header.VisiblePosition = 9;
            ultraGridColumn43.Hidden = true;
            ultraGridColumn43.Width = 150;
            ultraGridColumn44.Header.Caption = "Usuario Act.";
            ultraGridColumn44.Header.VisiblePosition = 10;
            ultraGridColumn44.Hidden = true;
            ultraGridColumn44.Width = 125;
            ultraGridColumn45.Format = "dd/MM/yyyy hh:mm tt";
            ultraGridColumn45.Header.Caption = "Fecha Act.";
            ultraGridColumn45.Header.VisiblePosition = 11;
            ultraGridColumn45.Hidden = true;
            ultraGridColumn45.Width = 150;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn11,
            ultraGridColumn1,
            ultraGridColumn16,
            ultraGridColumn2,
            ultraGridColumn17,
            ultraGridColumn40,
            ultraGridColumn14,
            ultraGridColumn41,
            ultraGridColumn42,
            ultraGridColumn43,
            ultraGridColumn44,
            ultraGridColumn45});
            this.grdMedicament.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdMedicament.DisplayLayout.InterBandSpacing = 10;
            this.grdMedicament.DisplayLayout.MaxColScrollRegions = 1;
            this.grdMedicament.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdMedicament.DisplayLayout.NewColumnLoadStyle = Infragistics.Win.UltraWinGrid.NewColumnLoadStyle.Hide;
            this.grdMedicament.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdMedicament.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdMedicament.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdMedicament.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this.grdMedicament.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.grdMedicament.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.SystemColors.Control;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlLightLight;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            this.grdMedicament.DisplayLayout.Override.CellAppearance = appearance3;
            this.grdMedicament.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance4.BackColor = System.Drawing.SystemColors.Control;
            appearance4.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance4.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdMedicament.DisplayLayout.Override.HeaderAppearance = appearance4;
            this.grdMedicament.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance5.AlphaLevel = ((short)(187));
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grdMedicament.DisplayLayout.Override.RowAlternateAppearance = appearance5;
            appearance6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdMedicament.DisplayLayout.Override.RowSelectorAppearance = appearance6;
            this.grdMedicament.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.BackColor2 = System.Drawing.SystemColors.ActiveCaption;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance7.FontData.BoldAsString = "True";
            this.grdMedicament.DisplayLayout.Override.SelectedRowAppearance = appearance7;
            this.grdMedicament.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdMedicament.DisplayLayout.RowConnectorColor = System.Drawing.SystemColors.ControlDarkDark;
            this.grdMedicament.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Dashed;
            this.grdMedicament.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdMedicament.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdMedicament.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdMedicament.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdMedicament.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.grdMedicament.Location = new System.Drawing.Point(5, 41);
            this.grdMedicament.Margin = new System.Windows.Forms.Padding(2);
            this.grdMedicament.Name = "grdMedicament";
            this.grdMedicament.Size = new System.Drawing.Size(384, 163);
            this.grdMedicament.TabIndex = 3;
            this.grdMedicament.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdMedicament_AfterSelectChange);
            // 
            // txtProductSearch
            // 
            this.txtProductSearch.Location = new System.Drawing.Point(53, 15);
            this.txtProductSearch.Name = "txtProductSearch";
            this.txtProductSearch.Size = new System.Drawing.Size(305, 20);
            this.txtProductSearch.TabIndex = 1;
            this.txtProductSearch.TextChanged += new System.EventHandler(this.txtProductSearch_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Medic.";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grdMedicament);
            this.groupBox2.Controls.Add(this.btnSearchMedicamento);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.lblRecordCount);
            this.groupBox2.Controls.Add(this.txtProductSearch);
            this.groupBox2.Location = new System.Drawing.Point(4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(395, 228);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Busqueda de Medicamentos";
            // 
            // btnSearchMedicamento
            // 
            this.btnSearchMedicamento.Image = global::Sigesoft.Node.WinClient.UI.Resources.find;
            this.btnSearchMedicamento.Location = new System.Drawing.Point(363, 14);
            this.btnSearchMedicamento.Margin = new System.Windows.Forms.Padding(2);
            this.btnSearchMedicamento.Name = "btnSearchMedicamento";
            this.btnSearchMedicamento.Size = new System.Drawing.Size(25, 22);
            this.btnSearchMedicamento.TabIndex = 2;
            this.btnSearchMedicamento.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearchMedicamento.UseVisualStyleBackColor = true;
            this.btnSearchMedicamento.Click += new System.EventHandler(this.btnSearchMedicamento_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Image = global::Sigesoft.Node.WinClient.UI.Resources.accept;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(503, 236);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "Aceptar";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::Sigesoft.Node.WinClient.UI.Resources.system_close;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(665, 236);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAddAndNew
            // 
            this.btnAddAndNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddAndNew.Image = global::Sigesoft.Node.WinClient.UI.Resources.add;
            this.btnAddAndNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddAndNew.Location = new System.Drawing.Point(584, 236);
            this.btnAddAndNew.Name = "btnAddAndNew";
            this.btnAddAndNew.Size = new System.Drawing.Size(80, 30);
            this.btnAddAndNew.TabIndex = 5;
            this.btnAddAndNew.Text = "&Agregar";
            this.btnAddAndNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ttAddMedication.SetToolTip(this.btnAddAndNew, "Agregar+Nuevo");
            this.btnAddAndNew.UseVisualStyleBackColor = true;
            this.btnAddAndNew.Click += new System.EventHandler(this.btnAddAndNew_Click);
            // 
            // uvAddMedication
            // 
            this.uvAddMedication.ErrorImageAlignment = System.Windows.Forms.ErrorIconAlignment.MiddleLeft;
            // 
            // ttAddMedication
            // 
            this.ttAddMedication.AutomaticDelay = 200;
            this.ttAddMedication.AutoPopDelay = 7000;
            this.ttAddMedication.InitialDelay = 200;
            this.ttAddMedication.ReshowDelay = 40;
            // 
            // frmAddMedicationMedicalConsult
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 268);
            this.Controls.Add(this.btnAddAndNew);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbMedicamentoSeleccionado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddMedicationMedicalConsult";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Agregar Medicación";
            this.Load += new System.EventHandler(this.frmAddMedicationMedicalConsult_Load);
            this.gbMedicamentoSeleccionado.ResumeLayout(false);
            this.gbMedicamentoSeleccionado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMedicament)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uvAddMedication)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMedicamentoSeleccionado;
        private System.Windows.Forms.Label lblPresentacion;
        private System.Windows.Forms.ComboBox cbVia;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDosis;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCantidad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnSearchMedicamento;
        private System.Windows.Forms.Label lblRecordCount;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdMedicament;
        private System.Windows.Forms.TextBox txtProductSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.Button btnAddAndNew;
        private Infragistics.Win.Misc.UltraValidator uvAddMedication;
        private System.Windows.Forms.ToolTip ttAddMedication;
    }
}
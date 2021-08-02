namespace Sigesoft.Node.WinClient.UI
{
    partial class frmAddAdditionalExam
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("v_ComponentName", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddAdditionalExam));
            this.button7 = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbExamenesSeleccionados = new System.Windows.Forms.GroupBox();
            this.lvExamenesSeleccionados = new System.Windows.Forms.ListView();
            this.chExamen = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chMedicalExamId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chServiceComponentConcatId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRemoverExamenAuxiliar = new System.Windows.Forms.Button();
            this.btnAgregarExamenAuxiliar = new System.Windows.Forms.Button();
            this.grdDataServiceComponent = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.gbExamenesSeleccionados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDataServiceComponent)).BeginInit();
            this.SuspendLayout();
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button7.BackColor = System.Drawing.SystemColors.Control;
            this.button7.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button7.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.button7.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button7.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Image = global::Sigesoft.Node.WinClient.UI.Resources.system_close;
            this.button7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button7.Location = new System.Drawing.Point(553, 348);
            this.button7.Margin = new System.Windows.Forms.Padding(2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 24);
            this.button7.TabIndex = 101;
            this.button7.Text = "Cancelar";
            this.button7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button7.UseVisualStyleBackColor = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.BackColor = System.Drawing.SystemColors.Control;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Image = global::Sigesoft.Node.WinClient.UI.Resources.system_save;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(474, 348);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 100;
            this.btnOK.Text = "      Guardar";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbExamenesSeleccionados
            // 
            this.gbExamenesSeleccionados.Controls.Add(this.lvExamenesSeleccionados);
            this.gbExamenesSeleccionados.Location = new System.Drawing.Point(340, 12);
            this.gbExamenesSeleccionados.Name = "gbExamenesSeleccionados";
            this.gbExamenesSeleccionados.Size = new System.Drawing.Size(289, 331);
            this.gbExamenesSeleccionados.TabIndex = 104;
            this.gbExamenesSeleccionados.TabStop = false;
            this.gbExamenesSeleccionados.Text = "Examenes seleccionados";
            // 
            // lvExamenesSeleccionados
            // 
            this.lvExamenesSeleccionados.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chExamen,
            this.chMedicalExamId,
            this.chServiceComponentConcatId});
            this.lvExamenesSeleccionados.FullRowSelect = true;
            this.lvExamenesSeleccionados.Location = new System.Drawing.Point(7, 17);
            this.lvExamenesSeleccionados.Name = "lvExamenesSeleccionados";
            this.lvExamenesSeleccionados.Size = new System.Drawing.Size(276, 308);
            this.lvExamenesSeleccionados.TabIndex = 0;
            this.lvExamenesSeleccionados.UseCompatibleStateImageBehavior = false;
            this.lvExamenesSeleccionados.View = System.Windows.Forms.View.Details;
            this.lvExamenesSeleccionados.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvExamenesSeleccionados_ItemSelectionChanged);
            // 
            // chExamen
            // 
            this.chExamen.Text = "Examen";
            this.chExamen.Width = 272;
            // 
            // chMedicalExamId
            // 
            this.chMedicalExamId.Text = "MedicalExamId";
            this.chMedicalExamId.Width = 0;
            // 
            // chServiceComponentConcatId
            // 
            this.chServiceComponentConcatId.Text = "ServiceComponentConcatId";
            this.chServiceComponentConcatId.Width = 0;
            // 
            // btnRemoverExamenAuxiliar
            // 
            this.btnRemoverExamenAuxiliar.Enabled = false;
            this.btnRemoverExamenAuxiliar.Image = global::Sigesoft.Node.WinClient.UI.Resources.system_close;
            this.btnRemoverExamenAuxiliar.Location = new System.Drawing.Point(315, 154);
            this.btnRemoverExamenAuxiliar.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemoverExamenAuxiliar.Name = "btnRemoverExamenAuxiliar";
            this.btnRemoverExamenAuxiliar.Size = new System.Drawing.Size(25, 22);
            this.btnRemoverExamenAuxiliar.TabIndex = 103;
            this.btnRemoverExamenAuxiliar.UseVisualStyleBackColor = true;
            this.btnRemoverExamenAuxiliar.Click += new System.EventHandler(this.btnRemoverExamenAuxiliar_Click);
            // 
            // btnAgregarExamenAuxiliar
            // 
            this.btnAgregarExamenAuxiliar.Enabled = false;
            this.btnAgregarExamenAuxiliar.Image = global::Sigesoft.Node.WinClient.UI.Resources.play_green;
            this.btnAgregarExamenAuxiliar.Location = new System.Drawing.Point(315, 113);
            this.btnAgregarExamenAuxiliar.Margin = new System.Windows.Forms.Padding(2);
            this.btnAgregarExamenAuxiliar.Name = "btnAgregarExamenAuxiliar";
            this.btnAgregarExamenAuxiliar.Size = new System.Drawing.Size(25, 22);
            this.btnAgregarExamenAuxiliar.TabIndex = 102;
            this.btnAgregarExamenAuxiliar.UseVisualStyleBackColor = true;
            this.btnAgregarExamenAuxiliar.Click += new System.EventHandler(this.btnAgregarExamenAuxiliar_Click);
            // 
            // grdDataServiceComponent
            // 
            this.grdDataServiceComponent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdDataServiceComponent.CausesValidation = false;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.Silver;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdDataServiceComponent.DisplayLayout.Appearance = appearance1;
            ultraGridColumn8.Header.Caption = "Examen";
            ultraGridColumn8.Header.VisiblePosition = 0;
            ultraGridColumn8.Width = 287;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn8});
            this.grdDataServiceComponent.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDataServiceComponent.DisplayLayout.InterBandSpacing = 10;
            this.grdDataServiceComponent.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDataServiceComponent.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdDataServiceComponent.DisplayLayout.NewColumnLoadStyle = Infragistics.Win.UltraWinGrid.NewColumnLoadStyle.Hide;
            this.grdDataServiceComponent.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdDataServiceComponent.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdDataServiceComponent.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdDataServiceComponent.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this.grdDataServiceComponent.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdDataServiceComponent.DisplayLayout.Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.grdDataServiceComponent.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.Color.White;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            this.grdDataServiceComponent.DisplayLayout.Override.CellAppearance = appearance3;
            this.grdDataServiceComponent.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.LightGray;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.BorderColor = System.Drawing.Color.DarkGray;
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.ForegroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance4.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDataServiceComponent.DisplayLayout.Override.HeaderAppearance = appearance4;
            this.grdDataServiceComponent.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance5.AlphaLevel = ((short)(187));
            appearance5.BackColor = System.Drawing.Color.Gainsboro;
            appearance5.BackColor2 = System.Drawing.Color.Gainsboro;
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.ForegroundAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDataServiceComponent.DisplayLayout.Override.RowAlternateAppearance = appearance5;
            appearance6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdDataServiceComponent.DisplayLayout.Override.RowSelectorAppearance = appearance6;
            this.grdDataServiceComponent.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance7.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            appearance7.BackColor2 = System.Drawing.SystemColors.GradientInactiveCaption;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance7.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            appearance7.BorderColor2 = System.Drawing.SystemColors.GradientActiveCaption;
            appearance7.FontData.BoldAsString = "False";
            appearance7.ForeColor = System.Drawing.Color.Black;
            this.grdDataServiceComponent.DisplayLayout.Override.SelectedRowAppearance = appearance7;
            this.grdDataServiceComponent.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDataServiceComponent.DisplayLayout.RowConnectorColor = System.Drawing.SystemColors.ControlDarkDark;
            this.grdDataServiceComponent.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Dashed;
            this.grdDataServiceComponent.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdDataServiceComponent.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdDataServiceComponent.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdDataServiceComponent.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdDataServiceComponent.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.grdDataServiceComponent.Location = new System.Drawing.Point(3, 12);
            this.grdDataServiceComponent.Margin = new System.Windows.Forms.Padding(2);
            this.grdDataServiceComponent.Name = "grdDataServiceComponent";
            this.grdDataServiceComponent.Size = new System.Drawing.Size(308, 331);
            this.grdDataServiceComponent.TabIndex = 105;
            this.grdDataServiceComponent.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdDataServiceComponent_AfterSelectChange);
            this.grdDataServiceComponent.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.grdDataServiceComponent_DoubleClickRow);
            // 
            // frmAddAdditionalExam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 375);
            this.Controls.Add(this.grdDataServiceComponent);
            this.Controls.Add(this.gbExamenesSeleccionados);
            this.Controls.Add(this.btnRemoverExamenAuxiliar);
            this.Controls.Add(this.btnAgregarExamenAuxiliar);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddAdditionalExam";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Agregar Examen Adicional";
            this.Load += new System.EventHandler(this.frmAddAdditionalExam_Load);
            this.gbExamenesSeleccionados.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDataServiceComponent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbExamenesSeleccionados;
        private System.Windows.Forms.ListView lvExamenesSeleccionados;
        private System.Windows.Forms.ColumnHeader chExamen;
        private System.Windows.Forms.ColumnHeader chMedicalExamId;
        private System.Windows.Forms.Button btnRemoverExamenAuxiliar;
        private System.Windows.Forms.Button btnAgregarExamenAuxiliar;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDataServiceComponent;
        private System.Windows.Forms.ColumnHeader chServiceComponentConcatId;
    }
}
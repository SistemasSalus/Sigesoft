namespace Sigesoft.Node.WinClient.UI.Reports
{
    partial class frmManagementReports_
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chklConsolidadoReportes = new System.Windows.Forms.CheckedListBox();
            this.chkTodos = new System.Windows.Forms.CheckBox();
            this.btnConsolidadoReportes = new System.Windows.Forms.Button();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnConsolidadoReportes);
            this.groupBox5.Controls.Add(this.chklConsolidadoReportes);
            this.groupBox5.Controls.Add(this.chkTodos);
            this.groupBox5.Location = new System.Drawing.Point(12, 10);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(483, 397);
            this.groupBox5.TabIndex = 137;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Reportes Seleccionados";
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
            this.chkTodos.CheckedChanged += new System.EventHandler(this.chkTodos_CheckedChanged);
            // 
            // btnConsolidadoReportes
            // 
            this.btnConsolidadoReportes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConsolidadoReportes.AutoSize = true;
            this.btnConsolidadoReportes.BackColor = System.Drawing.SystemColors.Control;
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
            // frmManagementReports_
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 417);
            this.Controls.Add(this.groupBox5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmManagementReports_";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administrador de Reportes";
            this.Load += new System.EventHandler(this.frmManagementReports__Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnConsolidadoReportes;
        private System.Windows.Forms.CheckedListBox chklConsolidadoReportes;
        private System.Windows.Forms.CheckBox chkTodos;
    }
}
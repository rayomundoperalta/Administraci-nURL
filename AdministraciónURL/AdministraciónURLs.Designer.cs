namespace AdministraciónURL
{
    partial class AdministraciónURLs
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
            this.label1 = new System.Windows.Forms.Label();
            this.URLs = new System.Windows.Forms.ListBox();
            this.informacionAPFDataSet = new AdministraciónURL.InformacionAPFDataSet();
            this.uRLToBeDownloadedBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.uRLToBeDownloadedTableAdapter = new AdministraciónURL.InformacionAPFDataSetTableAdapters.URLToBeDownloadedTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.informacionAPFDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uRLToBeDownloadedBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "URL Administradas";
            // 
            // URLs
            // 
            this.URLs.BackColor = System.Drawing.Color.LightBlue;
            this.URLs.FormattingEnabled = true;
            this.URLs.ItemHeight = 16;
            this.URLs.Location = new System.Drawing.Point(16, 34);
            this.URLs.MultiColumn = true;
            this.URLs.Name = "URLs";
            this.URLs.ScrollAlwaysVisible = true;
            this.URLs.Size = new System.Drawing.Size(710, 404);
            this.URLs.TabIndex = 1;
            this.URLs.SelectedIndexChanged += new System.EventHandler(this.URLs_SelectedIndexChanged);
            // 
            // informacionAPFDataSet
            // 
            this.informacionAPFDataSet.DataSetName = "InformacionAPFDataSet";
            this.informacionAPFDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uRLToBeDownloadedBindingSource
            // 
            this.uRLToBeDownloadedBindingSource.DataMember = "URLToBeDownloaded";
            this.uRLToBeDownloadedBindingSource.DataSource = this.informacionAPFDataSet;
            // 
            // uRLToBeDownloadedTableAdapter
            // 
            this.uRLToBeDownloadedTableAdapter.ClearBeforeFill = true;
            // 
            // AdministraciónURLs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Khaki;
            this.ClientSize = new System.Drawing.Size(738, 460);
            this.Controls.Add(this.URLs);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AdministraciónURLs";
            this.Text = "Administración de URLs";
            this.Load += new System.EventHandler(this.AdministraciónURLs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.informacionAPFDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uRLToBeDownloadedBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox URLs;
        private InformacionAPFDataSet informacionAPFDataSet;
        private System.Windows.Forms.BindingSource uRLToBeDownloadedBindingSource;
        private InformacionAPFDataSetTableAdapters.URLToBeDownloadedTableAdapter uRLToBeDownloadedTableAdapter;
    }
}


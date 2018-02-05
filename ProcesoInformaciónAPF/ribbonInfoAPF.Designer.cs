namespace ProcesoInformaciónAPF
{
    partial class ribbonInfoAPF : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ribbonInfoAPF()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabInfoAPF = this.Factory.CreateRibbonTab();
            this.Estado = this.Factory.CreateRibbonGroup();
            this.checkBox1 = this.Factory.CreateRibbonCheckBox();
            this.tabInfoAPF.SuspendLayout();
            this.Estado.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabInfoAPF
            // 
            this.tabInfoAPF.Groups.Add(this.Estado);
            this.tabInfoAPF.Label = "InfoAPF";
            this.tabInfoAPF.Name = "tabInfoAPF";
            // 
            // Estado
            // 
            this.Estado.Items.Add(this.checkBox1);
            this.Estado.Label = "Estado";
            this.Estado.Name = "Estado";
            // 
            // checkBox1
            // 
            this.checkBox1.Label = "Activado";
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.checkBox1_Click);
            // 
            // ribbonInfoAPF
            // 
            this.Name = "ribbonInfoAPF";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tabInfoAPF);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.InfoAPF_Load);
            this.tabInfoAPF.ResumeLayout(false);
            this.tabInfoAPF.PerformLayout();
            this.Estado.ResumeLayout(false);
            this.Estado.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tabInfoAPF;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup Estado;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox checkBox1;
    }

    partial class ThisRibbonCollection
    {
        internal ribbonInfoAPF InfoAPF
        {
            get { return this.GetRibbon<ribbonInfoAPF>(); }
        }
    }
}

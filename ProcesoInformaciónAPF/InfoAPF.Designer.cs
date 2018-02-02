namespace ProcesoInformaciónAPF
{
    partial class InfoAPF : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public InfoAPF()
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
            this.labelEstado = this.Factory.CreateRibbonLabel();
            this.buttonONOFF = this.Factory.CreateRibbonButton();
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
            this.Estado.Items.Add(this.labelEstado);
            this.Estado.Items.Add(this.buttonONOFF);
            this.Estado.Label = "Estado";
            this.Estado.Name = "Estado";
            // 
            // labelEstado
            // 
            this.labelEstado.Label = "Activado";
            this.labelEstado.Name = "labelEstado";
            this.labelEstado.ScreenTip = "Muestra es estado de complemento InfoAPF";
            // 
            // buttonONOFF
            // 
            this.buttonONOFF.Description = "Activa y desactiva el complemento InfoAPF";
            this.buttonONOFF.Label = "ON/OFF";
            this.buttonONOFF.Name = "buttonONOFF";
            this.buttonONOFF.ScreenTip = "Activa y desactiva el complemento";
            // 
            // InfoAPF
            // 
            this.Name = "InfoAPF";
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
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel labelEstado;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonONOFF;
    }

    partial class ThisRibbonCollection
    {
        internal InfoAPF InfoAPF
        {
            get { return this.GetRibbon<InfoAPF>(); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;

namespace ProcesoInformaciónAPF
{
    public partial class InfoAPF
    {
        private void buttonONOFF_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("Boton ON OFF", "Procesador Información APF");
        }

        private void InfoAPF_Load(object sender, RibbonUIEventArgs e)
        {
            this.buttonONOFF.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonONOFF_Click);
        }
    }
}

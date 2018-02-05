using Globals;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;
using Utility.ModifyRegistry;

namespace ProcesoInformaciónAPF
{
    public partial class ribbonInfoAPF
    {
        static Cadenas g = new Cadenas();
        private ModifyRegistry myModifyRegistry;

        private void InfoAPF_Load(object sender, RibbonUIEventArgs e)
        {
            myModifyRegistry = new ModifyRegistry(g.RegEditID());
        }

        private void checkBox1_Click(object sender, RibbonControlEventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                myModifyRegistry.Write(g.RegKeyEstado(), g.EstadoActivado());
            } else
            {
                myModifyRegistry.Write(g.RegKeyEstado(), g.EstadoDesactivado());
            }
        }

        public void set_checkBox1(bool state)
        {
            checkBox1.Checked = state;
        }
    }
}

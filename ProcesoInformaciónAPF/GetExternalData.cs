using System;
using System.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcesoInformaciónAPF
{
    [ComVisible(true)]
    public interface IMoveParameters
    {
        void PushInfo(int año);
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class MoveParameters : IMoveParameters
    {
        public void PushInfo(int año)
        {
            Globals.ThisAddIn.año = año;
        }
    }
}

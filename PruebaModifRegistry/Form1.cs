using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility.ModifyRegistry;

namespace PruebaModifRegistry
{
    public partial class Form1 : Form
    {
        private ModifyRegistry myModifyRegistry = new ModifyRegistry("PetaComputing");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                textBox1.Text = "default";
            }
            if (myModifyRegistry.Write("Acrtivado", textBox1.Text))
            {
                textBox1.Text = "";
            } else
            {
                textBox1.Text = "E R R O R";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string result = myModifyRegistry.Read("Acrtivado");
            label1.Text = result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            myModifyRegistry.DeleteKey("Acrtivado");
            myModifyRegistry.DeleteSubKeyTree();
        }
    }
}

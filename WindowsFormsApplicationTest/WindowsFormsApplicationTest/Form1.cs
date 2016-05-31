using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplicationTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public int getCurrentCursorLine(TextBox editText) //Returns the current Y cursor position
        {
            int selectionStart = editText.SelectionStart;
            //Layout layout = editText.getLayout();

            if (!(selectionStart == -1))
            {
                //return layout.getLineForOffset(selectionStart);
                return editText.GetLineFromCharIndex(selectionStart);
            }
            return -1;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int selectionStart;
            selectionStart = getCurrentCursorLine(commandWindow);
            Debug.WriteLine("current cursor position is: " + selectionStart);
        }
    }
}

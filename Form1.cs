using System;
using System.Windows.Forms;

namespace MyWinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();  // Wywołanie generowanej metody InitializeComponent
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Przycisk został kliknięty!");
        }
    }
}

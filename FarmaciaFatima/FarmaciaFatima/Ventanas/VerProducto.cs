using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FarmaciaFatima.Ventanas
{
    public partial class VerProducto : Form
    {
        public VerProducto()
        {
            InitializeComponent();
        }


        public void ponerFoto(string ruta) {
            try
            {
                pictureBox1.Image= Image.FromFile(@"C:\reportesFarmaciaFatima\foto\" + ruta + ".png");
                this.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("No hay foto");
            }
            
        }
    }
}

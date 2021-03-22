using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FarmaciaFatima.Ventanas
{
    public partial class DataBaseAdmin : Form
    {
        public DataBaseAdmin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Conexion conexion = new Conexion();
            string sConsulta = richTextBox1.Text;
            DataTable tabla = conexion.retornaTabla(sConsulta);
            if (tabla != null)
            {
                dataGridView1.DataSource = tabla;
            }
            else {
                MessageBox.Show("Consulta mala");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}

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
    public partial class EditarProducto : Form
    {

        public string id = "";
        public string detalle = "";
        public string descripcion = "";
        public string cantidad = "";
        public string precio = "";
        public string fecha = "";
        public string casaMedica = "";
        public string tipo = "";
        public string idProducto = "";
        public string idPresedencia = "";
        public string cantidadCP = "";

        DataGridView ew = null;
        public EditarProducto()
        {
            InitializeComponent();
        }

        public void Inicializador(string id, string detalle, string descripcion, string cantidad, string precio, string fecha, string casaMedica, string tipo, string idP, string idPresedencia, string cantidadP)
        {
            this.id = id;
            this.detalle = detalle;
            this.descripcion = descripcion;
            this.cantidad = cantidad;
            this.precio = precio;
            this.fecha = fecha;
            this.casaMedica = casaMedica;
            this.idPresedencia = idPresedencia;
            this.cantidadCP = cantidadP;
            txtId.Text = id;
            txtdetalle.Text = detalle;
            txtPrecedenciaM.Text = cantidad;
            if (tipo.Equals("1")) {
                txtPrecedenciaM.Text = cantidadCP;
            }
            textBox3.Text = precio;
            richDescripcion.Text = descripcion;
            this.tipo = tipo;
            this.idProducto = idP;
            mostrarCombobox2();
            try
            {
                dateTimePicker1.Value = DateTime.Parse(fecha);
                cbxCasaMedica.SelectedValue = retornaDato("SELECT * FROM casaMedica WHERE nombre='" + casaMedica + "'", "idCasaMedica");
            }
            catch (Exception)
            {

                MessageBox.Show("No existe fecha en este producto");
            }

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // solo 1 punto decimal
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtPrecedenciaM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Conexion conexion = new Conexion();
            string sConsulta = "";

            if (tipo.Equals("2"))
            {
                sConsulta = "UPDATE bodega SET cantidad=" + txtPrecedenciaM.Text + ", precio=" + textBox3.Text + " ,fechaCaducidad='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "', idCasaMedica='" + cbxCasaMedica.SelectedValue.ToString() + "' WHERE idBodega=" + txtId.Text;
                conexion.cargaMasiva(sConsulta);
                sConsulta = "UPDATE producto SET descripcion='" + richDescripcion.Text + "' WHERE idProducto=" + this.idProducto;
                conexion.cargaMasiva(sConsulta);
            }
            else {
                sConsulta = "UPDATE bodega SET cantidad=" + txtPrecedenciaM.Text + ",fechaCaducidad='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "', idCasaMedica='" + cbxCasaMedica.SelectedValue.ToString() + "' WHERE idBodega=" + txtId.Text;
                conexion.cargaMasiva(sConsulta);
                sConsulta = "UPDATE PresedenciaProducto SET Precio=" + textBox3.Text + "   WHERE idPresedenciaProducto=" + this.idPresedencia;
                conexion.cargaMasiva(sConsulta);
                sConsulta = "UPDATE producto SET descripcion='" + richDescripcion.Text + "' WHERE idProducto=" + this.idProducto;
                conexion.cargaMasiva(sConsulta);
            }
            try
            {
                ew.DataSource = null;
            }
            catch (Exception)
            {
                MessageBox.Show("Actualice en la ventana principal");
            }

            this.Close();
            GC.Collect();
        }

        public void removeD(DataGridView opt) {
            ew = opt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Conexion conexion = new Conexion();
            string sConsulta = "UPDATE bodega SET estado=0 WHERE idBodega=" + txtId.Text;
            conexion.cargaMasiva(sConsulta);
            conexion.cargaMasiva(sConsulta);
            this.Close();
            GC.Collect();
        }

        public void mostrarCombobox2()
        {
            Conexion conexion = new Conexion();
            string consulta = "SELECT idCasaMedica, nombre FROM casaMedica ORDER BY nombre ASC";
            DataTable tabla = conexion.retornaTabla(consulta);
            if (tabla != null)
            {
                cbxCasaMedica.DataSource = tabla;
                cbxCasaMedica.DisplayMember = "nombre";
                cbxCasaMedica.ValueMember = "idCasaMedica";
            }
            else
            {
                MessageBox.Show("No se pudo mostrar las casas medicas");
            }

            GC.Collect();
        }

        public string retornaDato(string sConsulta, string columna)
        {
            Conexion conexion = new Conexion();
            string consulta = sConsulta;
            DataTable tabla = conexion.retornaTabla(consulta);
            if (tabla != null)
            {
                if (tabla.Rows.Count == 0)
                {
                    return "";
                }

                if (tabla.Rows.Count > 1)
                {
                    return "";
                }

                if (tabla.Rows.Count == 1)
                {
                    string loque = tabla.Rows[0][columna].ToString();
                    return loque;
                }

            }
            else
            {
                MessageBox.Show("Algo malo comprobando existencia de casa medica");
                return "";
            }
            return "";
        }
    }
}

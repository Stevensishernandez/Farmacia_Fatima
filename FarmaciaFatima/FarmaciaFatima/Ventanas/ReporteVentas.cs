using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FarmaciaFatima.Procesos;

namespace FarmaciaFatima.Ventanas
{
    public partial class ReporteVentas : Form
    {
        public ReporteVentas()
        {
            InitializeComponent();
            mostrarCombobox();
        }

        private void Salir_Click(object sender, EventArgs e)
        {
            this.Close();
            GC.Collect();
        }

        private void Minimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Maximizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            Maximizar.Visible = false;
            restaurar.Visible = true;
        }

        private void restaurar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            restaurar.Visible = false;
            Maximizar.Visible = true;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                CrearReporte reporte = new CrearReporte();
                reporte.reporteVentas(dateTimePicker1.Value.ToString("MM-dd-yyyy"));
            }
            catch (Exception)
            {
                MessageBox.Show("Error al generar reporte del dia");
            }
            
            
        }

        private void btnVender_Click(object sender, EventArgs e)
        {

            try
            {
                createRInventario inventario = new createRInventario();
                inventario.hacerReporte(DateTime.Now.Date.ToString("yyyy-MM-dd"));
            }
            catch (Exception)
            {

                MessageBox.Show("No se pudo generar el reporte de inventario");
            }
            
        }

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mostrarCombobox();
            btnEliminarV.Visible = false;
        }

        public void mostrarCombobox()
        {

            Conexion conexion2 = new Conexion();
            string consulta2 = "SELECT * FROM venta ORDER BY idVenta ASC";
            DataTable tabla2 = conexion2.retornaTabla(consulta2);
            if (tabla2 != null)
            {
                cbxVentas.DataSource = tabla2;
                cbxVentas.DisplayMember = "idVenta";
                cbxVentas.ValueMember = "idVenta";
            }
            else
            {
                MessageBox.Show("No se pudo mostrar las Presentaciones en las listas");
            }

            GC.Collect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mostrarTabla();
        }

        public void mostrarTabla() {
            btnEliminarV.Visible = true;
            Conexion conexion = new Conexion();
            if (cbxVentas.SelectedValue == null) { MessageBox.Show("Seleccione una venta"); btnEliminarV.Visible = false; return; }
            string consulta = "SELECT DT.idDetalleVenta Venta, V.fecha, V.hora, DT.detalle, DT.cantidad, DT.subTotal FROM DetalleVenta DT " +
                        "INNER  JOIN Venta V ON(V.IdVenta = DT.idVenta) WHERE  DT.idVenta=" + cbxVentas.SelectedValue.ToString() + " ";
            DataTable tabla = conexion.retornaTabla(consulta);
            if (tabla != null)
            {
                gridVentas.Columns.Clear();
                DataGridViewButtonColumn btnaddCarrito = new DataGridViewButtonColumn();
                btnaddCarrito.Text = "Eliminar";
                btnaddCarrito.UseColumnTextForButtonValue = true;
                btnaddCarrito.Name = "Eliminar";
                gridVentas.Columns.Add(btnaddCarrito);
                gridVentas.DataSource = tabla;

            }
            else
            {
                MessageBox.Show("No se pudo mostrar las casas medicas");
            }
            GC.Collect();
        }

        private void gridVentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!txtPassword.Text.Equals("FatimaFarmVentas")) { MessageBox.Show("Contraseña no ingresa o incorrecta"); btnEliminarV.Visible = false; return; }
            if (this.gridVentas.Columns.Count == 0 || this.gridVentas.Rows.Count == 0) { btnEliminarV.Visible = false; return; }
            if (this.gridVentas.Columns[e.ColumnIndex].Name == "Eliminar")
            {
                Conexion conexion = new Conexion();
                string sConsulta = "DELETE FROM DetalleVenta WHERE idDetalleVenta="+gridVentas.CurrentRow.Cells[1].Value.ToString();
                conexion.cargaMasiva(sConsulta);
                mostrarTabla();
                MessageBox.Show("Venta Eliminada");
                GC.Collect();
            }
        }

        private void btnEliminarV_Click(object sender, EventArgs e)
        {
            if (!txtPassword.Text.Equals("FatimaFarmVentas")) { MessageBox.Show("Contraseña no ingresa o incorrecta"); return; }
            Conexion conexion = new Conexion();
            string sConsulta = "DELETE FROM VENTA WHERE idVenta=" + cbxVentas.SelectedValue.ToString();
            conexion.cargaMasiva(sConsulta);
            mostrarTabla();
            mostrarCombobox();
            btnEliminarV.Visible = false;
            GC.Collect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                createRInventario inventario = new createRInventario();
                inventario.hacerReporte2(DateTime.Now.Date.ToString("yyyy-MM-dd"));
            }
            catch (Exception)
            {

                MessageBox.Show("No se pudo generar el reporte de inventario");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            createRCostos costos = new createRCostos();
            costos.createReporte(DateTime.Now.Date.ToString("yyyy-MM-dd"));
        }

        private void tabReportes_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                createRInventario inventario = new createRInventario();
                inventario.hacerReporteOcM(DateTime.Now.Date.ToString("yyyy-MM-dd"));
            }
            catch (Exception)
            {

                MessageBox.Show("No se pudo generar el reporte de inventario");
            }
        }
    }
}

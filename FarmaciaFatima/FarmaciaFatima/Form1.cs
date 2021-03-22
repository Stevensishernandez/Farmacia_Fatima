using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FarmaciaFatima.Ventanas;
using FarmaciaFatima.Modelos;
using FarmaciaFatima.Procesos;

namespace FarmaciaFatima
{
    public partial class Form1 : Form
    {
        List<venta> lstVentas;
        int contadorY = 33;
        float TotalVenta = 0;
        public Form1()
        {
            InitializeComponent();
            SideBarWrapper.Width = 202;
            bunifuSeparator1.Width = 202;
            btnAbrir.Width = 202;
            lstVentas = new List<venta>();
            label5.Visible = false;
            contadorY += 27;
            IniciarDatos();
        }

        private void Salir_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void menuSideBar_Click(object sender, EventArgs e)
        {
            Console.WriteLine(SideBarWrapper.Size);
            if (SideBarWrapper.Width == 202)
            {
                SideBarWrapper.Visible = false;
                SideBarWrapper.Width = 58;
                bunifuSeparator1.Width = 58;
                AnimacionSidebarBack.Show(SideBarWrapper);
                btnAbrir.Text = "";
                btnBodega.Text = "";
            }
            else
            {
                SideBarWrapper.Visible = false;
                SideBarWrapper.Width = 202;
                bunifuSeparator1.Width = 202;
                AnimacionSidebarBack.Show(SideBarWrapper);
                btnAbrir.Text = "Abrir";
                btnBodega.Text = "Bodega";
            }

            GC.Collect();
        }

    

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            string respuesta = Microsoft.VisualBasic.Interaction.InputBox("Ingrese la contraseña ", "Contraseña de administracion", "", 100, 0);
            //Carga de los archivos 

            if (respuesta.Equals("FatimaFarm"))
            {
                /*if (OpenFile.ShowDialog() == DialogResult.OK) //abre el dialogo
                {
                    try
                    {
                        Conexion conexion = new Conexion();
                        string consulta = "BULK INSERT Temporal " +
                                          "FROM '" + OpenFile.FileName + "' " +
                                          "WITH " +
                                          "  (  " +
                                               " FIELDTERMINATOR = ',',  " +
                                               " ROWTERMINATOR = '\n' " +
                                           " ) ";

                        conexion.cargaMasiva(consulta);
                        conexion.cargaMasiva("INSERT INTO casaMedica (nombre) (SELECT DISTINCT casaMedica FROM Temporal)");
                        conexion.cargaMasiva("INSERT INTO presentacion (nombre) (SELECT DISTINCT presentacion FROM Temporal)");
                        conexion.cargaMasiva("INSERT INTO producto (nombre, fecha) (SELECT DISTINCT nombreP, fechaV FROM Temporal)");
                        conexion.cargaMasiva("DELETE Temporal");
                        GC.Collect();
                        MessageBox.Show("Se realizo la carga masiva");
                        
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error al cargar Archivo");
                    }
                }
                else
                {

                }*/

                DataBaseAdmin ventanaAdmin = new DataBaseAdmin();
                ventanaAdmin.Show();
                GC.Collect();

            }
            else {
                MessageBox.Show("Contraseña incorrecta");
            }
            
        }

        private void btnBodega_Click(object sender, EventArgs e)
        {
            Bodega bodega = new Bodega();
            bodega.Show();
            bodega.opt(gridProductos);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            MostrarProductos();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                MostrarProductos();
            }
        }

        public void MostrarProductos() {
            string consulta = "";
            Conexion conexion = new Conexion();
            if (textBox1.Text.Equals(""))
            {
                consulta = " (SELECT T.idBodega AS Codigo, P.nombre AS Producto, PR.nombre AS Presentacion, C.nombre AS Casa_Medica, T.precio AS Q_Precio, P.descripcion, T.cantidad AS Disponible, T.fechaCaducidad, T.tipo, C.nombre AS Representa FROM bodega T "
                                + " INNER JOIN producto P on(P.idProducto = T.idProducto) "
                                + " INNER JOIN casaMedica C on(C.idCasaMedica = T.idCasaMedica)" +
                                  " INNER JOIN presentacion PR ON (PR.IdPresentacion = T.idPresentacion) WHERE T.estado=1 )  " +
                                  " UNION ALL " +
                                  " ( SELECT B.IdBodega, PR.nombre, PS.nombre,C.nombre, PP.precio,PR.descripcion, (B.cantidad/PP.representacion) AS Cantidad, B.fechaCaducidad, B.tipo, STR(PP.representacion)  from PresedenciaProducto PP " +
                                  "  INNER JOIN bodega B ON(B.idBodega = PP.idBodega) "+
                                  "  INNER JOIN presentacion PS ON(PS.idPresentacion = PP.idPresedencia) "+
                                  "  INNER JOIN producto PR ON(PR.idProducto = B.idProducto) "+
                                  "  INNER JOIN casaMedica C ON(C.idCasaMedica = B.idCasaMedica) WHERE B.estado=1  )";
            }else {
                consulta = "(SELECT T.idBodega AS Codigo, P.nombre AS Producto, PR.nombre AS Presentacion,C.nombre AS Casa_Medica, T.precio AS Q_Precio, P.descripcion, T.cantidad AS Disponible, T.fechaCaducidad, T.tipo, C.nombre AS Representa FROM bodega T "
                                + " INNER JOIN producto P on(P.idProducto = T.idProducto) "
                                + " INNER JOIN casaMedica C on(C.idCasaMedica = T.idCasaMedica) " +
                                " INNER JOIN presentacion PR ON (PR.IdPresentacion = T.idPresentacion) " +
                                " WHERE (P.nombre Like '%" + textBox1.Text + "%' OR P.descripcion Like '%" + textBox1.Text + "%' ) AND T.estado=1  )  " +
                                " UNION ALL " +
                                  " ( SELECT B.IdBodega, PR.nombre, PS.nombre,C.nombre, PP.precio,PR.descripcion, (B.cantidad/PP.representacion), B.fechaCaducidad, B.tipo, STR(PP.representacion) from PresedenciaProducto PP  " +
                                  "  INNER JOIN bodega B ON(B.idBodega = PP.idBodega) " +
                                  "  INNER JOIN presentacion PS ON(PS.idPresentacion = PP.idPresedencia) " +
                                  "  INNER JOIN producto PR ON(PR.idProducto = B.idProducto) " +
                                  "  INNER JOIN casaMedica C ON(C.idCasaMedica = B.idCasaMedica) " +
                                  " WHERE (PR.nombre Like '%" + textBox1.Text + "%' OR PR.descripcion Like '%" + textBox1.Text + "%') AND B.estado=1 ) ";
            }
            
            DataTable tabla = conexion.retornaTabla(consulta);
            if (tabla != null)
            {
                gridProductos.Columns.Clear();
                DataGridViewButtonColumn btnaddCarrito = new DataGridViewButtonColumn();
                btnaddCarrito.Text = "Agregar";
                btnaddCarrito.UseColumnTextForButtonValue = true;
                btnaddCarrito.Name = "Agregar";

                DataGridViewButtonColumn btnAddDetails = new DataGridViewButtonColumn();
                btnAddDetails.Text = "Detalle";
                btnAddDetails.UseColumnTextForButtonValue = true;
                btnAddDetails.Name = "Detalle";

                gridProductos.Columns.Add(btnaddCarrito);
                gridProductos.Columns.Add(btnAddDetails);
                gridProductos.DataSource = tabla;
                gridProductos.Columns["tipo"].Visible = false;
                gridProductos.Columns["Representa"].Visible = false;

            }
            else
            {
                MessageBox.Show("No se pudo mostrar las casas medicas");
            }
            GC.Collect();
        }

        private void gridProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.gridProductos.Columns.Count==0 || this.gridProductos.Rows.Count==0) { return; }
            try
            {
                if (this.gridProductos.Columns[e.ColumnIndex].Name == "Agregar")
                {
                    if (float.Parse(gridProductos.CurrentRow.Cells[6].Value.ToString()) <= 0)
                    {
                        MessageBox.Show("No se puede vender un producto sin precio"); return;
                    }

                    if (float.Parse(gridProductos.CurrentRow.Cells[8].Value.ToString()) <= 0)
                    {
                        MessageBox.Show("No se puede vender un producto sin precio"); return;
                    }

                    if (yaEstaenelcarrito(gridProductos.CurrentRow.Cells[2].Value.ToString(), gridProductos.CurrentRow.Cells[11].Value.ToString(), gridProductos.CurrentRow.Cells[5].Value.ToString(), gridProductos.CurrentRow.Cells[10].Value.ToString()))
                    { MessageBox.Show("Este Producto ya esta en el carrito de compra"); return; }

                    string contenidoV = gridProductos.CurrentRow.Cells[3].Value.ToString() + " " + gridProductos.CurrentRow.Cells[5].Value.ToString() + " / " + gridProductos.CurrentRow.Cells[4].Value.ToString();
                    lstVentas.Add(new venta(gridProductos.CurrentRow.Cells[2].Value.ToString(), this.panelCa, contadorY, gridProductos.CurrentRow.Cells[3].Value.ToString(), gridProductos.CurrentRow.Cells[4].Value.ToString(), gridProductos.CurrentRow.Cells[5].Value.ToString(), gridProductos.CurrentRow.Cells[8].Value.ToString(), gridProductos.CurrentRow.Cells[6].Value.ToString(), lstVentas, gridProductos.CurrentRow.Cells[10].Value.ToString(), gridProductos.CurrentRow.Cells[11].Value.ToString(), label5));
                    contadorY += 27;

                }

                if (this.gridProductos.Columns[e.ColumnIndex].Name == "Detalle")
                {
                    VerProducto verProducto = new VerProducto();
                    verProducto.ponerFoto(gridProductos.CurrentRow.Cells[2].Value.ToString());
                }
            }
            catch (Exception)
            {

                MessageBox.Show("No se pudo agregar");
            }
            
        }

        public bool yaEstaenelcarrito(string id, string representa, string casaM, string tipoO) {

            foreach (var v in lstVentas)
            {
                if (tipoO.Equals("2"))
                {
                    if (v.idBodega.Equals(id))
                    {
                        return true;
                    }
                }
                else if (tipoO.Equals("1")) {
                    if (v.idBodega.Equals(id) && v.tipo.Equals(tipoO) && v.CasaMedica.Equals(casaM) && v.representacion.Equals(representa))
                    {
                        return true;
                    }
                }
                
            }

            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TotalVenta = 0;
            if (sePuedeVender() && lstVentas.Count > 0)
            {
                this.txtNombreProducto.Text = TotalVenta.ToString();
            }
            else {
                TotalVenta = 0;
            }
        }

        public bool sePuedeVender() {
            foreach (var sePuedeV in lstVentas)
            {
                if (!sePuedeV.sePuedeVender())
                {
                    MessageBox.Show("La cantidad para el producto " + sePuedeV.texto.Text + " Exede la cantidad disponible en bodega");
                    return false;
                }
            }

            foreach (var sePuedeV in lstVentas)
            {
                sePuedeV.actualizarPrecio();
                try
                {
                    sePuedeV.Vsubtotal = (float.Parse(sePuedeV.precio) * float.Parse(sePuedeV.cantidad.Text));
                    TotalVenta += (float.Parse(sePuedeV.precio) * float.Parse(sePuedeV.cantidad.Text));
                    
                }
                catch (Exception)
                {
                    MessageBox.Show("Algun dato no es numero");
                    return false;
                }
            }

            return true;
        }

        //Al inciciar secion 
        public void IniciarDatos() {
            if (!existeDato("SELECT * FROM presentacion WHERE nombre='000Seleccione'")) {
                Conexion conexion = new Conexion();
                string consulta = "INSERT INTO presentacion (nombre) VALUES (' 000Seleccione')";
                conexion.cargaMasiva(consulta);
            }
            if (!existeDato("SELECT * FROM casaMedica WHERE nombre='000Seleccione'"))
            {
                Conexion conexion = new Conexion();
                string consulta = "INSERT INTO casaMedica (nombre) VALUES (' 000Seleccione')";
                conexion.cargaMasiva(consulta);
            }

            GC.Collect();
        }
        //Ve si un dato existe
        public bool existeDato(string sConsulta)
        {
            Conexion conexion = new Conexion();
            string consulta = sConsulta;
            DataTable tabla = conexion.retornaTabla(consulta);
            if (tabla != null)
            {
                if (tabla.Rows.Count == 0)
                {
                    return false;
                }

                if (tabla.Rows.Count >= 1)
                {
                    return true;
                }

            }
            else
            {
                MessageBox.Show("Algo malo comprobando existencia de casa medica");
                return true;
            }
            return true;
        }

        private void btnVender_Click(object sender, EventArgs e)
        {
            TotalVenta = 0;
            if (sePuedeVender() && lstVentas.Count > 0)
            {
                this.txtNombreProducto.Text = TotalVenta.ToString();
                RealizarVenta nuevaVenta = new RealizarVenta(lstVentas, DateTime.Now.Date.ToString("MM-dd-yyyy"), DateTime.Now.ToString("hh:mm:ss"));
                nuevaVenta.vender();
                MessageBox.Show("Se vendio Q"+TotalVenta.ToString());
                limpiarCarrito();
                this.txtNombreProducto.Text = "0";
                gridProductos.DataSource = null;
            }
            TotalVenta = 0;
            GC.Collect();
        }

       

       

        public void limpiarCarrito()
        {
            foreach (var detalle in lstVentas)
            {
                this.panelCa.Controls.Remove(detalle.temp);
            }
            lstVentas.Clear();
        }

        private void panelCa_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ReporteVentas reporte = new ReporteVentas();
            reporte.Show();
        }
    }
}

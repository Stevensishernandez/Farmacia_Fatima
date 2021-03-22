using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FarmaciaFatima.Modelos;
using FarmaciaFatima.Procesos;

namespace FarmaciaFatima.Ventanas
{
    
    public partial class Bodega : Form
    {
        int ContadorP = 0;
        int PosY = 52;
        AddProduct aniadirProducto;
        List<PresentacionObj> lstPresentaciones;

        DataGridView form1 = null;
        public Bodega()
        {
            InitializeComponent();
            Inicializador();
        }

        public void opt(DataGridView opt) {
            this.form1=opt;
        }


        public void Inicializador() {
            aniadirProducto = new AddProduct();
            txtPrecedenciaM.Enabled = false;
            lstPresentaciones = new List<PresentacionObj>();
            PresentacionObj presentacionS = new PresentacionObj(ContadorP, this.panel3,this.PosY);
            ContadorP += 1;
            PosY += 54;
            lstPresentaciones.Add(presentacionS);
            label6.Visible = false;
            dateTimePicker1.Visible = false;
            foreach (var pre in lstPresentaciones)
            {
                pre.calendario.Visible = true;
            }
            mostrarTabla();
            mostrarCombobox();
            mostrarCombobox2();
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
            PresentacionObj presentacionInc = new PresentacionObj(ContadorP, this.panel3, this.PosY);
            PosY += 54;
            ContadorP += 1;
            lstPresentaciones.Add(presentacionInc);
            mostrarCombobox();
            hacerSiesjerarquia();
        }

        private void btnAniadirCM_Click(object sender, EventArgs e)
        {
            if (txtCasaMedica.Text=="") { MessageBox.Show("Campo Vacio"); return; }
            try
            {
                string sConsulta = "Select nombre FROM casaMedica Where nombre='" + txtCasaMedica.Text+"'";
                if (!(existeDato(sConsulta)) )
                {
                    Conexion conexion = new Conexion();
                    string consulta = "INSERT INTO casaMedica (nombre) Values('" + txtCasaMedica.Text + "')";
                    conexion.cargaMasiva(consulta);
                    GC.Collect();
                    MessageBox.Show("Se inserto una nueva casaMedica");
                    txtCasaMedica.Text = "";
                    mostrarTabla();
                }
                else {
                    MessageBox.Show("Esta Casa Medica Ya existe");
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("Error al cargar Archivo");
            }
        }

        public void mostrarTabla() {
            Conexion conexion = new Conexion();
            string consulta = "SELECT idCasaMedica AS Identificador, nombre AS CasaMedica FROM casaMedica";
            DataTable tabla = conexion.retornaTabla(consulta);
            if (tabla != null)
            {
                gridCasaMedica.DataSource = tabla;
            }
            else {
                MessageBox.Show("No se pudo mostrar las casas medicas");
            }
            Conexion conexion2 = new Conexion();
            consulta = "SELECT idPresentacion AS Identificador, nombre AS Presentacion FROM presentacion";
            DataTable tabla2 = conexion2.retornaTabla(consulta);
            if (tabla2 != null)
            {
                gridPresentacion.DataSource = tabla2;
            }
            else
            {
                MessageBox.Show("No se pudo mostrar las casas medicas");
            }

            GC.Collect();
        }

        public bool existeDato(string sConsulta) {
            Conexion conexion = new Conexion();
            string consulta = sConsulta;
            DataTable tabla = conexion.retornaTabla(consulta);
            if (tabla != null)
            {
                if (tabla.Rows.Count==0) {
                    return false;
                }

                if (tabla.Rows.Count >= 1) {
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

        private void btnAniadirPre_Click(object sender, EventArgs e)
        {
            if (txtPresentacion.Text == "") { MessageBox.Show("Campo Vacio"); return; }
            try
            {
                string sConsulta = "Select nombre FROM presentacion Where nombre='" + txtPresentacion.Text + "'";
                if (!(existeDato(sConsulta)))
                {
                    Conexion conexion = new Conexion();
                    string consulta = "INSERT INTO presentacion (nombre) Values('" + txtPresentacion.Text + "')";
                    conexion.cargaMasiva(consulta);
                    GC.Collect();
                    MessageBox.Show("Se inserto una nueva Presentacion");
                    txtPresentacion.Text = "";
                    mostrarTabla();
                }
                else
                {
                    MessageBox.Show("Esta Presentacion Ya existe");
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Error al agregar Presentacion");
            }
        }

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
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

                cbxCasaMedica2.DataSource = tabla;
                cbxCasaMedica2.DisplayMember = "nombre";
                cbxCasaMedica2.ValueMember = "idCasaMedica";
            }
            else
            {
                MessageBox.Show("No se pudo mostrar las casas medicas");
            }

            GC.Collect();
        }

        public void mostrarCombobox() {

            foreach (var cbx in lstPresentaciones)
            {
                Conexion conexion2 = new Conexion();
                string consulta2 = "SELECT idPresentacion,nombre FROM presentacion ORDER BY nombre ASC";
                DataTable tabla2 = conexion2.retornaTabla(consulta2);
                if (tabla2 != null)
                {
                    cbx.cbx.DataSource = tabla2;
                    cbx.cbx.DisplayMember = "nombre";
                    cbx.cbx.ValueMember = "idPresentacion";

                    cbxPresentacion2.DataSource = tabla2;
                    cbxPresentacion2.DisplayMember = "nombre";
                    cbxPresentacion2.ValueMember = "idPresentacion";
                }
                else
                {
                    MessageBox.Show("No se pudo mostrar las Presentaciones en las listas");
                }

            }
            
            GC.Collect();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            hacerSiesjerarquia();
        }

        public void hacerSiesjerarquia() {
            if (checkPresedencia.Checked == true)
            {
                txtPrecedenciaM.Enabled = true;
                label11.Text = "Presedencia";
                label12.Text = "Jerarquia";
                label6.Visible = true;
                dateTimePicker1.Visible = true;
                foreach (var pre in lstPresentaciones)
                {
                    pre.calendario.Visible = false;
                }
            }
            else
            {
                txtPrecedenciaM.Enabled = false;
                label11.Text = "Presentacion";
                label12.Text = "Cantidad";
                label6.Visible = false;
                dateTimePicker1.Visible = false;
                foreach (var pre in lstPresentaciones)
                {
                    pre.calendario.Visible = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            quitarPresentacion();
        }

        public void quitarPresentacion() {
            if (lstPresentaciones.Count - 1 > 0)
            {
                PresentacionObj ultimo = lstPresentaciones[lstPresentaciones.Count - 1];
                this.panel3.Controls.Remove(ultimo.cantidad);
                this.panel3.Controls.Remove(ultimo.cbx);
                this.panel3.Controls.Remove(ultimo.precio);
                this.panel3.Controls.Remove(ultimo.calendario);
                PosY -= 54;
                ContadorP -= 1;
                lstPresentaciones.RemoveAt(lstPresentaciones.Count - 1);
            }
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            string mostrar = aniadirProducto.anidiarProducto(txtNombreProducto.Text, richDescripcion.Text, cbxCasaMedica.SelectedValue.ToString(), lstPresentaciones, checkPresedencia.Checked, txtPrecedenciaM.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            if (mostrar.Equals("Producto Insertado"))
            {
                txtNombreProducto.Text = "";
                richDescripcion.Text = "";
                quitarPresentacion();
            }
            MessageBox.Show(mostrar);
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            Conexion conexion = new Conexion();
            string sConsulta = "UPDATE casaMedica set nombre='"+textBox1.Text+"' WHERE idCasaMedica="+ cbxCasaMedica2.SelectedValue.ToString();
            conexion.cargaMasiva(sConsulta);
            textBox1.Text = "";
            mostrarCombobox();
            GC.Collect();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Conexion conexion = new Conexion();
            string Consulta = "UPDATE presentacion set nombre='" + textBox2.Text + "' WHERE idPresentacion=" + cbxPresentacion2.SelectedValue.ToString();
            conexion.cargaMasiva(Consulta);
            textBox2.Text = "";
            mostrarCombobox();
            GC.Collect();
        } 

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                MostrarProductos();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MostrarProductos();
        }

        public void MostrarProductos() {
            string consulta = "";
            Conexion conexion = new Conexion();
            if (textBox3.Text.Equals(""))
            {
                consulta = " (SELECT T.idBodega AS Codigo, P.nombre AS Producto, PR.nombre AS Presentacion, C.nombre AS Casa_Medica, T.precio AS Q_Precio, P.descripcion, T.cantidad AS Disponible, T.fechaCaducidad, T.tipo, C.nombre AS Representa, T.idProducto, T.idProducto AS Bo, T.idProducto AS Bo2 FROM bodega T "
                                + " INNER JOIN producto P on(P.idProducto = T.idProducto) "
                                + " INNER JOIN casaMedica C on(C.idCasaMedica = T.idCasaMedica)" +
                                  " INNER JOIN presentacion PR ON (PR.IdPresentacion = T.idPresentacion) WHERE T.estado=1 )  " +
                                  " UNION ALL " +
                                  " ( SELECT B.IdBodega, PR.nombre, PS.nombre,C.nombre, PP.precio,PR.descripcion, (B.cantidad/PP.representacion) AS Cantidad, B.fechaCaducidad, B.tipo, STR(PP.representacion), B.idProducto, PP.idPresedenciaProducto, B.cantidad  from PresedenciaProducto PP " +
                                  "  INNER JOIN bodega B ON(B.idBodega = PP.idBodega) "+
                                  "  INNER JOIN presentacion PS ON(PS.idPresentacion = PP.idPresedencia) "+
                                  "  INNER JOIN producto PR ON(PR.idProducto = B.idProducto) "+
                                  "  INNER JOIN casaMedica C ON(C.idCasaMedica = B.idCasaMedica) WHERE B.estado=1 )";
            }else {
                consulta = "(SELECT T.idBodega AS Codigo, P.nombre AS Producto, PR.nombre AS Presentacion,C.nombre AS Casa_Medica, T.precio AS Q_Precio, P.descripcion, T.cantidad AS Disponible, T.fechaCaducidad, T.tipo, C.nombre AS Representa, T.idProducto, T.idProducto AS Bo, T.idProducto AS Bo2 FROM bodega T "
                                + " INNER JOIN producto P on(P.idProducto = T.idProducto) "
                                + " INNER JOIN casaMedica C on(C.idCasaMedica = T.idCasaMedica) " +
                                " INNER JOIN presentacion PR ON (PR.IdPresentacion = T.idPresentacion) " +
                                " WHERE (P.nombre Like '%" + textBox3.Text + "%' OR P.descripcion Like '%" + textBox3.Text + "%' ) AND T.estado=1  )  " +
                                " UNION ALL " +
                                  " ( SELECT B.IdBodega, PR.nombre, PS.nombre,C.nombre, PP.precio,PR.descripcion, (B.cantidad/PP.representacion), B.fechaCaducidad, B.tipo, STR(PP.representacion), B.idProducto, PP.idPresedenciaProducto, B.cantidad from PresedenciaProducto PP  " +
                                  "  INNER JOIN bodega B ON(B.idBodega = PP.idBodega) " +
                                  "  INNER JOIN presentacion PS ON(PS.idPresentacion = PP.idPresedencia) " +
                                  "  INNER JOIN producto PR ON(PR.idProducto = B.idProducto) " +
                                  "  INNER JOIN casaMedica C ON(C.idCasaMedica = B.idCasaMedica) " +
                                  " WHERE (PR.nombre Like '%" + textBox3.Text + "%' OR PR.descripcion Like '%" + textBox3.Text + "%') AND  B.estado=1 ) ";
            }
            
            DataTable tabla = conexion.retornaTabla(consulta);
            if (tabla != null)
            {
                gridProductos.Columns.Clear();

                DataGridViewButtonColumn btnAddDetails = new DataGridViewButtonColumn();
                btnAddDetails.Text = "Editar";
                btnAddDetails.UseColumnTextForButtonValue = true;
                btnAddDetails.Name = "Editar";

                gridProductos.Columns.Add(btnAddDetails);
                gridProductos.DataSource = tabla;
                gridProductos.Columns["tipo"].Visible = false;
                gridProductos.Columns["Representa"].Visible = false;
                gridProductos.Columns["idProducto"].Visible = false;
                gridProductos.Columns["Bo"].Visible = false;
                gridProductos.Columns["Bo2"].Visible = false;

            }
            else
            {
                MessageBox.Show("No se pudo mostrar las casas medicas");
            }
            GC.Collect();
        }

        private void gridProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.gridProductos.Columns.Count == 0 || this.gridProductos.Rows.Count == 0) { return; }
            if (this.gridProductos.Columns[e.ColumnIndex].Name == "Editar")
            {
                    EditarProducto editarP = new EditarProducto();
                    string detalleP = gridProductos.CurrentRow.Cells[2].Value.ToString() + " / " + gridProductos.CurrentRow.Cells[3].Value.ToString() +" / "+ gridProductos.CurrentRow.Cells[4].Value.ToString();
                    editarP.Inicializador(gridProductos.CurrentRow.Cells[1].Value.ToString(),detalleP, gridProductos.CurrentRow.Cells[6].Value.ToString(), gridProductos.CurrentRow.Cells[7].Value.ToString(), gridProductos.CurrentRow.Cells[5].Value.ToString(), gridProductos.CurrentRow.Cells[8].Value.ToString(), gridProductos.CurrentRow.Cells[4].Value.ToString(), gridProductos.CurrentRow.Cells[9].Value.ToString(), gridProductos.CurrentRow.Cells[11].Value.ToString(), gridProductos.CurrentRow.Cells[12].Value.ToString(), gridProductos.CurrentRow.Cells[13].Value.ToString());
                    editarP.Show();
                    editarP.removeD(this.form1);
                    gridProductos.DataSource = null;
                
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mostrarCombobox();
            mostrarCombobox2();
        }
    }
}

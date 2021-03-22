using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FarmaciaFatima.Modelos;
namespace FarmaciaFatima.Procesos
{
    class AddProduct
    {
        Conexion conexion = new Conexion();
        public string anidiarProducto(string nombre, string descripcion, string casaMedica, List<PresentacionObj> arreglo, bool check, string cantidadM, string fecha) {
            string idProducto = "";
            if (nombre=="") { return "Nombre del producto vacio"; }
            //Verifica si el producto ya existe
            string sConsulta = "Select * FROM bodega B INNER JOIN producto P on (P.idProducto=B.idProducto)" +
                                " Where B.idCasaMedica=" + casaMedica + " AND P.nombre='" + nombre + "' AND B.estado=1";

            if (existeDato(sConsulta)) { return "Este Producto ya existe"; }
            //Si el producto no existe añade el producto
            sConsulta = "INSERT INTO producto (nombre,descripcion) values ('" + nombre + "','" + descripcion + "')";
            conexion.cargaMasiva(sConsulta);
            idProducto = retornaDato("SELECT idProducto FROM producto WHERE nombre='" + nombre + "' ", "idProducto");

            if (idProducto.Equals("")) { return "Hubo un problema con el nombre del producto"; } //Verifica si hubo un problema en encontrar el producto
            //Si el producto tendra gerarquia o no 
            if (!check)
            {
                try
                {
                    foreach (var presentacion in arreglo)
                    {
                        sConsulta = "INSERT INTO bodega VALUES " +
                            " (" + presentacion.cbx.SelectedValue + "," + idProducto + "," + casaMedica + "," + presentacion.cantidad.Text + "," + presentacion.precio.Text + ",1,'" + presentacion.calendario.Value.ToString("yyyy-MM-dd") + "',2,'foto.png')";
                        conexion.cargaMasiva(sConsulta);
                    }

                    return "Producto Insertado";
                }
                catch (Exception)
                {
                    return "Error el con los datos del producto";
                }

            }
            else {
                try
                {
                    sConsulta = "INSERT INTO bodega VALUES " +
                            " (Null," + idProducto + "," + casaMedica + "," + cantidadM + ",Null,1,'" + fecha + "',1,'foto.png')";
                    conexion.cargaMasiva(sConsulta);
                    
                    string subConsulta = "Select * FROM bodega B INNER JOIN producto P on (P.idProducto=B.idProducto)" +
                                " Where B.idCasaMedica=" + casaMedica + " AND P.nombre='" + nombre + "'";

                    string idBodega = retornaDato(subConsulta, "idBodega");
                    if (idProducto.Equals("")) { return "Hubo un problema con el nombre de la bodega"; } //Verifica si hubo un problema en encontrar la bodega
                    foreach (var presentacion in arreglo)
                    {
                        sConsulta = "INSERT INTO PresedenciaProducto VALUES " +
                            " (" + presentacion.cbx.SelectedValue + "," + idBodega + "," + presentacion.cantidad.Text + "," + presentacion.precio.Text + ") ";
                        conexion.cargaMasiva(sConsulta);
                    }
                    return "Producto Insertado";
                }
                catch (Exception)
                {
                    return "Error el con los datos del producto";
                }
            }
            
            return "No se pudo añadir producto";
        }

        public bool existeDato(string sConsulta)
        {
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

        public string retornaDato(string sConsulta, string columna)
        {
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

        public bool IsNumero(string numero) {
            bool entero = true;


            char[] test = numero.ToCharArray();

            for (int i = 0; i < test.Length; i++)
            {
                if (test[i] == '.')
                {
                    entero = false;
                }
            }

            try
            {
                if (entero)
                {
                    Convert.ToInt32(numero);
                    return true;
                }
                else
                {
                    Convert.ToDouble(numero);
                    return true;
                }
            }
            catch (FormatException)
            {
                return false;
            }

        }
    }
}

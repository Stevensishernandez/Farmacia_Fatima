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
    class AgregarCarrito
    {
        List<venta> lstVentas;
        Conexion conexion;

        public AgregarCarrito(List<venta> lstVentas)
        {
            this.lstVentas = lstVentas;
            Conexion conexion;
        }

        public void añadirVenta() {
            foreach (var detalle in lstVentas)
            {
                string sConsulta = "SELECT * FROM bodega WHERE idBodega=" + detalle.idBodega;
                if (retornaRegistro(sConsulta) != null)
                {
                    
                }
                else { MessageBox.Show("Producto no exite "+detalle.texto.Text); }
            }
        
        }

        public DataRowCollection retornaRegistro(string sConsulta)
        {
            string consulta = sConsulta;
            DataTable tabla = conexion.retornaTabla(consulta);
            if (tabla != null)
            {
                if (tabla.Rows.Count == 0)
                {
                    return null;
                }

                if (tabla.Rows.Count > 1)
                {
                    return null;
                }

                if (tabla.Rows.Count == 1)
                {
                    return tabla.Rows; //Retorno una tabla porque no se como retorna solo un registro
                }

            }
            else
            {
                MessageBox.Show("Algo malo comprobando la existencia de este producto");
                return null;
            }
            return null;
        }
    }
}

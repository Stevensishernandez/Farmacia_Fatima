using FarmaciaFatima.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FarmaciaFatima.Procesos
{
    class RealizarVenta
    {
        List<venta> lstVentas;
        string fecha;
        string hora;

        public RealizarVenta(List<venta> lstVentas, string fecha, string hora)
        {
            this.lstVentas = lstVentas;
            this.fecha = fecha;
            this.hora = hora;
        }

        public void vender() {
            Conexion conexion = new Conexion();
            string sConsulta = "INSERT INTO venta (fecha, hora) VALUES ('"+fecha+"', '"+hora+"')";
            conexion.cargaMasiva(sConsulta);
            string idVenta = retornaDato("SELECT * FROM venta WHERE fecha='" + fecha + "' AND hora='" + hora + "' ", "IdVenta");
            foreach (var detalle in lstVentas)
            {
                sConsulta = "INSERT INTO DetalleVenta VALUES (" + idVenta + ","+detalle.idBodega+ "," + detalle.VCantidad + "," + detalle.Vsubtotal + "," + detalle.Vrestado + ",'"+detalle.texto.Text+"')";
                conexion.cargaMasiva(sConsulta);
            }
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

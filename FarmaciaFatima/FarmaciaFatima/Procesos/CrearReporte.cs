using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FarmaciaFatima.Procesos
{
    class CrearReporte
    {
        public string encabezado = " <html> " +
        " <head> " +
           " <link href=\"./bootstrap.min.css\" rel=\"stylesheet\"> " +
        " </head> " +
        " <body> <div style=\"text-align:center;\"> " +
        " <h1>Reporte del dia ";

        public string encabezado2 = "</h1> " +
        " </div> ";

        public string starFila = " <div style = \"text-align:center;\" class=\"row\">" +
                " ";


        public string primeraParte = " " +
        " <br> " +
        " <br> " +
        " <div class=\"col-md-3\"> " +
        "	<h7> ";

        public string segundaParte = "</h7> " ;

        public string segundaParte2 = " </div> "+ " <div class=\"col-md-6\">" +
         "	<table class=\"table\"> " +
         " <thead> " +
         "	<tr> ";
        //Despues va los encabezador 

        public string terceraParte = " </tr> "+
         " </thead > "+
         " <tbody> ";

        //Despues de esto van todas los registros

        public string cuartaParte = " </tbody> " +
        " </table> ";
        
        public string quintaParte = " </div > " +
        " <div class=\"col-md-3\"> " +
        " </div> " +
        " </div> ";

        public string fin = " </body> "+
         " </html > ";

        public void reporteVentas(string fecha) {
            string contenido = "";
            List<string> ids = new List<string>();
            Conexion conexion = new Conexion();
            string sConsulta = "SELECT * FROM venta V WHERE  V.fecha='" + fecha + "' ";
            string cabezas="";
            DataTable tabla = conexion.retornaTabla(sConsulta);
            if (tabla != null)
            {
                contenido += encabezado + fecha;
                contenido += encabezado2 ;
                if (tabla.Rows.Count > 0)
                {
                    for (int i = 0; i < tabla.Rows.Count; i++)
                    {
                        ids.Add(tabla.Rows[i]["IdVenta"].ToString());
                    }

                    cabezas = " <th scope = \"col\" > detalle </th > " +
                                      "  <th scope = \"col\" > cantidad </th > " +
                                        " <th scope = \"col\" > Sub Total Q </th > ";


                    foreach (var detalle in ids)
                    {
                        
                        conexion = new Conexion();
                        sConsulta = "SELECT CONCAT('VENTA',V.IdVenta) AS Venta, V.fecha, V.hora, DT.detalle, DT.cantidad, DT.subTotal FROM DetalleVenta DT " +
                        "INNER  JOIN Venta V ON(V.IdVenta = DT.idVenta) WHERE  DT.idVenta=" + detalle + " ";
 
                        tabla = conexion.retornaTabla(sConsulta);
                        if (tabla != null)
                        {
                            if (tabla.Rows.Count>0) {
                                contenido += starFila + primeraParte + " Venta " + detalle + segundaParte;
                                string cuerp = "";
                                contenido += " \n <h8> Hora: " + tabla.Rows[0]["hora"].ToString() + " </h8> ";
                                contenido += segundaParte2 + cabezas + terceraParte;
                                for (int i = 0; i < tabla.Rows.Count; i++)
                                {
                                    cuerp += " <tr> ";
                                    cuerp += " <td>" + tabla.Rows[i]["detalle"].ToString() + "</td> ";
                                    cuerp += " <td>" + tabla.Rows[i]["cantidad"].ToString() + "</td> ";
                                    cuerp += " <td>" + tabla.Rows[i]["subTotal"].ToString() + "</td> ";
                                    cuerp += " </tr> ";
                                }

                                contenido += cuerp + cuartaParte;
                                conexion = new Conexion();
                                sConsulta = "SELECT DT.idVenta, SUM(DT.subTotal) Total FROM DetalleVenta DT " +
                                "INNER  JOIN Venta V ON(V.IdVenta = DT.idVenta) WHERE  DT.idVenta=" + detalle + " GROUP BY DT.idVenta ";
                                tabla = conexion.retornaTabla(sConsulta);
                                if (tabla != null)
                                {
                                    string loque = tabla.Rows[0]["Total"].ToString();
                                    contenido += " \n <br> <H6> Total Q." + loque + " </H6> \n " + quintaParte;

                                }

                            }
                            

                        }
                    }

                    conexion = new Conexion();
                    sConsulta = "SELECT V.fecha, SUM(DT.subTotal) Total FROM DetalleVenta DT " +
                    "INNER  JOIN Venta V ON(V.IdVenta = DT.idVenta) WHERE  V.fecha='" + fecha + "' GROUP BY V.fecha ";
                    tabla = conexion.retornaTabla(sConsulta);
                    if (tabla != null)
                    {
                        string loque = tabla.Rows[0]["Total"].ToString();
                        contenido += " <br> <H3> Total vendido en el dia Q." + loque + " </H3>  " ;

                    }


                    contenido += fin;

                    try
                    {
                        GenerarTXT(contenido);
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("No se puedo abrir el reporte");
                    }

                }
                else {
                    MessageBox.Show("No hay datos en ventas");
                }
            }
        }

        // para crear el archivo
        public void GenerarTXT(string contenido )
        {
            string rutaCompleta = @" C:\reportesFarmaciaFatima\reporte.html";
            string texto = "HOLA MUNDO ";

            using (StreamWriter mylogs = new StreamWriter(rutaCompleta, false))         //se crea el archivo
            {
                
                //se adiciona alguna información y la fecha


                DateTime dateTime = new DateTime();
                dateTime = DateTime.Now;
                string strDate = Convert.ToDateTime(dateTime).ToString("yyMMdd");
                
                mylogs.Write(contenido);

                mylogs.Close();

                Process p = new Process();
                p.StartInfo.FileName = @"C:\reportesFarmaciaFatima\reporte.html";
                p.Start();
            }
        }

        // para escribir en el archivo
        
    }
}

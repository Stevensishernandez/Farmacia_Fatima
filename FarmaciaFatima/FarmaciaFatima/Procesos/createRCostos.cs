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
    class createRCostos
    {
		public string encabezado = " <html> \n" +
					" <head>\n " +
					" <link href=\"./bootstrap.min.css\" rel=\"stylesheet\"> \n" +
					" </head> \n" +
					" <body> \n" +
					" <div style = \"text-align:center;\" > \n" +
					" <h1> Reporte de Invenario Del dia ";

		public string primeraParte = "</h1> \n" +
								" </div> \n" +
								" <div style=\"text-align:center;\" class=\"row\">\n" +
								"	<br> \n" +
								"	<br> \n" +
									" <div class=\"col-md-1\">\n" +
									 " </div>\n" +
									 "<div class=\"col-md-10\">\n" +
										"<div class=\"card w-75\"> \n" +
											"<div class=\"card-body\"> \n" +
											"<h5 class=\"card-title\">Total Q.";

		public string primeraParte2 = "</h5> \n" +
				"<p class=\"card-text\"> ";

		public string primeraParte3 = "</p> \n" +
			   " </div> \n" +
			" </div> \n";

		public string segundaParte = " \n" +
		" \n" +
		 "</div> \n" +
		 "<div class=\"col-md-1\">\n" +
		 "</div> \n" +
		"</div> \n" +
		"</body>\n" +
		"</html>\n";

		public void createReporte(string fecha) {
			string contenido = "";
			Conexion conexion = new Conexion();
			string consulta = " SELECT  SUM(precio*cantidad) AS Total FROM bodega WHERE cantidad>0 and estado=1 and precio>0";
			string primerCosto = retornaDato(consulta,"Total");
			consulta = "select SUM(PS.Precio*b.cantidad) AS Total from PresedenciaProducto PS "+
						" INNER JOIN bodega B ON(B.IdBodega = PS.idBodega) "+
						" WHERE PS.representacion = 1";
			string segundoPrecio = retornaDato(consulta, "Total");

			consulta = "SELECT COUNT(*) AS Total FROM bodega WHERE cantidad>0 and estado=1";
			string cantidadDisponibles = retornaDato(consulta, "Total");

			consulta = "SELECT COUNT(*) AS Total FROM bodega WHERE estado=1";
			string cantidadTotal = retornaDato(consulta, "Total");

			float TotalFinal = 0;

            try
            {
				TotalFinal = float.Parse(primerCosto)+float.Parse(segundoPrecio);
				contenido = encabezado + fecha + primeraParte + TotalFinal + primeraParte2 + " Cantidad De Productos Disponibles: "
							+cantidadDisponibles+" Cantidad de productos Totales: "+cantidadTotal+primeraParte3+segundaParte;
				GenerarTXT(contenido);
			}
            catch (Exception)
            {

                MessageBox.Show("No se pudo calcular");
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

		public void GenerarTXT(string contenido)
		{
			string rutaCompleta = @" C:\reportesFarmaciaFatima\reporteCostos.html";
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
				p.StartInfo.FileName = @"C:\reportesFarmaciaFatima\reporteCostos.html";
				p.Start();
			}
		}

	}
}

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
    class createRInventario
    {
        public string encabezado = " <html> \n"+
                    " <head>\n "+
                    " <link href=\"./bootstrap.min.css\" rel=\"stylesheet\"> \n"+
                    " </head> \n"+
                    " <body> \n"+
                    " <div style = \"text-align:center;\" > \n"+
                    " <h1> Reporte de Invenario Del dia ";

        public string primeraParte = "</h1> \n"+
								" </div> \n" +
								" <div style=\"text-align:center;\" class=\"row\">\n" +
								"	<br> \n" +
								"	<br> \n" +
									" <div class=\"col-md-1\">\n" +
									 " </div>\n" +
									 "<div class=\"col-md-10\">\n" +
										" <table class=\"table\">\n" +
									  "<thead> \n" +
										" <tr> \n" +
										 " <th scope = \"col\" > Codigo </th> \n"+
										 " <th scope=\"col\">Producto</th> \n"+
										 " <th scope = \"col\" > Presentacion </th>\n"+
										 " <th scope=\"col\">Casa Medica</th>\n"+
										 " <th scope = \"col\" > Precio </th>\n"+
										  " <th scope= \"col\"> Descripcion </th> \n" +
										  " <th scope= \"col\" > Disponible </th> \n" +
										  " <th scope= \"col\" > fechaCaducidad </th> \n"+
										 " </tr>\n"+
									   "</thead>\n"+
									   "<tbody> \n";

		public string segundaParte = "</tbody> \n" +
		"</table> \n" +
		 "</div> \n" +
		 "<div class=\"col-md-1\">\n" +
		 "</div> \n" +
		"</div> \n" +
		"</body>\n" +
		"</html>\n";

		public void hacerReporte(string fecha)
		{
			string contenido = "";
			Conexion conexion = new Conexion();
			string consulta = " (SELECT T.idBodega AS Codigo, P.nombre AS Producto, PR.nombre AS Presentacion, C.nombre AS Casa_Medica, T.precio AS Q_Precio, P.descripcion, T.cantidad AS Disponible, T.fechaCaducidad, T.tipo, C.nombre AS Representa FROM bodega T "
								+ " INNER JOIN producto P on(P.idProducto = T.idProducto) "
								+ " INNER JOIN casaMedica C on(C.idCasaMedica = T.idCasaMedica)" +
								  " INNER JOIN presentacion PR ON (PR.IdPresentacion = T.idPresentacion) WHERE T.estado=1 )  " +
								  " UNION ALL " +
								  " ( SELECT B.IdBodega, PR.nombre, PS.nombre,C.nombre, PP.precio,PR.descripcion, (B.cantidad/PP.representacion) AS Cantidad, B.fechaCaducidad, B.tipo, STR(PP.representacion)  from PresedenciaProducto PP " +
								  "  INNER JOIN bodega B ON(B.idBodega = PP.idBodega) " +
								  "  INNER JOIN presentacion PS ON(PS.idPresentacion = PP.idPresedencia) " +
								  "  INNER JOIN producto PR ON(PR.idProducto = B.idProducto) " +
								  "  INNER JOIN casaMedica C ON(C.idCasaMedica = B.idCasaMedica) WHERE B.estado=1  )";
			DataTable tabla = conexion.retornaTabla(consulta);
			if (tabla != null)
			{
				string cuerp = "";
				contenido += encabezado + fecha + primeraParte;
				//contenido += segundaParte2 + cabezas + terceraParte;
				for (int i = 0; i < tabla.Rows.Count; i++)
				{
					cuerp += " <tr> \n";
					cuerp += " <td>" + tabla.Rows[i]["Codigo"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Producto"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Presentacion"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Casa_Medica"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Q_Precio"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["descripcion"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Disponible"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["fechaCaducidad"].ToString() + "</td> \n";
					cuerp += " </tr> \n";
				}

				contenido += cuerp + segundaParte;

                try
                {
					GenerarTXT(contenido);
                }
                catch (Exception)
                {

                    MessageBox.Show("No se pudo mostrar el reporte de inventario");
                }
				
			}

		}

		public void hacerReporteOcM(string fecha)
		{
			string contenido = "";
			Conexion conexion = new Conexion();
			string consulta = " SELECT * FROM "
+" ( " 
 +" (SELECT T.idBodega AS Codigo, P.nombre AS Producto, PR.nombre AS Presentacion, C.nombre AS Casa_Medica, T.precio AS Q_Precio, P.descripcion, T.cantidad AS Disponible, T.fechaCaducidad, T.tipo, C.nombre AS Representa FROM bodega T "
								+" INNER JOIN producto P on(P.idProducto = T.idProducto) "
								+" INNER JOIN casaMedica C on(C.idCasaMedica = T.idCasaMedica) "
								 +"  INNER JOIN presentacion PR ON(PR.IdPresentacion = T.idPresentacion) WHERE T.estado = 1) "
								  +" UNION ALL "
								  +" (SELECT B.IdBodega, PR.nombre, PS.nombre, C.nombre AS Casa_Medica, PP.precio, PR.descripcion, (B.cantidad/ PP.representacion) AS Cantidad, B.fechaCaducidad, B.tipo, STR(PP.representacion)  from PresedenciaProducto PP "
									 +" INNER JOIN bodega B ON(B.idBodega = PP.idBodega) "
									+" INNER JOIN presentacion PS ON(PS.idPresentacion = PP.idPresedencia) "
									+" INNER JOIN producto PR ON(PR.idProducto = B.idProducto) "
									+" INNER JOIN casaMedica C ON(C.idCasaMedica = B.idCasaMedica) WHERE B.estado = 1  ) "
				+" ) dum " 
+" ORDER BY Casa_Medica ASC ";
			DataTable tabla = conexion.retornaTabla(consulta);
			if (tabla != null)
			{
				string cuerp = "";
				contenido += encabezado + fecha + primeraParte;
				//contenido += segundaParte2 + cabezas + terceraParte;
				for (int i = 0; i < tabla.Rows.Count; i++)
				{
					cuerp += " <tr> \n";
					cuerp += " <td>" + tabla.Rows[i]["Codigo"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Producto"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Presentacion"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Casa_Medica"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Q_Precio"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["descripcion"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Disponible"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["fechaCaducidad"].ToString() + "</td> \n";
					cuerp += " </tr> \n";
				}

				contenido += cuerp + segundaParte;

				try
				{
					GenerarTXT(contenido);
				}
				catch (Exception)
				{

					MessageBox.Show("No se pudo mostrar el reporte de inventario");
				}

			}

		}

		public void hacerReporte2(string fecha)
		{
			string contenido = "";
			Conexion conexion = new Conexion();
			string consulta = " SELECT * FROM "
+ " ( "
 + " (SELECT T.idBodega AS Codigo, P.nombre AS Producto, PR.nombre AS Presentacion, C.nombre AS Casa_Medica, T.precio AS Q_Precio, P.descripcion, T.cantidad AS Disponible, T.fechaCaducidad, T.tipo, C.nombre AS Representa FROM bodega T " +
								" INNER JOIN producto P on(P.idProducto = T.idProducto) "+
								" INNER JOIN casaMedica C on(C.idCasaMedica = T.idCasaMedica) "+
								"  INNER JOIN presentacion PR ON(PR.IdPresentacion = T.idPresentacion) WHERE T.estado = 1 AND T.cantidad < 1 ) "+
								" UNION ALL "+
								" (SELECT T.idBodega AS Codigo, P.nombre AS Producto,'', C.nombre AS Casa_Medica, T.precio AS Q_Precio, P.descripcion, T.cantidad AS Disponible, T.fechaCaducidad, T.tipo, C.nombre AS Representa FROM bodega T "+
								" INNER JOIN producto P on(P.idProducto = T.idProducto) "+
								" INNER JOIN casaMedica C on(C.idCasaMedica = T.idCasaMedica) "+
								"  WHERE T.estado = 1 AND T.cantidad < 1 AND tipo = 1 ) "
								+ " ) dum "
+ " ORDER BY Casa_Medica ASC ";
			DataTable tabla = conexion.retornaTabla(consulta);
			if (tabla != null)
			{
				string cuerp = "";
				contenido += encabezado + fecha + primeraParte;
				//contenido += segundaParte2 + cabezas + terceraParte;
				for (int i = 0; i < tabla.Rows.Count; i++)
				{
					cuerp += " <tr> \n";
					cuerp += " <td>" + tabla.Rows[i]["Codigo"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Producto"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Presentacion"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Casa_Medica"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Q_Precio"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["descripcion"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["Disponible"].ToString() + "</td> \n";
					cuerp += " <td>" + tabla.Rows[i]["fechaCaducidad"].ToString() + "</td> \n";
					cuerp += " </tr> \n";
				}

				contenido += cuerp + segundaParte;

				try
				{
					GenerarTXT(contenido);
				}
				catch (Exception)
				{

					MessageBox.Show("No se pudo mostrar el reporte de inventario");
				}

			}

		}

		public void GenerarTXT(string contenido)
		{
			string rutaCompleta = @" C:\reportesFarmaciaFatima\reporteInventario.html";
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
				p.StartInfo.FileName = @"C:\reportesFarmaciaFatima\reporteInventario.html";
				p.Start();
			}
		}

	}
}

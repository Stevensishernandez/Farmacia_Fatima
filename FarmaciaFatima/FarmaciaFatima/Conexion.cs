using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace FarmaciaFatima
{
    class Conexion
    {
        string connexion = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;Connect Timeout=30";
        DataTable dt = new DataTable();
        public DataTable prueba() {
            try
            {
                dt.Clear();
                SqlConnection conn = new SqlConnection(connexion);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select * FROM Producto",conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                Console.WriteLine("Si conecto");
                conn.Close();
                return dt;
            }
            catch (Exception)
            {
                Console.WriteLine("Hubo un error en la conexion");
                MessageBox.Show("Este proceso no se pudo realizar, verifique los datos");
            }
            return null;
        }

        //Para la carga masiva
        public Boolean cargaMasiva(string consulta) {
            try
            {
                dt.Clear();
                SqlConnection conn = new SqlConnection(connexion);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = consulta;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Hubo un error en la cargamasiva");
                MessageBox.Show("Este proceso no se pudo realizar, verifique los datos");
                return false;
            }
            return false;
        }
        //Ejecuta una sentencia sql
        public Boolean ejecutarSentencia(string consulta)
        {
            try
            {
                dt.Clear();
                SqlConnection conn = new SqlConnection(connexion);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = consulta;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Hubo un error en la cargamasiva");
                return false;
            }
        }

        public DataTable retornaTabla(string consulta)
        {
            try
            {
                dt.Clear();
                SqlConnection conn = new SqlConnection(connexion);
                conn.Open();
                SqlCommand cmd = new SqlCommand(consulta, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                Console.WriteLine("Si retorna");
                conn.Close();
                return dt;
            }
            catch (Exception)
            {
                Console.WriteLine("No retorna");
            }
            return null;
        }

        public string unDato(string consulta) {
            string retorno = "";
            try
            {
                SqlConnection conn = new SqlConnection(connexion);
                conn.Open();
                SqlCommand cmd = new SqlCommand(consulta, conn);
                SqlDataReader respuesta = cmd.ExecuteReader();

                if (respuesta.Read()) {
                    
                }
            }
            catch (Exception)
            {
                return "";
            }

            return retorno;
        }
    }
}

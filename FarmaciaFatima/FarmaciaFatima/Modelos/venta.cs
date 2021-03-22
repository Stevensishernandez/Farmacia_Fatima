using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FarmaciaFatima.Modelos
{
    class venta
    {
        public TextBox texto;
        public string idBodega;
        public string presentacion;
        public TextBox cantidad;

        public TextBox newPrecio;
        public TextBox descripcionP;

        public Panel panel;
        public Panel temp;
        public Button quitar;
        public CheckBox chCambio;
        public Label titulo;
        public int posY;
        public string precio;
        public string stock;
        List<venta> lstVentas;
        public string tipo;
        public string representacion;
        public string CasaMedica;

        public string descripcionCambio;
        public string descripcionSinCambio;

        public string precioConCambio;
        public string precioSinCambio;

        public int VCantidad;
        public float Vsubtotal;
        public int Vrestado;

        public venta(string idBodega, Panel panel, int posY, string textoC, string presentacion, string CasaMe, string stock, string precio, List<venta> lstVentas, string tipo, string representa, Label label)
        {
            this.idBodega = idBodega;
            this.panel = panel;
            this.posY = posY;
            this.texto = new TextBox();
            this.cantidad = new TextBox();
            this.descripcionSinCambio= textoC + " " + presentacion + " / " + CasaMe + " / Q";
            this.texto.Text = descripcionSinCambio+precio;
            this.quitar = new Button();
            this.temp = new Panel();
            this.chCambio = new CheckBox();
            this.presentacion = presentacion;
            this.panel =panel;
            this.stock = stock;
            this.precio = precio;
            this.lstVentas = lstVentas;
            this.tipo = tipo;
            this.representacion = representa;
            this.CasaMedica = CasaMe;
            this.newPrecio = new TextBox();
            this.descripcionP = new TextBox();
            this.titulo = label;
            this.precioConCambio = "";
            this.precioSinCambio = precio;
            this.descripcionCambio = " /Precio Cambio/";

            Inicializador();
        }

        public void Inicializador() {

            this.texto.Location = new System.Drawing.Point(10, 3);
            this.texto.Font = new Font("Lucida Sans Unicode", 8, FontStyle.Regular);
            this.texto.Size = new System.Drawing.Size(700, 10);
            this.texto.Enabled = false;
            this.temp.Controls.Add(this.texto);


            this.cantidad.Location = new System.Drawing.Point(755, 3);
            this.cantidad.Font = new Font("Lucida Sans Unicode", 8, FontStyle.Regular);
            this.cantidad.Size = new System.Drawing.Size(30, 10);
            this.temp.Controls.Add(this.cantidad);
            this.cantidad.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };


            this.chCambio.Location = new System.Drawing.Point(825, 3);
            this.chCambio.Text = ".";
            this.chCambio.Font = new Font("Lucida Sans Unicode", 8, FontStyle.Regular);
            
            this.temp.Controls.Add(this.chCambio);
           
            this.chCambio.CheckedChanged += (sender, args) =>
            {
                titulo.Visible = true;

                if (chCambio.Checked == true)
                {
                    this.newPrecio.Visible = true;
                    this.descripcionP.Visible = true;
                    this.texto.Text = descripcionSinCambio+ precio + descripcionCambio;
                }
                else {
                    this.newPrecio.Visible = false;
                    this.descripcionP.Visible = false;
                    titulo.Visible = false;
                    this.texto.Text = descripcionSinCambio+ precio;
                }

            };

            this.quitar.Location = new System.Drawing.Point(790, 3);
            this.quitar.Font = new Font("Lucida Sans Unicode", 8, FontStyle.Regular);
            this.quitar.Text = "X";
            this.quitar.Size = new System.Drawing.Size(25, 25);
            this.quitar.Click += (sender, args) =>
            {
                this.panel.Controls.Remove(this.temp);
                this.lstVentas.Remove(this);
               
            };
            this.temp.Controls.Add(this.quitar);

            

            this.newPrecio.Location = new System.Drawing.Point(930, 3);
            this.newPrecio.Font = new Font("Lucida Sans Unicode", 8, FontStyle.Regular);
            this.newPrecio.Size = new System.Drawing.Size(30, 10);
            this.newPrecio.Visible = false;
            this.temp.Controls.Add(this.newPrecio);
            this.newPrecio.KeyPress += (sender, e) =>
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
            };

            this.descripcionP.Location = new System.Drawing.Point(965, 3);
            this.descripcionP.Font = new Font("Lucida Sans Unicode", 8, FontStyle.Regular);
            this.descripcionP.Size = new System.Drawing.Size(80, 10);
            this.descripcionP.Visible = false;
            this.temp.Controls.Add(this.descripcionP);

            this.temp.Size = new System.Drawing.Size(1300, 27);
            this.temp.Dock = DockStyle.Top;
            this.panel.Controls.Add(this.temp);

        }

        public bool sePuedeVender() {

            try
            {
                if (this.tipo.Equals("2")) {
                    if ((int.Parse(this.stock) - int.Parse(this.cantidad.Text)) >= 0)
                    {
                        this.VCantidad = int.Parse(this.cantidad.Text);
                        this.Vrestado = int.Parse(this.cantidad.Text);
                        return true;
                    }
                }
                else if(this.tipo.Equals("1")){
                    if ((int.Parse(this.stock) - int.Parse(this.cantidad.Text)) >= 0)
                    {
                        this.VCantidad = int.Parse(this.cantidad.Text);
                        this.Vrestado = int.Parse(this.cantidad.Text)*int.Parse(this.representacion);
                        return true;
                    }
                }
                
            }
            catch (Exception)
            {

                return false;
            }
            
            
            return false;
        }

        public void actualizarPrecio() {
            if (chCambio.Checked == true)
            {
                this.precio = newPrecio.Text;   
                this.texto.Text = descripcionSinCambio +precio+"/ Cambio Precio / "+ descripcionP.Text;
            }
            else
            {
                this.texto.Text = descripcionSinCambio+ precio;
            }
        }


    }
}

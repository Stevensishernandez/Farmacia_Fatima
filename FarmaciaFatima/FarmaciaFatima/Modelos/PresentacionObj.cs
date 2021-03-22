using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FarmaciaFatima.Modelos
{
    class PresentacionObj
    {
        public TextBox cantidad;
        public TextBox precio;
        public ComboBox cbx;
        public DateTimePicker calendario;
        public int ContadorPresentacion;
        public int PosY;
        public Panel panel;

        public PresentacionObj(int contadorPresentacion,Panel panel,int PosY)
        {
            ContadorPresentacion = contadorPresentacion;
            this.cantidad = new TextBox();
            this.precio = new TextBox();
            this.cbx = new ComboBox();
            this.calendario = new DateTimePicker();
            this.panel = panel;
            this.PosY = PosY;
            Inicializador();
        }

        public void Inicializador() {
    
            //Añadir combobox
            this.cbx.Location = new System.Drawing.Point(10, this.PosY);
            this.cbx.Size = new System.Drawing.Size(154, 24);
            this.panel.Controls.Add(this.cbx);

            //Añadir textBox
            this.cantidad.Location = new System.Drawing.Point(167, this.PosY );
            this.cantidad.Font = new Font("Lucida Sans Unicode", 10, FontStyle.Regular);
            this.cantidad.Size = new System.Drawing.Size(53, 35);
            this.cantidad.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
            this.panel.Controls.Add(this.cantidad);

            //Añadir precio
            this.precio.Location = new System.Drawing.Point(235, this.PosY);
            this.precio.Font = new Font("Lucida Sans Unicode", 10, FontStyle.Regular);
            this.precio.Size = new System.Drawing.Size(53, 35);
            this.precio.KeyPress += (sender, e) =>
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

            this.calendario.Location = new System.Drawing.Point(290, this.PosY);
            this.calendario.Size = new System.Drawing.Size(150, 24);
            this.panel.Controls.Add(this.calendario);
            this.panel.Controls.Add(this.precio);
        }
    }
}

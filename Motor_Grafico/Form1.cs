using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Motor_Grafico
{
    public partial class Form1 : Form
    {
        private Bitmap resultante;
        private int anchoVentana, altoVentana;

        private NCEngine motor;

        //variables para el double buffer y evitar el flicker
        private Bitmap dBufferBMP;
        private Graphics dBufferDC; // es una superficie de dibujo GDI+


        private NCSprite uno = new NCSprite(100, 100, 80, 60, "Sprite0.png", 5, 4, true, true,Color.FromArgb(0,38,255),1,1);


        public Form1()
        {
            InitializeComponent();

            //Creamos un bitmap y obtenemos su superficie de dibujo
            dBufferBMP = new Bitmap(this.Width, this.Height);
            dBufferDC = Graphics.FromImage(dBufferBMP);

            //creamos el bitmap resultante del cuadro

            resultante = new Bitmap(800, 600);

            //colocamos los valores para el dibujo con scrolls
            anchoVentana = 800;
            altoVentana = 600;

            //informacion de la version del sprite
            this.Text += " " + uno.Version.ToString();

            //creamos la instancia del motor
            motor = new NCEngine(anchoVentana, altoVentana, TiposColisiones.BoundingRectangle,"Fondo.png");

            motor.adicionaSprite(uno);
            motor.InicializaEngine();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simulaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (simulaToolStripMenuItem.Checked == true)
                timer1.Enabled = true;
            else
                timer1.Enabled = false;
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show( "Jose Veliz Estudiante de informatica UDONE", "Por Ducen29");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            uno.DeltaY = 2;
            
            motor.cicloJuego();
            resultante = motor.Canvas;
            uno.IsTransparente = true;
            uno.TypeAnim = TipoAnmacion.PingPong;
            uno.VelAnim = 1;

            //Codigo de la copia al buffer
            Graphics clientDC = this.CreateGraphics();
            if(resultante != null)
            {
                //calculamos el scroll
                AutoScrollMinSize = new Size(anchoVentana, altoVentana);
                clientDC.DrawImage(resultante,
                                    new Rectangle(this.AutoScrollPosition.X,
                                                  this.AutoScrollPosition.Y+27,
                                                  anchoVentana, altoVentana));
                //mandamos a dibujar al buffer
                clientDC.DrawImage(dBufferBMP, 0, 0);
            }
        }

        private void procesaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uno.IsTransparente = true;
            uno.AnimacionActual = 2;

            motor.cicloJuego();
            resultante = motor.Canvas;

            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (resultante != null)
            {
                Graphics g = e.Graphics;

                AutoScrollMinSize = new Size(anchoVentana, altoVentana);
                g.DrawImage(resultante,
                    new Rectangle(this.AutoScrollPosition.X,
                                  this.AutoScrollPosition.Y + 27,
                                  anchoVentana, altoVentana));
                g.Dispose();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor_Grafico
{

    class NCTexto
    {
        //posicion del texto
        private int posX;
        private int posY;

        // dimensiones
        private int tamano;

        //texto a presentar
        private string texto;
        private int caracteres;
        private int altoTexto;
        private int anchoTexto;

        // indica si se dibuja;
        private bool visible;

        //imagenes para dibujo
        private Bitmap canvas;
        private Bitmap recorte;
        private Bitmap imagen;
        private Bitmap imagenTexto;
        private bool reiniciaRecorte;

        private Color colorT;//Color de la transparencia
        private Color colorImagen;//color del pixel en la imagen fuente

        private bool usarCopia;//para saber si es necesario usar la copia
        private int rXC;//recorrido en x e la copia
        private int rYC;//recorrido en y de la copia
        private int xC;//coordenada x donde se inicia la copia 
        private int yC;//coordenada y donde se inicia la copia

        //constructor
        public NCTexto(int pX, int pY, int pTamano, string pTexto, string pImagen, bool pVisible, Color pTransparencia)
        {
            posX = pX;
            posY = pY;
            tamano = pTamano;
            texto = pTexto;
            caracteres = pTexto.Length;
            altoTexto = tamano;
            anchoTexto = tamano * caracteres;
            visible = pVisible;
            colorT = pTransparencia;

            imagen = new Bitmap(pImagen);

            //caracteres * tamano, tamano
            recorte = new Bitmap(caracteres * tamano, tamano);
            reiniciaRecorte = true;

            //mandamos a creal el texto
            CreaTexto();
            usarCopia = false;
        }
        public int X { get { return posX; } set { posX = value; } }
        public int Y { get { return posY; } set { posY = value; } }

        public string Texto
        {
            get { return texto; }
            set
            {
                texto = value;
                usarCopia = true;
                PintarFondo();
                caracteres = texto.Length;
                altoTexto = tamano;
                anchoTexto = tamano * caracteres;
                imagenTexto = new Bitmap(anchoTexto, tamano);
                recorte = new Bitmap(anchoTexto, tamano);
                CopiarFondo();
                CreaTexto();
                usarCopia = true;
                PintarFondo();
                DibujaTexto();

            }
        }
        public bool Visible { get { return visible; } set { visible = value; } }
        public void ColocaCanvas(Bitmap pCanvas)
        {
            canvas = pCanvas;
        }

        public void CreaTexto()
        {
            int x = 0;
            int y = 0;
            int xR = 0;
            int yR = 0;
            int c = 0;
            int columna = 0;
            int fila = 0;

            int xInicio = 0;
            int xFinal = 0;
            int yInicio = 0;
            int yFinal = 0;

            int n = 0;

            if (texto.Length == 0)
                return;

            for (n = 0; n < caracteres; n++)
            {
                c = texto[n] - 32;
                if (c < 0 || c > 94)
                    c = 0;
                fila = c / 10;
                columna = c - (fila * 10);

                xInicio = columna * tamano;
                xFinal = xInicio + tamano;
                yInicio = fila * tamano;
                yFinal = yInicio + tamano;

                for(xR = xInicio; xR < xFinal; xR++, x++)
                {
                    for (yR = yInicio; yR < yFinal; yR++, y++)
                    {
                        colorImagen = imagen.GetPixel(xR, yR);
                        imagenTexto.SetPixel(x, y, colorImagen);
                    }
                    y = 0;
                }
            }
        }

        public void DibujaTexto()
        {
            int x = 0;
            int y = 0;
            int xR = 0;
            int yR = 0;
            int xInicio = posX;
            int xFinal = posX+anchoTexto;
            int yInicio = posY;
            int yFinal = posY+altoTexto;

            if (reiniciaRecorte == true)
                return;
            if ((posY + altoTexto < 0) || (posY >= canvas.Height) || (posX + anchoTexto < 0) || (posX >= canvas.Width)||caracteres==0)
                return;

            if (posX < 0)
            {
                x += -posX;
                xInicio = 0;
            }else if(posX+anchoTexto >=canvas.Width){
                xFinal = canvas.Width;
            }
            int yreset = 0;
            if (posY < 0)
            {
                y += posY;
                yInicio = 0;
            }else if (posY+altoTexto>=canvas.Height)
            {
                yFinal = canvas.Height;
            }

            for (xR = xInicio; xR < xFinal; xR++, x++)
            {
                for (yR = yInicio; yR < yFinal; yR++, y++)
                {
                    colorImagen = imagenTexto.GetPixel(x, y);
                    if (colorImagen != colorT)
                        canvas.SetPixel(xR, yR, colorImagen);
                }
                y = yreset;
            }

        }
        public void CopiarFondo()
        {
            colorImagen = new Color();

            int xr = 0;
            int yr = 0;

            int x = posX;
            int y = posY;
            int limX = posX + anchoTexto;
            int limY = posY + altoTexto;
            if ((posY + altoTexto < 0) || (posY >= canvas.Height) || (posX + anchoTexto < 0) || (posX >= canvas.Width) || caracteres == 0)
            {
                usarCopia = false;
                return;
            }
            else
                usarCopia = true;

            if (posX < 0)
            {
                x = 0;
                limX = anchoTexto + posX;
            }else if(posX + anchoTexto >= canvas.Width)
            {
                limX = canvas.Width;
            }

            if (posY < 0)
            {
                y = 0;
                limY = altoTexto + posY;
            }else if(posY + altoTexto >= canvas.Height)
            {
                limY = canvas.Height;
            }

            xC = x;
            yC = y;
            int reinicioy = y;
            for(x=x,xr = 0; x < limX; x++, xr++)
            {
                for (y = reinicioy, yr = 0; y < limY; y++, yr++)
                {
                    colorImagen = canvas.GetPixel(x, y);
                    recorte.SetPixel(xr, yr, colorImagen);
                }
            }
        }

        public void PintarFondo()
        {
            if (usarCopia == false)
                return;

            Color colorImagen = new Color();
            int xr = 0;
            int yr = 0;
            int x = xC;
            int y = yC;

            for (xr = 0; xr < rXC; xr++)
            {
                for (yr = 0; yr < rYC; yr++)
                {
                    colorImagen = recorte.GetPixel(xr, yr);
                    canvas.SetPixel(x + xr, y + yr, colorImagen);
                }
            }

            if(reiniciaRecorte == true)
            {
                recorte = new Bitmap(anchoTexto, tamano);
                reiniciaRecorte = false;
            }
        }
    }
}

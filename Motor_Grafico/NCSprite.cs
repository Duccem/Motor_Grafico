using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Motor_Grafico
{
    public enum TipoAnmacion { sinAnimacion, UnaVez,Repite,PingPong};
    public enum DirAnimacion { normal = 1, reversa = -1};
    public enum TiposColisiones { BoundingRectangle, BoundingCircle,BoundingEmpty};
    //public delegate void DColision(int pID, int pIDCon);
    class NCSprite :GameObject, IComparable
    {
        //Atributos

        //Imagen con los cuadros
        private Bitmap imagen; //Imagen que contiene los cuadros de animacion

        //Informacion de animacion
        private int cuadros; //cantidad de cuadros de las animaciones
        private int cuadroActual; //Animacion actual que se esta mostrando
        private int animaciones; //cantidad de animaciones que tiene el sprite
        private int animacionActual; //Animacion actual que se muestra

        private bool activo; // Indica si el sprite hace el ciclo de animacion
        private bool visible; // indica si el sprite se dibuja

        //imagenes para dibujo
        private Bitmap canvas; // donde se va a dibujar
        private Bitmap recorte; // bitmap del recorte de la imagen

        //Flip
        private bool FH;
        private bool FV;

        //Animacion
        private TipoAnmacion typeAnim;//tipo de animacion
        private DirAnimacion dirAnim;//direcciono de la animacion
        private int velAnim;//velocidad de la animacion
        private int contAnim;



        //Efecto de recorte
        private bool usarCopia;
        private int recorridoXc;
        private int recorridoYC;
        private int xC;
        private int yC;

        //Transparencia
        private bool isTransparente;
        private Color colorTransparencia;



        //Sistema de capas
        private int capa;
        private static bool ordena;

        //constructor
        public NCSprite(int pX, int pY,int pAncho,int pAlto,string pImagen,
                        int pCuadros,int pAnimaciones,bool pActivo, bool pVisible,
                        Color pColor,int pID,int pCapa,TiposColisiones pColisiones):
                        base(pX, pY, pAncho, pAlto, pColisiones)
        {
            cuadros = pCuadros;
            animaciones = pAnimaciones;
            activo = pActivo;
            visible = pVisible;
            capa = pCapa;
            ordena = false;
            animacionActual = 0;
            cuadroActual = 0;
            FH = false;
            FV = false;
            imagen = new Bitmap(pImagen);
            typeAnim = TipoAnmacion.sinAnimacion;
            dirAnim = DirAnimacion.normal;
            velAnim = 2;
            contAnim = 0;
            usarCopia = false;
            recorridoXc = 0;
            recorridoYC = 0;
            recorte = new Bitmap(ancho, alto);
            isTransparente = false;
            colorTransparencia = pColor;
            

        }

        //creamos propiedades 
        public int CuadroActual {
            get { return cuadroActual; }

            set
            { cuadroActual = value;
                if (cuadroActual >= cuadros)
                    cuadroActual = cuadros;
                if (cuadroActual < 0)
                    cuadroActual = 0;
            }
        }
        public int Animaciones { get { return animaciones; } }
        public int AnimacionActual { get { return animacionActual; } set { animacionActual= value;} }

        public bool Activo { get { return activo; } set { activo = value; } }
        public bool Visible { get { return visible; }set { visible = value; } }

        public bool FlipH { get { return FH; } set { FH = value; } }
        public bool FlipV { get { return FV; } set { FV = value; } }
        public bool IsTransparente { get { return isTransparente; } set { isTransparente = value; } }
        public TipoAnmacion TypeAnim { get { return typeAnim; } set { typeAnim = value; } }
        public DirAnimacion DirAnim { get { return dirAnim; } set { dirAnim = value; } }
        public int VelAnim { get { return velAnim; } set { velAnim = value; } }
        public bool Ordena { get { return ordena; } set { ordena = value; } }

        public int Capa
        {
            get { return capa; }
            set
            {
                capa = value;
                ordena = true;
            }
        }
        

        //el pintado del sprite
        public void ColocaCanvas(Bitmap pCanvas)
        {
            canvas = pCanvas;
        }
        public void ColocaImagen(Bitmap pImagen)
        {
            imagen = pImagen;
        }

        public void DibujaSprite()
        {
            if ((X + alto < 0) || (Y >= canvas.Height) || (X + ancho < 0) || (Y >= canvas.Width))
                return;
            //aqui guardamos el color obtenido de la imagen
            Color colorImagen = new Color();
            
            // calculamos la posicion desde donde copiamos hasta donde
            int x = X;
            int y = Y;

            //Variables necesarias
            int inicioX =0;
            int inicioY =0;
            int finalX = 0;
            int finalY = 0;
            int avanceX = 0;
            int avanceY = 0;
            int xR = 0;
            int yR = 0;
            int reinicioY = Y;

            // calculamos el flip si es necesario
            //flip horizontal
            if (FH == false)
            {
                inicioX = cuadroActual * ancho;
                finalX = inicioX + ancho;
                avanceX = 1;
                //Verificamos clipping a la izquierda
                if (X < 0)
                {
                    x = 0;
                    inicioX += -X;
                }
                //Verificamos clipping a la derecha
                else if (X + ancho >= canvas.Width)
                {
                    finalX -= ((x + ancho) - canvas.Width);
                }
            }
            else
            {
                finalX = cuadroActual * ancho;
                inicioX = finalX + ancho - 1;
                avanceX = -1;

                //berificamos el clipping a la izquierda
                if (X < 0)
                {
                    x = 0;
                    inicioX += X;
                }
                //Verificamos clipping a la derecha
                if ((X + ancho) >= canvas.Width)
                {
                    finalX += (x + ancho) - canvas.Width;
                }
            }
            //flip vertical
            if (FV == false)
            {
                inicioY = animacionActual * alto;
                finalY = inicioY + alto;
                avanceY = 1;

                //verificamos si hay clipping arriba
                if (Y < 0)
                {
                    y = 0;
                    inicioY += -Y;
                    reinicioY = 0;
                }
                //verificamos si hay clipping abajo
                if ((Y + alto) >= canvas.Height)
                {
                    finalY -= ((y + alto) - canvas.Height);
                }
            }
            else
            {
                finalY = animacionActual * alto;
                inicioY = finalY + alto - 1;
                avanceY = -1;
                //verificamos si hay clipping arriba
                if (Y < 0)
                {
                    y = 0;
                    reinicioY = 0;
                    inicioY += Y;
                }
                //Verificamos si hay clipping abajo
                if ((Y + alto) >= canvas.Height)
                {
                    finalY += (y + alto) - canvas.Height;
                }
            }

            for (xR = inicioX; evaluaDir(xR, finalX, FH); xR += avanceX, x++)
            {
                for (yR = inicioY, y = reinicioY; evaluaDir(yR, finalY, FV); yR += avanceY, y++)
                {
                    colorImagen = imagen.GetPixel(xR, yR);
                    if (transparencia() == true)
                    {
                        if (colorImagen != colorTransparencia)
                        {
                            canvas.SetPixel(x, y, colorImagen);
                        }
                    }
                    else
                        canvas.SetPixel(x, y, colorImagen);
                }
            }
        }
        //verifica si el sprite lleva transparencia
        private bool transparencia()
        {
            if (isTransparente)
            {
                if (colorTransparencia == null)
                {
                    return false;
                } else
                    return true;
            } else
                return false;
        }
        //evalua la direccion de pintado del sprite
        private bool evaluaDir(int control, int rango, bool flip)
        {
            bool resultado = false;
            if(flip == false)
            {
                resultado = control < rango;
            }
            else
            {
                resultado = control >= rango;
            }
            return resultado;
        }
        //Las animaciones pasan por aqui
        public void AvanzaAnimacion()
        {
            //verificamos si es sin animacion
            if (typeAnim == TipoAnmacion.sinAnimacion)
                return;

            if (typeAnim == TipoAnmacion.UnaVez)
            {
                if(cuadroActual >= cuadros)
                {
                    cuadroActual = -1;
                    typeAnim = TipoAnmacion.sinAnimacion;
                }
                if (cuadroActual < 0)
                {
                    cuadroActual = 0;
                    typeAnim = TipoAnmacion.sinAnimacion;
                }
            }
            if(typeAnim == TipoAnmacion.PingPong)
            {
                if (cuadroActual >=cuadros -1)
                {
                    dirAnim = DirAnimacion.reversa;
                }
                if(cuadroActual ==0)
                    dirAnim = DirAnimacion.normal;
                
            }
            if(typeAnim == TipoAnmacion.Repite && contAnim >=30 - velAnim)
            {
                if(cuadroActual >=cuadros -1 && dirAnim == DirAnimacion.normal)
                {
                    cuadroActual = -1;
                }
                if(cuadroActual == 0 && dirAnim == DirAnimacion.reversa)
                {
                    cuadroActual = cuadros;
                }
            }
            contAnim += velAnim;
            if(contAnim >= 30)
            {
                cuadroActual += (int)dirAnim;
                contAnim = 0;

                if (cuadroActual >= cuadros)
                    cuadroActual = cuadros;
                if (cuadroActual < 0)
                    cuadroActual = 0;
            }

        }
        //copiar el fondo antes de terminar el cuadro
        public void CopiarFondo()
        {
            Color colorImagen = new Color();

            int xr = 0;
            int yr = 0;

            int x = X;
            int y = Y;
            int limX = X + ancho;
            int limY = Y + alto;

            if ((Y + alto < 0)||Y>=canvas.Height || (X + ancho < 0) || X >= canvas.Width)
            {
                usarCopia = false;
                return;
            }

            usarCopia = true;
            

            // izquierda
            if (X < 0)
            {
                x = 0;
                limX = ancho + X;

            }//clipping derecha 
            else if(Y + ancho >= canvas.Width)
            {
                limX = canvas.Width;
            }
            //clipping arriba
            if (Y < 0)
            {
                y = 0;
                limY = alto +Y;
            }//clipping abajo
            else if(Y +alto >= canvas.Height)
            {
                limY = canvas.Height;
            }

            recorridoXc = limX - x;
            recorridoYC = limY - y;
            int reinicioy = y;
            xC = x;
            yC = y;
            for(x=x,xr=0;x<limX; x++, xr++)
            {
                for(y=reinicioy,yr = 0; y < limY; y++, yr++)
                {
                    colorImagen = canvas.GetPixel(x, y);
                    recorte.SetPixel(xr, yr, colorImagen);
                }
            }
        }
        //pintar el fondo a cada cuadro
        public void PintarFondo()
        {
            if (usarCopia == false)
            {
                return;
            }
            Color colorImagen = new Color();
            int xr = 0;
            int yr = 0;
            int x = xC;
            int y = yC;

            for(xr = 0; xr < recorridoXc; xr++)
            {
                for (yr = 0; yr < recorridoYC; yr++)
                {
                    colorImagen = recorte.GetPixel(xr, yr);
                    canvas.SetPixel(x + xr, y + yr, colorImagen);
                }
            }
        }
        //la iteracion de las capas
        int IComparable.CompareTo(object obj)
        {
            NCSprite temp = (NCSprite)obj;
            if (capa > temp.Capa)
                return 1;
            if (capa < temp.Capa)
                return -1;

            return 0;
        }

        public virtual void Draw()
        {
            
        }

        
        public string Version
        {
            get { return "1.3"; }
        } 
    }
}

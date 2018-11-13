using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor_Grafico
{
    //public enum TiposColisiones { BoundingRectangle, BoundingCircle };
    class GameObject
    {
        //Atributos de la clase
        protected int id;

        //posicion y dimensiones
        protected int X;
        protected int Y;
        protected int ancho;
        protected int alto;
        protected int xan;
        protected int yal;
        protected int radioC;

        // Movimiento
        protected int deltaX;
        private int deltaY;

        //Colisiones
        protected bool colisionable;//Para saber  si este sprite detecta colisiones
        protected bool heColisionado;//Para saber si el sprite colisiono
        protected List<bool> listaColisiones;//Lista donde guarda el estado de colision con cada uno de los demas objetos
        protected IColisionable colisionador;//
        protected TiposColisiones tipoColision;

        //METODOS
        //CONSTRUCTORES
        //CONSTRUCTOR VACIO
        public GameObject()
        {
            id = 0;
            X = 0;
            Y = 0;
            ancho = 0;
            alto = 0;
            xan = 0;
            yal = 0;
            radioC = 0;
            deltaX = 0;
            deltaY = 0;
            colisionable = false;
            heColisionado = false;
            tipoColision = TiposColisiones.BoundingEmpty;
            colisionador = null;
        }
        //CONSTRUCTOR DE COORDENADAS
        public GameObject(int pX,int pY,int pAncho,int pAlto)
        {
            X = pX;
            Y = pY;
            ancho = pAncho;
            alto = pAlto;
            xan = X + ancho;
            yal = Y + alto;
            radioC = (ancho / 2) * (ancho / 2) + (alto / 2) * (alto / 2);
        }
        //CONSTRUCTOR COMPLETO
        public GameObject(int pX, int pY, int pAncho, int pAlto, TiposColisiones pTipos)
        {
            X = pX;
            Y = pY;
            ancho = pAncho;
            alto = pAlto;
            xan = X + ancho;
            yal = Y + alto;
            deltaX = 0;
            deltaY = 0;
            radioC = (ancho / 2) * (ancho / 2) + (alto / 2) * (alto / 2);
            tipoColision = pTipos;
            switch (pTipos)
            {
                case TiposColisiones.BoundingCircle:
                    colisionador = new NCBoundingCircle();
                    break;
                case TiposColisiones.BoundingRectangle:
                    colisionador = new NCBoundingRectangle();
                    break;
                case TiposColisiones.BoundingEmpty:
                    colisionador = null;
                    break;
            }
        }

        //PROPIEDADES
        //COORDENADAS Y DIMENSIONES
        public int PosX { get { return X; } set { X = value; } }
        public int PosY { get { return Y; } set { Y = value; } }
        public int Ancho { get { return ancho; } set { ancho = value; } }
        public int Alto { get { return alto; } set { alto = value; } }
        public int Xan { get { return xan; } set { xan = value; } }
        public int Yal { get { return yal; } set { yal = value; } }
        public int RadioC { get { return radioC; } set { radioC = value; } }

        //MOVIMIENTO
        public int DeltaX { get { return deltaX; } set { deltaX = value; } }
        public int DeltaY { get { return deltaY; } set { deltaY = value; } }

        //COLISIONES
        public bool Colisionable { get { return colisionable; } set { colisionable = value; } }
        public bool HeColisionado { get { return heColisionado; } set { heColisionado = value; } }

        public void ColocarDelta(int dX, int dY)
        {
            deltaX = dX;
            deltaY = dY;
        }

        public TiposColisiones TipoColision
        {
            set
            {
                switch (value)
                {
                    case TiposColisiones.BoundingRectangle:
                        tipoColision = value;
                        colisionador = new NCBoundingRectangle();
                        break;
                    case TiposColisiones.BoundingCircle:
                        tipoColision = value;
                        colisionador = new NCBoundingCircle();
                        break;
                }
            }
            get { return tipoColision; }
        }


        //METODOS DE COMPORTAMIENTO
        public void InicializaColisiones(int pCantidad)
        {
            listaColisiones = new List<bool>(pCantidad);
            for (int n = 0; n < pCantidad; n++)
                listaColisiones.Add(false);
        }
        public void ColocaColision(bool value, int indice)
        {
            if (value == true && listaColisiones[indice] == false)
                StartColision(indice);
            if (value == true && listaColisiones[indice] == true)
                OnColision(indice);
            if (value == false && listaColisiones[indice] == true)
                EndColision(indice);

            listaColisiones[indice] = value;
            heColisionado = value;
        }
        public void DetectaColision(GameObject spr, int indice, int indice2)
        {
            if (colisionador.DetectaColision(this, spr))
            {
                ColocaColision(true, indice);
                spr.ColocaColision(true, indice2);
            }
            else
            {
                ColocaColision(false, indice);
                spr.ColocaColision(false, indice2);
            }
        }
        public void Movimiento()
        {
            X += deltaX;
            Y += deltaY;
            if (colisionable)
            {
                xan = (X + ancho);
                yal = (Y + alto);
            }
        }

        //Handlers de los eventos
        //Eventos de las colisiones
        public virtual void StartColision(int pCon)//Empeiza colision
        {

        }
        public virtual void OnColision(int pCon)//Continua colision
        {

        }
        public virtual void EndColision(int pCon)//Finaliza colision
        {

        }
        //Eventos para el estado del objeto
        public virtual void Awake()//Para cuando se crea el objeto
        {

        }
        public virtual void Start()//Para cuando se inicia el objeto
        {

        }
        public virtual void End()//para cuando se destruye el objeto
        {

        }
        //Eventos para el movimiento
        public virtual void StartMovement()//Inicia movimiento
        {

        }
        public virtual void OnMovement()//Esta en movimiento
        {

        }
        public virtual void EndMovement()//Termina de moverse
        {

        }
       
    }
}

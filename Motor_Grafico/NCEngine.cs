using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor_Grafico
{
    //public enum TiposColisiones { BoundingRectangle, BoundingCircle};

    class NCEngine
    {
        //Bitmap donde se dibuja todo

        private Bitmap lienzo;

        private int ancho;
        private int alto;

        private string archivoLienzo = "";

        //lista donde se gurardan los sprites

        private List<NCSprite> lstSprites = new List<NCSprite>();
        private List<NCTexto> lstTexto = new List<NCTexto>();

        //private IColisionable colisionador;
        //private TiposColisiones tipoColision;

        public NCEngine(int pAncho,int pAlto, TiposColisiones pColisiones, string pLienzo)
        {
            ancho = pAncho;
            alto = pAlto;

            archivoLienzo = pLienzo;
            lienzo = new Bitmap(ancho, alto);

            //switch (pColisiones)
            //{
            //    case TiposColisiones.BoundingRectangle:
            //        tipoColision = pColisiones;
            //        colisionador = new NCBoundingRectangle();
            //        break;
            //    case TiposColisiones.BoundingCircle:
            //        tipoColision = pColisiones;
            //        colisionador = new NCBoundingCircle();
            //        break; 
            //}

            initPruebas();
        }
        //public TiposColisiones TipoColision
        //{
        //    set
        //    {
        //        switch (value)
        //        {
        //            case TiposColisiones.BoundingRectangle:
        //                tipoColision = value;
        //                colisionador = new NCBoundingRectangle();
        //                break;
        //            case TiposColisiones.BoundingCircle:
        //                tipoColision = value;
        //                colisionador = new NCBoundingCircle();
        //                break;
        //        }
        //    }
        //    get { return tipoColision; }
        //}

        public Bitmap Canvas { get { return lienzo; } }

        private void initPruebas()
        {
            //for (int x = 0; x < lienzo.Width; x++)
            //{
            //    for (int y = 0; y < lienzo.Height; y++)
            //    {
            //        lienzo.SetPixel(x, y, Color.DarkGreen);
            //    }
            //}
            lienzo = new Bitmap(archivoLienzo);
        }

        public void adicionaSprite(NCSprite pSprite)
        {
            if(pSprite!= null)
            {
                pSprite.ColocaCanvas(lienzo);
                lstSprites.Add(pSprite);
            }
        }
        public void adicionTexto(NCTexto pTexto)
        {
            if(pTexto != null)
            {
                pTexto.ColocaCanvas(lienzo);
                lstTexto.Add(pTexto);
            }
        }
        public void cicloJuego()
        {

            foreach(NCSprite sprite in lstSprites)
            {
                sprite.PintarFondo();
            }
            foreach(NCTexto texto in lstTexto)
            {
                texto.PintarFondo();
            }
            if(lstSprites[0].Ordena == true)
            {
                lstSprites.Sort();
                lstSprites[0].Ordena =false;
            }
            foreach (NCSprite sprite in lstSprites)
            {
                sprite.Movimiento();
                sprite.CopiarFondo();
                sprite.AvanzaAnimacion();
            }
            foreach (NCTexto texto in lstTexto)
                texto.CopiarFondo();
            foreach (NCSprite sprite in lstSprites)
            {
                sprite.DibujaSprite();
            }
            foreach (NCTexto texto in lstTexto)
                texto.DibujaTexto();
            verificaColisiones();
        }
        public void verificaColisiones()
        {
            int n = 0;
            int m = 0;


            for (n=0;n<lstSprites.Count-1;n++)
            {
                //verificamos colisiones contra otros si es colisionable
                if (lstSprites[n].Colisionable == true)
                {
                    for (m = n + 1; m < lstSprites.Count; m++)
                    {
                        if (lstSprites[m].Colisionable==true)
                        {

                            //verificamos los casos para las colisiones
                            lstSprites[n].DetectaColision(lstSprites[m], m, n);
                        }
                    }
                }
            }
        }
        public void InicializaEngine()
        {
            lstSprites.Sort();
            foreach(NCSprite sprite in lstSprites)
            {
                sprite.CopiarFondo();
                sprite.InicializaColisiones(lstSprites.Count);
            }
            foreach (NCTexto texto in lstTexto)
                texto.CopiarFondo();
        }
        public int IDporIndice(int pIndice)
        {
            return lstSprites[pIndice].ID;
        }
    }
}

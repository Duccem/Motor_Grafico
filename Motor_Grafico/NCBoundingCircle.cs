using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor_Grafico
{
    class NCBoundingCircle:IColisionable
    {
        

        public bool DetectaColision(NCSprite sp1, NCSprite sp2)
        {
            bool colision = false;
            int x = ((sp1.PosX + sp1.Ancho / 2) - (sp2.PosX + sp2.Ancho / 2));
            int y = ((sp1.PosY + sp1.Alto / 2) - (sp2.PosY + sp2.Alto / 2));
            int d = (x * x) + (y * y);

            if(d<=(sp1.RadioC + sp2.RadioC))
            {
                colision = true;
            }
            return colision;
        }

        public bool DetectaColision(GameObject gob1, GameObject gob2)
        {
            throw new NotImplementedException();
        }
    }
}

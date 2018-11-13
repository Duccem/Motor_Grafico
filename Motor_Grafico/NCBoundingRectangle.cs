using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor_Grafico
{
    class NCBoundingRectangle: IColisionable
    {
        public bool DetectaColision(NCSprite sp1, NCSprite sp2)
        {
            throw new NotImplementedException();
        }

        public bool DetectaColision(GameObject gob1, GameObject gob2)
        {
            throw new NotImplementedException();
        }

        public bool DetectaColison(NCSprite sp1, NCSprite sp2)
        {
            bool colision = false;

            if (((sp1.PosX >= sp2.PosX && sp1.PosX < sp2.Xan) || (sp1.Xan >= sp2.PosX && sp1.Xan < sp2.Xan))
                                                              && 
                ((sp1.PosY >= sp2.PosY && sp1.PosY < sp2.Yal) || (sp1.Yal >= sp2.PosY && sp1.Yal < sp2.Yal)))
            {
                colision = true;
            }
            return colision;
        }

        
    }
}

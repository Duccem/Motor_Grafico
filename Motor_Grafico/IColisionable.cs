using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor_Grafico
{
    interface IColisionable
    {
         bool DetectaColision(NCSprite sp1, NCSprite sp2);
         bool DetectaColision(GameObject gob1, GameObject gob2);
    }
}

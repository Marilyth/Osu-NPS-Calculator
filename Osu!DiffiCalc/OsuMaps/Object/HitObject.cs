using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osu_DiffiCalc.OsuMaps.Object
{
    class HitObject
    {
        public int time, positionX, positionY;
        public int NPS { get; set; }

        public HitObject(int objectXPos, int objectYPos, int objectTime)
        {
            time = objectTime;
            positionX = objectXPos;
            positionY = objectYPos;
        }
    }
}

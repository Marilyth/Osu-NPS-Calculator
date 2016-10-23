using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osu_DiffiCalc.Calculation.Advanced
{
    class AllMode
    {
        internal static double setGetNPS(List<Osu_DiffiCalc.OsuMaps.Object.HitObject> transSet)
        {
            int skip = Program.objectSkip, count = 0;
            List<Osu_DiffiCalc.OsuMaps.Object.HitObject> newSet = new List<OsuMaps.Object.HitObject>();
            newSet = transSet;

            foreach (OsuMaps.Object.HitObject obj in transSet)
            {
                if(!(count < skip))
                {
                    newSet = newSet.SkipWhile(x => x.time < obj.time - 1000).ToList();

                    obj.NPS = newSet.IndexOf(obj) +1;

                    count = 0;
                }
                
                else count++;
            }
            try
            {
                return transSet.Average(x => x.NPS);
            }
            catch (Exception) { return Double.NaN; }
        }
    }
}

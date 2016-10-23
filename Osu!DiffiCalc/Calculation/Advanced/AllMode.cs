using System;
using System.Collections.Generic;
using System.Linq;

namespace Osu_DiffiCalc.Calculation.Advanced
{
    class AllMode
    {
        internal static double setGetNPS(SortedList<int, OsuMaps.Object.HitObject> transSet)
        {
            int skip = Program.objectSkip, count = 0;

            foreach (OsuMaps.Object.HitObject obj in transSet.Values)
            {
                if(!(count < skip))
                {
                    obj.NPS = transSet.Count(x => x.Key <= obj.time && x.Key >= obj.time - 1000);

                    count = 0;
                }
                
                else count++;
            }
            try
            {
                return transSet.Average(x => x.Value.NPS);
            }
            catch (Exception) { return Double.NaN; }
        }
    }
}

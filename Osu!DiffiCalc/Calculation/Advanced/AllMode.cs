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


            foreach (OsuMaps.Object.HitObject obj in transSet)
            {
                if(!(count < skip))
                {
                    obj.NPS =
                    (from hitObject in transSet
                    where hitObject.time <= obj.time && hitObject.time >= obj.time - 1000
                    select hitObject).Count();

                    newSet.Add(obj);

                    count = 0;
                }
                
                else count++;
            }
            try
            {
                transSet = newSet;
                return transSet.Average(x => x.NPS);
            }
            catch (Exception) { return Double.NaN; }
        }
    }
}

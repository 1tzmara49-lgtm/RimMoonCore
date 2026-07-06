using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RimMoonCore
{
    // K-CORP ampule's regeneration
    public class HediffCompProperties_Regeneration : HediffCompProperties
    {
        public int intervalTicks = 600;
        public int maxTickAge = GenDate.TicksPerDay;
        
        public HediffCompProperties_Regeneration()
        {
            this.compClass = typeof(HediffComp_Regeneration);
        }
    }

    public class HediffComp_Regeneration : HediffComp
    {
        private int ticks;

        public HediffCompProperties_Regeneration Props =>
            (HediffCompProperties_Regeneration)props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            ticks++;

            if (ticks < Props.intervalTicks)
                return;

            ticks = 0;

            Pawn pawn = parent.pawn;

            if (pawn == null)
            {
                return;
            }
            
            var hediffList = new List<Hediff>();

            foreach (Hediff h in pawn.health.hediffSet.hediffs)
            {
                if (h is Hediff_Injury || h is Hediff_MissingPart)
                {
                    hediffList.Add(h);
                }
            }
            if (hediffList.Count == 0)
            {
                return;
            }
            var selectedHediff = hediffList.RandomElement();
            if (selectedHediff.ageTicks > Props.maxTickAge || !selectedHediff.IsPermanent())
            {
                pawn.health.RemoveHediff(selectedHediff);
            }

        }
    }
}

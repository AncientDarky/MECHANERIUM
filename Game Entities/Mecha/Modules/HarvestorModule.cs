using UnityEngine;
namespace Mechaerium
{
    public class HarvestorModule : Module
    {
        [SerializeField] float[] GatheringYield;
        [SerializeField] float[] GatheringRange;
        [SerializeField] int[] MaximumTargets;
        public void HarvestorTick()
        {

        }
    }
}

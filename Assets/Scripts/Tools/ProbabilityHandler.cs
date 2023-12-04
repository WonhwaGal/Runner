using Tools;
using UnityEngine;

namespace Factories
{
    internal static class ProbabilityHandler
    {
        public static int GetProbability(int number)
        {
            int randomnumber = Random.Range(0, 100);

            if (randomnumber <= Constants.ThreeUpgradesPercent)
                return number;
            else if(randomnumber <= Constants.TwoUpgradesPercent)
                return number - 1;
            else if(randomnumber <= Constants.OneUpgradePercent)
                return number - 2;

            return default;
        }
    }
}
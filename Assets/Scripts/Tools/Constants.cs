using UnityEngine;

namespace Tools
{
    internal static class Constants
    {
        public static float leftBorder = -5f;
        public static float rightBorder = 5f;
        public static float kmAddTimeSpan = 5.0f;
        public static int kmSpan = 10;
        public static int SaveDistance = 150;
        public static Vector3 jumpVector = new (0, 40, 10);
        public static float increaseSpeedSpan = 0.7f;
        public static float coinMagnetSpeed = 0.3f;
        public static float MAX_MIXER_VOLUME = 0f;
        public static float MIN_MIXER_VOLUME = -80f;

        public static int ThreeUpgradesPercent = 5;
        public static int TwoUpgradesPercent = 25;
        public static int OneUpgradePercent = 60;
    }
}
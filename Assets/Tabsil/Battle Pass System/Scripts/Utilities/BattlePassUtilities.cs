using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tabsil.BattlePassSystem
{
    public static class BattlePassUtilities
    {
        public static string CustomTimeSpanToString(TimeSpan timeLeft)
        {
            int days = timeLeft.Days;
            int hours = timeLeft.Hours;
            int minutes = timeLeft.Minutes;

            string daysString = days > 0 ? days + "d" : "";
            string separation = hours > 0 ? " " : "";
            string hoursString = hours > 0 ? hours + "h" : "";

            bool showMinutesCondition = (days == 0 && hours >= 0) || (days >= 0 && hours == 0);

            string separationBis = showMinutesCondition ? " " : "";
            string minutesString = showMinutesCondition ? minutes + "min" : "";

            return daysString + separation + hoursString + separationBis + minutesString;
        }
    }
}
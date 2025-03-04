using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Tabsil.BattlePassSystem
{
    public class BattlePassSlider : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI xpText;
        [SerializeField] private TextMeshProUGUI levelText;

        public void Initialize(bool hasCompletedSeason, float value, float maxValue, int level)
        {
            slider.minValue = 0;

            if (hasCompletedSeason)
                MaxLevelReached(level);
            else
            {
                slider.maxValue = maxValue;
                slider.value = value;

                UpdateLevelText(level);
                UpdateXPText();
            }
        }

        public void ConfigureAfterLevelUp(float targetXp, int level)
        {
            if(targetXp == 0)
            {
                MaxLevelReached(level);
                return;
            }

            slider.maxValue = targetXp;
            slider.value = 0;

            UpdateLevelText(level);
            UpdateXPText();
        }

        private void MaxLevelReached(int level)
        {
            slider.maxValue = 1;
            slider.value = 1;

            UpdateLevelText(level);
            UpdateXPText(true);
        }

        public void UpdateVisual(float value)
        {
            slider.value = value;
            UpdateXPText();
        }

        private void UpdateXPText(bool maxLevel = false)
        {
            if (!maxLevel)
                xpText.text = slider.value + " / " + slider.maxValue;
            else
                xpText.text = "";
        }

        private void UpdateLevelText(int level)
        {
            levelText.text = level.ToString();
        }
    }
}
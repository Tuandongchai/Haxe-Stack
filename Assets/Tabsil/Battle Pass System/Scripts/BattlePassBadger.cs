using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tabsil.BattlePassSystem
{
    public class BattlePassBadger : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private Sprite[] badges;
        [SerializeField] private Image badgeImage;

        public void Initialize(BattlePassSystem battlePassSystem)
        {
            int level = battlePassSystem.GetLevel();
            UpdateVisuals(level);
        }

        public void UpdateVisuals(int level)
        {
            level = Mathf.Min(level - 1, badges.Length - 1);
            badgeImage.sprite = badges[level];
        }
    }
}
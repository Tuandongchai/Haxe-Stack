using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Tabsil.BattlePassSystem
{   
    public class LevelContainer : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private TextMeshProUGUI text;

        public void Configure(int level) => text.text = level.ToString();
    }
}
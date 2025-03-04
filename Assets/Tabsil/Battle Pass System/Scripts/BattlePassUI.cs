using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabsil.BattlePassSystem
{
    public class BattlePassUI : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private CanvasGroup canvasGroup;

        private void Start() => Hide();

        public void Show()
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
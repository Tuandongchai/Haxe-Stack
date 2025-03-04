using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Tabsil.BattlePassSystem
{
    public class ClaimedRewardContainer : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private Transform halo;
        [SerializeField] private Image rewardIcon;
        [SerializeField] private TextMeshProUGUI rewardAmountText;

        [Header(" Settings ")]
        [SerializeField] private float haloRotationSpeed;

        public void Configure(Sprite sprite, string amountText)
        {
            rewardIcon.sprite = sprite;
            rewardAmountText.text = amountText;

            Enable();
        }

        private void Enable()   => gameObject.SetActive(true);
        public void Disable()  => gameObject.SetActive(false);

        private void Update()
        {
            if (gameObject.activeSelf)
                halo.Rotate(Vector3.forward * Time.deltaTime * haloRotationSpeed);
        }
    }
}
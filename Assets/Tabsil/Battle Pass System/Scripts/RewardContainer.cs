using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Tabsil.BattlePassSystem
{
    public class RewardContainer : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Button claimButton; 
        [SerializeField] private GameObject checkIcon; 
        [SerializeField] private GameObject lockIcon; 
        private BattlePassSystem system;

        [field: SerializeField] public bool IsGolden { get; private set; }
        public CompoundReward Reward { get; private set; }

        public void Configure(Day day, BattlePassSystem system, bool isUnlocked, bool isClaimed)
        {
            // Precaution
            if (isUnlocked && !isClaimed) 
            {
                Unlock();
                checkIcon.SetActive(false);
            }
            else if(isUnlocked && isClaimed)
            {
                SetAsClaimed();
            }
            else
            {
                claimButton.gameObject.SetActive(false);
                checkIcon.SetActive(false);
                
                if(IsGolden)
                    lockIcon.SetActive(true);
            }
            
            this.system = system;
            Reward = IsGolden ? day.goldenCompoundReward : day.compoundReward;

            string rewardText = GetRewardText(day);
            amountText.text = rewardText;

            RewardType rewardType = IsGolden ? day.goldenCompoundReward.GetRewardType() : day.compoundReward.GetRewardType();
            icon.sprite = system.GetRewardTypeSprite(rewardType);

            claimButton.onClick.RemoveAllListeners();
            claimButton.onClick.AddListener(Claim);
        }

        public void Unlock()
        {
            // The goal of this method is to show the claim button
            claimButton.gameObject.SetActive(true);

            if(IsGolden)
                lockIcon.SetActive(false);
        }

        public void Claim()
        {
            // At this point, we can reward the player with the reward
            system.ClaimReward(this);

            SetAsClaimed();
        }

        public void SetAsClaimed()
        {
            // This will disable the claim button & enable the check emoji
            claimButton.gameObject.SetActive(false);
            checkIcon.SetActive(true);
            
            if (IsGolden)
                lockIcon.SetActive(false);
        }

        private string GetRewardText(Day day)
        {
            string rewardString = IsGolden ? day.goldenCompoundReward.GetRewardText(): day.compoundReward.GetRewardText();
            return rewardString;
        }
    }
}

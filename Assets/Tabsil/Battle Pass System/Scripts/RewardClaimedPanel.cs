using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Tabsil.BattlePassSystem
{
    public class RewardClaimedPanel : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private TextMeshProUGUI rewardNameText;
        [SerializeField] private Transform claimedRewardContainersParent;
        [SerializeField] private Animator rewardClaimedAnimator;

        private void Awake()
        {
            BattlePassSystem.onRewardClaimed += RewardClaimedCallback;
        }

        private void OnDestroy()
        {
            BattlePassSystem.onRewardClaimed -= RewardClaimedCallback;
        }

        private void RewardClaimedCallback(BattlePassSystem system, CompoundReward reward)
        {
            if (reward.isGift)
                GiftClaimedCallback(system, reward);
            else
                SimpleRewardClaimedCallback(system, reward);

            rewardClaimedAnimator.Play("Open");
        }

        private void SimpleRewardClaimedCallback(BattlePassSystem system, CompoundReward reward)
        {
            Reward[] rewards = { reward.rewards[0] };
            string rewardName = reward.rewards[0].rewardType.ToString();
            Configure(system, rewards, rewardName);       
        }

        private void GiftClaimedCallback(BattlePassSystem system, CompoundReward reward)
        {
            Reward[] rewards = reward.rewards;
            Configure(system, rewards, "Gift");
        }

        private void Configure(BattlePassSystem system, Reward[] rewards, string rewardNameString)
        {
            rewardNameText.text = rewardNameString;

            for (int i = 0; i < claimedRewardContainersParent.childCount; i++)
                claimedRewardContainersParent.GetChild(i).gameObject.SetActive(false);

            for (int i = 0; i < rewards.Length; i++)
            {
                Reward simpleReward = rewards[i];
                Sprite sprite = system.GetRewardTypeSprite(simpleReward.rewardType);
                string amountText = simpleReward.GetRewardAmountString();

                ClaimedRewardContainer container = claimedRewardContainersParent.GetChild(i).GetComponent<ClaimedRewardContainer>();
                container.Configure(sprite, amountText);
            }
        }

        public void ClosePanel()
        {
            rewardClaimedAnimator.Play("Close");
        }
    }
}
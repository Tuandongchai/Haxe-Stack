using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

namespace Tabsil.BattlePassSystem
{
    public class BattlePassTester : MonoBehaviour
    {
        [Button]
        public void Show()              => BattlePassSystem.Show();

        [Button]
        public void Hide()              => BattlePassSystem.Hide();

        [Button]
        public void StartNextSeason()   => BattlePassSystem.StartNextSeason();

        [Button]
        public void AddOneXp()          => BattlePassSystem.AddXp(1);

        [Button]
        public void AddFiveXp()         => BattlePassSystem.AddXp(5);

        [Button]
        public void AddOneHour()        => BattlePassSystem.AddTime(1);

        [Button]
        public void AddOneDay()         => BattlePassSystem.AddTime(24);

        [Button]
        public void Purchase()          => BattlePassSystem.TryPurchase(true);

        [Button]
        public void ResetData()         => BattlePassSystem.ResetData();

        private void Awake()
        {
            BattlePassSystem.onRewardClaimed            += RewardClaimedCallback;
            BattlePassSystem.onPurchaseButtonClicked    += PurchaseButtonCallback;
            BattlePassSystem.onSeasonEnded              += SeasonEndedCallback;
        }

        private void OnDestroy()
        {
            BattlePassSystem.onRewardClaimed            -= RewardClaimedCallback;
            BattlePassSystem.onPurchaseButtonClicked    -= PurchaseButtonCallback;
            BattlePassSystem.onSeasonEnded              -= SeasonEndedCallback;
        }

        private void RewardClaimedCallback(BattlePassSystem system, CompoundReward compoundReward)
        {
            foreach (Reward reward in compoundReward.rewards)
                Debug.Log($"<color=#ff7500><b>[Test]</b> {reward.GetRewardAmountString()} of {reward.rewardType} has been claimed.</color>");
        }

        private void PurchaseButtonCallback(BattlePassSystem system)
        {
            BattlePassSystem.TryPurchase(true);
        }

        private void SeasonEndedCallback(BattlePassSystem system)
        {
            Debug.Log("<color=#ff7500>[Test] Season has ended.</color>");
        }
    }
}

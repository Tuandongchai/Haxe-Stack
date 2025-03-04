using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabsil.BattlePassSystem
{
    [CreateAssetMenu(fileName = "Season", menuName = "Scriptable Objects/Battle Pass System/Season", order = 0)]
    public class Season : ScriptableObject
    {
        [field: SerializeField] [Tooltip("Duration in hours")] public int Duration { get; private set; }
        [field: SerializeField] public List<Day> Days { get; private set; }
    }

    public enum RewardType
    {
        Null = 100,
        Bomb = 2,
        Potion = 0,
        Lightning = 1,
        Gift = 3,
        Speed = 4
    }

    [System.Serializable]
    public struct CompoundReward
    {
        public bool isGift;
        public Reward[] rewards;

        public RewardType GetRewardType()
        {
            if (isGift)
                return RewardType.Gift;
                      
            if(rewards.Length <= 0)
            {
                Debug.LogError("No reward found, please populate this compound reward.");
                return RewardType.Null;
            }

            return rewards[0].rewardType;
        }

        public string GetRewardText()
        {
            if (isGift)
                return "Gift";

            if (rewards.Length <= 0)
            {
                Debug.LogError("No reward found, please populate this compound reward.");
                return "Not Found";
            }

            return rewards[0].GetRewardAmountString();
        }
    }

    [System.Serializable]
    public struct Reward
    {
        public RewardType rewardType;
        public float rewardAmount;

        public string GetRewardAmountString()
        {
            switch (rewardType)
            {
                case RewardType.Bomb:
                    return "x" + rewardAmount;

                case RewardType.Potion:
                    return "x" + rewardAmount;

                case RewardType.Lightning:
                    return FormatPlayTime(rewardAmount);

                default:
                    return "";
            }
        }

        private string FormatPlayTime(float rewardAmount)
        {
            if (rewardAmount < 60)
                return rewardAmount + "min";

            int hours = Mathf.FloorToInt(rewardAmount / 60);
            int minutes = Mathf.FloorToInt(rewardAmount % 60);
            return hours + "h" + minutes + "min";
        }
    }

    [System.Serializable]
    public struct Day
    {
        public CompoundReward compoundReward;
        public CompoundReward goldenCompoundReward;
        
        public float requiredXp;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabsil.BattlePassSystem
{
    [CreateAssetMenu(fileName = "Sprite Data", menuName = "Scriptable Objects/Battle Pass System/Sprite Data", order = 1)]
    public class SpriteCorrespondance : ScriptableObject
    {
        [field: SerializeField] public List<SpriteData> data { get; private set; }

        public Sprite GetSprite(RewardType rewardType)
        {
            foreach (SpriteData sd in data)
                if (sd.rewardType == rewardType)
                    return sd.rewardSprite;

            Debug.LogWarning("No sprite found for this reward type : " + rewardType);

            return null;
        }
    }

    [System.Serializable]
    public struct SpriteData
    {
        public RewardType rewardType;
        public Sprite rewardSprite;
    }
}

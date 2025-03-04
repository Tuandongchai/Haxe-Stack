using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

namespace Tabsil.BattlePassSystem
{
    [RequireComponent(typeof(BattlePassSlider))]
    [RequireComponent(typeof(BattlePassSeasonTimer))]
    [RequireComponent(typeof(BattlePassBadger))]
    [RequireComponent(typeof(BattlePassUI))]
    public class BattlePassSystem : MonoBehaviour
    {
        public static BattlePassSystem instance;

        [field: SerializeField] public bool TestMode { get; private set; }

        [Header(" Elements ")]
        [SerializeField] private Button purchaseButton;
        [SerializeField] private RectTransform mainScroll;
        private BattlePassSlider slider;
        private BattlePassSeasonTimer seasonTimer;
        private BattlePassBadger badger;
        private BattlePassUI ui;

        [Header(" Data ")]
        [SerializeField] private Season[] seasons;
        [SerializeField] private SpriteCorrespondance spriteData;
        private Season currentSeason;
        private bool hasPurchased;
        private DateTime seasonStart;
        private DateTime seasonEnd;

        [Header(" Save Keys ")]
        private const string seasonIndexKey             = "BattlePassSeason";
        private const string purchasedStateKey          = "BattlePassPurchased";
        private const string seasonStartKey             = "SeasonStartTime";
        private const string seasonEndKey               = "SeasonEndTime";
        private const string battlePassLevelKey         = "BattlePassLevel";
        private const string xpKey                      = "BattlePassXp";
        private const string rewardClaimedKey           = "RewardClaimed";
        private const string goldenRewardClaimedKey     = "GoldenRewardClaimed";

        [Header(" Containers ")]
        private List<RewardContainer> goldenRewardContainers    = new List<RewardContainer>();
        private List<RewardContainer> rewardContainers          = new List<RewardContainer>();

        [Header(" Parents ")]
        [SerializeField] private Transform goldenRewardsParent;
        [SerializeField] private Transform levelsParent;
        [SerializeField] private Transform rewardsParent;

        [Header(" Prefabs ")]
        [SerializeField] private RewardContainer goldenRewardPrefab;
        [SerializeField] private LevelContainer levelPrefab;
        [SerializeField] private RewardContainer rewardPrefab;

        [Header(" Settings ")]
        private bool shouldInitialize = false;

        [Header(" XP Management ")]
        private int level;
        private int xp;

        [Header(" Actions ")]
        public static Action<BattlePassSystem, CompoundReward> onRewardClaimed;
        public static Action<BattlePassSystem> onSeasonEnded;
        public static Action<BattlePassSystem> onPurchaseButtonClicked;


        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            TestModeWarning();

            StoreComponents();

            LoadCurrentSeason();
            LoadSeasonDates();

            if (seasonEnd == DateTime.MinValue)
            {
                Debug.LogWarning("No season end found, skipping initialization");
                return;
            }

            shouldInitialize = true;
        }

        private void TestModeWarning()
        {
            if (!TestMode)
                return;

            Debug.LogWarning("You are Running the Battle Pass System in Test Mode.\nMake sure to disable Test Mode before publishing your game.");
        }

        private void StoreComponents()
        {
            slider      = GetComponent<BattlePassSlider>();
            seasonTimer = GetComponent<BattlePassSeasonTimer>();
            badger      = GetComponent<BattlePassBadger>();
            ui          = GetComponent<BattlePassUI>();
        }


        // Start is called before the first frame update
        void Start()
        {
            if(shouldInitialize)
                Initialize();
        }

        #region Initialization

        private void Initialize()
        {
            LoadData();

            goldenRewardContainers.Clear();
            rewardContainers.Clear();

            // Clear any active prefab
            goldenRewardsParent.Clear();
            levelsParent.Clear();
            rewardsParent.Clear();

            InitializePrefabs();         

            InitializeComponents();
            InitializePurchaseButton();
        }

        private void InitializePrefabs()
        {
            Day[] days = currentSeason.Days.ToArray();

            for (int i = 0; i < days.Length; i++)
                InitializeDay(i, days[i]);

            IEnumerator coroutine = InitializeMainScroll(days);
            StartCoroutine(coroutine);

            // This allows the player to claim the first normal reward 
            if (level == 0)
                LevelUp();
        }

        private void InitializeDay(int level, Day day)
        {
            RewardContainer goldenRewardInstance = Instantiate(goldenRewardPrefab, goldenRewardsParent);
            LevelContainer levelInstance = Instantiate(levelPrefab, levelsParent);
            RewardContainer rewardInstance = Instantiate(rewardPrefab, rewardsParent);

            bool isUnlocked = this.level > level;
            bool isGoldenUnlocked = isUnlocked && hasPurchased;

            bool isGoldenClaimed    = PlayerPrefs.GetInt(goldenRewardClaimedKey + level, 0) == 1;
            bool isClaimed          = PlayerPrefs.GetInt(rewardClaimedKey + level, 0) == 1;

            goldenRewardInstance.Configure(day, this, isGoldenUnlocked, isGoldenClaimed);
            levelInstance.Configure(level + 1);
            rewardInstance.Configure(day, this, isUnlocked, isClaimed);

            // Add the containrs to the lists so that we can use them later on when claiming
            goldenRewardContainers.Add(goldenRewardInstance);
            rewardContainers.Add(rewardInstance);
        }

        IEnumerator InitializeMainScroll(Day[] days)
        {
            yield return null;

            // The value 50 here represents the spacing between the reward containers.
            // You can tweak it here.

            float mainScrollWidth = days.Length * (goldenRewardPrefab.GetComponent<RectTransform>().rect.width + 50);

            mainScrollWidth -= 50;
            mainScrollWidth -= mainScroll.parent.GetComponent<RectTransform>().rect.width * .95f;

            mainScroll.sizeDelta = new Vector2(mainScrollWidth, mainScroll.sizeDelta.y);
        }

        private void InitializeComponents()
        {
            seasonTimer.Initialize(this);
            badger.Initialize(this);
            InitializeSlider();
        }

        private void InitializeSlider()
        {
            bool seasonCompleted = HasCompletedSeason();

            float sliderValue =     seasonCompleted ? 1 : GetRelativeXp(currentSeason);
            float maxSliderValue =  seasonCompleted ? 1 : currentSeason.Days[level].requiredXp;

            slider.Initialize(seasonCompleted, sliderValue, maxSliderValue, level);
        }

        private void InitializePurchaseButton()
        {
            if (hasPurchased)
                purchaseButton.gameObject.SetActive(false);
            else
            {
                purchaseButton.gameObject.SetActive(true);
                purchaseButton.onClick.RemoveAllListeners();
                purchaseButton.onClick.AddListener(() => onPurchaseButtonClicked?.Invoke(this));
            }
        }

        #endregion

        public Sprite GetRewardTypeSprite(RewardType rewardType) => spriteData.GetSprite(rewardType);

        private void AddXpAmount(int amount)
        {
            if (!seasonTimer.SeasonIsActive)
                return;

            if (level >= currentSeason.Days.Count)
            {
                Debug.LogWarning("Max Level Reached, Can't go further...");
                return;
            }

            xp += amount;
            XpChangedCallback();
            SaveXp();
        }

        private void XpChangedCallback()
        {
            float visualXp = GetRelativeXp(currentSeason, out bool shouldLevelUp);

            if(shouldLevelUp)
                LevelUp();
            else
                slider.UpdateVisual(visualXp);
        }

        private float GetRelativeXp(Season season, out bool shouldLevelUp)
        {
            shouldLevelUp = false;

            Day[] days = season.Days.ToArray();
            float xpSum = 0;

            for (int i = 0; i < days.Length; i++)
            {
                xpSum += days[i].requiredXp;

                if (i < level)
                    continue;

                if (xp >= xpSum)
                    shouldLevelUp = true;

                break;
            }

            // At this point, we have not leveled up
            // Let's simply update the slider
            float relativeXp = xp - (xpSum - days[level].requiredXp);
            return relativeXp;
        }

        private float GetRelativeXp(Season season) => GetRelativeXp(season, out bool unused);

        private void LevelUp()
        {
            Debug.Log("<color=#ffff00>Leveled Up !! </color>");

            int targetLevel = level + 1;

            UnlockCurrentReward();
            level++;

            if (targetLevel > currentSeason.Days.Count - 1)
                MaxLevelReached();
            else
            {               
                float sliderTargetXp = currentSeason.Days[level].requiredXp;
                slider.ConfigureAfterLevelUp(sliderTargetXp, level);
            }

            SaveLevel();

            badger.UpdateVisuals(level);
        }

        private void MaxLevelReached()
        {
            Debug.Log("<color=#bfff00>You've reached the max level !!! Congratulations !</color>");
            slider.ConfigureAfterLevelUp(0, level);
        }

        private void UnlockCurrentReward()
        {
            rewardContainers[level].Unlock();

            if (hasPurchased)
                goldenRewardContainers[level].Unlock();
        }

        public static void TryPurchase(bool purchaseCondition)
        {
            if (instance.hasPurchased || !instance.seasonTimer.SeasonIsActive || !purchaseCondition)
                return;           

            instance.Purchase();
        }

        private void Purchase()
        {
            Debug.Log("<color=#fffd00>Congratulations ! You've purchased the golden ticket !</color>");

            hasPurchased = true;

            for (int i = 0; i < goldenRewardContainers.Count; i++)
                if (i < level)
                    goldenRewardContainers[i].Unlock();

            purchaseButton.gameObject.SetActive(false);

            SavePurchasedState();
        }

        public void ClaimReward(RewardContainer rewardContainer)
        {
            CompoundReward reward = rewardContainer.Reward;
            onRewardClaimed?.Invoke(this, reward);

            string rewardKey = rewardContainer.IsGolden ? goldenRewardClaimedKey : rewardClaimedKey;
            PlayerPrefs.SetInt(rewardKey + rewardContainer.transform.GetSiblingIndex(), 1);
        }

        public bool HasCompletedSeason() => !seasonTimer.SeasonIsActive || level >= currentSeason.Days.Count;
        public void EndSeason() => onSeasonEnded?.Invoke(this);

        #region Saving & Loading

        private void LoadData()
        {
            LoadPurchasedState();
            LoadLevel();
            LoadXp();
        }

        private void SavePurchasedState()
        {
            PlayerPrefs.SetInt(purchasedStateKey, 1);
        }

        private void LoadPurchasedState()
        {
            hasPurchased = PlayerPrefs.GetInt(purchasedStateKey, 0) == 1;
        }

        private void SaveSeasonDates(DateTime seasonStart, DateTime seasonEnd)
        {
            PlayerPrefs.SetString(seasonStartKey,   seasonStart.ToString());
            PlayerPrefs.SetString(seasonEndKey,     seasonEnd.ToString());
            PlayerPrefs.Save();
        }

        private void LoadCurrentSeason()
        {
            int seasonIndex = PlayerPrefs.GetInt(seasonIndexKey, 0);

            if (seasonIndex > seasons.Length - 1)
            {
                Debug.LogError("Invalid season index...\nSeason could not be loaded");
                return;
            }

            currentSeason = seasons[seasonIndex];
        }

        private void LoadSeasonDates()
        {
            string seasonStartString    = PlayerPrefs.GetString(seasonStartKey, "");
            string seasonEndString      = PlayerPrefs.GetString(seasonEndKey, "");

            if (DateTime.TryParse(seasonStartString, out DateTime start))
                seasonStart = start;
            else
                Debug.LogWarning("Failed to parse season start");

            if (DateTime.TryParse(seasonEndString, out DateTime end))
                seasonEnd = end;
            else
                Debug.LogWarning("Failed to parse season end");
        }

        private void SaveLevel()
        {
            PlayerPrefs.SetInt(battlePassLevelKey, level);
        }

        private void LoadLevel()
        {
            level = PlayerPrefs.GetInt(battlePassLevelKey, 0);
        }

        // Set reset to true to reset the xp 
        private void SaveXp(bool reset = false)
        {
            int xp = this.xp;

            if (reset)
                xp = 0;
            
            PlayerPrefs.SetInt(xpKey, xp);
        }

        private void LoadXp()
        {
            xp = PlayerPrefs.GetInt(xpKey, 0);
        }

        private void ResetRewardStates()
        {
            Day[] days = currentSeason.Days.ToArray();

            for (int i = 0; i < days.Length; i++)
            {
                string goldenKey = goldenRewardClaimedKey + i;
                string key = rewardClaimedKey + i;

                PlayerPrefs.DeleteKey(goldenKey);
                PlayerPrefs.DeleteKey(key);
            }
        }

        private void ResetPassState()
        {
            hasPurchased = false;
            PlayerPrefs.SetInt(purchasedStateKey, 0);
        }

        #endregion

        #region Getters

        public DateTime GetSeasonEnd()      => seasonEnd;
        public Season GetCurrentSeason()    => currentSeason;
        public int GetLevel()               => level;

        #endregion

        #region Static Methods

        public static void Show() => instance.ui.Show();
        public static void Hide() => instance.ui.Hide();

        public static void StartSeason()
        {
            instance.seasonStart = DateTime.UtcNow;
            instance.seasonEnd = instance.seasonStart.AddHours(instance.currentSeason.Duration);

            instance.SaveSeasonDates(instance.seasonStart, instance.seasonEnd);

            instance.level = 0;
            instance.SaveLevel();

            instance.ResetPassState();
            instance.ResetRewardStates();

            instance.SaveXp(true);

            instance.Initialize();

            instance.seasonTimer.StartSeason();
        }

        public static void StartNextSeason()
        {
            int currentSeasonIndex = PlayerPrefs.GetInt(seasonIndexKey, -1);
            currentSeasonIndex++;

            currentSeasonIndex = currentSeasonIndex % instance.seasons.Length;

            PlayerPrefs.SetInt(seasonIndexKey, currentSeasonIndex);
            PlayerPrefs.Save();

            // Loop the seasons
            instance.currentSeason = instance.seasons[currentSeasonIndex];

            StartSeason();
        }

        public static void AddXp(int amount) => instance.AddXpAmount(amount);
        public static void AddTime(int hours) => instance.seasonTimer.AddHours(hours);

        public static void ResetData()
        {
            string[] keys =
            {
                seasonIndexKey,
                purchasedStateKey,
                seasonStartKey,
                seasonEndKey,
                battlePassLevelKey,
                xpKey,
                rewardClaimedKey,
                goldenRewardClaimedKey
            };

            foreach (string key in keys)
                PlayerPrefs.DeleteKey(key);

            instance.seasonTimer.ResetData();

            instance.Initialize();
        }

        #endregion
    }
}
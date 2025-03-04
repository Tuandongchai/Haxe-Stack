using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Tabsil.BattlePassSystem
{
    public class BattlePassSeasonTimer : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private TextMeshProUGUI timerText;
        private BattlePassSystem battlePassSystem;

        [Header(" Data ")]
        private DateTime lastPlayedTime;
        private const string lastPlayedTimeKey = "LastPlayedTime";

        public bool SeasonIsActive { get; private set; }

        private TimeSpan TimeLeft 
        { 
            get { return battlePassSystem.GetSeasonEnd() - lastPlayedTime; } 
            set { } 
        }

        public void Initialize(BattlePassSystem battlePassSystem)
        {
            this.battlePassSystem = battlePassSystem;

            LoadLastPlayedTime();

            if (lastPlayedTime == DateTime.MinValue)
                StartSeason();
            else
                ConfigureTimer();
        }

        public void StartSeason()
        {
            lastPlayedTime = DateTime.UtcNow;
            SaveLastPlayedTime();

            UpdateTimerText();

            StartCoroutine("TimerCoroutine");
            
            SeasonIsActive = true;
        }

        public void ConfigureTimer()
        {
            if (TimeLeft.TotalSeconds <= 0)
            {
                SeasonHasEnded();
                return;
            }

            SeasonIsActive = true;

            UpdateTimerText();

            StartCoroutine("TimerCoroutine");
        }

        IEnumerator TimerCoroutine()
        {
            while(true)
            {
                yield return new WaitForSeconds(10);

                if(battlePassSystem.TestMode)
                    lastPlayedTime += TimeSpan.FromSeconds(10);
                else
                    lastPlayedTime = DateTime.UtcNow;

                SaveLastPlayedTime();

                // Check for season end
                if (TimeLeft.TotalSeconds <= 0)
                {
                    SeasonEnd();
                    yield break;
                }

                UpdateTimerText();
            }
        }

        public void AddHours(int hours)
        {
            if (!SeasonIsActive)
            {
                Debug.LogWarning("Season ended, can't add one hour");
                return;
            }

            lastPlayedTime = lastPlayedTime + TimeSpan.FromHours(hours);
            SaveLastPlayedTime();

            // Check for season end
            if (TimeLeft.TotalSeconds <= 0)
            {
                SeasonEnd();
                return;
            }

            UpdateTimerText();
        }

        private void SeasonEnd()
        {
            SeasonHasEnded();

            StopAllCoroutines();

            battlePassSystem.EndSeason();
        }       
        
        private void SeasonHasEnded()
        {
            timerText.text = "Season Ended";
            lastPlayedTime = battlePassSystem.GetSeasonEnd();
            SeasonIsActive = false;
        }

        private void SaveLastPlayedTime()
        {
            PlayerPrefs.SetString(lastPlayedTimeKey, lastPlayedTime.ToString());
            PlayerPrefs.Save();
        }

        private void LoadLastPlayedTime()
        {
            string lastPlayedTimeString = PlayerPrefs.GetString(lastPlayedTimeKey);

            if (!DateTime.TryParse(lastPlayedTimeString, out lastPlayedTime))
                Debug.LogWarning("Failed to parse the last played time...");
        }

        private void UpdateTimerText()
        {
            timerText.text = BattlePassUtilities.CustomTimeSpanToString(TimeLeft);
        }

        private void OnApplicationPause(bool pause)
        {
            if(pause)
                SaveLastPlayedTime();
        }

        public void ResetData()
        {
            PlayerPrefs.DeleteKey(lastPlayedTimeKey);
        }
    }
}
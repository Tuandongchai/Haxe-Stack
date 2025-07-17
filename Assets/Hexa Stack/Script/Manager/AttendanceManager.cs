using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttendanceManager : MonoBehaviour
{
    public static AttendanceManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        DailyAttendanceButton.onClicked += Attendance;
        NewUserAttendanceButton.onClicked += NUAttendance;
    }
    private void OnDestroy()
    {
        DailyAttendanceButton.onClicked -= Attendance;
        NewUserAttendanceButton.onClicked -= NUAttendance;
    }
    public void Attendance(int day, int gold, int hammer, int swaps, int rolls)
    {
        int[] list = GameData.instance.GetDayArray();
        list[day-1] = 1;
        GameData.instance.SetDayArray(list);
        StatsManager.Instance.IncreasedGolds(gold);
        StatsManager.Instance.IncreasedTool(0, hammer);
        StatsManager.Instance.IncreasedTool(1, swaps);
        StatsManager.Instance.IncreasedTool(2, rolls);

        int[] amountArray = new int[] { gold, hammer, swaps, rolls};
        Debug.Log(string.Join(", ", amountArray));
        Dictionary<string, int> rewardDict = new Dictionary<string, int> { };
        for(int i=0; i< amountArray.Length; i++)
        {
            if (amountArray[i] == 0) continue;
            
            switch (i)
            {
                case 0:
                    rewardDict.Add("golds", amountArray[i]);
                    break;
                case 1:
                    rewardDict.Add("hammers", amountArray[i]);
                    break;
                case 2:
                    rewardDict.Add("swaps", amountArray[i]);
                    break;
                case 3:
                    rewardDict.Add("rolls", amountArray[i]);
                    break;
                default:
                    break;
            }
        }
        Debug.Log(string.Join(", ", rewardDict));
        RewardPopup.instance.ShowReward(rewardDict);
    }
    public void NUAttendance(int day, int gold, int hammer, int swaps, int rolls)
    {
        int[] list = GameData.instance.GetNUDayArray();
        list[day - 1] = 1;
        GameData.instance.SetNUDayArray(list);
        StatsManager.Instance.IncreasedGolds(gold);
        StatsManager.Instance.IncreasedTool(0, hammer);
        StatsManager.Instance.IncreasedTool(0, swaps);
        StatsManager.Instance.IncreasedTool(0, rolls);

        int[] amountArray = new int[] { gold, hammer, swaps, rolls };
        Dictionary<string, int> rewardDict = new Dictionary<string, int> { };
        for (int i = 0; i < amountArray.Length; i++)
        {
            if (amountArray[i] == 0) continue;

            switch (i)
            {
                case 0:
                    rewardDict.Add("golds", amountArray[i]);
                    break;
                case 1:
                    rewardDict.Add("hammers", amountArray[i]);
                    break;
                case 2:
                    rewardDict.Add("swaps", amountArray[i]);
                    break;
                case 3:
                    rewardDict.Add("rolls", amountArray[i]);
                    break;
                default:
                    break;
            }
        }
        RewardPopup.instance.ShowReward(rewardDict);
    }
}

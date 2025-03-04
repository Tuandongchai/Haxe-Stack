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
    }
    private void OnDestroy()
    {
        DailyAttendanceButton.onClicked -= Attendance;
    }
    public void Attendance(int day, int gold, int hammer, int swaps, int rolls)
    {
        int[] list = GameData.instance.GetDayArray();
        list[day-1] = 1;
        GameData.instance.SetDayArray(list);
        StatsManager.Instance.IncreasedGolds(gold);
        StatsManager.Instance.IncreasedTool(0, hammer);
        StatsManager.Instance.IncreasedTool(0, swaps);
        StatsManager.Instance.IncreasedTool(0, rolls);

    }

}

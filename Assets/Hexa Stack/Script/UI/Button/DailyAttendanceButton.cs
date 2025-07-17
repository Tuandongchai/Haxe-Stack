using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DailyAttendanceButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private int day;
    public static Action<int, int, int, int,int> onClicked;
    [SerializeField] private int golds, hammers, swaps, rolls;
    [SerializeField] private GameObject receivePanel;

    private void Awake()
    {
        button = GetComponent<Button>();
        int[] list = GameData.instance.GetDayArray();
        if (list[day - 1]==1)
        {
            button.enabled = false;
            receivePanel.SetActive(true);
        }
        button.onClick.AddListener(() => StartCoroutine(HandleButtonClick()));
    }

    private IEnumerator HandleButtonClick()
    {
        int[] list = GameData.instance.GetDayArray();
        int checkDay = GameData.instance.GetDay();

        yield return new WaitForSeconds(0.01f); 

        if (list[day - 1] == 0 && checkDay >= day)
        {
            onClicked?.Invoke(day, golds, hammers, swaps, rolls);
            StartCoroutine(DisableButton());
        }
    }
    
    IEnumerator DisableButton()
    {
        yield return new WaitForSeconds(0.1f);
        receivePanel.SetActive(true);  
        button.enabled = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyQuestPanel : BaseUI
{
    [SerializeField] private GameObject dailyQuestPanel, dailyPanel, weeklyPanel;

    private void Start()
    {
        dailyQuestPanel.SetActive(false);
    }
    public override void LoadIn()
    {
        dailyQuestPanel.SetActive(true);
        dailyQuestPanel.transform.localScale = Vector3.zero;
        LeanTween.scale(dailyQuestPanel,Vector3.one, 0.4f).setEase(LeanTweenType.easeInBounce);
    }

    public override void LoadOut()
    {
        dailyQuestPanel.transform.localScale = Vector3.one;
        LeanTween.scale(dailyQuestPanel, Vector3.zero, 0.4f).setEase(LeanTweenType.easeInBounce)
            .setOnComplete(()=>dailyQuestPanel.SetActive(false));
    }

    public void ShowDailyQuestPanel() {
        dailyPanel.SetActive(true);
        weeklyPanel.SetActive(false);
    }
    public void ShowWeeklyQuestPanel()
    {
        dailyPanel.SetActive(false);
        weeklyPanel.SetActive(true);
    }
}

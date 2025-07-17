using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : BaseButton<int>
{
    [SerializeField] private bool isUnlocked;
    [SerializeField] private Image locked;
    private int level;
    public static Action<int> onClicked;

    protected override void Start()
    {
        base.Start();
        t = GetLevel();
    }
    protected override void OnButtonClick()
    {
        base.OnButtonClick();
    }
    protected override void OnButtonClick(int t)
    {
        onClicked?.Invoke(t);
    }

    public int GetLevel() => int.Parse(gameObject.GetComponentInChildren<TextMeshProUGUI>().text);

    public void IsUnlocked(int lv)
    {
        if(lv>StatsManager.Instance.GetCurrentLevel())
            isUnlocked = true;
        else
            isUnlocked = false;

        locked.gameObject.SetActive(isUnlocked);
    }

}

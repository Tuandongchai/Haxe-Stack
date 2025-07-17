using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HammerButton : BaseButton,IToolButton
{
    [SerializeField] private GameObject clocked;   
    public static Action onClicked;

    protected override void Start()
    {
        base.Start();
        if(!Locked())
            clocked.SetActive(true);
        else clocked.SetActive(false);
    }

    protected override void OnButtonClick()
    {
        if (clocked.activeSelf)
            return;
        onClicked?.Invoke();
            
    }

    public bool Locked()
    {
        return StatsManager.Instance.GetCurrentLevel() >= 0;
    }
}

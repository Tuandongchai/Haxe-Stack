using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DailyQuestButton : BaseButton
{

    public static Action onclicked;
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClick()
    {
        onclicked?.Invoke();
    }
}

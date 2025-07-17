using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RetryButton : BaseButton
{
    public static Action onClicked;


    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        onClicked?.Invoke();
    }
}

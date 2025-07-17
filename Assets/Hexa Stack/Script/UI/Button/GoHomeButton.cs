using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoHomeButton : BaseButton
{
    public static Action onClicked;

    protected override void Start()
    {
        base.Start();
    }
    protected override void OnButtonClick()
    {
        /*base.OnButtonClick();*/
        onClicked?.Invoke();
    }
}

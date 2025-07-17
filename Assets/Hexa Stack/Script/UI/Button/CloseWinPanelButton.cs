using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CloseWinPanelButton : BaseButton
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

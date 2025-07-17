using NaughtyAttributes.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyItemButton : BaseButton<int, int>
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private int combo;
    public static Action<int,int> onClicked;

    protected override void Start()
    {
        t1 = int.Parse(priceText.text);
        t2 = combo;
        base.Start();
    }
    protected override void OnButtonClick()
    {
        base.OnButtonClick();
    }

    protected override void OnButtonClick(int price, int combo)
    {
        onClicked?.Invoke(price, combo);
    }
}

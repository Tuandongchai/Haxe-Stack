using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyItemButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private int combo;
    public static Action<int,int> onClicked;

    private void Awake()
    {
        int price = int.Parse(priceText.text);
        button = GetComponent<Button>();
        button.onClick.AddListener(() => onClicked?.Invoke(price, combo));
    }
}

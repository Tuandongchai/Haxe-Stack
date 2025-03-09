using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DailyQuestButton : MonoBehaviour
{
    private Button button;
    public static Action onclicked;
    void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(()=>onclicked?.Invoke());
    }

}

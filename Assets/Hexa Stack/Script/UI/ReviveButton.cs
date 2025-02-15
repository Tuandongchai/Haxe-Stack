using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveButton : MonoBehaviour
{
    private Button button;
    public static Action onClicked;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => onClicked?.Invoke());
    }
}

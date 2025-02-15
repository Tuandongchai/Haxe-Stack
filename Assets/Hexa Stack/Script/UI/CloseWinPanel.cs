using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CloseWinPanel : MonoBehaviour
{
    public Button button;
    public static Action onClicked;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(()=>onClicked?.Invoke());
    }
}

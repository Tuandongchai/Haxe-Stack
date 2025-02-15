using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour
{
    private Button button;
    public static Action clicked;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(()=>clicked?.Invoke());
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BuildButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Button button;
    private bool isPressed;
    [Header("Action")]
    public static Action onHoldStart;
/*    public static Action onHoldEnd;*/

    private void Awake()
    {
        button = GetComponent<Button>();

        
    }
    private void Update()
    {
        if (isPressed)
        {
            onHoldStart?.Invoke();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        /*onHoldEnd?.Invoke();*/
    }
}

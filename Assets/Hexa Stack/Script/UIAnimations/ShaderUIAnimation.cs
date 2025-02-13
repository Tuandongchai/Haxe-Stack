using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderUIAnimation : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject nameCity;
    [SerializeField] private GameObject percentGift;
    [SerializeField] private GameObject leftNext;
    [SerializeField] private GameObject rightNext;
    [SerializeField] private GameObject buildingMasterials;
    [SerializeField] private GameObject buildButton;

    private GameObject[] buttons;

    private void Start()
    {
        buttons = new GameObject[]
        {
            closeButton, nameCity, percentGift, leftNext, rightNext, buildingMasterials, buildButton
        };
    }
    public void LoadIn()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].transform.localScale = Vector3.zero;
        }

        for (int i =0; i< buttons.Length; i++)
        {
            
            if (buttons[i] == leftNext)
                LeanTween.scale(buttons[i], new Vector3(1, -1, 1), 0.1f + i * 0.2f).setEase(LeanTweenType.easeOutBounce);
            else
                LeanTween.scale(buttons[i], new Vector3(1, 1, 1), 0.1f + i * 0.2f).setEase(LeanTweenType.easeOutBounce);
        }
    }
    public void LoadOut()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            LeanTween.scale(buttons[i], Vector3.zero, 0.1f + i * 0.2f).setEase(LeanTweenType.easeOutBounce);
        }
    }

    
}

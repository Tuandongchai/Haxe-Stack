/*using Microsoft.Unity.VisualStudio.Editor;*/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ShaderUIAnimation : BaseUI
{
    [Header("Element")]
    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject nameCity;
    [SerializeField] private GameObject percentGift;
    [SerializeField] private GameObject leftNext;
    [SerializeField] private GameObject rightNext;
    [SerializeField] private GameObject buildingMasterials;
    [SerializeField] private GameObject buildButton;

    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private UnityEngine.UI.Image percentFill;
    [SerializeField] private TextMeshProUGUI sunsText;
    private float filldrawed=0;

    private GameObject[] buttons;

    private void Start()
    {
        GameData fillElement = GameData.instance;
        Show(fillElement.GetFill(fillElement.GetObjectFill()), fillElement.GetObjectFill());
        buttons = new GameObject[]
        {
            closeButton, nameCity, percentGift, leftNext, rightNext, buildingMasterials, buildButton
        };
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].transform.localScale = Vector3.zero;
        }
    }
    public override void Show()
    {
        base.Show();
    }
    public void Show(float x, int count)
    {
        sunsText.text = ((int)StatsManager.Instance.GetCurrentSuns()).ToString();
        switch (count) {
            case 0:
                filldrawed = x;
                break;
            case 1:
                filldrawed = 1 + x;
                break;
            case 2:
                filldrawed = 2 + x;
                break;
            case 3:
                filldrawed = 3 + x;
                break;
            default: break;
        }
        percentText.text = ((int)(filldrawed / (4f) * 100)) + "%";
        percentFill.fillAmount = ((4f - filldrawed) / 4f);
        if (percentText.text == "100%")
        {
            Dictionary<string, int> a = new Dictionary<string, int>() { { "golds", 10000 } };
            RewardPopup.instance.ShowReward(a);
        }
    }
    public override void LoadIn()
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
    public override void LoadOut()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            LeanTween.scale(buttons[i], Vector3.zero, 0.1f + i * 0.2f).setEase(LeanTweenType.easeOutBounce);
        }
    }

    
}

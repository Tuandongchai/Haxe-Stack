using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileUI : BaseUI
{
    [Header("Elements")]
    [SerializeField] private GameObject profilePanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI hammerQuantity;
    [SerializeField] private TextMeshProUGUI swapQuantity;
    [SerializeField] private TextMeshProUGUI rollQuantity;


    public override void Show()
    {
        base.Show();
        levelText.text = "Level "+base.currentLevel;
        hammerQuantity.text = base.hammerCount.ToString();
        swapQuantity.text = base.swapCount.ToString();
        rollQuantity.text = base.rollCount.ToString();
    }

    public override void LoadIn()
    {
        if (profilePanel.active == true)
            return;
        profilePanel.SetActive(true);
        Show();
        profilePanel.transform.localScale = Vector3.zero;
        profilePanel.transform.localPosition = Vector3.zero;

        LeanTween.scale(profilePanel, new Vector3(1, 1, 1), 0.4f).setEase(LeanTweenType.easeOutBounce);
        LeanTween.moveLocal(profilePanel, new Vector3(399.017944f, -910.814087f, 0), 0.4f).setEase(LeanTweenType.easeInBack);
    }
    public override void LoadOut()
    {
        
        profilePanel.transform.localScale = Vector3.one;
        profilePanel.transform.localPosition = new Vector3(399.017944f, -910.814087f, 0);

        LeanTween.scale(profilePanel, new Vector3(0, 0, 0), 0.4f).setEase(LeanTweenType.easeOutBounce);
        LeanTween.moveLocal(profilePanel, new Vector3(0,0,0), 0.4f).setEase(LeanTweenType.easeInBack).setOnComplete(()=> profilePanel.SetActive(false));
    }
}

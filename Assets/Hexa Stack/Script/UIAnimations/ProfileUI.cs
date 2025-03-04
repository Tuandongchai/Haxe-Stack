using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject profilePanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI hammerQuantity;
    [SerializeField] private TextMeshProUGUI swapQuantity;
    [SerializeField] private TextMeshProUGUI rollQuantity;


    private void Show()
    {
        levelText.text = "Level "+StatsManager.Instance.GetCurrentLevel();
        hammerQuantity.text = StatsManager.Instance.GetTool(0).ToString();
        swapQuantity.text = StatsManager.Instance.GetTool(1).ToString();
        rollQuantity.text = StatsManager.Instance.GetTool(2).ToString();
    }

    public void LoadIn()
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
    public void LoadOut()
    {
        
        profilePanel.transform.localScale = Vector3.one;
        profilePanel.transform.localPosition = new Vector3(399.017944f, -910.814087f, 0);

        LeanTween.scale(profilePanel, new Vector3(0, 0, 0), 0.4f).setEase(LeanTweenType.easeOutBounce);
        LeanTween.moveLocal(profilePanel, new Vector3(0,0,0), 0.4f).setEase(LeanTweenType.easeInBack).setOnComplete(()=> profilePanel.SetActive(false));
    }
}

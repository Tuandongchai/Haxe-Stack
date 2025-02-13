using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Loading;
using UnityEngine;

public class LoseUIAnimation : MonoBehaviour
{
    [Header("Element")]
    public GameObject losePanels;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI levelText;

    public void LoadIn()
    {
        goldText.text = ""+StatsManager.Instance.GetCurrentGolds();
        levelText.text ="Level "+ StatsManager.Instance.GetCurrentLevel();
        losePanels.transform.localPosition= new Vector3 (0, -2191, 0);

        LeanTween.moveLocalY(losePanels, 0, 1f).setEase(LeanTweenType.easeInBack);
    }


}

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIAnimation : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject tool;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start()
    {
        Show();
    }
    private void Show()
    {
        levelText.text = "Level " + StatsManager.Instance.GetCurrentLevel();
        goldText.text = StatsManager.Instance.GetCurrentGolds().ToString();
    }
    
    public void UseTool()
    {
        UI.transform.localPosition = new Vector3(89.539093f, 1053.95886f, 0);
        LeanTween.moveLocalY(UI, 1700, 0.4f).setEase(LeanTweenType.easeInBack);

        tool.transform.localPosition = new Vector3(0, -968.9454f, 0);
        LeanTween.moveLocalY(tool, -1900, 0.4f).setEase(LeanTweenType.easeInBack);
    }
    public void NoUseTool()
    {
        UI.transform.localPosition = new Vector3(89.539093f, 1700, 0);
        LeanTween.moveLocalY(UI, 1054, 0.4f).setEase(LeanTweenType.easeInBack);

        tool.transform.localPosition = new Vector3(0, -1900, 0);
        LeanTween.moveLocalY(tool, -968.9454f, 0.4f).setEase(LeanTweenType.easeInBack);
    }
}

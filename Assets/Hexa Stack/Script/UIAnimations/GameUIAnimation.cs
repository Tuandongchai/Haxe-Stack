using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIAnimation : MonoBehaviour
{
    [Header("Element")]
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
}

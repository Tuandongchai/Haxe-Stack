using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIAnimation : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        levelText.text = "level " + (1+(PlayerPrefs.GetInt("Level"))).ToString();
    }
}

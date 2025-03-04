using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private bool isUnlocked;
    [SerializeField] private Image locked;
    private int level;

    public static Action<int> onClicked;
    private void Start()
    {
        
        gameObject.GetComponent<Button>().onClick.AddListener(()=>onClicked?.Invoke(GetLevel()));
    }
    public void IsUnlocked(int lv)
    {
        if(lv>StatsManager.Instance.GetCurrentLevel())
            isUnlocked = true;
        else
            isUnlocked = false;

        locked.gameObject.SetActive(isUnlocked);
    }
    public int GetLevel() => int.Parse(gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
}

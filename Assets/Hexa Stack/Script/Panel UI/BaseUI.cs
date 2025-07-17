using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUI:MonoBehaviour
{
    protected int currentLevel, currentGold, currentSun,hammerCount,currentHeart, swapCount,rollCount;
    

    public abstract void LoadIn();
    public abstract void LoadOut();
    public virtual void Show() { 
        currentLevel = StatsManager.Instance.GetCurrentLevel();
        currentGold = StatsManager.Instance.GetCurrentGolds();
        hammerCount = StatsManager.Instance.GetTool(0);
        swapCount = StatsManager.Instance.GetTool(1);
        rollCount = StatsManager.Instance.GetTool(2);
        currentSun = (int)StatsManager.Instance.GetCurrentSuns();
        currentHeart = StatsManager.Instance.GetCurrentHearts();
    }
}

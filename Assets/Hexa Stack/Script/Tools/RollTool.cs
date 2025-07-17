using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RollTool : BaseTool
{
    [SerializeField] private GameObject stackSpawner;

    private void Start()
    {
        RollButton.onClicked += UseTool;
    }
    private void OnDisable()
    {
        RollButton.onClicked -= UseTool;
        
    }
    public override void UseTool()
    {
        goldsCost = 500;
        toolIndex = 2;
        base.UseTool();
    }
    protected override int CheckToolCount(int toolIndex)
    {
        return StatsManager.Instance.GetTool(toolIndex);
    }

    protected override void ExecuteTool(int toolIndex)
    {
        //them if
        if (gameObject != null)
        {
            for (int i = stackSpawner.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = stackSpawner.transform.GetChild(i);

                if (child.childCount > 0)
                {
                    Destroy(child.GetChild(0).gameObject);
                }
            }
            StatsManager.Instance.UseTool(toolIndex);

            AudioManager.instance.PlaySoundEffect(11);
            
        }
    }

    protected override void UpdateToolCountUI(int toolIndex)
    {
        if (StatsManager.Instance.GetCurrentGolds() < 500)
            return;
        StatsManager.Instance.UseGold(500);
        StatsManager.Instance.IncreasedTool(2, 1);
        GameManager.instance.gameUIAnimation.Show();

        AudioManager.instance.PlaySoundEffect(10);
    }

    
}

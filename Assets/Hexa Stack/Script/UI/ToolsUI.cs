using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolsUI : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private TextMeshProUGUI hammerText;
    [SerializeField] private TextMeshProUGUI lightningText;
    [SerializeField] private TextMeshProUGUI rollText;
    private TextMeshProUGUI[] textArray;

    private void Start()
    {
        textArray = new TextMeshProUGUI[]{hammerText, lightningText, rollText};
        Show();
    }
    
    private void OnDestroy()
    {
        
    }
    public void Show()
    {
        for (int i =0; i<textArray.Length; i++)
        {
            if (StatsManager.Instance.GetTool(i) <= 0)
                textArray[i].text = "+";
            else
                textArray[i].text = StatsManager.Instance.GetTool(i).ToString();
        }
        /*hammerText.text = StatsManager.Instance.GetTool(0).ToString();
        lightningText.text = StatsManager.Instance.GetTool(1).ToString();
        rollText.text = StatsManager.Instance.GetTool(2).ToString();*/
    }
}

using System.Collections;
using System.Collections.Generic;
/*using UnityEditor.Tilemaps;*/
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance;

    public RollTool rollTool;
    public SwapTool swapTool;
    public HammerTool hammerTool;
    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }
    
}

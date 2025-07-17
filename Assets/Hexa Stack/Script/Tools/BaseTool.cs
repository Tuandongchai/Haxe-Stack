using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTool : MonoBehaviour
{
    [SerializeField] protected GameObject grid;
    [SerializeField] protected GameObject hexSpawner;
    [SerializeField] protected LayerMask hexagonLayerMask;

    protected List<GameObject> listObInGrid;
    protected List<GameObject> listObInSpawner;

    protected int goldsCost;
    protected int toolIndex;

    [SerializeField] protected ToolsUI ShowToolUI;

    public virtual void UseTool()
    {
        
        if (CheckToolCount(toolIndex) <= 0)
        {
            UpdateToolCountUI(toolIndex);
        }
        else
            ExecuteTool(toolIndex);
        ShowToolUI.Show();
    }
    protected abstract int CheckToolCount(int toolIndex);
    protected abstract void UpdateToolCountUI(int toolIndex);
    protected abstract void ExecuteTool(int toolIndex);
}

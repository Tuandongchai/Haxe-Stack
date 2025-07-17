using System.Collections;
using System.Collections.Generic;
/*using UnityEditor.Tilemaps;*/
using UnityEngine;

public class SwapTool : BaseTool
{

    public bool moveTool = false;

    private void Start()
    {
        MoveButton.onClicked += UseTool;
    }
    private void OnDisable()
    {
        MoveButton.onClicked -= UseTool;
    }
    public override void UseTool()
    {
        base.UseTool();
        toolIndex = 1; 
        goldsCost = 1000;
    }
    protected override int CheckToolCount(int toolIndex)
    {
        return StatsManager.Instance.GetTool(1);
    }

    protected override void ExecuteTool(int toolIndex)
    {
        moveTool = true;

        GameManager.instance.gameUIAnimation.UseTool(1);

        listObInGrid = new List<GameObject> { };
        AbleCollider(grid, listObInGrid);

        listObInSpawner = new List<GameObject> { };
        DisableCollider(hexSpawner, listObInSpawner);

        StatsManager.Instance.UseTool(1);
    }

    protected override void UpdateToolCountUI(int toolIndex)
    {
        if (StatsManager.Instance.GetCurrentGolds() < goldsCost)
            return;
        StatsManager.Instance.UseGold(goldsCost);
        StatsManager.Instance.IncreasedTool(toolIndex, 1);
        GameManager.instance.gameUIAnimation.Show();

        AudioManager.instance.PlaySoundEffect(10);
    }
    private void AbleCollider(GameObject parent, List<GameObject> list)
    {
        GetAllComponentHasHexagon(parent, list);
        foreach (var hexa in list)
        {
            hexa.GetComponent<Collider>().enabled = true;
        }
    }
    private void DisableCollider(GameObject parent, List<GameObject> list)
    {

        GetAllComponentHasHexagon(parent, list);
        foreach (var hexa in list)
        {
            hexa.GetComponent<Collider>().enabled = false;
        }
    }
    private void GetAllComponentHasHexagon(GameObject parent, List<GameObject> list)
    {
        //if check parent null
        if (parent != null && parent.gameObject != null)
        {
            foreach (Transform child in parent.transform)
            {
                if (child.GetComponent<Hexagon>() != null)
                    list.Add(child.gameObject);
                GetAllComponentHasHexagon(child.gameObject, list);
            }
        }
        
    }
    private void ManageMouseDown()
    {
        //giu hexagon khi di chuot vao
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, hexagonLayerMask);

        if (hit.collider == null)
        {
            Debug.Log("not detected any hexagon");
            return;
        }
        Destroy(hit.collider.transform.parent?.gameObject);

        listObInGrid = new List<GameObject> { };
        DisableCollider(grid, listObInGrid);

        listObInSpawner = new List<GameObject> { };
        AbleCollider(hexSpawner, listObInSpawner);

        GameManager.instance.gameUIAnimation.NoUseTool(0);
        ToolManager.Instance.hammerTool.useTool = false;
    }
    private Ray GetClickedRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
    public void EndTool()
    {
        moveTool = false;

        listObInGrid = new List<GameObject> { };
        DisableCollider(grid, listObInGrid);

        listObInSpawner = new List<GameObject> { };
        AbleCollider(hexSpawner, listObInSpawner);

        GameManager.instance.gameUIAnimation.NoUseTool(1);

        AudioManager.instance.PlaySoundEffect(11);
    }
}

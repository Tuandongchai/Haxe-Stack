using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerTool : BaseTool
{
    
    public bool useTool = false;


    private void Start()
    {
        HammerButton.onClicked += UseTool;
    }
    private void nDisable()
    {
        
        HammerButton.onClicked -= UseTool;
    }
    private void Update()
    {
        if (!useTool)
            return;
        if (Input.GetMouseButtonDown(0))
            ManageMouseDown();
    }
    public override void UseTool()
    {
        goldsCost = 1000;
        toolIndex = 0;
        base.UseTool();
    }
    
    protected override int CheckToolCount(int toolIndex)
    {
        return StatsManager.Instance.GetTool(toolIndex);
    }

    protected override void ExecuteTool(int toolIndex)
    {
        useTool = true;

        GameManager.instance.gameUIAnimation.UseTool(toolIndex);

        listObInGrid = new List<GameObject> { };
        AbleCollider(grid, listObInGrid);

        listObInSpawner = new List<GameObject> { };
        DisableCollider(hexSpawner, listObInSpawner);

        StatsManager.Instance.UseTool(toolIndex);
        AudioManager.instance.PlaySoundEffect(11);
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
        //
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
        useTool = false;
    }
    private Ray GetClickedRay() => Camera.main.ScreenPointToRay(Input.mousePosition);

}

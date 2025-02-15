using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;

public class ToolsManager : MonoBehaviour
{
    public static ToolsManager Instance;

    [Header("Elements")]
    [SerializeField] private GameObject stackSpawner;
    [SerializeField] private ToolsUI toolUI;
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject hexSpawner;
    [SerializeField] private LayerMask hexagonLayerMask;

    public bool useTool=false;
    public bool moveTool=false;
    private List<GameObject> listObInGrid;
    private List<GameObject> listObInSpawner;

    /*   [Header("Actions")]*/
    /*    public static Action showAcion;*/
    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        moveTool = false;
        RollTool.clicked += RollTools;
        HammerButton.onClicked += HammerTool;
        MoveButton.clicked += SwapTool;
    }
    private void OnDestroy()
    {
        RollTool.clicked -= RollTools;
        HammerButton.onClicked -= HammerTool;
        MoveButton.clicked -= SwapTool;
    }
    private void Update()
    {
        if (!useTool)
            return;
        if(Input.GetMouseButtonDown(0))
            ManageMouseDown();
        if (!moveTool)
        {

        }

    }
    private void RollTools()
    {
        int currentRolls = StatsManager.Instance.GetTool(2);
        if (currentRolls <= 0)
        {
            if (StatsManager.Instance.GetCurrentGolds() < 500)
                return;
            StatsManager.Instance.UseGold(500);
            StatsManager.Instance.IncreasedTool(2);
            GameManager.instance.gameUIAnimation.Show();

            AudioManager.instance.PlaySoundEffect(10);

        }
        else
        {
            for (int i = stackSpawner.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = stackSpawner.transform.GetChild(i);

                if (child.childCount > 0)
                {
                    Destroy(child.GetChild(0).gameObject);
                }
            }
            StatsManager.Instance.UseTool(2);

            AudioManager.instance.PlaySoundEffect(11);

        }
        toolUI.Show();
        
    }
    //Hammer Tool
    private void HammerTool()
    {
        int currentHammers = StatsManager.Instance.GetTool(0);
        if (currentHammers <= 0)
        {
            if (StatsManager.Instance.GetCurrentGolds() < 1000)
                return;
            StatsManager.Instance.UseGold(1000);
            StatsManager.Instance.IncreasedTool(0);
            GameManager.instance.gameUIAnimation.Show();

            AudioManager.instance.PlaySoundEffect(10);
        }
        else
        {

            useTool = true;

            GameManager.instance.gameUIAnimation.UseTool(0);

            listObInGrid = new List<GameObject> { };
            AbleCollider(grid, listObInGrid);

            listObInSpawner = new List<GameObject> { };
            DisableCollider(hexSpawner, listObInSpawner);

            StatsManager.Instance.UseTool(0);
            AudioManager.instance.PlaySoundEffect(11);

        }
        toolUI.Show();

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
        foreach (Transform child in parent.transform)
        {
            if (child.GetComponent<Hexagon>() !=null)
                list.Add(child.gameObject);
            GetAllComponentHasHexagon(child.gameObject, list);
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

    //SwapTool
    private void SwapTool()
    {
        int currentMoves = StatsManager.Instance.GetTool(1);
        if (currentMoves <= 0)
        {
            if (StatsManager.Instance.GetCurrentGolds() < 1000)
                return;
            StatsManager.Instance.UseGold(1000);
            StatsManager.Instance.IncreasedTool(1);
            GameManager.instance.gameUIAnimation.Show();

            AudioManager.instance.PlaySoundEffect(10);
        }
        else
        {

            moveTool = true;

            GameManager.instance.gameUIAnimation.UseTool(1);

            listObInGrid = new List<GameObject> { };
            AbleCollider(grid, listObInGrid);

            listObInSpawner = new List<GameObject> { };
            DisableCollider(hexSpawner, listObInSpawner);

            StatsManager.Instance.UseTool(1);

        }
        toolUI.Show();
        
    }
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

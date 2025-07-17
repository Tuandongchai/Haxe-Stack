using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngineInternal;

public class StackSpawner : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform stackPositionsParent;
    [SerializeField] private Hexagon hexagonPrefab;
    [SerializeField] private HexStack hexagonStackPrefab;

    [Header(" Settings ")]
    [NaughtyAttributes.MinMaxSlider(2,8)]
    [SerializeField] private Vector2Int minMaxhexCount;
    [SerializeField] private Color[] colors;
    private int stackCounter;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        StackController.onStackPlaced += StackPlacedCallback;
        RollButton.onClicked += RollStacks;
    }
    private void OnDestroy()
    {
        StackController.onStackPlaced -= StackPlacedCallback;
        RollButton.onClicked -= RollStacks;
    }

    
    private void StackPlacedCallback(GridCell cell)
    {
        /*if (ToolsManager.Instance.moveTool == true || ToolsManager.Instance.useTool == true)
            return;*/
        if (ToolManager.Instance.swapTool.moveTool == true || ToolManager.Instance.hammerTool.useTool == true)
            return;
        stackCounter++;
        if(stackCounter >= 3)
        {
            stackCounter = 0;
            GenerateStacks();
        }
    }

    private void Start()
    {
        GenerateStacks();
    }


    private void GenerateStacks()
    {
        
        for(int i =0; i<stackPositionsParent.childCount; i++)
            //vi tri con 
            GenerateStack(stackPositionsParent.GetChild(i));
    }
    private void RollStacks()
    {
        if (StatsManager.Instance.GetTool(2) <= 0)
            return;

        for (int i = 0; i < stackPositionsParent.childCount; i++)
            //vi tri con 
            GenerateStack(stackPositionsParent.GetChild(i));
    }

    private void GenerateStack(Transform parent)
    {
        //sinh ra hexagonStackPrefab
        HexStack hexStack = Instantiate(hexagonStackPrefab, parent.position, parent.localRotation, parent);
        hexStack.name = $"Stack {parent.GetSiblingIndex()}";

        int amount = Random.Range(minMaxhexCount.x,minMaxhexCount.y);

        int firstColorHexagonCount = Random.Range(0, amount);

        Color[] colorArray = GetRandomColors();
        //sinh ra hexagonPrefab
        for(int i =0; i<amount; i++)
        {
            Vector3 hexagonLocalPos = Vector3.up * i * .2f;
            Vector3 spawnPosition = hexStack.transform.TransformPoint(hexagonLocalPos);

            Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPosition, parent.localRotation, hexStack.transform);
            hexagonInstance.Color = i<firstColorHexagonCount ? colorArray[0]: colorArray[1];

            hexagonInstance.Configure(hexStack);

            hexStack.Add(hexagonInstance);
        }
    }
    private Color[] GetRandomColors()
    {
        List<Color> colorList = new List<Color>();
        colorList.AddRange(colors);
        if(colorList.Count < 0)
        {
            Debug.LogError("Not Found Color");
            return null;
        }
        Color firstColor = colorList.OrderBy(x=> Random.value).First();
        colorList.Remove(firstColor);

        if (colorList.Count <= 0)
        {
            Debug.LogError("Only one color was found");
            return null;
        }
        Color secondColor = colorList.OrderBy(x => Random.value).First();
        return new Color[] {firstColor, secondColor };

    }
}

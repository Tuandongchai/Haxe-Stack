using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;

public class GridCell : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Hexagon hexagonPrefab;
    [SerializeField] private GameObject quantityPrefab;


    [Header("Setting")]
    [OnValueChanged("GenerateInitialHexagons")]
    [SerializeField] private Color[] hexagonColors;

    public GameObject quantity{get; private set;}
    public HexStack Stack { get; private set; }
    public bool IsOccupied
    {
        get => Stack != null;
        private set { }
    }

    private void Start()
    {
        if(hexagonColors.Length > 0) 
            GenerateInitialHexagons();

        /*if(transform.childCount > 1)
        {
            Stack = transform.GetChild(1).GetComponent<HexStack>();
            Stack.Initialize();
        }*/
    }
    public void AssignStack(HexStack stack)
    {
        Stack = stack;
    }
    private void GenerateInitialHexagons()
    {
        while(transform.childCount > 1)
        {
            Transform t = transform.GetChild(1);
            t.SetParent(null);
            DestroyImmediate(t.gameObject);
        }

        Stack = new GameObject("Initial Stack").AddComponent<HexStack>();   
        Stack.transform.SetParent(transform);
        Stack.transform.localPosition = Vector3.up* .2f;

        for(int i=0; i< hexagonColors.Length; i++)
        {
            Vector3 spawnPosition = Stack.transform.TransformPoint(Vector3.up * i* 0.2f);

            Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPosition, Quaternion.identity);
            hexagonInstance.Color = hexagonColors[i];
            hexagonInstance.GetComponent<MeshCollider>().enabled=false;
            Stack.Add(hexagonInstance);

        }
        /*Vector3 positionText = Stack.transform.TransformPoint(0, 0.2f*(hexagonColors.Length - 1) + 0.15f,0);
        quantity = Instantiate(quantityPrefab, positionText, Quaternion.identity);
        TextMeshPro quantityText = quantity.GetComponent<TextMeshPro>();
        quantityText.rectTransform.Rotate(90, 0, 0);
        quantityText.text = (hexagonColors.Length).ToString();*/

    }
}

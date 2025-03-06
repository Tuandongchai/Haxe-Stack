using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    [Header("Elements")]
    private List<GridCell> updateCells = new List<GridCell>();
    
    private void Awake()
    {

        StackController.onStackPlaced += StackPlacedCallback;
    }
    private void OnDestroy()
    {
        StackController.onStackPlaced -= StackPlacedCallback;
        
    }

    private void StackPlacedCallback(GridCell gridCell)
    {
        StartCoroutine((StackPlacedCoroutine(gridCell)));

    }
    IEnumerator StackPlacedCoroutine(GridCell gridCell)
    {
        updateCells.Add(gridCell);

        while(updateCells.Count > 0) 
            yield return CheckForMerge(updateCells[0]);
    }
    IEnumerator CheckForMerge(GridCell gridCell)
    {
        updateCells.Remove(gridCell);


        if(!gridCell.IsOccupied)
            yield break;
        //Does this cell has neighbors ?

       
        List<GridCell> neighborGridCells = GetNeighborGridCells(gridCell); 

        if(neighborGridCells.Count <= 0)
        {
            yield break;
        }
        
        // At this point, we have a list of the neighbor grid cells, that are occupied
        Color gridCellTopHexagonColor = gridCell.Stack.GetTopHexagonColor();

        //Do there neighbors have the same top hex color ?

        
        List<GridCell> similarNeighborGridCells = GetSimilarNeighborGridCells(gridCellTopHexagonColor, neighborGridCells.ToArray());

        
        if (similarNeighborGridCells.Count <= 0)
        {
            yield break;
            
        }
        // them cac o co mau dinh cung mau vao updateCells
        updateCells.AddRange(similarNeighborGridCells);

        //At this point we have a list of similar neighbors
        List<Hexagon> hexagonsToAdd = GetHexagonsToAdd(gridCellTopHexagonColor, similarNeighborGridCells.ToArray());


        //Remove the hexagons from their stacks
        RemoveHexagonsFromStacks(similarNeighborGridCells.ToArray(), hexagonsToAdd);

        // At this point we have removed the stacks we don't need anymore
        //we have some free grid cell

        //
        

        //
        MoveHexagons(gridCell, hexagonsToAdd);
        /*Wait to move*/
        yield return new WaitForSeconds(0.2f + (hexagonsToAdd.Count + 1) * 0.06f); //0.04

        /*yield return new WaitForSeconds(0.2f + (hexagonsToAdd.Count + 1) * 0.1f);*/
        // Is the stack on this cell complete
        // Does it have 10 or more similar hexagons ?

        yield return CheckForCompleteStack(gridCell, gridCellTopHexagonColor);

    }
    // tra ve cac o lien ke
    private List<GridCell> GetNeighborGridCells(GridCell gridCell)
    {
        LayerMask gridCellMask = 1 << gridCell.gameObject.layer;

        List<GridCell> neighborGridCells = new List<GridCell>();

        Collider[] neighborGridCellColliders = Physics.OverlapSphere(gridCell.transform.position, 2, gridCellMask);

        //At this point, we have the grid cell collider neighbor
        foreach (Collider gridCellCollider in neighborGridCellColliders)
        {
            GridCell neighborGridCell = gridCellCollider.GetComponent<GridCell>();
            if (!neighborGridCell.IsOccupied)
                continue;
            if (neighborGridCell == gridCell)
                continue;
            neighborGridCells.Add(neighborGridCell);
            
        }
        
        return neighborGridCells;
    }

    // tra ve cac o lien ke cung mau
    private List<GridCell> GetSimilarNeighborGridCells(Color gridCellTopHexagonColor, GridCell[] neighborGridCells)
    {
        
        List<GridCell> similarNeighborGridCells= new List<GridCell>();

        foreach (GridCell neighborGridCell in neighborGridCells)
        {
            Color neighborGridCellTopHexagonColor = neighborGridCell.Stack.GetTopHexagonColor();

            if (gridCellTopHexagonColor == neighborGridCellTopHexagonColor)
            {
                similarNeighborGridCells.Add(neighborGridCell);
            }
        }
        
        return similarNeighborGridCells;
    }
    private List<Hexagon> GetHexagonsToAdd(Color gridCellTopHexagonColor, GridCell[] similarNeighborGridCells)
    {
        List<Hexagon> hexagonsToAdd = new List<Hexagon>();

        foreach (GridCell neighborCell in similarNeighborGridCells)
        {
            HexStack neighborCellHexStack = neighborCell.Stack;
            for (int i = neighborCellHexStack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = neighborCellHexStack.Hexagons[i];

                if (hexagon.Color != gridCellTopHexagonColor)
                    break;

                hexagonsToAdd.Add(hexagon);
                hexagon.SetParent(null);
            }
        }
        return hexagonsToAdd;
    }
    private void RemoveHexagonsFromStacks(GridCell[] similarNeighborGridCells, List<Hexagon> hexagonsToAdd )
    {
        foreach (GridCell neighborCell in similarNeighborGridCells)
        {
            HexStack stack = neighborCell.Stack;
            foreach (Hexagon hexagon in hexagonsToAdd)
            {
                if (stack.Contains(hexagon))
                    stack.Remove(hexagon);
                
            }
        }
        
    }
    private void MoveHexagons(GridCell gridCell, List<Hexagon> hexagonsToAdd)
    {
        float initialY = gridCell.Stack.Hexagons.Count * .2f;
        for (int i = 0; i < hexagonsToAdd.Count; i++)
        {
            Hexagon hexagon = hexagonsToAdd[i];
            
            float targetY = initialY + i * .2f;
            Vector3 targetLocalPosition = Vector3.up * targetY;

            gridCell.Stack.Add(hexagon);

            hexagon.MoveToLocal(targetLocalPosition);
 
        }
    }
    private IEnumerator CheckForCompleteStack(GridCell gridCell, Color topColor)
    {
        if (gridCell.Stack.Hexagons.Count < 10)
            yield break;

        List<Hexagon> similarHexagons = new List<Hexagon>();
        for(int i = gridCell.Stack.Hexagons.Count -1; i >= 0; i--)
        {
            Hexagon hexagon = gridCell.Stack.Hexagons[i];

            if (hexagon.Color != topColor)
                break;
            similarHexagons.Add(hexagon);
        }
        int similarHexagonCount = similarHexagons.Count;

        if (similarHexagons.Count < 10)
            yield break;

        float delay = 0;

        while(similarHexagons.Count > 0)
        {
            similarHexagons[0].SetParent(null);
            similarHexagons[0].Vanish(delay);

            //DestroyImmediate(similarHexagons[0].gameObject);

            delay += 0.05f;
            
            gridCell.Stack.Remove(similarHexagons[0]);
            similarHexagons.RemoveAt(0);


            LevelManager.Instance.piecesCount += 1;
            LevelManager.Instance.UpdateFill();
        }
        /*yield return new WaitForSeconds(0.2f + (similarHexagonCount + 1) * 0.1f);*/
        yield return new WaitForSeconds(0.2f + (similarHexagonCount + 1) * 0.06f);
        updateCells.Add(gridCell);

    }
}

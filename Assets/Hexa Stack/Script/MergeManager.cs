using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
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
        //Does this cell has neighbors ?


        List<GridCell> neighborGridCells = GetNeighborGridCells(gridCell); 

        if(neighborGridCells.Count <= 0)
        {
            return;
        }
        // At this point, we have a list of the neighbor grid cells, that are occupied
        Color gridCellTopHexagonColor = gridCell.Stack.GetTopHexagonColor();

        //Do there neighbors have the same top hex color ?
        List<GridCell> similarNeighborGridCells = GetSimilarNeighborGridCells(gridCellTopHexagonColor, neighborGridCells.ToArray());
        if(similarNeighborGridCells.Count <= 0) { return; }

        //At this point we have a list of similar neighbors
        List<Hexagon> hexagonsToAdd = GetHexagonsToAdd(gridCellTopHexagonColor, similarNeighborGridCells.ToArray());

        //Remove the hexagons from their stacks
        RemoveHexagonsFromStacks(similarNeighborGridCells.ToArray(), hexagonsToAdd);

        // At this point we have removed the stacks we don't need anymore
        //we have some free grid cell

        MoveHexagons(gridCell, hexagonsToAdd);

        // Is the stack on this cell complete
        // Does it have 10 or more similar hexagons ?
        CheckForCompleteStack(gridCell, gridCellTopHexagonColor);
    }


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
            hexagon.transform.localPosition = targetLocalPosition;
        }
    }
    private void CheckForCompleteStack(GridCell gridCell, Color topColor)
    {
        if (gridCell.Stack.Hexagons.Count < 10)
            return;

        List<Hexagon> similarHexagons = new List<Hexagon>();
        for(int i = gridCell.Stack.Hexagons.Count -1; i >= 0; i--)
        {
            Hexagon hexagon = gridCell.Stack.Hexagons[i];

            if (hexagon.Color != topColor)
                break;
            similarHexagons.Add(hexagon);
        }

        if (similarHexagons.Count < 10)
            return;
        while(similarHexagons.Count > 0)
        {
            similarHexagons[0].SetParent(null);
            gridCell.Stack.Remove(similarHexagons[0]);

            DestroyImmediate(similarHexagons[0].gameObject);
            similarHexagons.RemoveAt(0);
        }
    }
}

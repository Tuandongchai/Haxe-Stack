using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [Header(" Settings ")]
    [SerializeField] private LayerMask hexagonLayerMask;
    [SerializeField] private LayerMask gridHexagonLayerMask;
    [SerializeField] private LayerMask groundLayerMask;
    private HexStack currentStack;
    private Vector3 currentStackInitialPos;

    [Header("Data")]
    private GridCell targetCell;

    [Header("Actions")]
    public static Action<GridCell> onStackPlaced;
    private void Update()
    {
        ManageControl();
    }
    private void ManageControl()
    {
        if (Input.GetMouseButtonDown(0))
            ManageMouseDown();
        else if (Input.GetMouseButton(0) && currentStack != null)
            ManageMouseDrag();
        else if (Input.GetMouseButtonUp(0) && currentStack != null)
            ManageMouseUp();
    }
    private void ManageMouseDown()
    {
        //giu hexagon khi di chuot vao
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, hexagonLayerMask);

        if (hit.collider == null)
        {
            Debug.Log("We have not detected any hexagon");
            return;
        }
        currentStack = hit.collider.GetComponent<Hexagon>().HexStack;
        currentStackInitialPos = currentStack.transform.position;
    }
    private void ManageMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, gridHexagonLayerMask);

        if (hit.collider == null)
            DraggingAboveGround();
        else
            DraggeingAboveGridCell(hit);
    }

    


    private void DraggingAboveGround()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, groundLayerMask);

        if(hit.collider == null)
        {
            Debug.LogError("No ground detected");
            return;
        }
        Vector3 currentStackTargetPos = hit.point.With(y:2);
        currentStack.transform.position = Vector3.MoveTowards(currentStack.transform.position, 
            currentStackTargetPos, 
            Time.deltaTime *30);

        targetCell = null;
    }

    private void DraggeingAboveGridCell(RaycastHit hit)
    {
        GridCell gridCell = hit.collider.GetComponent<GridCell>();

        if (gridCell.IsOccupied)
            DraggingAboveGround();
        else
            DraggingAboveNonOccupiedGridCell(gridCell);

        targetCell = gridCell;
    }

    private void DraggingAboveNonOccupiedGridCell(GridCell gridCell)
    {
        Vector3 currentStackTargetPos = gridCell.transform.position.With(y:2);

        currentStack.transform.position = Vector3.MoveTowards(currentStack.transform.position,
            currentStackTargetPos,
            Time.deltaTime * 30);
    }
    private void ManageMouseUp()
    {
        if (targetCell == null)
        {
            currentStack.transform.position = currentStackInitialPos;
            currentStack = null;
            return;
        }
        currentStack.transform.position = targetCell.transform.position.With(y: 0.2f);
        currentStack.transform.SetParent(targetCell.transform);
        currentStack.Place();

        targetCell.AssignStack(currentStack);

        onStackPlaced?.Invoke(targetCell);

        targetCell = null;
        currentStack = null;
    }
    private Ray GetClickedRay()=> Camera.main.ScreenPointToRay(Input.mousePosition);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class GridRotate : MonoBehaviour
{
    [SerializeField] private LayerMask hexagonLayerMask;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform[] target = new Transform[] { };
    private Vector3 lastMousePos;
    private bool canRotation;

    
    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500, hexagonLayerMask);

            if (hit.collider != null)
            {
                Debug.Log("We have detected any hexagon");
                canRotation = false;
                return;
            }
            lastMousePos = Input.mousePosition;
            canRotation = true;

        }
        if (!canRotation)
            return;
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            float dis=0;
            if (delta.x + delta.y < 0)
                dis = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(delta.x, 2) + Mathf.Pow(delta.y, 2)));
            else if (delta.x + delta.y > 0)
                dis =-1* Mathf.Abs(Mathf.Sqrt(Mathf.Pow(delta.x, 2) + Mathf.Pow(delta.y, 2)));
            
            float angle = dis * rotationSpeed*Time.deltaTime;
            foreach (Transform t in target)
            {
                t.RotateAround(t.position, Vector3.up,angle);
            }
            lastMousePos = Input.mousePosition;
        }

    }
}

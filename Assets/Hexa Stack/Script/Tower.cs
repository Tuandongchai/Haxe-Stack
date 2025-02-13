using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Tower : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Animator animator;
    private Renderer renderer;

    [Header(" Settings")]
    [SerializeField] private float fillIncrement;
    [SerializeField] private float maxFillPercent;
    private float fillPercent;

    private void Awake()
    {
        renderer = GetComponent<Renderer>(); 
    }
    private void Start()
    {
        UpdateMaterials();
        BuildButton.onHoldStart += Fill;
    }
    private void Update()
    {
        
    }
    private void OnDestroy()
    {
        BuildButton.onHoldStart -= Fill;
    }
    private void Fill()
    {
        if (fillPercent >= 1)
            return;
        fillPercent += fillIncrement*0.1f;
        UpdateMaterials();

        animator.Play("Bump");
    }
    private void UpdateMaterials()
    {
        foreach (Material material in renderer.materials)
        {
            material.SetFloat("_Fill_Percent", fillPercent*maxFillPercent);
        }
    }

}

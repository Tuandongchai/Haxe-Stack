using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/*[RequireComponent(typeof(Renderer))]*/
public class FillElement : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] protected Animator animator;
    protected Renderer renderer;

    [Header(" Settings")]
    [SerializeField] protected float fillIncrement;
    [SerializeField] protected float maxFillPercent;
    protected float fillPercent;
    [SerializeField] protected int element;

    protected virtual void Awake()
    {
        renderer = GetComponent<Renderer>();
    }
    protected virtual void Start()
    {

        fillPercent = GameData.instance.GetFill(element);
        UpdateMaterials();
        BuildButton.onHoldStart += Fill;
        BuildButton.onHoldEnd += SaveFill;

    }
    protected virtual void OnDestroy()
    {
        BuildButton.onHoldStart -= Fill;
        BuildButton.onHoldEnd -= SaveFill;
    }
    protected virtual void Fill(int count)
    {
        if (StatsManager.Instance.GetCurrentSuns()<=0)
            return;
        if (count != element)
            return;

        if (fillPercent >= 1)
        {
            GameData.instance.UpdateObjectFill();
            SaveFill(count);
            return;
        }
        SunSpawner.instance.GetObject();

        /*fillPercent += fillIncrement * 0.1f; */
        fillPercent += Time.deltaTime*0.2f;
        UpdateMaterials();

        animator.Play("Bump");

        StatsManager.Instance.UseSuns(Time.deltaTime*7);
        MenuGameManager.instance.shaderUIAnimation.Show(fillPercent,count);

    }
    protected virtual void UpdateMaterials()
    {
        foreach (Material material in renderer.materials)
        {
            material.SetFloat("_Fill_Percent", fillPercent * maxFillPercent);
        }
    }
    
    protected virtual void SaveFill(int count)
    {
        if (count != element)
        {
            return;
        }
        GameData.instance.UpdateFill(fillPercent, element);
    }

}

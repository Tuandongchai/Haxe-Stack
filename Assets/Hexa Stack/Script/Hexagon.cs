using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private new Renderer renderer;
    [SerializeField] private new Collider collider;
    public HexStack HexStack { get; private set; }

    public Color Color
    {
        get => renderer.material.color;
        set => renderer.material.color = value;
    }
    public void Configure(HexStack hexStack)
    {
        HexStack = hexStack;
    }
    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }
    public void DisableCollider()=> collider.enabled = false;

    public void Vanish(float delay)
    {
        LeanTween.cancel(gameObject);

        LeanTween.scale(gameObject, Vector3.zero, 0.2f)
            .setEase(LeanTweenType.easeInBack)
            .setDelay(delay)
            .setOnComplete(()=>Destroy(gameObject));
    }

    public void MoveToLocal(Vector3 targetLocalPos)
    {
        LeanTween.moveLocal(gameObject, targetLocalPos, .2f)
            .setEase(LeanTweenType.easeInOutSine)
            .setDelay(transform.GetSiblingIndex() * .01f);

    }
}

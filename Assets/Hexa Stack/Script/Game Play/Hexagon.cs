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
        /*int x;
        LeanTween.value(gameObject, 0, 10, 0.05f) 
        .setOnUpdate((float val) =>
        {
            x = Mathf.FloorToInt(val);
            Debug.Log($"x = {x}");
        }).setOnComplete(()=> AudioManager.instance.PlaySoundEffect(1));*/

        LeanTween.scale(gameObject, Vector3.zero, 0.5f)
            .setEase(LeanTweenType.easeInBack)
            .setDelay(delay)
            .setOnStart(() => AudioManager.instance.PlaySoundEffect(1))
            .setOnComplete(() =>
            {
                AudioManager.instance.PlaySoundEffect(1);
                Destroy(gameObject);
            });
    }

    /*public void MoveToLocal(Vector3 targetLocalPos)
    {
        LeanTween.cancel(gameObject);

        float delay = transform.GetSiblingIndex() * 0.02f;
        LeanTween.moveLocal(gameObject, targetLocalPos, .2f)
            .setEase(LeanTweenType.easeInOutSine)
            .setDelay(delay);
        //direction bi anh huong boi xoay grid
        Vector3 direction = (targetLocalPos - transform.localPosition).With(y: 20).normalized;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        LeanTween.rotateAround(gameObject, rotationAxis, 180, .2f)
            .setEase(LeanTweenType.easeInOutSine)
            .setDelay(delay);

        AudioManager.instance.PlaySoundEffect(0);
    }*/
    public void MoveToLocal(Vector3 targetLocalPos)
    {
        LeanTween.cancel(gameObject);

        float delay = transform.GetSiblingIndex() * 0.03f;
        LeanTween.moveLocal(gameObject, targetLocalPos.With(y:4), .25f)
            .setEase(LeanTweenType.easeInOutSine)
            .setDelay(delay).setOnComplete(() =>
            {
                LeanTween.moveLocal(gameObject, targetLocalPos, .15f)
                    .setEase(LeanTweenType.easeInOutSine);
                AudioManager.instance.PlaySoundEffect(0);

            });
        //direction bi anh huong boi xoay grid
        Vector3 direction = (targetLocalPos - transform.localPosition).With(y: 100).normalized;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        LeanTween.rotateAround(gameObject, rotationAxis, 180, .3f)
            .setEase(LeanTweenType.easeInOutSine)
            .setDelay(delay);

    }


}

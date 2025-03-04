using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSpawner : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject prefabs;
    [SerializeField] private GameObject parentSpawner;
    [SerializeField] private Transform[] target;

    [SerializeField] Queue<GameObject> pool= new Queue<GameObject>();
    public static SunSpawner instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public void GetObject()
    {
        
        GameObject obj;
        if(pool.Count>0)
            obj = pool.Dequeue();
        else
            obj = Instantiate(prefabs, parentSpawner.transform.position, Quaternion.identity);

        obj.transform.SetParent(parentSpawner.transform);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = parentSpawner.transform.localPosition+ new Vector3(Random.Range(-50,50), Random.Range(-50, 50), 0);
        obj.SetActive(true);
        StartCoroutine(Move(obj,0.7f,GameData.instance.GetObjectFill()));

    }

    IEnumerator Move(GameObject gameObject,float time, int count)
    {
       
        LeanTween.moveLocal(gameObject, target[count].localPosition, time)
            .setEase(LeanTweenType.easeInOutBack);
        LeanTween.scale(gameObject, Vector3.zero, time)
            .setEase(LeanTweenType.easeInBack);
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }
}

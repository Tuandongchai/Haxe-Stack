using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GoldAnimation : MonoBehaviour
{
    [Header("Element")] 
    [SerializeField] private GameObject goldPrefab, parent, target, goldPanel;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private List<GameObject> goldList;

    public void Animation(int _increateGolds)
    {
        int number = Random.Range(10,15);
        float delay = 0.3f;
        while (number > 0)
        {
            GoldSpawn(delay);
            number--;
            delay += 0.05f;
        }
        LeanTweenValue(goldText, int.Parse(goldText.text), _increateGolds);
    }
    private void GoldSpawn(float delay)
    {
        GameObject newGold;
        int randomPosX = Random.Range(-70, 70);
        int randomPosY = Random.Range(-70, 70);
        if (goldList.Count > 0)
        {
            newGold = goldList[0];
            goldList.RemoveAt(0);
        }
        else
            newGold = Instantiate(goldPrefab);

        newGold.SetActive(true);
        newGold.transform.SetParent(parent.transform);
        newGold.transform.localPosition = new Vector3(randomPosX, randomPosY, 0);
        LeanTweenMove(newGold, 0.5f, delay);
    }
    private void GoldDespawn(GameObject go)
    {
        go.SetActive(false);
        goldList.Add(go);
    }
    


    private void LeanTweanScale(GameObject go, float time, float delay) => LeanTweanScale((GameObject)go, time, delay);
    private void LeanTweenMove(GameObject go, float time, float delay) => LeanTween.moveLocal(go, target.transform.localPosition, time)
        .setEase(LeanTweenType.easeInBack).setDelay(delay).setOnComplete(()=>GoldDespawn(go));

    private void LeanTweenValue(TextMeshProUGUI _goldText,int currentCoin, int increatCoin)=> LeanTween.value(gameObject, currentCoin, currentCoin + increatCoin, 0.7f)
            .setOnUpdate((float val) =>
            {
                _goldText.text = Mathf.FloorToInt(val).ToString();
            })
            .setEase(LeanTweenType.easeOutQuad).setDelay(0.5f);


}

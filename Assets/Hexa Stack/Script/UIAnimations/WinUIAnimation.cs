using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUIAnimation : MonoBehaviour
{
    [Header("Element")]
    public GameObject winPanel;
    [SerializeField] private GameObject coinPrefab;

    [Header("Setting")]
    [SerializeField] private Transform coinParent;
    [SerializeField] private GameObject coinEnd;


    public void LoadIn()
    {
        winPanel.transform.localScale = Vector3.zero;
        LeanTween.scale(winPanel, new Vector3(1,1,1), 0.3f).setEase(LeanTweenType.easeOutBounce);

        LeanTween.delayedCall(0.5f, () => CollectCoins());
    }
    public void LoadOut()
    {
        winPanel.transform.localScale = Vector3.one;
        LeanTween.scale(winPanel, new Vector3(0, 0, 0), 0.3f).setEase(LeanTweenType.easeOutBounce);
    }
    public void CollectCoins()
    {
        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnCoins()
    {
        int coinCount = Random.Range(10, 20);
        GameObject[] coins = new GameObject[coinCount];

        for (int i = 0; i < coinCount; i++)
        {
            coins[i] = Instantiate(
                coinPrefab,
                coinParent.position + new Vector3(Random.Range(-70, 70), Random.Range(-70, 70), 0),
                Quaternion.Euler(0, 0, Random.Range(-180, 180))
            );
            coins[i].transform.SetParent(coinParent);

        }

        for (int i = 0; i < coinCount; i++)
        {
            LeanTween.move(coins[i], coinEnd.transform.position , 0.25f)
                .setEase(LeanTweenType.easeInBack);
            yield return new WaitForSeconds(0.15f); // Chờ trước khi coin tiếp theo bay
        }
        for (int i = 0; i < coinCount; i++)
        {
            if (Vector3.Distance(coins[i].transform.position, coinEnd.transform.position) <= 0.01)
            {
                Destroy(coins[i]);
            }
        }

    }



}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tabsil.BattlePassSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopup : MonoBehaviour
{
    public static RewardPopup instance;
    [SerializeField] private GameObject rewardPrefab, rewardPanel, header, footer;
    [SerializeField] private Sprite goldSprite, hammerSprite, swapSprite, rollSprite;
    [SerializeField] private GoldAnimation goldAnimaiton;
    private bool hasGold = false;
    private int increateGold;
    private Dictionary<string, Sprite> rewardDict;
    [SerializeField] LeanTweenType type;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        rewardDict = new Dictionary<string, Sprite> {
            {"golds", goldSprite },
            {"hammers", hammerSprite },
            {"swaps", swapSprite },
            {"rolls",rollSprite }
        };
    }
    private void Start()
    {
      /*  Dictionary<string, int> a = new Dictionary<string, int> {
            {"golds", 1000 },
            {"hammers", 2 },
            {"swaps", 2 },
        };
        ShowReward(a);*/
        
    }
    public void ShowReward(Dictionary<string, int> rewards)
    {
        rewardPanel.SetActive(true);
        SetScaleToZero(header);
        SetScaleToZero(footer);
        LeanTweanScale(rewardPanel, 0.2f, Vector3.one);
        LeanTweanScale(header, 0.2f, Vector3.one);
        LeanTweanScale(footer, 0.2f, Vector3.one);

        Transform content = rewardPanel.transform.Find("Content");
        foreach (Transform c in content)
        {
            Destroy(c.gameObject);
        }
        float delay = 0.35f;
        for (int i = 0; i < rewards.Count; i++)
        {
            GameObject newReward = Instantiate(rewardPrefab, content);
            newReward.transform.SetParent(content.transform);
            newReward.transform.Find("Icon").GetComponent<Image>().sprite = rewardDict[rewards.ElementAt(i).Key.ToString()];
            newReward.transform.Find("AmountText").GetComponent<TextMeshProUGUI>().text = rewards.ElementAt(i).Value.ToString();
            SetScaleToZero(newReward);

            LeanTweanScale(newReward, delay, Vector3.one);
            delay+=0.1f;
            if (rewards.ElementAt(i).Key.ToString() == "golds")
            {
                hasGold = true;
                increateGold = rewards.ElementAt(i).Value;
                Debug.Log(hasGold);
            }
            Debug.Log("check" + hasGold);
        }

    }
    public void HideReward()
    {
        SetScaleToOne(rewardPanel);    
        LeanTweanScale(rewardPanel, 0f, Vector3.zero);
        Debug.Log("check1"+hasGold);
        if (hasGold)
        {
            goldAnimaiton.Animation(increateGold);

        }
        hasGold = false;
        Debug.Log("check2" + hasGold);
        StartCoroutine(Disable(rewardPanel));
    }
    private void LeanTweanScale(GameObject go, float time, Vector3 position) => LeanTween.scale(go, position, 0.3f).setEase(type).setDelay(time);
    
    private void SetScaleToZero(GameObject go) => go.transform.localScale = Vector3.zero;
    private void SetScaleToOne(GameObject go) => go.transform.localScale = Vector3.one;
    IEnumerator Disable(GameObject go) {
        yield return new WaitForSeconds(0.3f);
        go.SetActive(false);
    } 
    
}

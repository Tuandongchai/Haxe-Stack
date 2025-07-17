using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ShopUI : BaseUI
{
    [Header("Element")]
    [SerializeField] private GameObject[] items;
    [SerializeField] private TextMeshProUGUI goldsText;
    [SerializeField] private TextMeshProUGUI heartText;

    [SerializeField] private GameObject textNotEnoughGold;

    private void Start()
    {
        BuyItemButton.onClicked += BuyItem;
    }
    private void OnDestroy()
    {
        BuyItemButton.onClicked -= BuyItem;
    }
    public override void Show()
    {
        base.Show();
        goldsText.text =base.currentGold.ToString();
        heartText.text = base.currentHeart.ToString();
    }
    public override void LoadIn()
    {

        Show();
        gameObject.transform.localPosition = new Vector3(0, -2526.31f, 0);
        LeanTween.moveLocalY(gameObject, 153.31f, 0.3f).setEase(LeanTweenType.easeInBack);
        StartCoroutine(loadItem());
    }

    IEnumerator loadItem()
    {
        foreach (var item in items)
        {
            item.transform.localScale = Vector3.zero;
        }
        yield return new WaitForSeconds(0.3f);
        float delay = 0.2f;
        for (int i=0; i<items.Length; i++)
        {

            LeanTween.scale(items[i], Vector3.one, 0.4f).setEase(LeanTweenType.easeOutQuint).setDelay(delay+i*0.3f);
        }
    }
    public override void LoadOut()
    {
        LeanTween.moveLocalY(gameObject, 2526.31f, 0.3f).setEase(LeanTweenType.easeInBack)
            .setOnComplete(()=>gameObject.SetActive(false));

    }

    public void BuyItem(int price, int combo)
    {
        int currentGold = StatsManager.Instance.GetCurrentGolds();
        switch (combo)
        {
            case 1:
                if (Check(price, currentGold))
                {
                    StatsManager.Instance.UseGold(price);
                    StatsManager.Instance.IncreasedTool(0,1);
                    StatsManager.Instance.IncreasedTool(1, 1);
                    StatsManager.Instance.IncreasedTool(2, 1);
                    StatsManager.Instance.IncreasedHeart(1);
                    Dictionary<string, int> dict = new Dictionary<string, int> {
                        {"hammers",1 },
                        {"swaps",1 },
                        {"rolls",1 }
                    };
                    RewardPopup.instance.ShowReward(dict);
                }
                else
                    ShowText();
                break;
            case 2:
                if (Check(price, currentGold))
                {
                    StatsManager.Instance.UseGold(price);
                    StatsManager.Instance.IncreasedTool(0, 3);
                    StatsManager.Instance.IncreasedTool(1, 3);
                    StatsManager.Instance.IncreasedTool(2, 3);
                    StatsManager.Instance.IncreasedHeart(3);
                    Dictionary<string, int> dict = new Dictionary<string, int> {
                        {"hammers",3 },
                        {"swaps",3 },
                        {"rolls",3 }
                    };
                    RewardPopup.instance.ShowReward(dict);
                }
                else
                    ShowText();
                break;
            case 3:
                if (Check(price, currentGold))
                {
                    StatsManager.Instance.UseGold(price);
                    StatsManager.Instance.IncreasedTool(0, 5);
                    StatsManager.Instance.IncreasedTool(1, 5);
                    StatsManager.Instance.IncreasedTool(2, 5);
                    StatsManager.Instance.IncreasedHeart(5);
                    Dictionary<string, int> dict = new Dictionary<string, int> {
                        {"hammers",5 },
                        {"swaps",5 },
                        {"rolls",5 }
                    };
                    RewardPopup.instance.ShowReward(dict);
                }
                else
                    ShowText();
                break;
            default:
                break;
        }
        MenuGameManager.instance.menuUIAnimation.Show();
        Show(); 
    }
    public bool Check(int price, int currentGold)
    {
        if (price <= currentGold)
            return true;
        return false;
    }
    public void Hide() =>textNotEnoughGold.SetActive(false);
    public void ShowText() => textNotEnoughGold.SetActive(true);
}

using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using static CoppraGames.QuestManager;

public class DailyQuestManager : MonoBehaviour
{
    [SerializeField] private List<DailyQuestSetup> listQuest = new List<DailyQuestSetup>();

    [SerializeField] private List<ClaimBonusButton> listBonus = new List<ClaimBonusButton>();

    [SerializeField] private Image fillBonusDQ, fillBonusWQ;
    [SerializeField] private TextMeshProUGUI percentBonusDQ, percentBonusWQ;


    private int[] currentDQ, totalDQ, currentWQ, totalWQ, questDQState, questWQState;
    public Color colorClaim, colorClaimed;

    private void Awake()
    {
        
    }
    private void Start()
    {
        
        DailyQuestButton.onclicked += UpdateDailyQuest;
        ClaimBonusButton.onClicked += UpdateDailyQuest;
        ClaimButton.onClicked += Claim;
        
        UpdateDailyQuest();

        
    }
    private void OnDestroy()
    {
        DailyQuestButton.onclicked -= UpdateDailyQuest;
        ClaimBonusButton.onClicked -= UpdateDailyQuest;
        ClaimButton.onClicked -= Claim;
    }
    private void UpdateDailyQuest()
    {
        int timePlay = (int)(Time.time / 60);
        GameData.instance.IncreatedCurrentWeeklyQuest(0, timePlay);
        GameData.instance.IncreatedCurrentDailyQuest(0, timePlay);

        /*colorClaimed = new Color(70f / 255f, 70f / 255f, 70f / 255f, 255f / 255f);
        colorClaim = new Color(160f / 255f, 160f / 255f, 160f / 255f, 255f / 255f);*/
        /*colorClaim = new Color(0, 0, 0, 1);*/

        currentDQ = GameData.instance.GetCurrentDailyQuest();
        totalDQ = GameData.instance.GetTotalDailyQuest();
        questDQState = GameData.instance.GetDailyQuest();

        currentWQ = GameData.instance.GetCurrentWeeklyQuest();
        totalWQ = GameData.instance.GetTotalWeeklyQuest();
        questWQState = GameData.instance.GetWeeklyQuest();

        for (int i = 0; i < questDQState.Length; i++)
        {
            if (questDQState[i] == -1)
            {
                if (currentDQ[i] >= totalDQ[i])
                    GameData.instance.SetCompleteDailyQuest(i);
            }
        }
        for (int i = 0; i < questWQState.Length; i++)
        {
            if (questWQState[i] == -1)
            {
                if (currentWQ[i] >= totalWQ[i])
                    GameData.instance.SetCompleteWeeklyQuest(i);
            }
        }
        questDQState = GameData.instance.GetDailyQuest();
        questWQState = GameData.instance.GetWeeklyQuest();
        

        foreach (DailyQuestSetup quest in listQuest)
        {
            /*LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());*/
            if (quest.id < 6)
                quest.Initialize(quest.id ,quest.id, questDQState, currentDQ[quest.id], totalDQ[quest.id], colorClaimed, colorClaim);
            else
                quest.Initialize(quest.id, (quest.id - 6), questWQState, currentWQ[quest.id - 6], totalWQ[quest.id - 6], colorClaimed, colorClaim);
        }

        
        //update UI bonus daily quest
        int totalDQComplete = 6;
        foreach (int i in questDQState)
            if (i == -1)
                totalDQComplete--;
        fillBonusDQ.fillAmount = totalDQComplete/(float)6;
        percentBonusDQ.text = $"{totalDQComplete}/6";

        switch ((int)totalDQComplete/2)
        {
            case 0:
                 break;
            case 1:
                if (GameData.instance.GetDQBonusState(0) == -1)
                    GameData.instance.SetCompleteDQBonus(0);
                break;
            case 2:
                if (GameData.instance.GetDQBonusState(0) == -1)
                    GameData.instance.SetCompleteDQBonus(0);
                if (GameData.instance.GetDQBonusState(1) == -1)
                    GameData.instance.SetCompleteDQBonus(1);
                break;
            case 3:
                if (GameData.instance.GetDQBonusState(0) == -1)
                    GameData.instance.SetCompleteDQBonus(0);
                if (GameData.instance.GetDQBonusState(1) == -1)
                    GameData.instance.SetCompleteDQBonus(1);
                if (GameData.instance.GetDQBonusState(2) == -1)
                    GameData.instance.SetCompleteDQBonus(2);
                break;
            default: break;

        }

        //update UI bonus daily quest
        int totalWQComplete = 6;
        foreach (int i in questWQState)
            if (i == -1)
                totalWQComplete--;
        fillBonusWQ.fillAmount = totalWQComplete / 6;
        percentBonusWQ.text = $"{totalWQComplete}/6";

        switch ((int)totalWQComplete / 2)
        {
            case 0:
                break;
            case 1:
                if (GameData.instance.GetWQBonusState(0) == -1)
                    GameData.instance.SetCompleteWQBonus(0);
                break;
            case 2:
                if (GameData.instance.GetWQBonusState(0) == -1)
                    GameData.instance.SetCompleteWQBonus(0);
                if (GameData.instance.GetWQBonusState(1) == -1)
                    GameData.instance.SetCompleteWQBonus(1);
                break;
            case 3:
                if (GameData.instance.GetWQBonusState(0) == -1)
                    GameData.instance.SetCompleteWQBonus(0);
                if (GameData.instance.GetWQBonusState(1) == -1)
                    GameData.instance.SetCompleteWQBonus(1);
                if (GameData.instance.GetWQBonusState(2) == -1)
                    GameData.instance.SetCompleteWQBonus(2);
                break;
            default: break;

        }
        for (int i = 0; i<listBonus.Count; i++)
        {
            listBonus[i].Initialized(i);
        }
        

        StartCoroutine(RebuildLayout());
    }
    IEnumerator RebuildLayout()
    {
        yield return null; // Đợi 1 frame để Layout Group cập nhật
        RectTransform contentRect = GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }
    private void Claim(int _id)
    {
        foreach (DailyQuestSetup quest in listQuest)
        {
            /*LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());*/
            if (quest.id == _id)
            {
                if(_id < 6)
                {
                    GameData.instance.SetClaimedDailyQuest(_id);
                    questDQState = GameData.instance.GetDailyQuest();
                    Dictionary<string, int> reward = new Dictionary<string, int> { { "golds", 100 } };
                    RewardPopup.instance.ShowReward(reward);
                    StatsManager.Instance.IncreasedGolds(100);
                    quest.Initialize(quest.id,quest.id, questDQState, currentDQ[quest.id], totalDQ[quest.id], colorClaimed, colorClaim);

                }
                else
                {
                    GameData.instance.SetClaimWeeklyQuest(_id-6);
                    questWQState = GameData.instance.GetWeeklyQuest();
                    Dictionary<string, int> reward = new Dictionary<string, int> { { "golds", 1000 } };
                    RewardPopup.instance.ShowReward(reward);
                    StatsManager.Instance.IncreasedGolds(1000);
                    quest.Initialize(quest.id, quest.id-6, questWQState, currentWQ[quest.id - 6], totalWQ[quest.id - 6], colorClaimed, colorClaim);

                }


            }
        }
        
    } 
    
}

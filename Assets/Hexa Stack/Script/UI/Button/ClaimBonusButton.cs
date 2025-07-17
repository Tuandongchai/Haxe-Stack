using System;
using System.Collections;
using System.Collections.Generic;
/*using TMPro.EditorUtilities;*/
using UnityEngine;

public class ClaimBonusButton : BaseButton
{
    public int id, goldReward;
    [SerializeField] private GameObject locked, received;

    public static Action onClicked;
    protected override void Start()
    {
        base.Start();
    }
    public void Initialized(int _id)
    {
        int state = 0;
        if(_id<3)
            state = GameData.instance.GetDQBonusState(_id);
        else
            state = GameData.instance.GetWQBonusState(_id - 3);
        switch (state)
        {
            case -1:
                received.SetActive(false);
                locked.SetActive(true);
                break;
            case 0:
                received.SetActive(false);
                locked.SetActive(false);
                break;
            case 1:
                received.SetActive(true);
                locked.SetActive(false);
                break;
            default: 
                break;
        }
    }
    protected override void OnButtonClick()
    {
        Debug.Log(id);
        if (id < 3)
        {
            if (GameData.instance.GetDQBonusState(id) == 0)
            {
                Dictionary<string, int> reward = new Dictionary<string, int> { { "golds", goldReward } };
                RewardPopup.instance.ShowReward(reward);
                GameData.instance.SetClaimDQBonus(id);
                Debug.Log("golds" + id);
                Initialized(id);
            }
            else return;
        }

        else
        {
            if (GameData.instance.GetWQBonusState(id - 3) == 0)
            {
                Dictionary<string, int> reward = new Dictionary<string, int> { { "golds", goldReward } };
                RewardPopup.instance.ShowReward(reward);
                GameData.instance.SetClaimWQBonus(id - 3);
                Debug.Log("golds" + id);
                Initialized(id);
            }
            else return;
        }
    }
}

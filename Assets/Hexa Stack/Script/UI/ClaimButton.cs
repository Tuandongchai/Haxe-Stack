using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClaimButton : MonoBehaviour
{
    private Button button;
    public int id, total, current;
    public static Action<int> onClicked;

    private void Start()
    {
        button = GetComponent<Button>();
        id=gameObject.GetComponentInParent<DailyQuestUI>().id;

        button.onClick.AddListener(()=> {
            int[] stateDQ = GameData.instance.GetDailyQuest();
            int[] stateWQ = GameData.instance.GetWeeklyQuest();
            Debug.Log("Check id claim "+id);
            if (id < 6)
            {
                current = GameData.instance.GetCurrentDailyQuest()[id];
                total = GameData.instance.GetTotalDailyQuest()[id];
                if (stateDQ[id] == 0 && current >= total)
                {
                    onClicked?.Invoke(id);
                }
            }
            else
            {
                current = GameData.instance.GetCurrentWeeklyQuest()[id-6];
                total = GameData.instance.GetTotalWeeklyQuest()[id-6];
                if (stateWQ[id-6] == 0 && current>=total)
                {
                    onClicked?.Invoke(id);
                }
            }
        });
    }
}

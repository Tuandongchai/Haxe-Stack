using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DailyQuestSetup : BaseButton<int>
{

    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private GameObject buttonColor;
    [SerializeField] private Image percentFill;
    public int id;

    private int[] questArray;


    public static Action<int> onclicked;

    protected override void Start()
    {
        base.Start();
        t = id;

    }

    public void Initialize(int _id, int _idState,int[] _questState, int _current, int _total, Color _colorClaimed, Color _colorClaim)
    {
        if(_id==id)
            if (_questState[_idState] == -1)
                    buttonColor.GetComponent<Image>().color = _colorClaimed;
            if (_questState[_idState]==0)
                buttonColor.GetComponent<Image>().color = _colorClaim;
            else
                buttonColor.GetComponent<Image>().color = _colorClaimed;

            percentText.text = $"{_current}/{_total}";
            percentFill.fillAmount = _current / (float)_total;


    }

    protected override void OnButtonClick(int t)
    {
        onclicked?.Invoke(id);
    }
}

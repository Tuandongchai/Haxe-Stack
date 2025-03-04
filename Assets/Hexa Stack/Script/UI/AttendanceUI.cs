using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttendanceUI : MonoBehaviour
{
    [SerializeField] private GameObject attendancePanel, dailyPanel, newUserPanel;
    [SerializeField] private GameObject[] buttonArray= new GameObject[] { };
    [SerializeField] private Button dailyButton, newUserButton;
    [SerializeField] private GameObject underLine1, underLine2;

    private void Start()
    {
        dailyButton.onClick.AddListener(()=>ShowPanel(dailyPanel));
        newUserButton.onClick.AddListener(()=>ShowPanel(newUserPanel));
        /*ShowPanel(dailyPanel);*/
        foreach (GameObject gb in buttonArray)
        {
            gb.transform.localScale = Vector3.zero;
        }
    }
    public void LoadIn()
    {
        attendancePanel.transform.localScale = Vector3.zero;
        LeanTween.scale(attendancePanel, Vector3.one, 0.5f).setEase(LeanTweenType.easeInBounce);
        StartCoroutine("LoadDayButton");
    }
    IEnumerator LoadDayButton()
    {
        yield return new WaitForSeconds(0.5f);
        float delay = 0;
        for(int i=0; i<buttonArray.Length; i++)
        {
            GameObject b = buttonArray[i];
            delay += 0.05f;
            LeanTween.scale(b, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack).setDelay(delay);
        }
    }
    public void LoadOut()
    {
        attendancePanel.transform.localScale = Vector3.one;
        foreach (GameObject gb in buttonArray)
        {
            gb.transform.localScale = Vector3.zero;
        }
        LeanTween.scale(attendancePanel, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBounce).setOnComplete(()=>attendancePanel.SetActive(false));
    }
    private void ShowPanel(GameObject panel)
    {
        switch (panel.name)
        {
            case "Daily Attendance":
                dailyPanel.SetActive(true);
                newUserPanel.SetActive(false);
                underLine1.SetActive(true);
                underLine2.SetActive(false);
                break;
            case "New User Attendance ":
                newUserPanel.SetActive(true);
                dailyPanel.SetActive(false);
                underLine1.SetActive(false);
                underLine2.SetActive(true);
                break;
            default: 
                break;
        }
    }
}

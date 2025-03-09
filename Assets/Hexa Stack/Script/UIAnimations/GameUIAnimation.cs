using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIAnimation : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject loadScene;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject tool;

    [SerializeField] private GameObject toolTutorial;
    [SerializeField] private GameObject hammerPanel;
    [SerializeField] private GameObject swapPanel;


    public GameObject settingPanel;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start()
    {
        Show();
        GoHomeButton.onClicked += Load;
        
    }
    private void OnDestroy()
    {
        GoHomeButton.onClicked -= Load;
    }
    public void Show()
    {
        levelText.text = "Level " + StatsManager.Instance.GetSelectLevel();
        goldText.text = StatsManager.Instance.GetCurrentGolds().ToString();

       
    }
    public void UseTool(int i)
    {
        UI.transform.localPosition = new Vector3(89.539093f, 1053.95886f, 0);
        LeanTween.moveLocalY(UI, 1700, 0.4f).setEase(LeanTweenType.easeInBack);

        tool.transform.localPosition = new Vector3(0, -968.9454f, 0);
        LeanTween.moveLocalY(tool, -1900, 0.4f).setEase(LeanTweenType.easeInBack);

        toolTutorial.SetActive(true);
        if (i == 0)
        {
            hammerPanel.SetActive(true);
            hammerPanel.transform.localScale= new Vector3(0,0,0);
            LeanTween.scale(hammerPanel, new Vector3(1,1,1), 0.4f).setEase(LeanTweenType.easeInBounce);
        }
        else if (i == 1)
        {
            swapPanel.SetActive(true);
            swapPanel.transform.localScale = new Vector3(0, 0, 0);
            LeanTween.scale(swapPanel, new Vector3(1, 1, 1), 0.4f).setEase(LeanTweenType.easeInBounce);
        }


    }
    public void NoUseTool(int i)
    {
        UI.transform.localPosition = new Vector3(89.539093f, 1700, 0);
        LeanTween.moveLocalY(UI, 1054, 0.4f).setEase(LeanTweenType.easeInBack);

        tool.transform.localPosition = new Vector3(0, -1900, 0);
        LeanTween.moveLocalY(tool, -968.9454f, 0.4f).setEase(LeanTweenType.easeInBack);

        
        if (i == 0)
        {
            
            hammerPanel.transform.Rotate(1, 1, 1);
            LeanTween.scale(hammerPanel, new Vector3(0, 0, 0), 0.4f).setEase(LeanTweenType.easeInBounce)
                .setOnComplete(() =>
                {
                    hammerPanel.SetActive(false);
                    toolTutorial.SetActive(false);

                });
        }
        else if (i == 1)
        {
            swapPanel.transform.Rotate(1, 1, 1);
            LeanTween.scale(swapPanel, new Vector3(0, 0, 0), 0.4f).setEase(LeanTweenType.easeInBounce)
                .setOnComplete(()=> {
                    swapPanel.SetActive(false);
                    toolTutorial.SetActive(false);

                });
                
        }
    }
    public void Load()
    {
        AudioManager.instance.PlaySoundEffect(7);

        loadScene.SetActive(true);
        loadScene.transform.localPosition = new Vector3(0, 3000f, 0);
        LeanTween.moveLocalY(loadScene, 0, 0.2f);

        int timePlay = (int)(Time.time / 60);
        GameData.instance.IncreatedCurrentWeeklyQuest(0, timePlay);
        GameData.instance.IncreatedCurrentDailyQuest(0, timePlay);

        StartCoroutine(LoadMenuSence());
    }
    IEnumerator LoadMenuSence()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
        yield return new WaitForSeconds(0.1f);
        MenuGameManager.instance.menuUIAnimation.LoadIn();
    }


}

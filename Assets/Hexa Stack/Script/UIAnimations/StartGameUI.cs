using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class StartGameUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI requirePieces;
    [SerializeField] private GameObject[] tools;

    [SerializeField] private GameObject[] toolsParent;


    private void Start()
    {
/*        
        panel.SetActive(true);*/
        Show();
        LoadIn();
    }

    private void Show()
    {
        levelText.text = "Level "+StatsManager.Instance.GetSelectLevel();
        requirePieces.text = LevelManager.Instance.piecesRequire.ToString();

    }
    private void LoadIn()
    {
        levelPanel.SetActive(true);
        levelPanel.transform.localPosition = new Vector3(-1400, 228.83374f, 0);
        /*        tools[3].transform.localScale= new Vector3(1,1,1);*/
        
        foreach (GameObject go in tools)
        {
            if (go == tools[3])
                continue;
            go.transform.localScale = new Vector3(0, 0, 0);
        }
        LeanTween.moveLocalX(tools[3], 29, 0.4f).setEase(LeanTweenType.easeInBack);

        LeanTween.moveLocalX(levelPanel, 0, 0.4f).setEase(LeanTweenType.easeInBack)
            .setOnComplete(() =>
            {
                inGameUI.SetActive(true);
                for (int i=0; i<tools.Length; i++)
                {
                    if (i == 3)
                        continue;
                    LeanTween.scale(tools[i], new Vector3(1,1,1), 0.3f);
                }


            });


        // 
        for(int i=0; i < tools.Length; i++)
        {
            LeanTween.moveLocal(tools[i], toolsParent[i].transform.localPosition, 0.4f)
                .setEase(LeanTweenType.easeInBack)
                .setDelay(1.2f);
            LeanTween.scale(tools[i], new Vector3(0.1f,0.1f,0.1f),0.4f)
                .setEase(LeanTweenType.easeOutBounce)
                .setDelay(1.2f);
        }
        LeanTween.moveLocalX(levelPanel, -1400, 0.4f).setEase(LeanTweenType.easeInBack)
            .setDelay(1.2f);

        StartCoroutine(SpawnerLevel());
    }
    IEnumerator SpawnerLevel()
    {
        yield return new WaitForSeconds(1.6f);
        panel.SetActive(false);
    }
    
}

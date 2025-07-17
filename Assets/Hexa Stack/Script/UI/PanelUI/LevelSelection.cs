using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using TMPro;
using System.Runtime.CompilerServices;

public class LevelSelection : BaseUI
{
    [SerializeField] private GameObject levelPanel;
    public GameObject levelPrefab; 
    public Transform levelContainer;
    public Button nextButton, prevButton;
    public int levelsPerPage = 9; 
    public int totalLevels=20;

    private int currentPage = 0;
    private int totalPages;
    [SerializeField]private List<GameObject> levelButtons = new List<GameObject>();

    private void Start()
    {
        currentPage = 0;
    }

    IEnumerator UpdateUI(int pageNumber)
    {
        yield return new WaitForSeconds(0.1f);
        while (levelContainer.childCount<levelsPerPage)
        {
            GameObject ob;
            if (levelButtons.Count<=0)
                ob = Instantiate(levelPrefab);
            else
            {
                ob = levelButtons.First();
                levelButtons.Remove(levelButtons.First());
            }
            ob.SetActive(true);
            ob.transform.parent = levelContainer.transform;
            for (int i = 0; i < levelContainer.childCount; i++)
            {
                GameObject btn = levelContainer.GetChild(i).gameObject; // Lưu đối tượng chính xác
                
                LeanTween.scale(btn, Vector3.zero, Random.Range(0.1f, 0.5f))
                    .setEase(LeanTweenType.easeInOutQuad)
                    .setOnComplete(() =>
                    {
                        LeanTween.scale(btn, Vector3.one, Random.Range(0.1f, 0.5f))
                            .setEase(LeanTweenType.easeInOutQuad);
                    });
            }
        }
        
        for(int i=0; i<levelContainer.childCount; i++)
        {
            Transform btn = levelContainer.GetChild(i);
            btn.GetComponentInChildren<LevelButton>().IsUnlocked(pageNumber*9+i);
            
            btn.GetComponentInChildren<TextMeshProUGUI>().text = (pageNumber * 9+i).ToString();
            
        }

    }
    public void Next()
    {
        if (currentPage + 1 >= Mathf.CeilToInt((float)totalLevels / levelsPerPage))
            return;

        currentPage++;

        for (int i = 0; i < levelContainer.childCount; i++)
        {
            GameObject btn = levelContainer.GetChild(i).gameObject;

            LeanTween.scale(btn, Vector3.zero, Random.Range(0.1f, 0.5f))
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(() =>
                {
                    LeanTween.scale(btn, Vector3.one, Random.Range(0.1f, 0.5f))
                        .setEase(LeanTweenType.easeInOutQuad);
                });

            levelButtons.Add(btn);
        }

        StartCoroutine(UpdateUI(currentPage));
    }
    public void Previouts()
    {
        if (currentPage - 1 < 0)
            return;

        currentPage--;

        for (int i = 0; i < levelContainer.childCount; i++)
        {
            GameObject btn = levelContainer.GetChild(i).gameObject; // Lưu đối tượng chính xác

            LeanTween.scale(btn, Vector3.zero, Random.Range(0.1f, 0.5f))
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(() =>
                {
                    LeanTween.scale(btn, Vector3.one, Random.Range(0.1f, 0.5f))
                        .setEase(LeanTweenType.easeInOutQuad);
                });

            levelButtons.Add(btn);
        }
        StartCoroutine(UpdateUI(currentPage));
    }
    public override void LoadIn()
    {
        levelPanel.SetActive(true);
        levelPanel.transform.localScale = Vector3.zero;
        StartCoroutine(UpdateUI(currentPage));
        LeanTween.scale(levelPanel,Vector3.one, 0.3f).setEase(LeanTweenType.easeInBack);
        ButtonAnimation(nextButton.gameObject);
        ButtonAnimation(prevButton.gameObject);
    }
    public override void LoadOut()
    {
        levelPanel.transform.localScale = Vector3.one;
        LeanTween.scale(levelPanel, Vector3.zero, 0.3f)
            .setEase(LeanTweenType.easeInBack).setOnComplete(()=>levelPanel.SetActive(false));
    }
    private void ButtonAnimation(GameObject btn)
    {
        LeanTween.scale(btn, new Vector3(0.7f, 0.7f,0), Random.Range(0.6f, 1.2f))
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(() =>
                {
                    LeanTween.scale(btn, new Vector3(1.3f, 1.3f, 0), Random.Range(0.6f, 1.2f))
                        .setEase(LeanTweenType.easeInOutQuad);
                }).setOnComplete(()=>ButtonAnimation(btn));
    }
}

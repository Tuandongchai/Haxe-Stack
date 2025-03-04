using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Component")]
    [SerializeField] private GameObject[] level;
    [SerializeField] private int[] pieces;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI percentLevel;

    public int piecesCount = 0;
    public int piecesRequire;


    public GameObject levelSpawner;

    private int currentLv;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }

    private void Start() {

        currentLv = StatsManager.Instance.GetSelectLevel();
        GenerateLevels(currentLv);

        piecesRequire = pieces[currentLv];
        UpdateFill();

        NextLevelButton.onClicked += NextLevel;
        RetryButton.onClicked += ResetLevel;
        
    }
    private void OnDestroy()
    {
        NextLevelButton.onClicked -= NextLevel;
        RetryButton.onClicked -= ResetLevel;
    }
    private void Update()
    {
        /*UpdateFill();*/
        Lose();
        if (piecesCount >= piecesRequire)
            StartCoroutine(Win());
    }
    public void UpdateFill()
    {

        fill.fillAmount = (piecesRequire - piecesCount) / (float)piecesRequire;

        
        percentLevel.text = $"{piecesCount} / {piecesRequire}";
    }
    private void GenerateLevels(int lv)
    {
        GameObject currentLevel = level[lv];

        Vector3 position = transform.position;
        levelSpawner = Instantiate(currentLevel, position, Quaternion.identity);
        levelSpawner.transform.SetParent(transform);

        

    }
    public void NextLevel()
    {
        AudioManager.instance.PlaySoundEffect(7);

        StartCoroutine(LoadNextLevel()); 

        
    }
    IEnumerator LoadNextLevel()
    {
        GameUIManager.Instance.winUIAnimation.LoadOut();
        yield return new WaitForSeconds(0.5f);
        StatsManager.Instance.IncreasedSelectLevel();
        SceneManager.LoadScene(1);

    }
   


    public void ResetLevel()
    {
        AudioManager.instance.PlaySoundEffect(7);
        StartCoroutine(LoadLevel());
    }
    IEnumerator LoadLevel()
    {
        /*GameUIManager.Instance.gameUIAnimation.settingPanel.SetActive(false);*/
        GameUIManager.Instance.winUIAnimation.LoadOut();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);

    }
    IEnumerator Win()
    {
        yield return new WaitForSeconds(0.5f);
        if (GameUIManager.Instance.winUIAnimation.winPanel.active == true)
            yield break;
        GameManager.instance.SetGameState(GameState.Win);
        StatsManager.Instance.IncreasedSuns(piecesRequire);
        GameUIManager.Instance.winUIAnimation.LoadIn();
    }
    public void Lose()
    {
        if (GameState.Lose==GameManager.instance.gameState)
            return;
        int childCount = levelSpawner.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            if (levelSpawner.transform.GetChild(i).childCount < 2)
            {
                return;
            }
        }
        StartCoroutine(CheckLose(childCount));
    }
    IEnumerator CheckLose(int childCount)
    {
        if (GameState.Lose == GameManager.instance.gameState)
            yield break;
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < childCount; i++)
        {
            if (levelSpawner.transform.GetChild(i).childCount < 2)
            {
                yield break;
            }
        }
        Losed();
    }
    public void Losed()
    {
        if (GameUIManager.Instance.loseUIAnimation.losePanels.active==true)
            return;
        GameManager.instance.SetGameState(GameState.Lose);
        GameUIManager.Instance.loseUIAnimation.LoadIn();
        StatsManager.Instance.UseHeart();
    }
}

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


    private GameObject levelSpawner;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


    }

    private void Start() {
        GenerateLevels();

        NextLevelButton.onClicked += NextLevel;
    }
    private void OnDestroy()
    {
        NextLevelButton.onClicked -= NextLevel;
    }
    private void Update()
    {
        UpdateFill();
        Lose();
        if (piecesCount >= piecesRequire)
            Win();
    }
    private void UpdateFill()
    {
        fill.fillAmount = (piecesRequire - piecesCount) / 100;
        percentLevel.text = $"{piecesCount} / {piecesRequire}";
    }
    private void GenerateLevels()
    {
        GameObject currentLevel = level[StatsManager.Instance.GetCurrentLevel()];

        Vector3 position = transform.position;
        levelSpawner = Instantiate(currentLevel, position, Quaternion.identity);
        levelSpawner.transform.SetParent(transform);

        piecesRequire = pieces[StatsManager.Instance.GetCurrentLevel()];

    }
    public void NextLevel()
    {
        StartCoroutine(LoadNextLevel()); 

        
    }
    IEnumerator LoadNextLevel()
    {
        GameUIManager.Instance.winUIAnimation.LoadOut();
        yield return new WaitForSeconds(0.5f);
        StatsManager.Instance.IncreasedLevel();
        SceneManager.LoadScene(1);

    }
    public void Win()
    {
        if (GameUIManager.Instance.winUIAnimation.winPanel.active == true)
            return;
        GameManager.instance.SetGameState(GameState.Win);
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

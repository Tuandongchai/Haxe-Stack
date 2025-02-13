using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private GameState gameState = GameState.Win;

    [SerializeField] private GameObject[] level;
    [SerializeField] private int[] pieces;

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

    }
    private void Update()
    {
         Lose();

        if (piecesCount >= piecesRequire)
            Win();
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
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < childCount; i++)
        {
            if (levelSpawner.transform.GetChild(i).childCount < 2)
            {
                yield break;
            }
        }
        GameManager.instance.SetGameState(GameState.Lose);
        if (GameUIManager.Instance.loseUIAnimation.losePanels.active == true)
            yield break;
        GameUIManager.Instance.loseUIAnimation.LoadIn();
    }
}

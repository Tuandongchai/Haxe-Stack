using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MenuState
{
    Menu,
    Shader
}

public class MenuGameManager : MonoBehaviour
{
    public static MenuGameManager instance;
    public MenuState menuState;
    public MenuUIAnimation menuUIAnimation;
    public ShaderUIAnimation shaderUIAnimation;
    public LevelSelection levelSelectionUI;

    public GameObject Loading, LoadingUI;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        LevelButton.onClicked += SelectLevel;

        Loading.SetActive(false);
        LoadingUI.SetActive(false);
    }
    private void OnDestroy()
    {
        LevelButton.onClicked -= SelectLevel;
    }
    //
    public void MenuCallBack()
    {
        AudioManager.instance.PlaySoundEffect(7);
        StartCoroutine(LoadMenuScene());
    }
    IEnumerator LoadMenuScene()
    {
        shaderUIAnimation.LoadOut();
        yield return new WaitForSeconds(0.4f);
        SetGameState(MenuState.Menu);
        yield return new WaitForSeconds(0.1f);
        menuUIAnimation.LoadIn();
    }
    //
    public void ShaderCallBack()
    {
        AudioManager.instance.PlaySoundEffect(7);
        StartCoroutine(LoadShaderScene());
    }

    IEnumerator LoadShaderScene()
    {
        menuUIAnimation.LoadOut();
        yield return new WaitForSeconds(0.5f);
        SetGameState(MenuState.Shader);
        yield return new WaitForSeconds(0.1f);
        shaderUIAnimation.LoadIn();
    }

    //
    public void GameCallBack()
    {
        StartCoroutine(LoadGameScene());
    }
    IEnumerator LoadGameScene()
    {
        StatsManager.Instance.SetSelectLevel(StatsManager.Instance.GetCurrentLevel());
        /*PlayerPrefs.SetInt("Level", 0);*/
        menuUIAnimation.LoadGamePlayScence();
        yield return new WaitForSeconds(1.5f);

        /*LoadingManager.instance.nameScene = "PlayScene";
        SceneManager.LoadScene(0);*/

        Loading.SetActive(true);
        LoadingUI.SetActive(true);

        /*SceneManager.LoadScene(2);*/
    }
    public void SelectLevel(int lv)
    {
        if (lv >StatsManager.Instance.GetCurrentLevel())
            return;

        levelSelectionUI.LoadOut();

        //daily and weekly
        int timePlay = (int)(Time.time/60);
        GameData.instance.IncreatedCurrentWeeklyQuest(0, timePlay);
        GameData.instance.IncreatedCurrentDailyQuest(0, timePlay);
        StartCoroutine(LoadLoadSelectLevelGameScene(lv));
        
    }
    IEnumerator LoadLoadSelectLevelGameScene(int lv)
    {
        StatsManager.Instance.SetSelectLevel(lv);
        /*PlayerPrefs.SetInt("Level", 0);*/
        menuUIAnimation.LoadGamePlayScence();
        
        yield return new WaitForSeconds(1.5f);

        /*LoadingManager.instance.nameScene = "PlayScene";
        SceneManager.LoadScene(0);*/

        Loading.SetActive(true);
        LoadingUI.SetActive(true);
        /*SceneManager.LoadScene(2);*/
    }
    public void SetGameState(MenuState gameState)
    {
        this.menuState = gameState;

        IEnumerable<IMenuGameStateListener> gameStateListeners =
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IMenuGameStateListener>();

        foreach (IMenuGameStateListener gameStateListener in gameStateListeners)
            gameStateListener.GameStateChangedCallback(gameState);
    }
}
public interface IMenuGameStateListener
{
    void GameStateChangedCallback(MenuState gameState);
}

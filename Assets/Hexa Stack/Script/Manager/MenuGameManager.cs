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

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
    }
    //
    public void MenuCallBack()
    {
        StartCoroutine(LoadMenuScene());
    }
    IEnumerator LoadMenuScene()
    {
        shaderUIAnimation.LoadOut();
        yield return new WaitForSeconds(0.5f);
        SetGameState(MenuState.Menu);
        yield return new WaitForSeconds(0.1f);
        menuUIAnimation.LoadIn();
    }
    //
    public void ShaderCallBack()
    {
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
        menuUIAnimation.LoadGamePlayScence();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
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

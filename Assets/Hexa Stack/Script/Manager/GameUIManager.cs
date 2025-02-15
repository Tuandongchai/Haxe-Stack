using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameUIManager : MonoBehaviour, IGameStateListener
{
    public static GameUIManager Instance { get; private set; }

    [Header("Element")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    private GameObject[] panels;

    public WinUIAnimation winUIAnimation;
    public LoseUIAnimation loseUIAnimation;

    private void Start()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        panels = new GameObject[]
        {
            gamePanel,
            winPanel,
            losePanel
        };
    }

    private void Show(GameObject panel)
    {
        for(int i =0; i<panels.Length; i++)
            panels[i].SetActive(panels[i]==panel);
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Play:
                Show(gamePanel); 
                break;
            case GameState.Win:
                Show(winPanel); 
                break;
            case GameState.Lose:
                Show(losePanel);
                break;
            default: 
                break;
        }
    }

    


}

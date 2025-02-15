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
    public GameUIAnimation gameUIAnimation;

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
        AudioManager.instance.BGSoundOn(2);
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
                AudioManager.instance.BGSoundOn(2);
                break;
            case GameState.Win:
                Show(winPanel);
                AudioManager.instance.PlaySoundEffect(8);
                break;
            case GameState.Lose:
                Show(losePanel);
                AudioManager.instance.PlaySoundEffect(9);
                break;
            default: 
                break;
        }
    }

    


}

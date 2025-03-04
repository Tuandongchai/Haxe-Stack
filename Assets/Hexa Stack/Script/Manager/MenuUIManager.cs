using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MenuUIManager : MonoBehaviour, IMenuGameStateListener
{
    [Header(" Element ")]
    [SerializeField] private GameObject menuPanel, shaderPanel;
    private GameObject[] panels;

    private void Start()
    {
        
        AudioManager.instance.BGSoundOn(0);
        panels = new GameObject[] { 
            menuPanel,
            shaderPanel
        };
    }
    private void ShowPanel(GameObject panel)
    {
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(panels[i]==panel);
        }

    }
    
    public void GameStateChangedCallback(MenuState state)
    {
        switch (state)
        {
            case MenuState.Menu:
                ShowPanel(menuPanel);
                AudioManager.instance.BGSoundOn(0);
                break;
            case MenuState.Shader:
                ShowPanel(shaderPanel);
                AudioManager.instance.BGSoundOn(1);
                break;
            default: 
                break;

        }
    }
}

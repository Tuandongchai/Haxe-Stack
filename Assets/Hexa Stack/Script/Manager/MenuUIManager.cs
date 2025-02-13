using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour, IMenuGameStateListener
{
    [Header(" Settings ")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject shaderPanel;
    private GameObject[] panels;

    private void Start()
    {
        panels = new GameObject[] { 
            menuPanel,
            shaderPanel,
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
                break;
            case MenuState.Shader:
                ShowPanel(shaderPanel);
                break;
            default: 
                break;

        }
    }
}

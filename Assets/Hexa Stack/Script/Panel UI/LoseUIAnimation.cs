﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseUIAnimation : BaseUI
{
    [Header("Element")]
    public GameObject losePanels;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI collectPiece;
    [SerializeField] private GameObject loadingUI, loadingMenu, loadingGamePlay;    
    
    private void Awake()
    {
        ReviveButton.onClicked += Revive;
    }
    private void OnDestroy()
    {
        ReviveButton.onClicked -= Revive;
    }

    public void ReTryCallBack()
    {
        AudioManager.instance.PlaySoundEffect(7);

        LoadOut();
        StartCoroutine(LoadGame());
    }
    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(0.3f);
        loadingUI.SetActive(true);
        loadingGamePlay.SetActive(true);

        /*SceneManager.LoadScene(1);*/
    }
    
    public override void LoadIn()
    {
        collectPiece.text = ""+LevelManager.Instance.piecesCount;
        goldText.text = ""+StatsManager.Instance.GetCurrentGolds();
        levelText.text ="Level "+ StatsManager.Instance.GetSelectLevel();
        losePanels.transform.localPosition= new Vector3 (0, -2191, 0);

        LeanTween.moveLocalY(losePanels, 0, 0.25f).setEase(LeanTweenType.easeInBack);
    }

    public override void LoadOut()
    {
        LeanTween.moveLocalY(losePanels, -2191, 0.25f).setEase(LeanTweenType.easeInBack);
    }
    private void Revive()
    {
        if (StatsManager.Instance.GetCurrentGolds() < 1000)
            return;

        AudioManager.instance.PlaySoundEffect(7);

        List<GameObject> listHexStack = new List<GameObject>();
        GameObject parent = LevelManager.Instance.levelSpawner.gameObject;
        GetGridContainHexStack(parent, listHexStack);

        RemoveHexStack(listHexStack, 3);
        StatsManager.Instance.IncreasedHeart(1);
        GameManager.instance.SetGameState(GameState.Play);
        Invoke("()=>GameManager.instance.SetGameState(GameState.Play)",0.01f);

    }
    //return grid contain hexstack
    private void GetGridContainHexStack(GameObject parent, List<GameObject> list)
    {
        foreach (Transform child in parent.transform) 
        {
            GetGridContainHexStack(child.gameObject, list);

            if (child.GetComponent<HexStack>() != null)
                list.Add(child.gameObject);
        }
    }

    private void RemoveHexStack(List<GameObject> gb, int n)
    {
        if (gb.Count < n)
        {
            foreach (GameObject chill in gb)
            {
                Destroy(chill);
            }
            gb.Clear();
        }
        else
        {
            for (int i = 0; i < n; i++)
            {
                int index = Random.Range(0, gb.Count);
                Destroy(gb[index]); 
                gb.RemoveAt(index);
            }
        }
    }



}

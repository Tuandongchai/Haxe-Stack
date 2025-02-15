using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseUIAnimation : MonoBehaviour
{
    [Header("Element")]
    public GameObject losePanels;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI collectPiece;

    public void ReTryCallBack()
    {
        LoadOut();
        StartCoroutine(LoadGame());

    }
    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(1);
    }

    public void LoadIn()
    {
        collectPiece.text = ""+LevelManager.Instance.piecesCount;
        goldText.text = ""+StatsManager.Instance.GetCurrentGolds();
        levelText.text ="Level "+ StatsManager.Instance.GetCurrentLevel();
        losePanels.transform.localPosition= new Vector3 (0, -2191, 0);

        LeanTween.moveLocalY(losePanels, 0, 0.25f).setEase(LeanTweenType.easeInBack);
    }

    public void LoadOut()
    {
        LeanTween.moveLocalY(losePanels, -2191, 0.25f).setEase(LeanTweenType.easeInBack);
    }
}

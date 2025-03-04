using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MenuUIAnimation : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private float duration;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject shaderButton;
    [SerializeField] private GameObject avata;
    [SerializeField] private GameObject settingButton;
    [SerializeField] private GameObject gold;
    [SerializeField] private GameObject heath;
    [SerializeField] private GameObject menu;

    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI heartText;
    [SerializeField] private TextMeshProUGUI goldText;


    [SerializeField] private GameObject bg;

    private void Start()
    {
        Show();
    }

    public void StartAnimation()
    {

        gameObject.SetActive(true);
    }
    public void LoadGamePlayScence()
    {
        LeanTween.moveY(startButton,-1396f, duration).setEase(LeanTweenType.easeInBack);
        LeanTween.moveY(shaderButton, -1396f, duration).setEase(LeanTweenType.easeInBack);
        LeanTween.moveY(menu, -1396f, duration).setEase(LeanTweenType.easeInBack);

        GameObject[] a = new GameObject[] {avata, settingButton, gold, heath};
        foreach (GameObject go in a)
        {
           LeanTween.moveY(go, 4500, duration).setEase(LeanTweenType.easeInBack);
        }
        StartCoroutine(LoadBG());
    }
    IEnumerator LoadBG()
    {
        yield return new WaitForSeconds(duration);
        RectTransform rect = bg.GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(0, 0);
        rect.anchoredPosition = new Vector2(0, 0);
        bg.SetActive(true);
        LeanTween.value(bg, rect.sizeDelta, new Vector2(1078.9f, 2333.8f), 0.5f)
            .setEase(LeanTweenType.easeOutQuint)
            .setOnUpdate((Vector2 value) => rect.sizeDelta = value);
    }
    public void LoadIn()
    {
        LeanTween.moveLocalY(startButton, -603, duration).setEase(LeanTweenType.easeInBack);
        LeanTween.moveLocalY(shaderButton, -219f, duration).setEase(LeanTweenType.easeInBack);
        LeanTween.moveLocalY(menu, -1013.602f, duration).setEase(LeanTweenType.easeInBack);

        GameObject[] a = new GameObject[] { avata, settingButton, gold, heath};
        foreach (GameObject go in a)
        {
            LeanTween.moveLocalY(go, 916, duration).setEase(LeanTweenType.easeInBack);
        }
        Show();
    }
    public void LoadOut()
    {
        LeanTween.moveLocalY(startButton, -1396f, duration).setEase(LeanTweenType.easeInBack);
        LeanTween.moveLocalY(shaderButton, -1396f, duration).setEase(LeanTweenType.easeInBack);
        LeanTween.moveY(menu, -1396f, duration).setEase(LeanTweenType.easeInBack);

        GameObject[] a = new GameObject[] { avata, settingButton, gold, heath};
        foreach (GameObject go in a)
        {
            LeanTween.moveLocalY(go, 1400, duration).setEase(LeanTweenType.easeInBack);
        }
    }
    
    public void Show()
    {
        levelText.text = "Level " + StatsManager.Instance.GetCurrentLevel();
        goldText.text = StatsManager.Instance.GetCurrentGolds().ToString();
        ButtonAnimation(startButton);
        ButtonAnimation(shaderButton);
    }

    public void ButtonAnimation(GameObject ob)
    {
        float randomScale = Random.Range(0.8f, 1.2f);
        LeanTween.scale(ob, new Vector3(1.1f, 1.1f, 1.1f), 1f)
            .setEase(LeanTweenType.easeInOutSine)
            .setDelay(2)
            .setOnComplete(() =>
                LeanTween.scale(ob, new Vector3(0.9f, 0.9f, 0.9f), 1f)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() => ButtonAnimation(ob))
            );
    }

}

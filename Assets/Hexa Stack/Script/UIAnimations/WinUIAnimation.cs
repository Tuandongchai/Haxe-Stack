using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUIAnimation : MonoBehaviour
{
    [Header("Element")]
    public GameObject winPanel;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI piecesText;
    [SerializeField] private TextMeshProUGUI goldReceived;
    [SerializeField] private TextMeshProUGUI sunReceived;
    [SerializeField] private GameObject coinPanel;
    


    [Header("Setting")]
    [SerializeField] private GameObject coinParent;
    [SerializeField] private GameObject coinEnd;
    float duration = 0.5f;

    private void Start()
    {
        CloseWinPanelButton.onClicked += Close;
    }
    private void OnDestroy()
    {
        CloseWinPanelButton.onClicked -= Close;
    }

    private void Close()
    {
        AudioManager.instance.PlaySoundEffect(7);

        LoadOut();
        StartCoroutine(ClosePanel());
    }
    IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(0.4f);
        StatsManager.Instance.IncreasedSelectLevel();
        SceneManager.LoadScene(0);

    }
    public void LoadIn()
    {
        levelText.text = "Level "+ StatsManager.Instance.GetSelectLevel();
        goldText.text = "" + StatsManager.Instance.GetCurrentGolds();
        piecesText.text = LevelManager.Instance.piecesRequire.ToString();
        goldReceived.text = "100";
        sunReceived.text = LevelManager.Instance.piecesRequire.ToString();

        winPanel.transform.localScale = Vector3.zero;
        LeanTween.scale(winPanel, new Vector3(1,1,1), 0.6f).setEase(LeanTweenType.easeOutBounce);

        LeanTween.delayedCall(0.5f, () => CollectCoins());
    }
    public void LoadOut()
    {
        winPanel.transform.localScale = Vector3.one;
        LeanTween.scale(winPanel, new Vector3(0, 0, 0), 0.4f).setEase(LeanTweenType.easeOutBounce);
    }
    public void CollectCoins()
    {
        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnCoins()
    {
        int coinCount = Random.Range(10, 20);
        GameObject[] coins = new GameObject[coinCount];

        for (int i = 0; i < coinCount; i++)
        {
            coins[i] = Instantiate(
                coinPrefab,
                coinParent.transform.position + new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0),
                Quaternion.Euler(0, 0, Random.Range(-180, 180))
            );
            coins[i].transform.SetParent(coinParent.transform);
        }

        for (int i = 0; i < coinCount; i++)
        {
            LeanTween.move(coins[i], coinEnd.transform.position , 0.4f)
                .setEase(LeanTweenType.easeInBack).setOnComplete(CoinPanelAnimation);
            yield return new WaitForSeconds(0.05f); // Chờ trước khi coin tiếp theo bay
        }
        /*yield return new WaitForSeconds(0f);*/
        int currentCoin = int.Parse(goldText.text);

        LeanTween.value(gameObject, currentCoin, currentCoin+100, 0.4f)
            .setOnUpdate((float val) =>
            {
                goldText.text = Mathf.FloorToInt(val).ToString(); // Cập nhật UI với số nguyên
            })
            .setEase(LeanTweenType.easeOutQuad);

        LeanTween.value(gameObject, int.Parse(goldReceived.text), 0, 0.4f)
            .setOnUpdate((float val) =>
            {
                goldReceived.text = Mathf.FloorToInt(val).ToString(); // Cập nhật UI với số nguyên
            })
            .setEase(LeanTweenType.easeOutQuad);
        StatsManager.Instance.IncreasedGolds(100);
        float delay= 0.5f;
        yield return new WaitForSeconds(delay);
        coinPanel.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        DestroyCoin(coinCount, coins);
        
    }
    private void DestroyCoin(int count, GameObject[] coins )
    {
        for (int i = 0; i < count; i++)
        {
            Destroy(coins[i]);
        }
    }
    private void CoinPanelAnimation()
    {
        float randomScale = Random.Range(0.8f, 2f);

        LeanTween.scale(coinPanel, Vector3.one * randomScale, 0.04f)
            .setEase(LeanTweenType.easeInOutSine);
            
    }

    //chay animation lien tuc sau mot thoi gian thi dung
    private void Animation()
    {
        float randomScale = Random.Range(1.1f, 1.6f);

        LeanTween.scale(coinPanel, Vector3.one * randomScale, 0.05f)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                float newRandomScale = Random.Range(0.8f, 1.4f);
                LeanTween.scale(coinPanel, Vector3.one * newRandomScale, 0.05f)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setOnComplete(Animation); // Lặp lại hiệu ứng
            });
        // Dừng sau 0.5 giây
        LeanTween.delayedCall(0.5f, () =>
        {
            LeanTween.cancel(coinPanel); // Hủy mọi LeanTween đang chạy trên object này
        });
    }



}

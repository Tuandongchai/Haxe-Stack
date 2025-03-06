using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour
{

    public static StatsManager Instance;

    [Header("Heart")]
    [SerializeField] private int maxHearts = 5; // S? m?ng t?i ?a
    private int currentHearts;
    private int heartRecoveryTime = 300; // 5 phút (tính theo giây)
    private float timer;

    [Header("Gold")]
    [SerializeField] private int currentGolds;

    [Header ("Sun")]
    [SerializeField] private float currentSuns;

    [Header("Level")]
    [SerializeField] private int currentLevel;

    [Header("Tools")]
    [SerializeField] private int currentHammer;
    [SerializeField] private int currentLightning;
    [SerializeField] private int currentRolls;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        if (!PlayerPrefs.HasKey("Hearts"))
            PlayerPrefs.SetInt("Hearts", 5);

        if (!PlayerPrefs.HasKey("Golds"))
            PlayerPrefs.SetInt("Golds", 100);

        if (!PlayerPrefs.HasKey("Suns"))
            PlayerPrefs.SetFloat("Suns", 30);

        if (!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", 0);

        if (!PlayerPrefs.HasKey("Hammers"))
            PlayerPrefs.SetInt("Hammers", 2);

        if (!PlayerPrefs.HasKey("Lightning"))
            PlayerPrefs.SetInt("Lightning", 2);

        if (!PlayerPrefs.HasKey("Rolls"))
            PlayerPrefs.SetInt("Rolls", 2);

        if (!PlayerPrefs.HasKey("SelectLevel"))
            PlayerPrefs.SetInt("SelectLevel", 0);
    }

    private void Start()
    {
        // L?y s? tim hi?n t?i
        SetCurrentLevel(0);
        SetSelectLevel(0);


        currentHearts = PlayerPrefs.GetInt("Hearts", maxHearts);

        currentGolds = PlayerPrefs.GetInt("Golds", currentGolds);

        currentSuns = PlayerPrefs.GetFloat("Suns", currentSuns);

        currentLevel = PlayerPrefs.GetInt("Level", currentLevel);

        currentHammer = PlayerPrefs.GetInt("Hammers", currentHammer);

        currentLightning = PlayerPrefs.GetInt("Lightning", currentLightning);

        currentRolls = PlayerPrefs.GetInt("Rolls", currentRolls);

        // Ki?m tra th?i gian k? t? l?n cu?i thoát game
        string lastTimeString = PlayerPrefs.GetString("LastPlayTime", "");
        if (!string.IsNullOrEmpty(lastTimeString))
        {
            DateTime lastPlayTime = DateTime.Parse(lastTimeString);
            TimeSpan timeAway = DateTime.Now - lastPlayTime;

            // Tính s? tim ???c h?i
            int heartsRecovered = (int)(timeAway.TotalSeconds / heartRecoveryTime);
            currentHearts = Mathf.Min(currentHearts + heartsRecovered, maxHearts);

            // Tính th?i gian còn l?i cho tim ti?p theo
            timer = heartRecoveryTime - (float)(timeAway.TotalSeconds % heartRecoveryTime);
        }
        else
        {
            timer = heartRecoveryTime;
        }

        // L?u d? li?u
        PlayerPrefs.SetInt("Hearts", currentHearts);
        PlayerPrefs.SetString("LastPlayTime", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    private void Update()
    {
        if (currentHearts < maxHearts)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                currentHearts++;
                PlayerPrefs.SetInt("Hearts", currentHearts);
                PlayerPrefs.Save();
                timer = heartRecoveryTime;
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastPlayTime", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    public void UseHeart()
    {
        if (currentHearts > 0)
        {
            currentHearts--;
            PlayerPrefs.SetInt("Hearts", currentHearts);
            PlayerPrefs.Save();
        }
    }
    public void IncreasedHeart(int a)
    {
        currentHearts+=a;
        if(currentHearts>5)
            currentHearts=5;
        PlayerPrefs.SetInt("Hearts", currentHearts);
        PlayerPrefs.Save();
    }

    public int GetCurrentHearts()
    {
        return currentHearts;
    }
    public int GetCurrentGolds()
    {
        return currentGolds;
    }

    public void IncreasedGolds(int golds)
    {
        currentGolds += golds;

        PlayerPrefs.SetInt("Golds", currentGolds);
        PlayerPrefs.Save();
    }
    public void UseGold(int golds)
    {
        if (golds>currentGolds)
            return;
        currentGolds -= golds;

        PlayerPrefs.SetInt("Golds", currentGolds);
        PlayerPrefs.Save();
    }
    //
    public float GetCurrentSuns()
    {
        return currentSuns;
    }

    public void IncreasedSuns(float sun)
    {
        currentSuns += sun;

        PlayerPrefs.SetFloat("Suns", currentSuns);
        PlayerPrefs.Save();
    }
    public void UseSuns(float sun)
    {
        if (sun > currentSuns)
            return;
        currentSuns -= sun;

        PlayerPrefs.SetFloat("Suns", currentSuns);
        PlayerPrefs.Save();
    }
    //
    public void SetCurrentLevel(int lv) => PlayerPrefs.SetInt("Level", lv);
    public int GetCurrentLevel()=> PlayerPrefs.GetInt("Level");

    public void IncreasedLevel()
    {
        currentLevel ++;
        
        PlayerPrefs.SetInt("Level", currentLevel);
        PlayerPrefs.Save();
    }
    public void SetSelectLevel(int lv)
    {
        PlayerPrefs.SetInt("SelectLevel", lv);
        PlayerPrefs.Save();
    }
    public int GetSelectLevel() => PlayerPrefs.GetInt("SelectLevel");

    public void IncreasedSelectLevel()
    {
        PlayerPrefs.SetInt("SelectLevel", GetSelectLevel()+1);
        if(GetSelectLevel()>=GetCurrentLevel())
            SetCurrentLevel(GetSelectLevel());
        PlayerPrefs.Save();
    }
        
    public int GetTool(int a)
    {
        switch (a)
        {
            case 0:
                return PlayerPrefs.GetInt("Hammers");
            case 1:
                return PlayerPrefs.GetInt("Lightning");
            case 2:
                return PlayerPrefs.GetInt("Rolls");
            default:
                return 0;
        }
    }
    public void UseTool(int a)
    {
        switch (a)
        {
            case 0:
                currentHammer--;
                PlayerPrefs.SetInt("Hammers", currentHammer);
                PlayerPrefs.Save();
                break;
            case 1:
                currentLightning--;
                PlayerPrefs.SetInt("Lightning", currentLightning);
                PlayerPrefs.Save();
                break;
            case 2:
                currentRolls--;
                PlayerPrefs.SetInt("Rolls", currentRolls);
                PlayerPrefs.Save();
                break;
            default:
                break;
        }
    }
    public void IncreasedTool(int a, int b)
    {
        switch (a)
        {
            case 0:
                currentHammer+=b;
                PlayerPrefs.SetInt("Hammers", currentHammer);
                PlayerPrefs.Save();
                break;
            case 1:
                currentLightning+=b;
                PlayerPrefs.SetInt("Lightning", currentLightning);
                PlayerPrefs.Save();
                break;
            case 2:
                currentRolls+=b;
                PlayerPrefs.SetInt("Rolls", currentRolls);
                PlayerPrefs.Save();
                break;
            default:
                break;
        }
    }

}

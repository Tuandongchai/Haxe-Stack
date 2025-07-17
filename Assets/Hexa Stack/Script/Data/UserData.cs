using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Unity.Mathematics;
using System.Threading;
using UnityEngine.SocialPlatforms.Impl;
using System.Data;

public class UserData : MonoBehaviour
{
    private string filePath;
    public static UserData instance;
    private List<string> fullNameList;
/*    private List<string> lastNameList;
    private List<string> middleNameList;*/
    private List<string> firstNameList;

    public Sprite[] rankSpriteList;
    [SerializeField] private int[] rankList;

    public  Sprite[] avataSpriteList;
    [SerializeField] private int[] avataList;
    private int[] scoreList= new int[30];

    private List<int[]> inforList= new List<int[]>();
    public Dictionary<string, int[]> data;
    public Dictionary<string, int[]> dataSorted;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        /*lastNameList = new List<string>() {
            "Nguyen", "Tran", "Le", "Ma", "Pham", "Huynh", "Hoang", "Phan", 
            "Vu", "Vo", "Dang", "Bui", "Do", "Ho", "Ngo", "Duong", "Ly", "Trinh"
        };
    
        middleNameList = new List<string>()
        {
            "Van", "Thi", "Huu", "Minh", "Ngoc", "Bao", "Gia", "Anh", "Hoai",
            "Hai", "Đuc", "Nhat", "Quang", "Thanh", "Khanh", "Thien", "Tan",
            "Trong", "Chi", "Xuan", "Truc"
        };*/
        firstNameList = new List<string>()
        {
            "Tuan", "Dung", "Hung", "Son", "Long", "Phuc", "Loc", "Thinh",
            "Quang", "Nam", "Hai", "Dat", "Cưong", "Tai", "Huy", "Thanh", "Linh", "Trang", "Hanh", "Huong", "Thao", "Thu", "Nhung", "Yen",
            "Hoa", "Mai", "Ngan", "Diep", "Oanh", "Quynh", "Vy", "Nguyet",
            "Dung", "Anh", "Bich", "Loan",
            "Nghia", "Kiet", "Khang", "Bao"
        };
        rankList = new int[] {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19 };
        avataList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        }
    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "userData.json");
        if (!File.Exists(filePath))
            SaveData();
        /*SaveData();*/
        dataSorted = LoadData();
    }
    public void SaveData(Dictionary<string, int[]> data = null)
    {
        if (data == null)
        {
            fullNameList = new List<string>();
            inforList = new List<int[]>();

            GenerateUser(30);
            data = new Dictionary<string, int[]> ();
            for (int i=0; i<30; i++)
            {
                data.Add(fullNameList[i], inforList[i].ToArray());
            }
            data.Add(GameData.instance.GetName(),new int[]{2,2, StatsManager.Instance.GetCurrentLevel() });

            dataSorted = new Dictionary<string, int[]>();
            dataSorted = SortedDict(data);
        }
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filePath, json);
        
    }
    private void GenerateUser(int count)
    {
        GenerateNames(count);
        GenerateScore(count);
        GenerateInfors(count);
    }
    public void GenerateNames(int count)
    {
        System.Random random = new System.Random();
        for(int i = 0; i < count; i++)
        {
/*            string lastName = lastNameList[random.Next(lastNameList.Count)];
            string middleName = middleNameList[random.Next(middleNameList.Count)];*/
            string firstName = firstNameList[i];
/*            string fullName = $"{lastName} {middleName} {firstName}";*/
            fullNameList.Add(firstName);
        }
        
    }
    public void GenerateInfors(int count)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < count; i++)
        {
            int[] inforUser = new int[] { rankList[random.Next(rankList.Length)], avataList[random.Next(avataList.Length)], scoreList[i] };
            inforList.Add(inforUser);
        }

    }
    public Dictionary<string, int[]> LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Dictionary<string, int[]> data = JsonConvert.DeserializeObject<Dictionary<string, int[]>>(json);
            return data;
        }
        else
        {
            Debug.Log("No save file found!");
            return null;
        }
    }
    public void GenerateScore(int core)
    {
        System.Random random = new System.Random();
        int currentPlayer= StatsManager.Instance.GetCurrentLevel();
        for(int i = 0; i<core; i++)
        {
            scoreList[i] = random.Next(currentPlayer, currentPlayer+50);
        }
    }
    //chua check duoc UpdateScore dung hay sai
    //dang dang gap bug singleton 
    public void UpdateScore(int core)
    {
        System.Random random = new System.Random();
        Dictionary<string, int[]> newData = LoadData();
        for (int i = 0; i < core; i++)
        {
            if (newData.ElementAt(i).Key == GameData.instance.GetName())
            {
                newData.ElementAt(i).Value[2] = StatsManager.Instance.GetCurrentLevel();
                break;
            }

            newData.ElementAt(i).Value[2] = random.Next(newData.ElementAt(i).Value[2], newData.ElementAt(i).Value[2] + 30);
           
        }
        SaveData(SortedDict(newData));
    }
    public Dictionary<string, int[]> SortedDict(Dictionary<string, int[]> myDict)
    {
        var sortedDict = myDict.OrderByDescending(x => x.Value[2])
                              .ToDictionary(x => x.Key, x => x.Value);
        Debug.Log(sortedDict["hello"].GetValue(2));
        return sortedDict; 

    }
    public void UpdatePlayerScore(int core)
    {
        Dictionary<string, int[]> newData = LoadData();
        for (int i = 0; i < core; i++)
        {
            if (newData.ElementAt(i).Key == GameData.instance.GetName())
            {
                newData.ElementAt(i).Value[2] = StatsManager.Instance.GetCurrentLevel();
                Debug.Log(newData.ElementAt(i).Value[2]) ;
                Debug.Log(StatsManager.Instance.GetCurrentLevel());
                SaveData(SortedDict(newData));
            }
           
        }
        
    }
    
    public Sprite FindAvatarByName(string name)
    {
        Dictionary<string, int[]> data = LoadData();
        List<string> listName = data.Keys.ToList();
        int i =listName.IndexOf(name);
        return avataSpriteList[data.ElementAt(i).Value[1]];
    }
}

using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

public class GameData : MonoBehaviour
{
    private string filePath;
    public static GameData instance;
    public Dictionary<string, object> data;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        if (!File.Exists(filePath))
            SaveData();
        /*SaveData();*/
        data =LoadData();  // Đọc dữ liệu từ file
        if (data["createdDay"] == "")
            data["createdDay"] = DateTime.Now.ToString("dd/MM/yyyy");
        if (data["currentDay"] == "")
            data["currentDay"] = DateTime.Now.ToString("dd/MM/yyyy");
        SaveData(data);

        CheckDay();
    }

    public void SaveData(Dictionary<string, object> data = null)
    {
        if (data == null)
        {
            data = new Dictionary<string, object>
            {
                { "playerName", "hello" },
                { "fill", new List<float> { 0f, -0.195f ,-0.95f,-0.81f} },
                { "object", 0},
                { "createdDay", ""},
                { "currentDay", ""},
                {"day", 1 },
                {"newUserDay", 1},
                {"attendanceDaily",new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}},
                {"attendanceNewUser",new int[]{0,0,0,0,0,0,0}},
                {"dailyQuestState", new int[]{-1,-1,-1,-1,-1,-1} },
                {"weeklyQuestState", new int[]{-1,-1,-1,-1,-1,-1} },
                {"totalQuantityDQ", new int[]{10,3,1000,3,3,3} },
                {"curentQuantityDQ", new int[]{0,0,0,0,0,0} },
                {"totalQuantityWQ", new int[]{60,30,10000,15,15,15} },
                {"curentQuantityWQ", new int[]{0,0,0,0,0,0} },
                {"DQBonusState", new int[]{-1,-1,-1 } },
                {"WQBonusState", new int[]{-1,-1,-1}}
            };
        }

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filePath, json);

    }

    public Dictionary<string, object> LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            // Chuyển kiểu cho dữ liệu "fill"
            List<float> fillList = JsonConvert.DeserializeObject<List<float>>(data["fill"].ToString());
            data["fill"] = fillList;
            Debug.Log($"Loaded Data: {data["playerName"]}, fill: {string.Join(", ", fillList)}, object: {(int)(long)data["object"]}");
            return data;
        }
        else
        {
            Debug.Log("No save file found!");
            return null;
        }
    }
    public void SavedData(Dictionary<string, object> data) => SaveData(data);
    public void UpdateFill(float newFill, int i)
    {
        Dictionary<string, object> data = LoadData();

        if (data != null)
        {
            // Lấy danh sách fill và cập nhật giá trị
            List<float> listFill = (List<float>)data["fill"];
            listFill[i] = newFill;
            data["fill"] = listFill;

            // Lưu lại sau khi cập nhật
            SaveData(data);
            Debug.Log("Updated fill and saved file.");
        }
    }
    public void UpdateObjectFill()
    {
        Dictionary<string, object> data = LoadData();
        if (data != null)
        {
            int n = (int)(long)data["object"];
            data["object"] = n+1;
            SaveData(data);
        }
    }
    public float GetFill(int i)
    {
        Dictionary<string, object> data = LoadData();
        List<float> listFill = (List<float>)data["fill"];
        //chua bo sung them gameObject tam thoi de if o day tranh loi
        if (i == 4)
            return i = 3;
        return listFill[i];

    }
    public string GetName()
    {
        Dictionary<string, object> data = LoadData();
        return data["playerName"].ToString();
    }

    public int GetObjectFill()
    {
        Dictionary <string, object> data = LoadData();
        //Co them object thi return sau sau
        return (int)(long)data["object"];
    }
    //dattendanceDaily
    public void CheckDay()
    {
        string currentDayStr = data["currentDay"].ToString();
        DateTime currentDay;
        if (DateTime.TryParseExact(currentDayStr, "dd/MM/yyyy",
    System.Globalization.CultureInfo.InvariantCulture,
    System.Globalization.DateTimeStyles.None, out currentDay))
        {
            if (currentDay.Date < DateTime.Now.Date)
            {
                data["day"] = int.Parse(data["day"].ToString()) + 1;

                data["newUserDay"] = int.Parse(data["newUserDay"].ToString()) + 1;
                data["currentDay"] = DateTime.Now.ToString("dd/MM/yyyy");

                SavedData(data);
                ResetDailyQuest();
                if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
                {
                    ResetWeeklyQuest();
                }

                Debug.Log("Check day Dữ liệu sau khi load lại lan nua: " + JsonConvert.SerializeObject(data, Formatting.Indented));

                //bug singleton chua duoc
                /*UserData.instance.UpdateScore(31);*/
            }
        }
    }
    public int[] GetDayArray()
    {
        Debug.Log(data["attendanceDaily"].GetType());
        int[] dayArray = ((JArray)data["attendanceDaily"]).ToObject<int[]>();
        Debug.Log($"lay: {string.Join(", ", dayArray)}");
        return dayArray;
    }
    public void SetDayArray(int[] newArray)
    {
        Debug.Log($"luu: {string.Join(", ", newArray)}");
        data["attendanceDaily"] = JArray.FromObject(newArray);
        SaveData(data);
    }
    public int GetDay()
    {
        return int.Parse(data["day"].ToString());
    }
    public int[] GetNUDayArray()
    {
        Debug.Log(data["attendanceNewUser"].GetType());
        int[] dayArray = ((JArray)data["attendanceNewUser"]).ToObject<int[]>();
        Debug.Log($"lay: {string.Join(", ", dayArray)}");
        return dayArray;
    }
    public void SetNUDayArray(int[] newArray)
    {
        Debug.Log($"luu: {string.Join(", ", newArray)}");
        data["attendanceNewUser"] = JArray.FromObject(newArray);
        SaveData(data);
    }
    public int GetNUDay()
    {
        return int.Parse(data["newUserDay"].ToString());
    }
    //DailyQuest
    public void ResetDailyQuest()
    {
        Dictionary<string, object> data = LoadData();

        Debug.Log("trước khi lưu: " + JsonConvert.SerializeObject(data, Formatting.Indented));

        data["dailyQuestState"] = new int[] { -1, -1, -1, -1, -1, -1 };
        data["curentQuantityDQ"] = new int[] { 0, 0, 0, 0, 0, 0 };


        SavedData(data);
        Debug.Log("khong chay");

        // Kiểm tra dữ liệu sau khi load lại
        Dictionary<string, object> newData = LoadData();
        Debug.Log("sau khi load lại: " + JsonConvert.SerializeObject(newData, Formatting.Indented));

        /*Dictionary<string, object> data = LoadData();
        data["dailyQuestState"] = new int[] { -1, -1, -1, -1, -1,-1 };
        data["curentQuantityDQ"] = new int[] { 0, 0, 0, 0, 0, 0 };
        SaveData(data);*/

    }
    public void SetClaimedDailyQuest(int i)
    {
        Dictionary<string, object> data = LoadData();
        int[] dailyQuestArray = ((JArray)data["dailyQuestState"]).ToObject<int[]>();
        dailyQuestArray[i] = 1;
        data["dailyQuestState"] = dailyQuestArray;
        SaveData(data);
    }
    public void SetCompleteDailyQuest(int i)
    {
        Dictionary<string, object> data = LoadData();
        int[] dailyQuestArray = ((JArray)data["dailyQuestState"]).ToObject<int[]>();
        dailyQuestArray[i] = 0;
        data["dailyQuestState"] = dailyQuestArray;
        SaveData(data);
    }
    public int[] GetDailyQuest()
    {
        Dictionary<string,object> data = LoadData();
        return ((JArray)data["dailyQuestState"]).ToObject<int[]>();
    }
    
    public int[] GetCurrentDailyQuest()
    {
        Dictionary<string, object> data = LoadData();
        return ((JArray)data["curentQuantityDQ"]).ToObject<int[]>(); ;
    }
    public int[] GetTotalDailyQuest()
    {
        return ((JArray)data["totalQuantityDQ"]).ToObject<int[]>(); ;
    }
    public void IncreatedCurrentDailyQuest(int id, int increateQuantity)
    {
        Dictionary<string, object> data = LoadData();
        int[] curentQuantityDQArray = ((JArray)data["curentQuantityDQ"]).ToObject<int[]>();
        curentQuantityDQArray[id] += increateQuantity;
        data["curentQuantityDQ"] = curentQuantityDQArray;
        SaveData(data);
    }
    //WeeklyQuest
    public void ResetWeeklyQuest()
    {
        Dictionary<string, object> data = LoadData();
        data["weeklyQuestState"] = new int[] { -1, -1, -1, -1, -1, -1 };
        data["curentQuantityWQ"] = new int[] { 0, 0, 0, 0, 0, 0 };
        SaveData(data);
    }
    public void SetClaimWeeklyQuest(int i)
    {
        Dictionary<string, object> data = LoadData();
        int[] weeklyQuestArray = ((JArray)data["weeklyQuestState"]).ToObject<int[]>();
        weeklyQuestArray[i] = 1;
        data["weeklyQuestState"] = weeklyQuestArray;
        SaveData(data);
    }
    public void SetCompleteWeeklyQuest(int i)
    {
        Dictionary<string, object> data = LoadData();
        int[] weeklyQuestArray = ((JArray)data["weeklyQuestState"]).ToObject<int[]>();
        weeklyQuestArray[i] = 0;
        data["weeklyQuestState"] = weeklyQuestArray;
        SaveData(data);
    }
    public int[] GetWeeklyQuest()
    {
        Dictionary<string, object> data = LoadData();
        return ((JArray)data["weeklyQuestState"]).ToObject<int[]>();
    }
    public int[] GetCurrentWeeklyQuest()
    {
        Dictionary<string, object> data = LoadData();
        return ((JArray)data["curentQuantityWQ"]).ToObject<int[]>(); ;
    }
    public int[] GetTotalWeeklyQuest()
    {
        return ((JArray)data["totalQuantityWQ"]).ToObject<int[]>(); ;
    }
    public void IncreatedCurrentWeeklyQuest(int id, int increateQuantity)
    {
        Dictionary<string, object> data = LoadData();
        int[] curentQuantityWQArray = ((JArray)data["curentQuantityWQ"]).ToObject<int[]>();
        curentQuantityWQArray[id] += increateQuantity;
        data["curentQuantityWQ"] = curentQuantityWQArray;
        SaveData(data);
    }

    //DailyQuestBonus
    public int GetDQBonusState(int _id)
    {
        Dictionary<string, object> data = LoadData() ;
        int[] DQBonusState = ((JArray)data["DQBonusState"]).ToObject<int[]>();
        return DQBonusState[_id];
    }
    public void SetClaimDQBonus(int i)
    {
        Dictionary<string, object> data = LoadData();
        int[] DQBonusState = ((JArray)data["DQBonusState"]).ToObject<int[]>();
        DQBonusState[i] = 1;
        data["DQBonusState"] = DQBonusState;
        SaveData(data);
    }
    public void SetCompleteDQBonus(int i)
    {
        Dictionary<string, object> data = LoadData();
        int[] DQBonusState = ((JArray)data["DQBonusState"]).ToObject<int[]>();
        DQBonusState[i] = 0;
        data["DQBonusState"] = DQBonusState;
        SaveData(data);
    }
    //WeeklyQuestBonus
    public int GetWQBonusState(int _id)
    {
        Dictionary<string, object> data = LoadData();
        int[] WQBonusState = ((JArray)data["WQBonusState"]).ToObject<int[]>();
        Debug.Log(_id);
        
        return WQBonusState[_id];
    }
    public void SetClaimWQBonus(int i)
    {
        Dictionary<string, object> data = LoadData();
        int[] WQBonusState = ((JArray)data["WQBonusState"]).ToObject<int[]>();
        WQBonusState[i] = 1;
        data["WQBonusState"] = WQBonusState;
        SaveData(data);
    }
    public void SetCompleteWQBonus(int i)
    {
        Dictionary<string, object> data = LoadData();
        int[] WQBonusState = ((JArray)data["WQBonusState"]).ToObject<int[]>();
        WQBonusState[i] = 0;
        data["WQBonusState"] = WQBonusState;
        SaveData(data);
    }
    
}

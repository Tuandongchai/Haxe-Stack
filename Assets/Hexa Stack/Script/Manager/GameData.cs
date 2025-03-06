using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

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
        /*if (!File.Exists(filePath))
            SaveData();*/
        SaveData();
        data =LoadData();  // Đọc dữ liệu từ file
        if (data["createdDay"] == "")
            data["createdDay"] = DateTime.Now.ToString("dd/MM/yyyy");
        if (data["currentDay"] == "")
            data["currentDay"] = DateTime.Now.ToString("dd/MM/yyyy");
        SaveData(data);
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
                {"attendanceNewUser",new int[]{0,0,0,0,0,0,0}}
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
        return (int)(long)data["object"];
    }

    public void CheckDay()
    {
        if (DateTime.Parse(data["currentDay"].ToString()).Date < DateTime.Now.Date)
        {
            data["day"] = int.Parse(data["day"].ToString()) + 1;
            data["currentDay"]=DateTime.Now.ToString("dd/MM/yyyy");
            SaveData(data);
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
}

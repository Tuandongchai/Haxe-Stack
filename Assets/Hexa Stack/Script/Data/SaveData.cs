using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ListWrapper<T>
{
    public List<T> items = new List<T>();
}

public class SaveData<T> where T : class
{
    private string filePath;

    public SaveData(string fileName)
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
    }

    public void Save(List<T> data)
    {
        ListWrapper<T> wrapper = new ListWrapper<T> { items = data };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"D? li?u ?ã ???c l?u vào: {filePath}");
    }

    public List<T> Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            ListWrapper<T> wrapper = JsonUtility.FromJson<ListWrapper<T>>(json);
            Debug.Log("D? li?u ?ã ???c t?i t? JSON");
            return wrapper.items;
        }
        else
        {
            Debug.LogWarning("Không tìm th?y file, t?o danh sách m?i.");
            return new List<T>();
        }
    }

    public void EditData(List<T> list, Predicate<T> match, Action<T> editAction)
    {
        T itemToEdit = list.Find(match);
        if (itemToEdit != null)
        {
            editAction(itemToEdit);
            Save(list);
            Debug.Log("?ã ch?nh s?a d? li?u.");
        }
        else
        {
            Debug.LogWarning("Không tìm th?y d? li?u ?? ch?nh s?a.");
        }
    }

    public T SearchData(List<T> list, Predicate<T> match)
    {
        return list.Find(match);
    }
}

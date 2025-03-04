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

    // ?? 1?? Constructor - Xác ??nh ???ng d?n file JSON
    public SaveData(string fileName)
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
    }

    // ?? 2?? L?u d? li?u xu?ng file JSON
    public void Save(List<T> data)
    {
        ListWrapper<T> wrapper = new ListWrapper<T> { items = data };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"D? li?u ?ã ???c l?u vào: {filePath}");
    }

    // ?? 3?? T?i d? li?u t? file JSON
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

    // ?? 4?? Thêm d? li?u vào danh sách
    /*public void AddData(List<T> list, T item)
    {
        list.Add(item);
        Save(list);
        Debug.Log("?ã thêm d? li?u.");
    }
*/
    // ?? 5?? Xóa d? li?u kh?i danh sách
   /* public void DeleteData(List<T> list, Predicate<T> match)
    {
        T itemToRemove = list.Find(match);
        if (itemToRemove != null)
        {
            list.Remove(itemToRemove);
            Save(list);
            Debug.Log("?ã xóa d? li?u.");
        }
        else
        {
            Debug.LogWarning("Không tìm th?y d? li?u ?? xóa.");
        }
    }*/

    // ?? 6?? Ch?nh s?a d? li?u
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

    // ?? 7?? Tìm ki?m d? li?u
    public T SearchData(List<T> list, Predicate<T> match)
    {
        return list.Find(match);
    }
}

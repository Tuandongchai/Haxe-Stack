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

    // ?? 1?? Constructor - X�c ??nh ???ng d?n file JSON
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
        Debug.Log($"D? li?u ?� ???c l?u v�o: {filePath}");
    }

    // ?? 3?? T?i d? li?u t? file JSON
    public List<T> Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            ListWrapper<T> wrapper = JsonUtility.FromJson<ListWrapper<T>>(json);
            Debug.Log("D? li?u ?� ???c t?i t? JSON");
            return wrapper.items;
        }
        else
        {
            Debug.LogWarning("Kh�ng t�m th?y file, t?o danh s�ch m?i.");
            return new List<T>();
        }
    }

    // ?? 4?? Th�m d? li?u v�o danh s�ch
    /*public void AddData(List<T> list, T item)
    {
        list.Add(item);
        Save(list);
        Debug.Log("?� th�m d? li?u.");
    }
*/
    // ?? 5?? X�a d? li?u kh?i danh s�ch
   /* public void DeleteData(List<T> list, Predicate<T> match)
    {
        T itemToRemove = list.Find(match);
        if (itemToRemove != null)
        {
            list.Remove(itemToRemove);
            Save(list);
            Debug.Log("?� x�a d? li?u.");
        }
        else
        {
            Debug.LogWarning("Kh�ng t�m th?y d? li?u ?? x�a.");
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
            Debug.Log("?� ch?nh s?a d? li?u.");
        }
        else
        {
            Debug.LogWarning("Kh�ng t�m th?y d? li?u ?? ch?nh s?a.");
        }
    }

    // ?? 7?? T�m ki?m d? li?u
    public T SearchData(List<T> list, Predicate<T> match)
    {
        return list.Find(match);
    }
}

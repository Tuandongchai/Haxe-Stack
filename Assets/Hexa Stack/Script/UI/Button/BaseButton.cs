using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    protected Button button;

    protected virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(()=>OnButtonClick());
    }
    protected abstract void OnButtonClick();
    
}
public abstract class BaseButton<T> : BaseButton
{
    protected T t;
    protected override void OnButtonClick()
    {
        OnButtonClick(t);
    }
    protected abstract void OnButtonClick(T t);
}
public abstract class BaseButton<T1, T2> : BaseButton
{
    protected T1 t1;
    protected T2 t2;
    protected override void OnButtonClick()
    {
        OnButtonClick(t1,t2);
    }
    protected abstract void OnButtonClick(T1 t1, T2 t2);
}


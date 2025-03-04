using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[RequireComponent(typeof(Renderer))]
public class Tower : FillElement
{

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void Fill(int count)
    {
        base.Fill(count);
    }

    protected override void UpdateMaterials()
    {
        base.UpdateMaterials();
    }

    protected override void SaveFill(int count)
    {
        base.SaveFill(count);
    }
}

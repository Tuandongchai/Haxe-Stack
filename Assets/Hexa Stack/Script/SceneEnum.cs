using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Scene {Menu, Game}
public class SceneEnum :MonoBehaviour{
    public static SceneEnum Instance { get; private set; }
}

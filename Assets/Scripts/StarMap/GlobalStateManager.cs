using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalStateManager", menuName = "LevelData/GlobalStateManager")]
public class GlobalStateManager : ScriptableObject
{
    public MapScriptableData loadThisMap;
}
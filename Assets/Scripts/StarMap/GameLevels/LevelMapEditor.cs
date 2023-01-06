using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMapEditor : MonoBehaviour
{
    public MapScriptableData targetMapEdit;

    public List<LevelMapObject> planets;

    public List<LevelMapObject> spaceDebris;

    public List<LevelMapObject> encounters;

    public List<LevelMapObject> station;

    public LevelMapObject Player;


    // prefab stuff
    public GameObject planetPrefab;
    
    public GameObject spaceDebrisPrefab;
    
    public GameObject encountersPrefab;

    public GameObject playerPrefab;

    public GameObject stationPrefab;
}


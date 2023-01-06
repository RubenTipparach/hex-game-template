using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static MapScriptableData;

[ExecuteInEditMode]
public class LevelMapObject : MonoBehaviour
{

    public enum LevelObjectType
    {
        ShipMapData,
        StationMapData,
        PlanetMapData,
        SpaceDebrisData,
    }

    //public string GetMapObjName
    //{
    //    get
    //    {
    //        return metaData.name;
    //    }
    //}

    //private ObjectMapData metaData;

    public LevelObjectType levelObjectType = LevelObjectType.ShipMapData;

    public bool isPlayer = false;

    public string customObjectName;

    //public ShipMapData shipIngame;

    //public StationMapData mainStation;

    //public PlanetMapData planets;

    //public SpaceDebrisData debridFields;

    private void Update()
    {
        float x = transform.position.x;
        float y = transform.position.z;

        
        //if (levelObjectType == LevelObjectType.ShipMapData)
        //{
        //    shipIngame.mapPosition = new Vector2(x, y);
        //    shipIngame.rotationYAxis = transform.rotation.eulerAngles.y;
        //    metaData = shipIngame;
        //}

        //if (levelObjectType == LevelObjectType.StationMapData)
        //{
        //    mainStation.mapPosition = new Vector2(x, y);
        //    mainStation.rotationYAxis = transform.rotation.eulerAngles.y;

        //    metaData = mainStation;
        //}

        //if (levelObjectType == LevelObjectType.PlanetMapData)
        //{
        //    planets.mapPosition = new Vector2(x, y);
        //    planets.rotationYAxis = transform.rotation.eulerAngles.y;

        //    metaData = planets;
        //}

        //if (levelObjectType == LevelObjectType.SpaceDebrisData)
        //{
        //    debridFields.mapPosition = new Vector2(x, y);
        //    debridFields.rotationYAxis = transform.rotation.eulerAngles.y;

        //    metaData = debridFields;
        //}
    }

    private void OnDrawGizmos()
    { 
        //if(metaData!=null)
        //    Handles.Label(transform.position + Vector3.up * 250f, $"{metaData.name}");
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelMapObject))]
public class LevelMapObjectEditor : Editor
{
    SerializedProperty objectType;

    SerializedProperty shipObject;

    SerializedProperty stationObject;

    SerializedProperty planetObject;

    SerializedProperty debirsFieldObject;


    void OnEnable()
    {
        objectType = serializedObject.FindProperty("levelObjectType");
        shipObject = serializedObject.FindProperty("shipIngame");

        stationObject = serializedObject.FindProperty("mainStation");
        planetObject = serializedObject.FindProperty("planets");
        debirsFieldObject = serializedObject.FindProperty("debridFields");

    }

    public override void OnInspectorGUI()
    {
        LevelMapObject lmo = (LevelMapObject)target;


        EditorGUILayout.PropertyField(objectType, new GUIContent("objectType"));


        if (lmo.levelObjectType == LevelMapObject.LevelObjectType.ShipMapData)
        {
            EditorGUILayout.PropertyField(shipObject, true);
            lmo.isPlayer = EditorGUILayout.Toggle("Is Player" , lmo.isPlayer);
        }


        if (lmo.levelObjectType == LevelMapObject.LevelObjectType.StationMapData)
        {
            EditorGUILayout.PropertyField(stationObject, true);
        }

        if (lmo.levelObjectType == LevelMapObject.LevelObjectType.PlanetMapData)
        {

            EditorGUILayout.PropertyField(planetObject, true);
        }

        if (lmo.levelObjectType == LevelMapObject.LevelObjectType.SpaceDebrisData)
        {

            EditorGUILayout.PropertyField(debirsFieldObject, true);
        }

        serializedObject.ApplyModifiedProperties();

    }
}

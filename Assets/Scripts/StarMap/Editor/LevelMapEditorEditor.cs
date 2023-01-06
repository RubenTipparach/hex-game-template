using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelMapEditor))]
public class LevelMapEditorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        LevelMapEditor lme = (LevelMapEditor)target;
        DrawDefaultInspector();

        if(GUILayout.Button("Save"))
        {
            LevelMapObject[] mapObjects = FindObjectsOfType<LevelMapObject>();

            Debug.Log("Saving game objects.....");

            lme.planets = new List<LevelMapObject>();
            lme.spaceDebris = new List<LevelMapObject>();
            lme.encounters = new List<LevelMapObject>();
            lme.station = new List<LevelMapObject>();

            foreach (LevelMapObject mo in mapObjects)
            {
                //Debug.Log($"{mo.GetMapObjName}");

                if(!mo.gameObject.activeInHierarchy)
                {
                    continue;
                }

                // If object is a ship at all :)
                if(mo.isPlayer)
                {
                    lme.Player = mo;
                }
                else if(mo.levelObjectType == LevelMapObject.LevelObjectType.ShipMapData)
                {
                    lme.encounters.Add(mo);
                }

                // Planets!
                if (mo.levelObjectType == LevelMapObject.LevelObjectType.PlanetMapData)
                {
                    lme.planets.Add(mo);
                }
                
                // Space junk.
                if (mo.levelObjectType == LevelMapObject.LevelObjectType.SpaceDebrisData)
                {
                    lme.spaceDebris.Add(mo);
                }

                // Stations.
                if (mo.levelObjectType == LevelMapObject.LevelObjectType.StationMapData)
                {
                    lme.station.Add(mo);
                }
            }

            if (lme.targetMapEdit != null)
            {

                // Save player's ship.
                //lme.targetMapEdit.playerShipStart = lme.Player.shipIngame;

                //lme.targetMapEdit.planets = lme.planets.Select(p => p.planets).ToList();
                //lme.targetMapEdit.encounters = lme.encounters.Select(p => p.shipIngame).ToList();
                //lme.targetMapEdit.debridFields = lme.spaceDebris.Select(p => p.debridFields).ToList();
                //lme.targetMapEdit.stations = lme.station.Select(p => p.mainStation).ToList();

                EditorUtility.SetDirty(lme.targetMapEdit);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.Log("UNABLE TO SAVE!!!!");

            }
        }


        if (GUILayout.Button("Load/Reset Map"))
        {
            Debug.Log("Loading Map objects.....");

            LevelMapObject[] mapObjects = FindObjectsOfType<LevelMapObject>();

            lme.planets = new List<LevelMapObject>();
            lme.spaceDebris = new List<LevelMapObject>();
            lme.encounters = new List<LevelMapObject>();
            lme.station = new List<LevelMapObject>();

            foreach (LevelMapObject mo in mapObjects)
            {
                Debug.Log($"destroying.... {mo.gameObject.name}");
                DestroyImmediate(mo.gameObject);
            }

            // TODO: Load rotation.
            if(lme.targetMapEdit != null)
            {
                //// load player
                //var player = lme.targetMapEdit.playerShipStart;
                //lme.Player = Instantiate(lme.playerPrefab).GetComponent<LevelMapObject>();
                //lme.Player.isPlayer = true;
                //lme.Player.shipIngame = player;
                //lme.Player.transform.position = 
                //    new Vector3(player.mapPosition.x, 0, player.mapPosition.y);
                //lme.Player.transform.rotation = Quaternion.Euler(0, player.rotationYAxis,0);

                //// load planets
                //foreach (var p in lme.targetMapEdit.planets)
                //{
                //    var planetInstance = Instantiate(lme.planetPrefab).GetComponent<LevelMapObject>();
                //    planetInstance.planets = p;
                //    planetInstance.transform.position = new Vector3(p.mapPosition.x, 0, p.mapPosition.y);
                //    planetInstance.transform.rotation = Quaternion.Euler(0, p.rotationYAxis, 0);

                //    lme.planets.Add(planetInstance);
                //}

                ////load encounters
                //foreach (var e in lme.targetMapEdit.encounters)
                //{
                //    var encounterInstance = Instantiate(lme.encountersPrefab).GetComponent<LevelMapObject>();
                //    encounterInstance.shipIngame = e;
                //    encounterInstance.transform.position = new Vector3(e.mapPosition.x, 0, e.mapPosition.y);
                //    encounterInstance.transform.rotation = Quaternion.Euler(0, e.rotationYAxis, 0);

                //    lme.encounters.Add(encounterInstance);
                //}

                //// load debris fields
                //foreach (var d in lme.targetMapEdit.debridFields)
                //{
                //    var debrisInstance = Instantiate(lme.spaceDebrisPrefab).GetComponent<LevelMapObject>();
                //    debrisInstance.debridFields = d;
                //    debrisInstance.transform.position = new Vector3(d.mapPosition.x, 0, d.mapPosition.y);
                //    debrisInstance.transform.rotation = Quaternion.Euler(0, d.rotationYAxis, 0);

                //    lme.spaceDebris.Add(debrisInstance);

                //}

                //// load stations
                ////lme.targetMapEdit.stations = lme.station.Select(p => p.mainStation).ToList();
                //foreach (var s in lme.targetMapEdit.stations)
                //{
                //    var stationInstance = Instantiate(lme.stationPrefab).GetComponent<LevelMapObject>();
                //    stationInstance.mainStation = s;
                //    stationInstance.transform.position = new Vector3(s.mapPosition.x, 0, s.mapPosition.y);
                //    stationInstance.transform.rotation = Quaternion.Euler(0, s.rotationYAxis, 0);

                //    lme.station.Add(stationInstance);

                //}
            }
        }
    }
}

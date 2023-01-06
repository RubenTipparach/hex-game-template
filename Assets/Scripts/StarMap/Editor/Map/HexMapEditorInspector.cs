using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexMapEditor))]
public class HexMapEditorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        HexMapEditor hme = (HexMapEditor)target;
        DrawDefaultInspector();

        //EditorGUI.DrawPreviewTexture(new Rect(10, 60, 64, 64), arc.uiSprite.texture);

        if (GUILayout.Button("Save"))
        {
            if (hme.hexMap != null)
            {
                Debug.Log("Saving Map");

                hme.hexMap.height = hme.hexGrid.height;
                hme.hexMap.width = hme.hexGrid.width;
                hme.hexMap.hexCells = new List<HexCellData>();
                //hme.hexGrid.ForEach(p =>
                //{
                //    //copy from mem to disk.
                //    hme.hexMap.hexCells.Add(new HexCellData() { coordinates = p.coordinates, color = p.color });
                //});
                hme.hexMap.hexCells.AddRange(hme.hexGrid.cells.AsEnumerable()
                    .Select(p =>
                    {
                        var hexCell =
                            new HexCellData()
                            {
                                x = p.coordinates.X,
                                z = p.coordinates.Z,

                                raceType = p.raceType,


                                hasEnemy = p.hasEnemy,
                                hasPlanet = p.hasPlanet,
                                hasStation = p.hasStation,
                            };

                        hexCell.hasPlayer = p.Unit != null ? p.Unit.isPlayer : false;

                        return hexCell;
                    }));

                EditorUtility.SetDirty(hme.hexMap);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.Log("Oh No! you forgot to assign a map to save!");
            }
        }

        if (GUILayout.Button("Generate Random Objects"))
        { 

            foreach(HexCell cell in hme.hexGrid.cells)
            {
                // apply random factor
                hme.hexGrid.GenerateHexObjects(cell);

                // create map objects.
                hme.hexGrid.LoadHexObjects(cell);
            }
        }
    }
}

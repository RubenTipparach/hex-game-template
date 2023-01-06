using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HexMapGenerator01", menuName = "GenerateMap/HexMap")]
public class HexMap : ScriptableObject
{
    public string mapName;

    public int width = 12;

    public int height = 12;

    public List<HexCellData> hexCells;
}

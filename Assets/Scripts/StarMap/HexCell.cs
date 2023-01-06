using System;
using UnityEngine;
using UnityEngine.UI;

// This class is designed only FOR EDITOR use, DO NOT use at runtime.
[Serializable] // defines current map cell.
public class HexCellData
{
    public int x;
    public int z;

    public RaceType raceType;

    public MapScriptableData mapScriptableData;

    public bool hasPlanet;

    public bool hasStation;

    public bool hasEnemy;

    public bool hasPlayer;
}


public class HexCell : MonoBehaviour {

    public bool hasPlanet = false;
    public bool hasStation = false;
    public bool hasEnemy = false;

    public RectTransform uiRect;

    public HexCoordinates coordinates;

    public Vector3 gamePosition;

    public RaceType raceType
    {
        get { return raceSelectionData == null ? RaceType.Neutral : raceSelectionData.raceType; }
        set { raceSelectionData = RaceDataLoader.RaceDataLibrary.GetRace(value); }
    }

    public RaceSelectionData raceSelectionData;

    //public HexGridChunk chunk;

    public int Index { get; set; }

    int distance;

    int visibility;

    public bool IsVisible
    {
        get
        {
            return visibility > 0;
        }
    }
    
    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }

    public int Distance
    {
        get
        {
            return distance;
        }
        set
        {
            distance = value;
        }
    }

    public HexUnit Unit { get; set; }

    public HexCell PathFrom { get; set; }

    public int SearchHeuristic { get; set; }

    public int SearchPriority
    {
        get
        {
            return distance + SearchHeuristic;
        }
    }


    public int SearchPhase { get; set; }

    public HexCell NextWithSamePriority { get; set; }

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    [SerializeField]
    public HexCell[] neighbors;

    public void SetLabel(string text)
    {
        UnityEngine.UI.Text label = uiRect.GetComponent<Text>();
        label.text = text;
    }

    public void DisableHighlight()
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.enabled = false;
    }

    public void EnableHighlight(Color color)
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.color = color;
        highlight.enabled = true;
    }
}



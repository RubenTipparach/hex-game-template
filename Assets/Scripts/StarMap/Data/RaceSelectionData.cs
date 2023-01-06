using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "RaceData", menuName = "UIData/RaceInfo")]
public class RaceSelectionData : ScriptableObject
{
    public int RaceID = 0;

    public Sprite raceEmblem;

    // todo Race portrait....
    public Sprite racePortrait;

    public string RaceName = "DefaultRace";

    public string RaceInformation = "";

    public Color PrimaryColor;

    public Color SecondaryColor;

    public RaceType raceType;
}

[Serializable]
public enum RaceType
{
    Neutral = 0,
    R1_Federation = 1, R2_Republic = 2, R3_Union = 3, R4_Socialist = 4, R5_Convictions = 5, R6_Vipasanayans = 6, R7 = 7, R8 = 8, R9 = 9
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
 * Its a psychological trick to avoiding over generaliztion. I don't want too many races, hardcoding is a way to prevent too much of that. 
 * I want to directly reference a race in the programming code to customize dialogue and create special abilities.
 */

public class RaceDataLoader
{
    public static RaceDataLoader RaceDataLibrary
    {
        get
        {
            if (_raceDataLoader == null)
            {
                return _raceDataLoader = new RaceDataLoader();
            }
            else
            {
                return _raceDataLoader;
            }
        }
    }

    private static RaceDataLoader _raceDataLoader;

    private RaceDataLoader()
    {
        Race0_Neutral = Resources.Load<RaceSelectionData>("RaceData\\RaceData_0_Neutral");

        Race1_Federation = Resources.Load<RaceSelectionData>("RaceData\\RaceData_1_Federation");
        Race2_Republic = Resources.Load<RaceSelectionData>("RaceData\\RaceData_2_Republic");
        Race3_Union = Resources.Load<RaceSelectionData>("RaceData\\RaceData_3_Union");
        Race4_Socialist = Resources.Load<RaceSelectionData>("RaceData\\RaceData_4_Socialist");
        Race5_Convictions = Resources.Load<RaceSelectionData>("RaceData\\RaceData_5_Convictions");
        Race6_Vipasnayans = Resources.Load<RaceSelectionData>("RaceData\\RaceData_6_Vipasnayans");
    }

    public RaceSelectionData Race0_Neutral { get; private set; }

    public RaceSelectionData Race1_Federation { get; private set; }

    public RaceSelectionData Race2_Republic { get; private set; }

    public RaceSelectionData Race3_Union { get; private set; }

    public RaceSelectionData Race4_Socialist { get; private set; }

    public RaceSelectionData Race5_Convictions { get; private set; }

    public RaceSelectionData Race6_Vipasnayans { get; private set; }

    public RaceSelectionData GetRace(RaceType raceType)
    {
        RaceSelectionData raceSelected = Race0_Neutral;

        switch (raceType)
        {
            case RaceType.Neutral: raceSelected = Race0_Neutral; break;

            case RaceType.R1_Federation: raceSelected = Race1_Federation; break;
            case RaceType.R2_Republic: raceSelected = Race2_Republic; break;
            case RaceType.R3_Union: raceSelected = Race3_Union; break;
            case RaceType.R4_Socialist: raceSelected = Race4_Socialist; break;
            case RaceType.R5_Convictions: raceSelected = Race5_Convictions; break;
            case RaceType.R6_Vipasanayans: raceSelected = Race6_Vipasnayans; break;

            default: raceSelected = Race1_Federation; break;

        }

        return raceSelected;
    }
}

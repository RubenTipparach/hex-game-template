using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HexMissionDisplayUI : MonoBehaviour
{

    HexCell currentHexCell;

    HexUnit selectedUnit;

    public HexGrid grid;

    public GameObject missionGroup;

    public GameObject travelingMessage;

    public GameObject MissionButtonPrefab;

    public Slider HealthSlider;
    // Start is called before the first frame update
    void Start()
    {
        grid.LoadPlayerState(HealthSlider);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Usually this is some kind of call back function for when the player arrives 
    // at their intended destination.
    public void SwitchToMissions(HexCell cell, HexUnit unit)
    {
        currentHexCell = cell;
        selectedUnit = unit;

        missionGroup.SetActive(true);
        travelingMessage.SetActive(false);

        if(cell.hasEnemy)
        {
            // generate combat mission.
            GameObject mission = Instantiate(MissionButtonPrefab, missionGroup.transform);
            mission.transform.GetChild(0).GetComponent<Text>().text = "Combat Mission";

            mission.GetComponent<Button>().onClick.RemoveAllListeners();
            mission.GetComponent<Button>().onClick.AddListener(grid.LoadGameScenario);
        }

        if (cell.hasPlanet)
        {
            // generate combat mission.
            GameObject mission = Instantiate(MissionButtonPrefab, missionGroup.transform);
            mission.transform.GetChild(0).GetComponent<Text>().text = "Exploration Mission";
        }

        if (cell.hasStation)
        {
            // generate combat mission.
            GameObject mission = Instantiate(MissionButtonPrefab, missionGroup.transform);
            mission.transform.GetChild(0).GetComponent<Text>().text = "Dock Station";
        }

        // technically this unit is the player.
        grid.SavePlayerState(cell, unit, true);
    }

    public void SwitchToTravel()
    {
        missionGroup.SetActive(false);
        travelingMessage.SetActive(true);

        foreach (Transform t in missionGroup.transform)
        {
            Destroy(t.gameObject); //obviously we can just disable these. I'm just lazy :P
        }

    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexGameUI : MonoBehaviour {

	public HexGrid grid;

	HexCell currentCell;

	HexUnit selectedUnit;

   public HexMissionDisplayUI missionUI;
    
    void Update () {
	    if (!EventSystem.current.IsPointerOverGameObject()) {
            if (selectedUnit == null && Input.GetMouseButtonDown(0))
            {
                DoSelection();
            }
			else if (selectedUnit) {
				if (Input.GetMouseButtonDown(0)) {
                    // call back function to current state of the cell/unit.
					DoMove(()=>missionUI.SwitchToMissions(currentCell, selectedUnit));
                    missionUI.SwitchToTravel();
                }
                else 
                {
					DoPathfinding();
				}
			}
            
		}

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            grid.ClearPath();
            UpdateCurrentCell();
            selectedUnit = null;

        }
    }

	void DoSelection () {
		grid.ClearPath();
		UpdateCurrentCell();
        if (currentCell) {
			selectedUnit = currentCell.Unit;
            grid.ShowStart(currentCell);

        }
    }

	void DoPathfinding () {
		if (UpdateCurrentCell()) {
			if (currentCell && selectedUnit.IsValidDestination(currentCell)) {
				grid.FindPath(selectedUnit.Location, currentCell, 24);
			}
			else {
				grid.HidePath();
			}
		}
	}
    //public void SetEditMode(bool toggle)
    //{
    //    enabled = !toggle;
    //    grid.ShowUI(!toggle);
    //    grid.ClearPath();
    //}

    void DoMove (Action arrivalCallback) {

        //Debug.Log($"Has path? {grid.HasPath}");

        if (grid.HasPath) {
//			selectedUnit.Location = currentCell;
			selectedUnit.Travel(grid.GetPath(), arrivalCallback);
			grid.ClearPath();
		}

        selectedUnit = null;
	}

	bool UpdateCurrentCell () {
		HexCell cell =
			grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
		if (cell != currentCell) {
			currentCell = cell;
			return true;
		}
		return false;
	}
}
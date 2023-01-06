using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

	public int width = 6;
	public int height = 6;

	public RaceType defaultRace = RaceType.Neutral;

	public HexCell cellPrefab;

	public Text cellLabelPrefab;

	public HexCell[] cells;

	Canvas gridCanvas;

	HexMesh hexMesh;

    public HexMapEditor mapEditor;

    public HexMapLoader mapLoader;

    bool loadHexCell = false;

    public bool enableLoadMap = false;

    // prefab stuff
    public GameObject planetPrefab;

    public GameObject spaceDebrisPrefab;

    public GameObject encountersPrefab;

    public GameObject playerPrefab;

    public GameObject stationPrefab;

    public Vector3 planetOffset;

    public Vector3 stationOffset;

    public Vector3 encounterOffset;


    public float planetScale = 0.005f;

    public float stationScale = 0.01f;

    public float encounterScale = 0.015f;
    List<HexUnit> units = new List<HexUnit>();
    public HexUnit unitPrefab;

   // HexGridChunk[] chunks;

    int chunkCountX, chunkCountZ;

    int searchFrontierPhase;

    HexCell currentPathFrom, currentPathTo;
    bool currentPathExists;
    HexCellPriorityQueue searchFrontier;

    [Range(0,1f)]
    public float randomPlanet = .5f;
    
    [Range(0, 1f)]
    public float randomEnemy = .2f;
    
    [Range(0, 1f)]
    public float randomStation = .1f;

    public Color highlightPath;

    public Color highlightStart;

    public Color highlightEnd;

    public HexUnit playerUnit;

    [SerializeField]
    private GlobalStateManager globalStateManager;

    public bool HasPath
    {
        get
        {
            return currentPathExists;
        }
    }


    void Awake () {
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();
        HexUnit.unitPrefab = unitPrefab;

        if (mapEditor != null)
        {
            if (mapEditor.hexMap != null)
            {
                width = mapEditor.hexMap.width;
                height = mapEditor.hexMap.height;

                if (enableLoadMap && mapEditor.hexMap.hexCells.Count > 0)
                {
                    loadHexCell = true;
                }
            }
        }
        else
        {
            // TODO: implement random generator mode.
            if(mapLoader != null)
            {
                width = mapLoader.hexMap.width;
                height = mapLoader.hexMap.height;
                if(mapLoader.hexMap.hexCells.Count > 0)
                {
                    loadHexCell = true;
                }
            }
        }

        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void Start () {
		hexMesh.Triangulate(cells);
	}

    public void AddUnit(HexUnit unit, HexCell location, float orientation)
    {
        units.Add(unit);
        unit.Grid = this;
        unit.transform.SetParent(transform, false);
        unit.Location = location;
        unit.Orientation = orientation;
    }

    public void RemoveUnit(HexUnit unit)
    {
        units.Remove(unit);
        unit.Die();
    }


    public Vector3 GetCellPosition(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;

        //Vector3 position = new Ve
        try
        {
            HexCell cell = cells[index];
            return cell.gamePosition;
        }catch (IndexOutOfRangeException e)
        {
            return Vector3.zero; // to do implement something to handle this.
        }
        
    }
    public void ShowUI(bool visible)
    {
        //for (int i = 0; i < chunks.Length; i++)
        //{
        //    chunks[i].ShowUI(visible);
        //}
        //todo show ui....
    }

    public void FindPath(HexCell fromCell, HexCell toCell, int speed)
    {
        ClearPath();
        currentPathFrom = fromCell;
        currentPathTo = toCell;

        if (fromCell.neighbors.Contains(toCell))
        {
            currentPathExists = Search(fromCell, toCell, speed);
            ShowPath(speed);
        }
    }

    bool Search(HexCell fromCell, HexCell toCell, int speed)
    {
        searchFrontierPhase += 2;
        if (searchFrontier == null)
        {
            searchFrontier = new HexCellPriorityQueue();
        }
        else
        {
            searchFrontier.Clear();
        }

        fromCell.SearchPhase = searchFrontierPhase;
        fromCell.Distance = 0;
        searchFrontier.Enqueue(fromCell);
        while (searchFrontier.Count > 0)
        {
            HexCell current = searchFrontier.Dequeue();
            current.SearchPhase += 1;

            if (current == toCell)
            {
                return true;
            }

            int currentTurn = (current.Distance - 1) / speed;

            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = current.GetNeighbor(d);
                if (
                    neighbor == null ||
                    neighbor.SearchPhase > searchFrontierPhase
                )
                {
                    continue;
                }
              
                int moveCost = 0;

                int distance = current.Distance + moveCost;
                int turn = (distance - 1) / speed;
                if (turn > currentTurn)
                {
                    distance = turn * speed + moveCost;
                }

                if (neighbor.SearchPhase < searchFrontierPhase)
                {
                    neighbor.SearchPhase = searchFrontierPhase;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    neighbor.SearchHeuristic =
                        neighbor.coordinates.DistanceTo(toCell.coordinates);
                    searchFrontier.Enqueue(neighbor);
                }
                else if (distance < neighbor.Distance)
                {
                    int oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    searchFrontier.Change(neighbor, oldPriority);
                }
            }
        }
        return false;
    }

    public void SpawnPlayer(HexCell cell)
    {
        if (playerUnit != null)
        {
            //destroy player first.

        }

        HexUnit playerUnitLocal = Instantiate(HexUnit.unitPrefab);
        playerUnitLocal.isPlayer = true;

        AddUnit(
            playerUnitLocal, cell, 0//UnityEngine.Random.Range(0f, 360f)
        );

        playerUnit = playerUnitLocal;
    }

    public void ClearPath()
    {
        if (currentPathExists)
        {
            HexCell current = currentPathTo;
            while (current != currentPathFrom)
            {
                current.SetLabel(null);
                current.DisableHighlight();
                current = current.PathFrom;
            }
            current.DisableHighlight();
            currentPathExists = false;
        }
        else if (currentPathFrom)
        {
            currentPathFrom.DisableHighlight();
            currentPathTo.DisableHighlight();
        }

        currentPathFrom = currentPathTo = null;
    }

    public void HidePath()
    {
        if (currentPathFrom)
        {
            currentPathTo.DisableHighlight();
        }
    }

    public void ShowStart(HexCell cell)
    {
        if (currentPathFrom)
        {
            currentPathTo.DisableHighlight();
        }

        ClearPath();
        currentPathFrom = cell;
        currentPathTo = cell;

        // speed doesn't matter right now.
        ShowPath(1);
        currentPathTo.DisableHighlight();
    }

    void ShowPath(int speed)
    {
        if (currentPathExists)
        {
            HexCell current = currentPathTo;
            while (current != currentPathFrom)
            {
                int turn = (current.Distance - 1) / speed;
                current.SetLabel(turn.ToString());
                // var shadeOffset = (float)current.Distance/(float)speed;

                //Debug.Log($"distance {current.Distance} speed {speed}");

                Color colorShade = highlightPath; //new Color(highlightPath.r * shadeOffset, highlightPath.g * shadeOffset, highlightPath.b * shadeOffset, 1);
                current.EnableHighlight(colorShade);
                current = current.PathFrom;
            }
        }
        currentPathFrom.EnableHighlight(highlightStart);
        currentPathTo.EnableHighlight(highlightEnd);
    }

    public void ColorCell (Vector3 position, RaceType raceSelection) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		HexCell cell = cells[index];
        cell.raceType = raceSelection;
        hexMesh.Triangulate(cells);
	}

    public HexCell GetHexCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];

        return cell;
    }


    void CreateCell (int x, int z, int i) {
        Vector3 position = Vector3.zero;
        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.neighbors = new HexCell[6];
        cell.Index = i;

        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }

        //load data
        if (loadHexCell)
        {
            // map loader.
            position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
            position.y = 0f;
            position.z = z * (HexMetrics.outerRadius * 1.5f);

            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = position;
            
            // TODO: add more data here.
            //cell.coordinates = new HexCoordinates( mapEditor.hexMap.hexCells[i].x, mapEditor.hexMap.hexCells[i].z);
            cell.coordinates = new HexCoordinates(x, z);
            //Debug.Log(cell.coordinates.ToStringOnSeparateLines());

            HexMap hexMap = null;


            if (mapEditor != null)
            {
                hexMap = mapEditor.hexMap;

                //exlsucive to editor for now :)

                //if (randomScenario < mapEditor.scriptableMapsTest.Count)
                //{
                //    var mapScenario = mapEditor.scriptableMapsTest[randomScenario];
                //}

            }
            else
            {
                hexMap = mapLoader.hexMap;
            }

            // Load hex map data ;)
            var loadedCell = hexMap.hexCells[i];
            cell.raceType = loadedCell.raceType;
            
            cell.hasEnemy = loadedCell.hasEnemy;
            cell.hasPlanet = loadedCell.hasPlanet;
            cell.hasStation = loadedCell.hasStation;

            //if (!globalStateManager.hasPlayerData && loadedCell.hasPlayer)
            //{
            //    SpawnPlayer(cell);

            //}
            //else if( globalStateManager.hasPlayerData && globalStateManager.playerState.playerStarMapLocation == cell.coordinates)
            //{
            //    SpawnPlayer(cell);


            //    // also if in combat or mission complete...
            //    if (globalStateManager.missionState && globalStateManager.inCombat)
            //    {
            //        cell.hasEnemy = false;
            //        loadedCell.hasEnemy = false;
            //        globalStateManager.missionState = false;
            //        globalStateManager.inCombat = false;

            //        Debug.Log("Defeated the enemy!");
            //    }

            //}

            LoadHexObjects(cell);

        }
        else
        {
            position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
            position.y = 0f;
            position.z = z * (HexMetrics.outerRadius * 1.5f);

            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = position;
            cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
            cell.raceType = defaultRace;
            
            //Debug.Log(cell.coordinates.ToStringOnSeparateLines());
        }

        cell.gamePosition = position;

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
        cell.uiRect = label.rectTransform;
       // AddCellToChunk(x, z, cell);

    }

    public void LoadHexObjects(HexCell cell)
    {
        var hexPosition = cell.transform.position;

        if (cell.hasEnemy)
        {
            GameObject instance = Instantiate(encountersPrefab, hexPosition + encounterOffset, Quaternion.identity, cell.transform);
            instance.transform.localScale = instance.transform.localScale * encounterScale;
        }

        if(cell.hasPlanet)
        {
            GameObject instance = Instantiate(planetPrefab, hexPosition + planetOffset, Quaternion.identity, cell.transform);
            instance.transform.localScale = instance.transform.localScale * planetScale;
        }

        if(cell.hasStation)
        {
            GameObject instance = Instantiate(stationPrefab, hexPosition + stationOffset, Quaternion.identity, cell.transform);
            instance.transform.localScale = instance.transform.localScale * stationScale;
        }
    }

    public void GenerateHexObjects(HexCell cell){
        if (randomEnemy > UnityEngine.Random.Range(0f, 1f))
        {
            cell.hasEnemy = true;
        }

        if (randomPlanet > UnityEngine.Random.Range(0f, 1f))
        {
            cell.hasPlanet = true;
        }

        if (randomStation > UnityEngine.Random.Range(0f, 1f))
        {
            cell.hasStation = true;
        }
    }

    public List<HexCell> GetPath()
    {
        if (!currentPathExists)
        {
            return null;
        }
        List<HexCell> path = ListPool<HexCell>.Get();
        for (HexCell c = currentPathTo; c != currentPathFrom; c = c.PathFrom)
        {
            path.Add(c);
        }
        path.Add(currentPathFrom);
        path.Reverse();
        return path;
    }

    public HexCell GetCell(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return GetCell(hit.point);
        }
        return null;
    }


    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        
        if(index >= cells.Length || index < 0)
        {
            return null;
        }

        return cells[index];
    }

    public void LoadGameScenario()
    {
        int randomScenario = UnityEngine.Random.Range(0, mapEditor.scriptableMapsTest.Count);
  
        var mapScenario = mapEditor.scriptableMapsTest[randomScenario];

        globalStateManager.loadThisMap = mapScenario;

       SceneManager.LoadSceneAsync("GenericScenario");

    }

    public void SavePlayerState(HexCell cell, HexUnit player, 
        bool inCombat = false, List<HexUnit> mapUnits = null)
    {
        //if (globalStateManager.playerState == null)
        //{
        //    globalStateManager.playerState = new GlobalStateManager.PlayerState();
        //}

        //globalStateManager.playerState.playerStarMapLocation = cell.coordinates;

        //globalStateManager.hasPlayerData = true;
        //globalStateManager.inCombat = inCombat; // if true the player is entering  a combat scenario...
        Debug.Log("Save player state");
    }

    // Other things like spare parts moneys....
    public void LoadPlayerState(Slider healthSlider)
    {
        //healthSlider.value = globalStateManager.playerState.playerShipState.CurrentHealth;
        Debug.Log("Load player state");
    }

}
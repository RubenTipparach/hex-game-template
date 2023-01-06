using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapLoader : MonoBehaviour
{
    public HexGrid hexGrid;

    public HexMap hexMap;

    //public List<HexCell> saveCells;

    //InputMaster inputMaster;

    public Camera gameCamera;

    Vector3 moveDirection;

    Vector3 smooth;

    public float cameraSpeed = 10;

    public GameObject marker;

    void Awake()
    {
        //saveCells = new List<HexCell>();
        //inputMaster = new InputMaster();

    }

    private void OnEnable()
    {
        //inputMaster.Player.Enable();
    }

    private void OnDisable()
    {
        //inputMaster.Player.Disable();
    }

    void Update()
    {
        if (
            Input.GetMouseButton(0) &&
            !EventSystem.current.IsPointerOverGameObject()
        )
        {
            HandleInput();
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            DisplaySelection();
        }

        // move stuff!
        Vector3 moveSpeed = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveSpeed += Vector3.forward * cameraSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveSpeed += Vector3.left * cameraSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveSpeed -= Vector3.forward * cameraSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveSpeed -= Vector3.left * cameraSpeed;
        }

        moveDirection = Vector3.SmoothDamp(moveDirection, moveSpeed, ref smooth, Time.deltaTime);
        // Debug.Log(moveDirection.ToString("0.000"));
        gameCamera.transform.Translate(moveDirection * Time.deltaTime, Space.World);
    }

    void DisplaySelection()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // display object snap to grid.
        if (Physics.Raycast(inputRay, out hit))
        {
            Vector3 position = hexGrid.GetCellPosition(hit.point);

            //hexGrid.ColorCell(hit.point, activeColor);
            if (marker != null)
            {
                marker.transform.position = position;
            }
        }
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // TODO: replace this with input system?
        if (Physics.Raycast(inputRay, out hit))
        {
            //hexGrid.ColorCell(hit.point, activeColor);
            //var hexCell = hexGrid.GetHexCell(hit.point);
            //if (!saveCells.Contains(hexCell))
            //{
            //    saveCells.Add(hexCell);
            //}
        }


    }
}

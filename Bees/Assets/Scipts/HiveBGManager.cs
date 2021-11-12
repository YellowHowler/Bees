using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEngine.SceneManagement;

public class HiveBGManager : MonoBehaviour
{
    [SerializeField] GameObject honeycombObj;
    [SerializeField] GameObject InputManager;
    [SerializeField] GameObject RoomManager;
    [SerializeField] GameObject mouseClickIcon;
    [SerializeField] GameObject RoomButtons;
    [SerializeField] Tilemap BGGrid;
    [SerializeField] Tilemap FloorGrid;
    [SerializeField] Tile BGTile; 
    [SerializeField] Tile FloorTile; 

    HoneycombManager hiveSC;
    InputManager IPScript;
    RoomManager RMScript;

    private Vector3Int[] exitPos;
    private Vector3Int mouseTilePos;

    private int excessTileNum;
    private int hcNum;
    
    private int index;

    private int left;
    private int right;
    private int up;
    private int down;

    private bool exitClicked = false;

    void Awake()
    {
        hiveSC = honeycombObj.GetComponent<HoneycombManager>();
        IPScript = InputManager.GetComponent<InputManager>();
        RMScript = RoomManager.GetComponent<RoomManager>();
        excessTileNum = 8;

        exitPos = new Vector3Int[]{new Vector3Int(-2, -2, 0), new Vector3Int(-1, -2, 0), new Vector3Int(-2, -1, 0), new Vector3Int(-2, -3, 0), new Vector3Int(-1, -3, 0), new Vector3Int(-3, -3, 0)};
        //hiveCol = hiveSC.getHiveCol();
        
        RoomButtons.SetActive(false);
    }

    public void GetDirection()
    {
        left = hiveSC.getLeft();
        right = hiveSC.getRight();
        up = hiveSC.getUp();
        down = hiveSC.getDown();
    }

    public Vector3 getExitPos()
    {
        return BGGrid.GetCellCenterWorld(exitPos[2]);
    }

    void Start()
    {
        GetDirection();

        hcNum = hiveSC.getHoneycombNum();

        setupBG();
    }

    // Update is called once per frame
    void Update()
    {
        mouseTilePos = IPScript.getMouseTilePos();

        if(Array.IndexOf(exitPos, mouseTilePos) > -1)
        {
            if(!exitClicked) mouseClickIcon.SetActive(true);
            mouseClickIcon.transform.position = BGGrid.GetCellCenterWorld(exitPos[2]);

            if(Input.GetMouseButtonDown(0))
            {
                exitClicked = true;
                mouseClickIcon.SetActive(false);
                RoomButtons.SetActive(true);
                RoomButtons.transform.position = BGGrid.GetCellCenterWorld(exitPos[2]);
            }
        }
        else
        {
            mouseClickIcon.SetActive(false);
            RoomButtons.SetActive(false);
            exitClicked = false;
        }
    }

    public void setupBG()
    {
        for(int i = -1 * excessTileNum + left; i < right + excessTileNum; i++)
        {
            for(int j = -1 * excessTileNum + down; j < up + excessTileNum; j++)
            {
                BGGrid.SetTile(new Vector3Int(i, j, 0), BGTile);
            }
        }

        foreach(Vector3Int pos in exitPos)
        {
            BGGrid.SetTile(pos, null);
        }

        for(int i = -1 * excessTileNum + left; i < right + excessTileNum; i++)
        {
            FloorGrid.SetTile(new Vector3Int(i, -4, 0), FloorTile);
        }
    }

    
}

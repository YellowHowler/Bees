using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class HoneycombManager : MonoBehaviour
{
    [SerializeField] Tilemap hiveGrid;
    [SerializeField] Tilemap buyGrid;
    [SerializeField] Tilemap BGGrid;
    [SerializeField] Tile[] hcTile;
    [SerializeField] Tile buyTile;
    [SerializeField] Tile buyTileEmpty;
    [SerializeField] GameObject HCPriceObj;
    [SerializeField] GameObject HCPriceTxtObj;
    [SerializeField] GameObject StorageManager;

    BuyHoneycombText HCPriceTxt;
    StorageManager SMScript;
    

    Camera camera;

    private List<int[]> hcPos;

    private List<int> honeyStorage;

    private int honeycombNum = 0;
    private int HCPrice;

    private Vector2 mousePos;
    private Vector3 mouseWorldPos;
    private Vector3 tempPos;
    private Vector3Int mouseTilePos;
    private Vector3Int buyGridPos;
    private Vector3Int buyGridPastPos;

    private int left;
    private int right;
    private int up;
    private int down;

    private float honey;
    private float nectar;
    private float pollen;
    private float wax;

    private int honeyM;
    private int nectarM;
    private int pollenM;
    private int waxM;

    private string HCPriceStr;

    public int getLeft(){ return left; }
    public int getRight(){ return right; }
    public int getUp(){ return up; }
    public int getDown(){ return down; }
    public int getHoneycombNum(){ return honeycombNum; }
    
    private void getStorage()
    {
        honey = SMScript.getHoney();
        nectar = SMScript.getNectar();
        pollen = SMScript.getPollen();
        wax = SMScript.getWax();
    }

    void Awake()
    {
        HCPriceStr = "100";
        HCPriceTxt = HCPriceTxtObj.GetComponent<BuyHoneycombText>();
        SMScript = StorageManager.GetComponent<StorageManager>();

        hcPos = new List<int[]>();
        honeyStorage = new List<int>();

        camera = Camera.main;

        getStorage();

        honeycombNum = 5;
        for(int i = 0; i < honeycombNum; i++)
        {
            hcPos.Add(new int[]{i, 0});
            honeyStorage.Add(0);
        }

        left = 0;
        right = 5;
        up = 0;
        down = 0;

        setupHC();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mouseWorldPos = camera.ScreenToWorldPoint(mousePos);
        mouseTilePos = hiveGrid.WorldToCell(mouseWorldPos);
        mouseTilePos = new Vector3Int(mouseTilePos.x, mouseTilePos.y, 0);

        // if(mouseTilePos.Equals(buyGridPos))
        // {
        //     buyGrid.SetTile(buyGridPos, buyTileEmpty);
        //     tempPos = camera.WorldToScreenPoint(hiveGrid.GetCellCenterWorld(mouseTilePos));
        //     HCPriceObj.transform.position = new Vector3(tempPos.x, tempPos.y + 30, 0);
        //     HCPriceTxt.setPriceText(HCPriceStr);
        //     HCPriceObj.SetActive(true);
        // }
        // else
        // {
        //     buyGrid.SetTile(buyGridPos, null);
        //     HCPriceObj.SetActive(false);
        // }

        buyGrid.SetTile(buyGridPastPos, null);
        
        if(!hiveGrid.HasTile(mouseTilePos))
        {
            if(BGGrid.HasTile(mouseTilePos))
            {
                buyGrid.SetTile(mouseTilePos, buyTileEmpty);
                tempPos = hiveGrid.GetCellCenterWorld(mouseTilePos);
                HCPriceObj.transform.position = new Vector3(tempPos.x, tempPos.y, 0);
                HCPriceTxt.setPriceText(HCPriceStr);
                HCPriceObj.SetActive(true);
            }
        }
        else
        {
            HCPriceObj.SetActive(false);
        }

        buyGridPastPos = mouseTilePos;

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log(mouseTilePos);

            if(hiveGrid.HasTile(mouseTilePos))
            {
                Debug.Log("has tile");
            }
            else
            {
                buyHC();
            }
        }
    }

    void buyHC()
    {
        if(wax >= HCPrice)
        {
            
        }
    }

    void setupHC()
    {
        // for(int i = 0; i < honeycombNum; i++)
        // {
        //     hiveGrid.SetTile(new Vector3Int(i % hiveCol, i / hiveCol, 0), hcTile[honeyStorage[i]]);
        // }

        // buyGridPos = new Vector3Int(honeycombNum % hiveCol, honeycombNum / hiveCol, 0);

        for(int i = 0; i < honeycombNum; i++)
        {
            hiveGrid.SetTile(new Vector3Int(hcPos[i][0], hcPos[i][1], 0), hcTile[honeyStorage[i]]);
        }
    }

    // public Vector3 GetBuyGridPos()
    // {
    //     return buyGridPos;
    // }
}

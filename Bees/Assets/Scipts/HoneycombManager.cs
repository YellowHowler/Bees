using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class HoneycombManager : MonoBehaviour
{
    [SerializeField] private int hiveCol;
    [SerializeField] Tilemap hiveGrid;
    [SerializeField] Tilemap buyGrid;
    [SerializeField] Tile[] hcTile;
    [SerializeField] Tile buyTile;
    [SerializeField] Tile buyTileEmpty;
    [SerializeField] GameObject HCPriceObj;
    [SerializeField] GameObject StorageManager;

    BuyHoneycombText HCPriceTxt;
    StorageManager SMScript;

    Camera camera;

    private int[] honeyStorage;

    private int honeycombNum = 10;
    private int HCPrice;

    private Vector2 mousePos;
    private Vector3 mouseWorldPos;
    private Vector3 tempPos;
    private Vector3Int mouseTilePos;
    private Vector3Int buyGridPos;

    private float honey;
    private float nectar;
    private float pollen;
    private float wax;

    private int honeyM;
    private int nectarM;
    private int pollenM;
    private int waxM;

    private string HCPriceStr;

    public int getHiveCol(){ return hiveCol; }
    public int getHoneycombNum(){ return honeycombNum; }

    void Awake()
    {
        HCPriceStr = "100";
        HCPriceTxt = HCPriceObj.GetComponent<BuyHoneycombText>();
        SMScript = StorageManager.GetComponent<StorageManager>();

        honeyStorage = new int[honeycombNum];
        for(int i = 0; i < honeycombNum; i++) honeyStorage[i] = 0;

        camera = Camera.main;

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

        if(mouseTilePos.Equals(buyGridPos))
        {
            buyGrid.SetTile(buyGridPos, buyTile);
            tempPos = camera.WorldToScreenPoint(hiveGrid.GetCellCenterWorld(mouseTilePos));
            HCPriceObj.transform.position = new Vector3(tempPos.x - 60, tempPos.y, 0);
            HCPriceTxt.setPriceText(HCPriceStr);
            HCPriceObj.SetActive(true);
        }
        else
        {
            buyGrid.SetTile(buyGridPos, buyTileEmpty);
            HCPriceObj.SetActive(false);
        }

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log(mouseTilePos);

            if(hiveGrid.HasTile(mouseTilePos))
            {
                Debug.Log("has tile");
            }
        }
    }

    void setupHC()
    {
        for(int i = 0; i < honeycombNum; i++)
        {
            hiveGrid.SetTile(new Vector3Int(i % hiveCol, i / hiveCol, 0), hcTile[honeyStorage[i]]);
        }

        buyGridPos = new Vector3Int(honeycombNum % hiveCol, honeycombNum / hiveCol, 0);

        buyGrid.SetTile(buyGridPos, buyTileEmpty);
    }

    public Vector3 GetBuyGridPos()
    {
        return buyGridPos;
    }
}

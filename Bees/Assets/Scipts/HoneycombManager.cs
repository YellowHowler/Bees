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
    [SerializeField] GameObject InputManager;
    [SerializeField] GameObject BGManager;
    [SerializeField] GameObject CameraManager;

    BuyHoneycombText HCPriceTxt;
    StorageManager SMScript;
    InputManager IPScript;
    HiveBGManager BGScript;
    CameraManager CMScript;

    private Vector3Int mouseTilePos;

    Camera camera;

    private List<int[]> hcPos;
    private List<int> honeyStorage;
    private List<int> nectarStorage;

    private int honeycombNum = 0;
    private float HCPrice = 50;
    private int HCPriceM = 0;

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

    private int honeyCapacity = 150;
    private int nectarCapacity = 150;

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

        honeyM = SMScript.getHoneyM();
        nectarM = SMScript.getNectarM();
        pollenM = SMScript.getPollenM();
        waxM = SMScript.getWaxM();
    }

    public float getHCPrice() { return HCPrice; }
    public int getHCPriceM() { return HCPriceM; }

    void Awake()
    {
        HCPriceTxt = HCPriceTxtObj.GetComponent<BuyHoneycombText>();
        SMScript = StorageManager.GetComponent<StorageManager>();
        IPScript = InputManager.GetComponent<InputManager>();
        BGScript = BGManager.GetComponent<HiveBGManager>();
        CMScript = CameraManager.GetComponent<CameraManager>();

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
        right = 6;
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
        mouseTilePos = IPScript.getMouseTilePos();
    }

    public int getEmptyHCNectar()
    {
        for(int i = 0; i < honeycombNum; i++)
        {
            if(honeyStorage[i] == 0 && nectarStorage[i] < nectarCapacity)
            {
                return i;
            }
        }
        return -1;
    }

    public void buyHC()
    {
        getStorage();

        if(waxM > HCPriceM || (wax >= HCPrice && waxM == HCPriceM))
        {
            Debug.Log("buy");

            hcPos.Add(new int[]{mouseTilePos.x, mouseTilePos.y});
            honeyStorage.Add(0);
            honeycombNum++;

            BGScript.setupBG();

            if(mouseTilePos.x < left) left = mouseTilePos.x;
            else if(mouseTilePos.x > right) right = mouseTilePos.x;
            if(mouseTilePos.y < down) down = mouseTilePos.y;
            else if(mouseTilePos.y > up) up = mouseTilePos.y;

            Debug.Log(right + " " + left + " " + up + " " + down);

            BGScript.GetDirection();
            BGScript.setupBG();
            CMScript.setBound();

            //SMScript.setWax();
            setupHC();
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
            hiveGrid.SetTile(new Vector3Int(hcPos[i][0], hcPos[i][1], 0), hcTile[(honeyStorage[i] / (honeyCapacity / (hcTile.Length - 1)))]);
        }
    }

    // public Vector3 GetBuyGridPos()
    // {
    //     return buyGridPos;
    // }
}

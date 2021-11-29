using System;
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
    [SerializeField] Tilemap BGGridTemp;
    [SerializeField] Tile[] hcTile;
    [SerializeField] Tile[] hcNectarTile;
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
    private List<float> honeyStorage;
    private List<float> nectarStorage;
    private List<int> honeyStorageM;
    private List<int> nectarStorageM;

    private int honeycombNum = 0;
    private float HCPrice = 600;
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

    private float honeyCapacity = 150;
    private int honeyCapacityM = 0;
    private float nectarCapacity = 150;
    private int nectarCapacityM = 0;

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
        honeyStorage = new List<float>();
        honeyStorageM = new List<int>();
        nectarStorage = new List<float>();
        nectarStorageM = new List<int>();

        camera = Camera.main;

        getStorage();

        honeycombNum = 5;

        for(int i = 0; i < honeycombNum; i++)
        {
            hcPos.Add(new int[]{i, 0});
            honeyStorage.Add(0);
            nectarStorage.Add(0);
            honeyStorageM.Add(0);
            nectarStorageM.Add(0);
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

    public void setNectarStorage(int index)
    {
        
    }

    public int getEmptyHCNectar()
    {
        for(int i = 0; i < honeycombNum; i++)
        {
            if(honeyStorage[i] == 0 && nectarStorage[i] < nectarCapacity)
            {
                Debug.Log(i);
                return i;
            }
        }
        return -1;
    }

    public Vector3 getHCTilePos(int index)
    {
        return BGGridTemp.GetCellCenterWorld(new Vector3Int(hcPos[index][0], hcPos[index][1], 0));
    }

    public void buyHC()
    {
        getStorage();

        if(waxM > HCPriceM || (wax >= HCPrice && waxM == HCPriceM))
        {
            Debug.Log("buy");

            hcPos.Add(new int[]{mouseTilePos.x, mouseTilePos.y});
            honeyStorage.Add(0);
            nectarStorage.Add(0);
            honeyStorageM.Add(0);
            nectarStorageM.Add(0);
            honeycombNum++;

            wax = wax - HCPrice * (float)Math.Pow(1000, -waxM + HCPriceM);
            if(wax <= 1)
            {
                wax *= 1000;
                waxM--;
            }

            SMScript.setWax(wax, waxM);
            SMScript.changeText();

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

    public float changeNectarStorage(int index, float nectar, int nectarM)
    {
        nectarStorage[index] += nectar * (float)Math.Pow(1000, -nectarStorageM[index] + nectarM);

        if(nectarStorage[index] > 1000)
        {
            nectarStorage[index] /= 1000;
            nectarStorageM[index]++;
        }

        if(nectarCapacityM < nectarStorageM[index] || (nectarCapacityM == nectarStorageM[index] && nectarCapacity < nectarStorage[index]))
        {
            Debug.Log("full");
            float returnValue = (nectarStorage[index] - nectarCapacity * (float)Math.Pow(1000, nectarCapacityM - nectarStorageM[index])) * (float)Math.Pow(1000, nectarStorageM[index] - nectarM);

            nectarStorage[index] = nectarCapacity;
            nectarStorageM[index] = nectarCapacityM;

            drawTile(index);
            Debug.Log(returnValue);
            return returnValue;
        }

        drawTile(index);
        return 0;
    }

    void drawTile(int i)
    {
        int tileNum;
        if(honeyStorage[i] > 0)
        {
            tileNum = (int)((honeyStorage[i]) / (honeyCapacity) * (float)Math.Pow(1000, honeyStorageM[i] - honeyCapacityM) * (hcTile.Length - 1));
            hiveGrid.SetTile(new Vector3Int(hcPos[i][0], hcPos[i][1], 0), hcTile[tileNum]);
        }
            
        else if(nectarStorage[i] > 0)
        {
            tileNum = (int)((nectarStorage[i]) / (nectarCapacity) * (float)Math.Pow(1000, nectarStorageM[i] - nectarCapacityM) * (hcTile.Length - 1));
            hiveGrid.SetTile(new Vector3Int(hcPos[i][0], hcPos[i][1], 0), hcNectarTile[tileNum]);
        } 
        else
        {
            tileNum = 0;
            hiveGrid.SetTile(new Vector3Int(hcPos[i][0], hcPos[i][1], 0), hcNectarTile[tileNum]);
        }
    }

    void setupHC()
    {
        for(int i = 0; i < honeycombNum; i++)
        {
            drawTile(i);
        }
    }

    // public Vector3 GetBuyGridPos()
    // {
    //     return buyGridPos;
    // }
}

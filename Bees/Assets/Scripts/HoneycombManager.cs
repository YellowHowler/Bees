using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class HoneycombManager : Singleton<HoneycombManager>
{
    [SerializeField] Tilemap hiveGrid;
    [SerializeField] Tilemap buyGrid;
    [SerializeField] Tilemap BGGrid;
    [SerializeField] Tilemap BGGridTemp;
    [SerializeField] Tile[] hcTile; //honey
    [SerializeField] Tile[] hcNectarTile;
    [SerializeField] Tile[] hcPollenTile;
    [SerializeField] Tile hcEggTile;
    [SerializeField] Tile buyTile;
    [SerializeField] Tile buyTileEmpty;
    [SerializeField] GameObject HCPriceObj;
    [SerializeField] GameObject HCPriceTxtObj;

    BuyHoneycombText HCPriceTxt;

    private Vector3Int mouseTilePos;

    Camera camera;

    private List<int[]> hcPos;
    private List<float> honeyStorage;
    private List<float> nectarStorage;
    private List<int> honeyStorageM;
    private List<int> nectarStorageM;
    private List<float> pollenStorage;
    private List<int> pollenStorageM;
    private List<bool> eggStorage;

    private int honeycombNum = 0;
    private float HCPrice = 1;
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

    public float honeyCapacity{get;set;}
    public int honeyCapacityM{get;set;}
    public float nectarCapacity{get;set;}
    public int nectarCapacityM{get;set;}
    public float pollenCapacity{get;set;}
    public int pollenCapacityM{get;set;}

    private string HCPriceStr;

    public int getLeft(){ return left; }
    public int getRight(){ return right; }
    public int getUp(){ return up; }
    public int getDown(){ return down; }
    public int getHoneycombNum(){ return honeycombNum; }
    
    private void getStorage()
    {
        honey = StorageManager.Instance.getHoney();
        nectar = StorageManager.Instance.getNectar();
        pollen = StorageManager.Instance.getPollen();
        wax = StorageManager.Instance.getWax();

        honeyM = StorageManager.Instance.getHoneyM();
        nectarM = StorageManager.Instance.getNectarM();
        pollenM = StorageManager.Instance.getPollenM();
        waxM = StorageManager.Instance.getWaxM();
    }

    public float getHCPrice() { return HCPrice; }
    public int getHCPriceM() { return HCPriceM; }

    void Awake()
    {
        honeyCapacity = 10;
        honeyCapacityM = 1;
        nectarCapacity = 10;
        nectarCapacityM = 1;
        pollenCapacity = 200;
        pollenCapacityM = 1;


        HCPriceTxt = HCPriceTxtObj.GetComponent<BuyHoneycombText>();

        hcPos = new List<int[]>();
        honeyStorage = new List<float>();
        honeyStorageM = new List<int>();
        nectarStorage = new List<float>();
        nectarStorageM = new List<int>();
        pollenStorage = new List<float>();
        pollenStorageM = new List<int>();
        eggStorage = new List<bool>();

        camera = Camera.main;

        getStorage();

        honeycombNum = 5;

        for(int i = 0; i < honeycombNum; i++)
        {
            hcPos.Add(new int[]{i, 0});
            honeyStorage.Add(0);
            nectarStorage.Add(0);
            pollenStorage.Add(0);
            honeyStorageM.Add(0);
            nectarStorageM.Add(0);
            pollenStorageM.Add(0);
            eggStorage.Add(false);
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
        mouseTilePos = InputManager.Instance.getMouseTilePos();
    }

    private float[] round(float value, float valueM)
    {
        bool isRounding = true;

        float v = value;
        float vM = valueM;

        if(value <= 0f)
        {
            return new float[]{0f, valueM};
        }

        while(isRounding)
        {
            isRounding = false;

            if(v >= 1000f)
            {
                v /= 1000f;
                vM++;
                isRounding = true;
            }
            else if(value < 1f)
            {
                v *= 1000f;
                vM--;
                isRounding = true;
            }
            else
            {
                isRounding = false;
            }
        }  

        return new float[]{v, vM};
    }

    public void setNectarStorage(int index)
    {
        
    }

    public float[] getStorageHC(int index, bool emptyHC)
    {
        float storageType = -1f;
        float storage = 0f;
        float storageM = 0f;

        if(honeyStorage[index] > 0f)
        {
            storageType = 0f;
            storage = honeyStorage[index];
            storageM = honeyStorageM[index];
        }
        else if(nectarStorage[index] > 0f)
        {
            storageType = 1f;
            storage = nectarStorage[index];
            storageM = nectarStorageM[index];
        }
        else if(pollenStorage[index] > 0f)
        {
            storageType = 2f;
            storage = pollenStorage[index];
            storageM = pollenStorageM[index];
        }
        else if(eggStorage[index])
        {
            storageType = 3f;
            storage = 0;
            storageM = 0;
        }

        if(emptyHC)
        {
            honeyStorage[index] = 0f;
            honeyStorageM[index] = 0;

            nectarStorage[index] = 0f;
            nectarStorageM[index] = 0;

            pollenStorage[index] = 0f;
            pollenStorageM[index] = 0;

            drawTile(index);
        }

        return new float[]{storageType, storage, storageM};
    }

    public int getEmptyHCNectar()
    {
        for(int i = 0; i < honeycombNum; i++)
        {
            if(!eggStorage[i] && honeyStorage[i] == 0 && pollenStorage[i] == 0 && nectarStorage[i] * Math.Pow(1000, nectarStorageM[i]) < nectarCapacity * Math.Pow(1000, nectarCapacityM))
            {
                Debug.Log(i);
                return i;
            }
        }
        return -1;
    }

    public int getEmptyHCPollen()
    {
        for(int i = 0; i < honeycombNum; i++)
        {
            if(!eggStorage[i] && honeyStorage[i] == 0 && nectarStorage[i] == 0 && pollenStorage[i] * Math.Pow(1000, pollenStorageM[i]) < pollenCapacity * Math.Pow(1000, pollenCapacityM))
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
            pollenStorage.Add(0);
            pollenStorageM.Add(0);
            eggStorage.Add(false);
            honeycombNum++;

            wax = wax - HCPrice * (float)Math.Pow(1000, -waxM + HCPriceM);
            if(wax <= 1)
            {
                wax *= 1000;
                waxM--;
            }

            StorageManager.Instance.setWax(wax, waxM);
            StorageManager.Instance.changeText();

            HiveBGManager.Instance.setupBG();

            if(mouseTilePos.x < left) left = mouseTilePos.x;
            else if(mouseTilePos.x > right) right = mouseTilePos.x;
            if(mouseTilePos.y < down) down = mouseTilePos.y;
            else if(mouseTilePos.y > up) up = mouseTilePos.y;

            Debug.Log(right + " " + left + " " + up + " " + down);

            HiveBGManager.Instance.GetDirection();
            HiveBGManager.Instance.setupBG();
            CameraManager.Instance.setBound();

            setupHC();
        }
    }

    public float changeHoneyStorage(int index, float honey, int honeyM)
    {
        honeyStorage[index] += honey * (float)Math.Pow(1000, -honeyStorageM[index] + honeyM);

        float[] roundedValues = round(honeyStorage[index], honeyStorageM[index]);
        honeyStorage[index] = roundedValues[0];
        honeyStorageM[index] = (int)roundedValues[1];

        if(honeyStorage[index] <= 0f)
        {
            honeyStorage[index] = 0f;
            honeyStorageM[index] = 0;
        }

        if(honeyCapacityM < honeyStorageM[index] || (honeyCapacityM == honeyStorageM[index] && honeyCapacity < honeyStorage[index]))
        {
            Debug.Log("full");
            StorageManager.Instance.setHoney(StorageManager.Instance.getHoney() + honey * (float)Math.Pow(1000, -StorageManager.Instance.getHoneyM() + honeyM), StorageManager.Instance.getHoneyM());
            float returnValue = (honeyStorage[index] - honeyCapacity * (float)Math.Pow(1000, honeyCapacityM - honeyStorageM[index])) * (float)Math.Pow(1000, honeyStorageM[index] - honeyM);
            StorageManager.Instance.setHoney(StorageManager.Instance.getHoney() - returnValue * (float)Math.Pow(1000, -StorageManager.Instance.getHoneyM()), StorageManager.Instance.getHoneyM());

            honeyStorage[index] = honeyCapacity;
            honeyStorageM[index] = honeyCapacityM;

            drawTile(index);
            Debug.Log(returnValue);
            return returnValue;
        }

        StorageManager.Instance.setHoney(StorageManager.Instance.getHoney() + honey * (float)Math.Pow(1000, -StorageManager.Instance.getHoneyM() + honeyM), StorageManager.Instance.getHoneyM());
        drawTile(index);
        return 0;
    }

    public float changeNectarStorage(int index, float nectar, int nectarM)
    {
        nectarStorage[index] = nectarStorage[index] + nectar * (float)Math.Pow(1000, -nectarStorageM[index] + nectarM);

        float[] roundedValues = round(nectarStorage[index], nectarStorageM[index]);
        nectarStorage[index] = roundedValues[0];
        nectarStorageM[index] = (int)roundedValues[1];

        if(nectarStorage[index] <= 0f)
        {
            nectarStorage[index] = 0f;
            nectarStorageM[index] = 0;
        }

        if(nectarCapacityM < nectarStorageM[index] || (nectarCapacityM == nectarStorageM[index] && nectarCapacity < nectarStorage[index]))
        {
            Debug.Log("full");
            StorageManager.Instance.setNectar(StorageManager.Instance.getNectar() + nectar * (float)Math.Pow(1000, -StorageManager.Instance.getNectarM() + nectarM), StorageManager.Instance.getNectarM());
            float returnValue = (nectarStorage[index] - nectarCapacity * (float)Math.Pow(1000, nectarCapacityM - nectarStorageM[index])) * (float)Math.Pow(1000, nectarStorageM[index] - nectarM);
            StorageManager.Instance.setNectar(StorageManager.Instance.getNectar() - returnValue * (float)Math.Pow(1000, -StorageManager.Instance.getNectarM()), StorageManager.Instance.getNectarM());

            nectarStorage[index] = nectarCapacity;
            nectarStorageM[index] = nectarCapacityM;

            drawTile(index);
            Debug.Log(returnValue);
            return returnValue;
        }

        StorageManager.Instance.setNectar(StorageManager.Instance.getNectar() + nectar * (float)Math.Pow(1000, -StorageManager.Instance.getNectarM() + nectarM), StorageManager.Instance.getNectarM());
        drawTile(index);
        return 0;
    }

    public int findHcPos(int x, int y)
    {
        for(int i = 0; i < hcPos.Count; i++)
        {
            if(x == hcPos[i][0] && y == hcPos[i][1])
            {
                return i;
            }
        }

        return -1;
    }

    public int findHcPos(float x, float y)
    {
        Vector3Int tilePos = BGGrid.WorldToCell(new Vector3(x, y, 0));

        return findHcPos(tilePos.x, tilePos.y);
    }

    public float changePollenStorage(int index, float pollen, int pollenM)
    {
        pollenStorage[index] = pollenStorage[index] + pollen * (float)Math.Pow(1000, -pollenStorageM[index] + pollenM);

        float[] roundedValues = round(pollenStorage[index], pollenStorageM[index]);
        pollenStorage[index] = roundedValues[0];
        pollenStorageM[index] = (int)roundedValues[1];

        if(pollenStorage[index] <= 0f)
        {
            pollenStorage[index] = 0f;
            pollenStorageM[index] = 0;
        }

        if(pollenCapacityM < pollenStorageM[index] || (pollenCapacityM == pollenStorageM[index] && pollenCapacity < pollenStorage[index]))
        {
            Debug.Log("full");
            StorageManager.Instance.setPollen(StorageManager.Instance.getPollen() + pollen * (float)Math.Pow(1000, -StorageManager.Instance.getPollenM() + pollenM), StorageManager.Instance.getPollenM());
            float returnValue = (pollenStorage[index] - pollenCapacity * (float)Math.Pow(1000, pollenCapacityM - pollenStorageM[index])) * (float)Math.Pow(1000, pollenStorageM[index] - pollenM);
            StorageManager.Instance.setPollen(StorageManager.Instance.getPollen() - returnValue * (float)Math.Pow(1000, -StorageManager.Instance.getPollenM()), StorageManager.Instance.getPollenM());

            pollenStorage[index] = pollenCapacity;
            pollenStorageM[index] = pollenCapacityM;

            drawTile(index);
            Debug.Log(returnValue);
            return returnValue;
        }

        StorageManager.Instance.setPollen(StorageManager.Instance.getPollen() + pollen * (float)Math.Pow(1000, -StorageManager.Instance.getPollenM() + pollenM), StorageManager.Instance.getPollenM());

        drawTile(index);
        return 0;
    }

    public void changeEggStorage(int index, bool change)
    {
        eggStorage[index] = change;
    }

    public void drawTile(int i)
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
        else if(pollenStorage[i] > 0)
        {
            tileNum = (int)((pollenStorage[i]) / (pollenCapacity) * (float)Math.Pow(1000, pollenStorageM[i] - pollenCapacityM) * (hcTile.Length - 1));
            hiveGrid.SetTile(new Vector3Int(hcPos[i][0], hcPos[i][1], 0), hcPollenTile[tileNum]);
        } 
        else if(eggStorage[i])
        {
            hiveGrid.SetTile(new Vector3Int(hcPos[i][0], hcPos[i][1], 0), hcEggTile);
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

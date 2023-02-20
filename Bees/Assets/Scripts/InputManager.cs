using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEngine.SceneManagement;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] Tilemap hiveGrid;
    [SerializeField] Tilemap buyGrid;
    [SerializeField] Tilemap BGGrid;
    [SerializeField] Tilemap HiveGrid;
    [SerializeField] Tilemap BuildGrid;
    [SerializeField] Tile buyTile;
    [SerializeField] Tile buyTileEmpty;
    [SerializeField] Tile HiveTile;
    [SerializeField] Tile HiveTileSelected;
    [SerializeField] AnimatedTile[] buildTile;
    [SerializeField] GameObject HCHovered;
    [SerializeField] GameObject HCPriceObj;
    [SerializeField] GameObject HCPriceTxtObj;
    [SerializeField] GameObject BuyHCSliderObj;
    [SerializeField] GameObject BuildPanel;
    [SerializeField] GameObject item;
    [SerializeField] Slider BuyHCSlider; 
    [SerializeField] Button BuildButton;

    BuyHoneycombText HCPriceTxt;

    public bool couldBuy {get;set;}
    public bool isSelected{get;set;}

    public bool isHovering = false;

    public bool isBuilding{get;set;}
    public int buildType = 0;

    public Vector3 hoveredPos;

    private string HCPriceStr;

    private Vector2 mousePos;
    private Vector3 mouseWorldPos;
    private Vector3Int mouseTilePos;
    private Vector3Int mouseTilePosHive;
    private Vector3 tempPos;
    private Vector3Int buyGridPos;
    private Vector3Int buyGridPastPos;
    private Vector3Int sliderPos;

    private Vector3Int[] hivePos;

    private float HCPrice;
    private int HCPriceM;

    private float sliderValue = 0;
    private float sliderSpeed = 1f;

    private string[] multipliers;

    Camera camera;

    void Start()
    {
        isSelected = false;
        isBuilding = false;
        camera = Camera.main;
        couldBuy = true;

        BuildPanel.SetActive(false);

        resetSlider();

        HCPrice = HoneycombManager.Instance.getHCPrice();
        HCPriceM = HoneycombManager.Instance.getHCPriceM();

        HCPriceTxt = HCPriceTxtObj.GetComponent<BuyHoneycombText>();

        multipliers = StorageManager.Instance.getMultipliers();

        HCPriceStr = (HCPriceM == 0 ? Mathf.RoundToInt(HCPrice) : Mathf.Round(HCPrice * 100.0f) * 0.01f).ToString() + multipliers[HCPriceM];
        hivePos = new Vector3Int[]{new Vector3Int(-5, 5, 0), new Vector3Int(-4, 5, 0), new Vector3Int(-3, 5, 0), new Vector3Int(-2, 5, 0), new Vector3Int(-5, 4, 0), new Vector3Int(-4, 4, 0), new Vector3Int(-3, 4, 0), new Vector3Int(-2, 4, 0), new Vector3Int(-5, 3, 0), new Vector3Int(-4, 3, 0), new Vector3Int(-3, 3, 0), new Vector3Int(-2, 3, 0)};
    }

    public void TogglePanel()
    {
        BuildPanel.SetActive (!BuildPanel.activeInHierarchy);
        //BuildButton
    }

    public void ChangeBuild()
    {
        isBuilding = !isBuilding;
        //BuildButton.Selected(isBuilding);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mouseWorldPos = camera.ScreenToWorldPoint(mousePos);
        mouseTilePos = BGGrid.WorldToCell(mouseWorldPos);
        mouseTilePos = new Vector3Int(mouseTilePos.x, mouseTilePos.y, 0);

        buyGrid.SetTile(buyGridPastPos, null);
        
        buyGridPastPos = mouseTilePos;

        if(RoomManager.Instance.GetCurrentRoom().Equals("Garden"))
        {
            mouseTilePosHive = HiveGrid.WorldToCell(mouseWorldPos);
            //-4, 5, 0

            if(Array.IndexOf(hivePos, mouseTilePosHive) > -1)
            {
                HiveGrid.SetTile(new Vector3Int(-4, 5, 0), HiveTileSelected);
                if(Input.GetMouseButtonDown(0))
                {
                    RoomManager.Instance.SetCurrentRoom("Storage");
                }
            }
            else{
                HiveGrid.SetTile(new Vector3Int(-4, 5, 0), HiveTile);
            }
        }
        else if((RoomManager.Instance.GetCurrentRoom().Equals("Storage") || RoomManager.Instance.GetCurrentRoom().Equals("Machinery")) && !QueenBeeManager.Instance.guidingQueen)
        {
            if(!hiveGrid.HasTile(mouseTilePos) && BGGrid.HasTile(mouseTilePos) && mouseTilePos.y >= -2 && RoomManager.Instance.GetCurrentRoom().Equals("Storage") && !isBuilding)
            {
                if(BGGrid.HasTile(mouseTilePos))
                {
                    buyGrid.SetTile(mouseTilePos, buyTileEmpty);
                    tempPos = hiveGrid.GetCellCenterWorld(mouseTilePos);
                    HCPriceObj.transform.position = new Vector3(tempPos.x, tempPos.y + 0.1f, 0);
                    HCPriceTxt.setPriceText("-" + HCPriceStr);
                    HCPriceObj.SetActive(true);
                }
            }
            else
            {
                HCPriceObj.SetActive(false);
            }

            if(Input.GetMouseButtonDown(0))
            {
                if(hiveGrid.HasTile(mouseTilePos)) //있는거 위에 클릭
                {
                    int index = HoneycombManager.Instance.findHcPos(mouseTilePos.x, mouseTilePos.y);
                    Debug.Log(index);

                    float[] itemStorage;

                    if(!isBuilding) itemStorage = HoneycombManager.Instance.getStorageHC(index, true);
                    else itemStorage = HoneycombManager.Instance.getStorageHC(index, false);

                    Debug.Log("storage: " + itemStorage[0]);
                    if(!isBuilding && itemStorage[1] > 0f && !isSelected && !QueenBeeManager.Instance.guidingQueen)
                    {
                        GameObject newItem = Instantiate(item, BGGrid.GetCellCenterWorld(mouseTilePos), Quaternion.identity);
                        newItem.GetComponent<Item>().setItem(itemStorage, RoomManager.Instance.GetCurrentRoom());
                        switch(itemStorage[0])
                        {
                            case 0f:
                                HoneycombManager.Instance.changeHoneyStorage(index, -itemStorage[1], (int)itemStorage[2]);
                                break;
                            case 1f:
                                HoneycombManager.Instance.changeNectarStorage(index, -itemStorage[1], (int)itemStorage[2]);
                                break;
                            case 2f:
                                HoneycombManager.Instance.changePollenStorage(index, -itemStorage[1], (int)itemStorage[2]);
                                break;
                        }
                    }

                    if(isBuilding && itemStorage[1] <= 0f) //지을꺼 짓기
                    {
                        Debug.Log("ajfksjdf;kasjd");
                        BuildGrid.SetTile(mouseTilePos, buildTile[buildType]);
                        if(isBuilding) ChangeBuild();
                    }
                }
                else if(BGGrid.HasTile(mouseTilePos))
                {
                    sliderPos = mouseTilePos;
                }
            }
            
            if(Input.GetMouseButton(0))
            {
                if(hiveGrid.HasTile(mouseTilePos))
                {
                }
                else if(BGGrid.HasTile(mouseTilePos) && couldBuy)
                {
                    if(sliderPos == mouseTilePos)
                    {
                        if(sliderValue >= 1f)
                        {
                            Debug.Log("buy");
                            HoneycombManager.Instance.buyHC();
                            resetSlider();
                            BuyHCSliderObj.SetActive(false);
                        }
                        else 
                        {
                            sliderValue += sliderSpeed * Time.deltaTime;
                            BuyHCSlider.value = sliderValue;
                            BuyHCSliderObj.SetActive(true);
                        }
                    }  
                    else 
                    {
                        BuyHCSliderObj.SetActive(false);
                        resetSlider();
                    }
                }
            }
            else
            {
                BuyHCSliderObj.SetActive(false);
                resetSlider();
            }
        }

        if(RoomManager.Instance.GetCurrentRoom().Equals("Storage"))
        {
            if(hiveGrid.HasTile(mouseTilePos))
            {
                int index = HoneycombManager.Instance.findHcPos(mouseTilePos.x, mouseTilePos.y);
                Debug.Log(index);

                float[] itemStorage = HoneycombManager.Instance.getStorageHC(index, false);
                Debug.Log("storage: " + itemStorage[0]);

                if(itemStorage[1] <= 0 && isBuilding)
                {
                    HCHovered.GetComponent<SpriteRenderer>().enabled = true;
                    HCHovered.transform.position = BGGrid.GetCellCenterWorld(mouseTilePos);
                    hoveredPos = hiveGrid.GetCellCenterWorld(mouseTilePos);

                    int ind = HoneycombManager.Instance.findHcPos(mouseTilePos.x, mouseTilePos.y);

                    if(HoneycombManager.Instance.getStorageHC(ind, false)[0] <= 0) isHovering = true;
                    else isHovering = false;
                }
            }
        
            if(hiveGrid.HasTile(mouseTilePos) && (QueenBeeManager.Instance.guidingQueen || isBuilding))
            {
                HCHovered.GetComponent<SpriteRenderer>().enabled = true;
                HCHovered.transform.position = BGGrid.GetCellCenterWorld(mouseTilePos);
                hoveredPos = hiveGrid.GetCellCenterWorld(mouseTilePos);

                int index = HoneycombManager.Instance.findHcPos(mouseTilePos.x, mouseTilePos.y);

                if(HoneycombManager.Instance.getStorageHC(index, false)[0] == -1) isHovering = true;
                else isHovering = false;
            }
            else
            {
                HCHovered.GetComponent<SpriteRenderer>().enabled = false;
                isHovering = false;
            }
        }
    }

    private void resetSlider()
    {
        sliderValue = 0;
        BuyHCSlider.value = 0;
    }

    public Vector3Int getMouseTilePos() { return mouseTilePos; }
}

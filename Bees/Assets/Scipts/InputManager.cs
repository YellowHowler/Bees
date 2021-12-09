using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [SerializeField] Tilemap hiveGrid;
    [SerializeField] Tilemap buyGrid;
    [SerializeField] Tilemap BGGrid;
    [SerializeField] Tilemap HiveGrid;
    [SerializeField] Tile buyTile;
    [SerializeField] Tile buyTileEmpty;
    [SerializeField] Tile HiveTile;
    [SerializeField] Tile HiveTileSelected;
    [SerializeField] GameObject HCPriceObj;
    [SerializeField] GameObject HCPriceTxtObj;
    [SerializeField] GameObject HCManager;
    [SerializeField] GameObject StorageManager;
    [SerializeField] GameObject RoomManager;
    [SerializeField] GameObject BuyHCSliderObj;
    [SerializeField] Slider BuyHCSlider; 

    BuyHoneycombText HCPriceTxt;
    HoneycombManager HCScript;
    StorageManager SMScript;
    RoomManager RMScript;

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
        camera = Camera.main;

        resetSlider();

        HCPriceTxt = HCPriceTxtObj.GetComponent<BuyHoneycombText>();
        HCScript = HCManager.GetComponent<HoneycombManager>();
        SMScript = StorageManager.GetComponent<StorageManager>();
        RMScript = RoomManager.GetComponent<RoomManager>();

        HCPrice = HCScript.getHCPrice();
        HCPriceM = HCScript.getHCPriceM();

        multipliers = SMScript.getMultipliers();

        HCPriceStr = (HCPriceM == 0 ? Mathf.RoundToInt(HCPrice) : Mathf.Round(HCPrice * 100.0f) * 0.01f).ToString() + multipliers[HCPriceM];
        hivePos = new Vector3Int[]{new Vector3Int(-5, 5, 0), new Vector3Int(-4, 5, 0), new Vector3Int(-3, 5, 0), new Vector3Int(-2, 5, 0), new Vector3Int(-5, 4, 0), new Vector3Int(-4, 4, 0), new Vector3Int(-3, 4, 0), new Vector3Int(-2, 4, 0), new Vector3Int(-5, 3, 0), new Vector3Int(-4, 3, 0), new Vector3Int(-3, 3, 0), new Vector3Int(-2, 3, 0)};
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

        if(RMScript.GetCurrentRoom().Equals("Garden"))
        {
            mouseTilePosHive = HiveGrid.WorldToCell(mouseWorldPos);
            //-4, 5, 0

            if(Array.IndexOf(hivePos, mouseTilePosHive) > -1)
            {
                HiveGrid.SetTile(new Vector3Int(-4, 5, 0), HiveTileSelected);
            }
            else{
                HiveGrid.SetTile(new Vector3Int(-4, 5, 0), HiveTile);
            }

            if(Input.GetMouseButtonDown(0))
            {
                RMScript.SetCurrentRoom("Storage");
            }
        }
        else if(RMScript.GetCurrentRoom().Equals("Storage") || RMScript.GetCurrentRoom().Equals("Machinery"))
        {
            if(!hiveGrid.HasTile(mouseTilePos) && BGGrid.HasTile(mouseTilePos) && mouseTilePos.y >= -2 && RMScript.GetCurrentRoom().Equals("Storage"))
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
                if(hiveGrid.HasTile(mouseTilePos))
                {
                    int index = HCScript.findHcPos(mouseTilePos.x, mouseTilePos.y);
                    Debug.Log(index);
                    HCScript.getStorageHC(index);
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
                else if(BGGrid.HasTile(mouseTilePos))
                {
                    if(sliderPos == mouseTilePos)
                    {
                        if(sliderValue >= 1f)
                        {
                            Debug.Log("buy");
                            HCScript.buyHC();
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
    }

    private void resetSlider()
    {
        sliderValue = 0;
        BuyHCSlider.value = 0;
    }

    public Vector3Int getMouseTilePos() { return mouseTilePos; }
}

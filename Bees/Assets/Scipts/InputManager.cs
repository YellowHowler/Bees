using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class InputManager : MonoBehaviour
{
    [SerializeField] Tilemap hiveGrid;
    [SerializeField] Tilemap buyGrid;
    [SerializeField] Tilemap BGGrid;
    [SerializeField] Tile buyTile;
    [SerializeField] Tile buyTileEmpty;
    [SerializeField] GameObject HCPriceObj;
    [SerializeField] GameObject HCPriceTxtObj;
    [SerializeField] GameObject HCManager;
    [SerializeField] GameObject StorageManager;
    [SerializeField] GameObject RoomManager;

    BuyHoneycombText HCPriceTxt;
    HoneycombManager HCScript;
    StorageManager SMScript;
    RoomManager RMScript;

    private string HCPriceStr;

    private Vector2 mousePos;
    private Vector3 mouseWorldPos;
    private Vector3Int mouseTilePos;
    private Vector3 tempPos;
    private Vector3Int buyGridPos;
    private Vector3Int buyGridPastPos;

    private float HCPrice;
    private int HCPriceM;

    private string[] multipliers;

    Camera camera;

    void Start()
    {
        camera = Camera.main;

        HCPriceTxt = HCPriceTxtObj.GetComponent<BuyHoneycombText>();
        HCScript = HCManager.GetComponent<HoneycombManager>();
        SMScript = StorageManager.GetComponent<StorageManager>();
        RMScript = RoomManager.GetComponent<RoomManager>();

        HCPrice = HCScript.getHCPrice();
        HCPriceM = HCScript.getHCPriceM();

        multipliers = SMScript.getMultipliers();

        HCPriceStr = (HCPriceM == 0 ? Mathf.RoundToInt(HCPrice) : Mathf.Round(HCPrice * 100.0f) * 0.01f).ToString() + multipliers[HCPriceM];;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mouseWorldPos = camera.ScreenToWorldPoint(mousePos);
        mouseTilePos = BGGrid.WorldToCell(mouseWorldPos);
        mouseTilePos = new Vector3Int(mouseTilePos.x, mouseTilePos.y, 0);

        buyGrid.SetTile(buyGridPastPos, null);
        
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

        buyGridPastPos = mouseTilePos;

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log(mouseTilePos);

            if(hiveGrid.HasTile(mouseTilePos))
            {
                Debug.Log("has tile");
            }
            else if(BGGrid.HasTile(mouseTilePos))
            {
                HCScript.buyHC();
            }
        }
    }

    public Vector3Int getMouseTilePos() { return mouseTilePos; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class HiveBGManager : MonoBehaviour
{
    [SerializeField] GameObject honeycombObj;
    [SerializeField] Tilemap BGGrid;
    [SerializeField] Tile BGTile; 

    private int excessTileNum;

    private int hiveCol;
    private int hiveRow;
    private int hcNum;

    void Awake()
    {
        HoneycombManager hiveSC = honeycombObj.GetComponent<HoneycombManager>();
        excessTileNum = 6;
        hiveCol = hiveSC.getHiveCol();
        hcNum = hiveSC.getHoneycombNum();
        hiveRow = hcNum / hiveCol + 1;

        setupBG();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setupBG()
    {
        for(int i = -1 * excessTileNum; i < hiveCol + excessTileNum; i++)
        {
            for(int j = -1 * excessTileNum; j < hiveRow + excessTileNum; j++)
            {
                BGGrid.SetTile(new Vector3Int(i, j, 0), BGTile);
            }
        }
    }
}

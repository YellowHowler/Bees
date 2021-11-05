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

    HoneycombManager hiveSC;

    private Vector3Int[] exitPos;

    private int excessTileNum;
    private int hcNum;

    private int left;
    private int right;
    private int up;
    private int down;

    void Awake()
    {
        hiveSC = honeycombObj.GetComponent<HoneycombManager>();
        excessTileNum = 6;

        exitPos = new Vector3Int[]{new Vector3Int(-2, -2, 0), new Vector3Int(-1, -2, 0), new Vector3Int(-2, -1, 0)};
        //hiveCol = hiveSC.getHiveCol();
        
    }

    void Start()
    {
        left = hiveSC.getLeft();
        right = hiveSC.getRight();
        up = hiveSC.getUp();
        down = hiveSC.getDown();

        hcNum = hiveSC.getHoneycombNum();

        setupBG();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setupBG()
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
    }
}

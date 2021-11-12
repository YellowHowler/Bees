using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class GardenBGManager : MonoBehaviour
{
    [SerializeField] Tilemap FloorGrid;
    [SerializeField] Tile FloorTile; 
    private int excessTileNum = 8;
    private int left;
    private int right;
    void Awake()
    {
        left = 0;
        right = 5;

        setUpFloor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setUpFloor()
    {
        for(int i = -1 * excessTileNum + left; i < right + excessTileNum; i++)
        {
            FloorGrid.SetTile(new Vector3Int(i, -3, 0), FloorTile);
        }
    }
}

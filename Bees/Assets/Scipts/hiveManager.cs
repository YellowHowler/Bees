using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class hiveManager : Singleton<hiveManager>
{
    [SerializeField] GameObject RoomManager;
    [SerializeField] Tilemap HiveGrid;
    [SerializeField] Tile hiveTile;

    void Awake()
    {
        Debug.Log(Screen.width);
        HiveGrid.SetTile(new Vector3Int(-4, 5, 0), hiveTile);
    }

    // Update is called once per frame
    void Update()
    {
    }
}

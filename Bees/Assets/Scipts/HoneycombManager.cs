using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class HoneycombManager : MonoBehaviour
{
    [SerializeField] private int hiveCol;
    [SerializeField] Tilemap hiveGrid;
    [SerializeField] Tilemap buyGrid;
    [SerializeField] Tile[] hcTile;
    [SerializeField] Tile buyTile;

    private int[] honeyStorage;

    private int honeycombNum = 10;

    public int getHiveCol(){ return hiveCol; }
    public int getHoneycombNum(){ return honeycombNum; }

    void Awake()
    {
        honeyStorage = new int[honeycombNum];
        for(int i = 0; i < honeycombNum; i++) honeyStorage[i] = 0;

        setupHC();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setupHC()
    {
        for(int i = 0; i < honeycombNum; i++)
        {
            hiveGrid.SetTile(new Vector3Int(i % hiveCol, i / hiveCol, 0), hcTile[honeyStorage[i]]);
        }

        buyGrid.SetTile(new Vector3Int(honeycombNum % hiveCol, honeycombNum / hiveCol, 0), buyTile);
    }
}

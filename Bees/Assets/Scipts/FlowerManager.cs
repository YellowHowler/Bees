using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class FlowerManager : MonoBehaviour
{
    [SerializeField] Tilemap FlowerGrid;
    [SerializeField] Tilemap FlowerGridTemp;
    [SerializeField] Tile[] flowerTiles;

    private List<int> flowers;
    private List<bool> flowerFull;

    [SerializeField] private string[] flowerNames;
    [SerializeField] private ulong[] flowerNectar;
    [SerializeField] private ulong[] flowerPollen;

    private int totalFlower = 2;
    private int left;
    private int right;

    public int getLeft() { return left; }
    public int getRight() { return right; }
    void Awake()
    {
        flowers = new List<int>();
        flowerFull = new List<bool>();

        flowers.Add(0);
        flowers.Add(1);
        flowerFull.Add(false);
        flowerFull.Add(false);

        left = 0;
        right = 4;

        setUpFlower();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getIdleFlower()
    {
        return flowerFull.IndexOf(false);
    }

    public ulong getFlowerNectar(int index)
    {
        return flowerNectar[flowers[index]];
    }
    public ulong getFlowerPollen(int index)
    {
        return flowerNectar[flowers[index]];
    }

    public int getFlowerNum()
    {
        return flowers.Count;
    }

    public Vector3 getFlowerPos(int index)
    {
        float xDif = 0f;
        float yDif = 0f;

        switch(flowerNames[index])
        {
            case "daisy":
                yDif = 1.6f;
                break;
            case "cosmos":
                yDif = 1.6f;
                break;
        }

        return FlowerGridTemp.GetCellCenterWorld(new Vector3Int(index * 2, -1, 0)) + new Vector3(xDif, yDif, 0);
    }

    public void setFlowerFull(int index)
    {
        flowerFull[index] = true;
    }

    
    void setUpFlower()
    {
        for(int i = 0; i < flowers.Count; i++)
        {
            FlowerGrid.SetTile(new Vector3Int(i * 2, -1, 0), flowerTiles[flowers[i]]);
        }
    }
}

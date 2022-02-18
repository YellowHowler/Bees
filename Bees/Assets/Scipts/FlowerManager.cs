using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class FlowerManager : Singleton<FlowerManager>
{
    [SerializeField] Tilemap FlowerGrid;
    [SerializeField] Tilemap FlowerGridTemp;
    [SerializeField] Tile[] flowerTiles;

    private List<int> flowers;
    private List<bool> flowerFull;

    [SerializeField] private string[] flowerNames;
    [SerializeField] private float[] flowerNectar;
    [SerializeField] private int[] flowerNectarM;
    [SerializeField] private float[] flowerPollen;
    [SerializeField] private int[] flowerPollenM;

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
        flowers.Add(2);
        flowers.Add(3);

        flowerFull.Add(false);
        flowerFull.Add(false);
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

    public float getFlowerNectar(int index)
    {
        return flowerNectar[flowers[index]];
    }
    public int getFlowerNectarM(int index)
    {
        return flowerNectarM[flowers[index]];
    }
    public float getFlowerPollen(int index)
    {
        return flowerPollen[flowers[index]];
    }
    public int getFlowerPollenM(int index)
    {
        return flowerPollenM[flowers[index]];
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
            case "cosmos":
                yDif = 1.6f;
                break;
            case "oxeye_daisy":
                yDif = 1.6f;
                break;
            case "lavender":
                yDif = 2.2f;
                break;
            case "california_poppy":
                yDif = 0.4f;
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
            FlowerGrid.SetTile(new Vector3Int(i * 2, 0, 0), flowerTiles[flowers[i]]);
        }
    }
}

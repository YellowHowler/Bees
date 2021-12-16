using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject[] select;
    private int slotNum = 3;
    private int selected = 0;
    private int hovered = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < slotNum; i++)
        {
            select[i].SetActive(i == selected || i == hovered);
        }
    }

    public void setSelected(int num)
    {
        selected = num;
    }
    public void setHovered(int num)
    {
        hovered = num;
    }
}

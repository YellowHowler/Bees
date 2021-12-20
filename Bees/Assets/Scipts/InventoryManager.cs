using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject[] inventory;
    [SerializeField] Sprite[] itemImages;
    GameObject[] select;
    GameObject[] itemSlot;
    Text[] itemValue;

    private float[][] items; //{type, value, valueM}
    
    private int slotNum = 3;
    private int selected = 0;
    private int hovered = -1;

    // Start is called before the first frame update
    void Start()
    {
        select = new GameObject[slotNum];
        itemSlot = new GameObject[slotNum];
        itemValue = new Text[slotNum];

        for(int i = 0; i < slotNum; i++)
        {
            select[i] = gameObject.transform.GetChild(1).gameObject;
            itemSlot[i] = gameObject.transform.GetChild(0).gameObject;
            itemValue[i] = itemSlot[i].transform.GetChild(0).gameObject.GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < slotNum; i++)
        {
            select[i].SetActive(i == selected || i == hovered);
        }
    }

    public void updateSlots()
    {
        string[] multipliers = new string[]{"ug", "mg", "g", "kg"};

        for(int i = 0; i < slotNum; i++)
        {
            if(items[i][1] == null || items[i][1] <= 0f)
            {
                itemSlot[i].SetActive(false);
                continue;
            }
            itemSlot[i].SetActive(true);
            itemSlot[i].GetComponent<Image>().sprite = itemImages[(int)items[i][0]];
            itemValue[i].text = (items[i][1]).ToString() + multipliers[(int)items[i][2]];
        }
    }

    public void addItem(float[] item, int index)
    {
        items[index] = item;
        updateSlots();
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

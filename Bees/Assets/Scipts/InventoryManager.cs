using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject[] inventory;
    [SerializeField] Sprite[] itemImages;
    [SerializeField] GameObject item;
    GameObject[] select;
    GameObject[] itemSlot;
    Text[] itemValue;

    RoomManager RMScript;
    HoneycombManager HCScript;

    private float[][] items; //{type, value, valueM}
    
    private int slotNum = 3;
    private int selected = 0;
    private int hovered = -1;

    // Start is called before the first frame update
    void Start()
    {
        RMScript = GameObject.FindWithTag("RM").GetComponent<RoomManager>();
        HCScript = GameObject.FindWithTag("HC").GetComponent<HoneycombManager>();

        select = new GameObject[slotNum];
        itemSlot = new GameObject[slotNum];
        itemValue = new Text[slotNum];

        items = new float[slotNum][];
        for(int i = 0; i < slotNum; i++)
        {
            items[i] = new float[]{-1, 0, 0};
        }

        for(int i = 0; i < slotNum; i++)
        {
            itemSlot[i] = inventory[i].transform.GetChild(0).gameObject;
            itemValue[i] = itemSlot[i].transform.GetChild(0).gameObject.GetComponent<Text>();
            select[i] = inventory[i].transform.GetChild(1).gameObject;
        }

        updateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < slotNum; i++)
        {
            select[i].SetActive(i == selected || i == hovered);
        }
    }

    private float[] round(float value, float valueM)
    {
        bool isRounding = true;

        float v = value;
        float vM = valueM;

        if(value <= 0f)
        {
            return new float[]{0f, valueM};
        }

        while(isRounding)
        {
            isRounding = false;

            if(v >= 1000f)
            {
                v /= 1000f;
                vM++;
                isRounding = true;
            }
            else if(value < 1f)
            {
                v *= 1000f;
                vM--;
                isRounding = true;
            }
            else
            {
                isRounding = false;
            }
        }  

        return new float[]{v, vM};
    }

    public void checkItem(int index)
    {
        float[] newValues = round(items[index][1], items[index][2]);
        items[index][1] = newValues[0];
        items[index][2] = newValues[1];

        if(checkExceed(index))
        {
            switch(items[index][0])
            {
                case 0f:
                    if(items[index][2] > HCScript.honeyCapacityM || (items[index][2] == HCScript.honeyCapacityM && items[index][1] > HCScript.honeyCapacity))
                    {
                        float save = items[index][1];
                        float saveM = items[index][2];

                        items[index][1] = HCScript.honeyCapacity;
                        items[index][2] = HCScript.honeyCapacityM;
                
                        GameObject newItem = Instantiate(item, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                        newItem.GetComponent<Item>().setItem(new float[]{items[index][0], save - HCScript.honeyCapacity * (float)Math.Pow(1000, HCScript.honeyCapacityM - saveM), items[index][2]}, RMScript.GetCurrentRoom());
                    }
                    break;
                case 1f:
                    if(items[index][2] > HCScript.nectarCapacityM || (items[index][2] == HCScript.nectarCapacityM && items[index][1] > HCScript.nectarCapacity))
                    {
                        float save = items[index][1];
                        float saveM = items[index][2];

                        items[index][1] = HCScript.nectarCapacity;
                        items[index][2] = HCScript.nectarCapacityM;
                
                        GameObject newItem = Instantiate(item, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                        newItem.GetComponent<Item>().setItem(new float[]{items[index][0], save - HCScript.nectarCapacity * (float)Math.Pow(1000, HCScript.nectarCapacityM - saveM), items[index][2]}, RMScript.GetCurrentRoom());
                    }
                    break;
                case 2f:
                    if(items[index][2] > HCScript.pollenCapacityM || (items[index][2] == HCScript.pollenCapacityM && items[index][1] > HCScript.pollenCapacity))
                    {
                        float save = items[index][1];
                        float saveM = items[index][2];

                        items[index][1] = HCScript.pollenCapacity;
                        items[index][2] = HCScript.pollenCapacityM;
                
                        GameObject newItem = Instantiate(item, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                        newItem.GetComponent<Item>().setItem(new float[]{items[index][0], save - HCScript.pollenCapacity * (float)Math.Pow(1000, HCScript.pollenCapacityM - saveM), items[index][2]}, RMScript.GetCurrentRoom());
                    }
                    break;
            }
        }

        updateSlots();
    }

    public bool checkExceed(int index)
    {
        switch(items[index][0])
        {
            case 0f:
                if(items[index][2] > HCScript.honeyCapacityM || (items[index][2] == HCScript.honeyCapacityM && items[index][1] > HCScript.honeyCapacity))
                {
                    return true;
                }
                break;
            case 1f:
                if(items[index][2] > HCScript.nectarCapacityM || (items[index][2] == HCScript.nectarCapacityM && items[index][1] > HCScript.nectarCapacity))
                {
                    return true;
                }
                break;
            case 2f:
                if(items[index][2] > HCScript.pollenCapacityM || (items[index][2] == HCScript.pollenCapacityM && items[index][1] > HCScript.pollenCapacity))
                {
                    return true;
                }
                break;
        }

        return false;
    }

    public void updateSlots()
    {
        string[] multipliers = new string[]{"ug", "mg", "g", "kg"};

        for(int i = 0; i < slotNum; i++)
        {
            if(items[i][1] <= 0f)
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
    public float[] getItem(int index)
    {
        return items[index];
    }
    public void setSelected(int num)
    {
        selected = num;
    }
    public void setHovered(int num)
    {
        hovered = num;
    }
    public int getHovered()
    {
        return hovered;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;
public class Item : MonoBehaviour
{
    [SerializeField] GameObject item;
    [SerializeField] Text valueText;
    [SerializeField] Sprite[] itemSprite;

    Tilemap hiveGrid;

    Rigidbody2D rgbody;

    public float type{get; set;}
    public float storage{get; set;}
    public float storageM{get; set;}
    
    public  bool isMerge {get; set;}
    private bool canMerge = false;
    private bool canStore = false;
    private bool isDrag = false;
    private bool isSelected = false;
    private bool onMouse = false;

    private string location;

    private void OnMouseEnter()
    {
        onMouse = true;
        if(Input.GetMouseButton(0))   
        {
            Destroy(rgbody);
        }
    }
    private void OnMouseDrag()
    {
        isDrag = true;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
        InputManager.Instance.couldBuy = false;
        isSelected = true;
    }
    private void OnMouseExit()
    {
        onMouse = false;
        if(gameObject.GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            rgbody = gameObject.GetComponent<Rigidbody2D>();
        }
        InputManager.Instance.couldBuy = true;
        canStore = true;
    }

    public bool checkExceed()
    {
        switch(type)
        {
            case 0f:
                if(storageM > HoneycombManager.Instance.honeyCapacityM || (storageM == HoneycombManager.Instance.honeyCapacityM && storage >= HoneycombManager.Instance.honeyCapacity))
                {
                    return true;
                }
                break;
            case 1f:
                if(storageM > HoneycombManager.Instance.nectarCapacityM || (storageM == HoneycombManager.Instance.nectarCapacityM && storage >= HoneycombManager.Instance.nectarCapacity))
                {
                    return true;
                }
                break;
            case 2f:
                if(storageM > HoneycombManager.Instance.pollenCapacityM || (storageM == HoneycombManager.Instance.pollenCapacityM && storage >= HoneycombManager.Instance.pollenCapacity))
                {
                    return true;
                }
                break;
        }

        return false;
    }

    public bool checkExceedWOEqual()
    {
        switch(type)
        {
            case 0f:
                if(storageM > HoneycombManager.Instance.honeyCapacityM || (storageM == HoneycombManager.Instance.honeyCapacityM && storage > HoneycombManager.Instance.honeyCapacity))
                {
                    return true;
                }
                break;
            case 1f:
                if(storageM > HoneycombManager.Instance.nectarCapacityM || (storageM == HoneycombManager.Instance.nectarCapacityM && storage > HoneycombManager.Instance.nectarCapacity))
                {
                    return true;
                }
                break;
            case 2f:
                if(storageM > HoneycombManager.Instance.pollenCapacityM || (storageM == HoneycombManager.Instance.pollenCapacityM && storage > HoneycombManager.Instance.pollenCapacity))
                {
                    return true;
                }
                break;
        }

        return false;
    }

    public void setItem(float[] setup, string loc)
    {
        isMerge = true;
        rgbody = gameObject.GetComponent<Rigidbody2D>();

        if(gameObject.GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            rgbody = gameObject.GetComponent<Rigidbody2D>();
        }

        location = loc;

        string[] multipliers = new string[]{"ug", "mg", "g", "kg"};
        type = setup[0];
        storage = setup[1];
        storageM = setup[2];

        if(storage <= 0f || type < 0)
        {
            Destroy(gameObject);
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = itemSprite[(int)type];
        valueText.text = storage.ToString() + multipliers[(int)storageM];

        hiveGrid = GameObject.FindWithTag("HCGrid").GetComponent<Tilemap>();

        float[] roundedValues = round(storage, storageM);
        storage = roundedValues[0];
        storageM = (int)roundedValues[1];

        switch(type)
        {
            case 0f:
                if(storageM > HoneycombManager.Instance.honeyCapacityM || (storageM == HoneycombManager.Instance.honeyCapacityM && storage > HoneycombManager.Instance.honeyCapacity))
                {
                    float colliderScaleY = gameObject.transform.lossyScale.y / 2;
                    float colliderPositionY = gameObject.transform.position.y;
                    colliderPositionY += colliderScaleY;
                                
                    float spawnObjectScaleY = gameObject.transform.lossyScale.y / 2;
                                
                    spawnObjectScaleY += colliderPositionY;
            
                    float save = storage;
                    float saveM = storageM;

                    storage = HoneycombManager.Instance.honeyCapacity;
                    storageM = HoneycombManager.Instance.honeyCapacityM;
            
                    GameObject newItem = Instantiate(item, new Vector3(transform.position.x, spawnObjectScaleY + 0.1f, 0), Quaternion.identity);
                    newItem.GetComponent<Item>().setItem(new float[]{type, save - HoneycombManager.Instance.honeyCapacity * (float)Math.Pow(1000, HoneycombManager.Instance.honeyCapacityM - saveM), storageM}, RoomManager.Instance.GetCurrentRoom());
                }
                break;
            case 1f:
                if(storageM > HoneycombManager.Instance.nectarCapacityM || (storageM == HoneycombManager.Instance.nectarCapacityM && storage > HoneycombManager.Instance.nectarCapacity))
                {
                    float colliderScaleY = gameObject.transform.lossyScale.y / 2;
                    float colliderPositionY = gameObject.transform.position.y;
                    colliderPositionY += colliderScaleY;
                                
                    float spawnObjectScaleY = gameObject.transform.lossyScale.y / 2;
                                
                    spawnObjectScaleY += colliderPositionY;
            
                    float save = storage;
                    float saveM = storageM;

                    storage = HoneycombManager.Instance.nectarCapacity;
                    storageM = HoneycombManager.Instance.nectarCapacityM;
            
                    GameObject newItem = Instantiate(item, new Vector3(transform.position.x, spawnObjectScaleY + 0.1f, 0), Quaternion.identity);
                    newItem.GetComponent<Item>().setItem(new float[]{type, save - HoneycombManager.Instance.nectarCapacity * (float)Math.Pow(1000, HoneycombManager.Instance.nectarCapacityM - saveM), storageM}, location);
                }
                break;
            case 2f:
                if(storageM > HoneycombManager.Instance.pollenCapacityM || (storageM == HoneycombManager.Instance.pollenCapacityM && storage > HoneycombManager.Instance.pollenCapacity))
                {
                    float colliderScaleY = gameObject.transform.lossyScale.y / 2;
                    float colliderPositionY = gameObject.transform.position.y;
                    colliderPositionY += colliderScaleY;
                                
                    float spawnObjectScaleY = gameObject.transform.lossyScale.y / 2;
                                
                    spawnObjectScaleY += colliderPositionY;

                    float save = storage;
                    float saveM = storageM;

                    storage = HoneycombManager.Instance.pollenCapacity;
                    storageM = HoneycombManager.Instance.pollenCapacityM;
            
                    GameObject newItem = Instantiate(item, new Vector3(transform.position.x, spawnObjectScaleY + 0.1f, 0), Quaternion.identity);
                    newItem.GetComponent<Item>().setItem(new float[]{type, save - HoneycombManager.Instance.pollenCapacity * (float)Math.Pow(1000, HoneycombManager.Instance.pollenCapacityM - saveM), storageM}, location);
                }
                break;
        }

        canMerge = true;

        storage = storageM == 0 ? Mathf.RoundToInt(storage) : Mathf.Round(storage * 100.0f) * 0.01f;
        roundedValues = round(storage, storageM);
        storage = roundedValues[0];
        storageM = (int)roundedValues[1];
        valueText.text = storage.ToString() + multipliers[(int)storageM];

        Destroy(gameObject, 180f);
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

        for(int i = 0; i < 10; i++)
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

    void OnCollisionEnter2D(Collision2D col)
    {
        canStore = true;
        
        if(canMerge && col.gameObject.CompareTag("Item") && col.gameObject.GetComponent<Item>().type == type && !checkExceed() && !(col.gameObject.GetComponent<Item>().checkExceed()))
        {
            Debug.Log("collided");
            Item ITScript = col.gameObject.GetComponent<Item>();
            float newStorage = ITScript.storage + storage * (float)Math.Pow(1000, -ITScript.storageM + storageM);
            float newStorageM = ITScript.storageM;

            float[] roundedValues = round(newStorage, newStorageM);
            newStorage = roundedValues[0];
            newStorageM = (int)roundedValues[1];

            if(isMerge)
            {
                GameObject newItem = Instantiate(item, transform.position, Quaternion.identity);
                newItem.GetComponent<Item>().setItem(new float[]{type, newStorage, newStorageM}, location);
                ITScript.isMerge = false;
            }
            
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;

        if(!isDrag && transform.position.y < -3.46f)
        {
            transform.position = new Vector3(transform.position.x, -3.46f, 0);
        }

        if(Input.GetMouseButtonUp(0))
        {
            isDrag = false;

            if(gameObject.GetComponent<Rigidbody2D>() == null)
            {
                gameObject.AddComponent<Rigidbody2D>();
                rgbody = gameObject.GetComponent<Rigidbody2D>();
            }
            
            InputManager.Instance.couldBuy = true;  

            if(onMouse && !checkExceedWOEqual() && InventoryManager.Instance.getHovered() != -1 && (InventoryManager.Instance.getItem(InventoryManager.Instance.getHovered())[0] == type || InventoryManager.Instance.getItem(InventoryManager.Instance.getHovered())[0] == -1) && !InventoryManager.Instance.checkExceed(InventoryManager.Instance.getHovered()))
            {
                int index = InventoryManager.Instance.getHovered();
                float[] invenItem = InventoryManager.Instance.getItem(index);
                InventoryManager.Instance.addItem(new float[]{type, storage + invenItem[1] * (float)Math.Pow(1000, invenItem[2] - storageM), storageM}, InventoryManager.Instance.getHovered());
                InventoryManager.Instance.checkItem(index);
                Destroy(gameObject);
            }

            if(canStore && onMouse)
            {
                Vector3Int cellPos = hiveGrid.WorldToCell(transform.position);

                if(hiveGrid.HasTile(cellPos))
                {
                    int index = HoneycombManager.Instance.findHcPos(cellPos.x, cellPos.y);
                    
                    if(HoneycombManager.Instance.getStorageHC(index, false)[0] == type || HoneycombManager.Instance.getStorageHC(index, false)[0] == -1)
                    {
                        switch(type)
                        {
                            case 0f:
                                storage = HoneycombManager.Instance.changeHoneyStorage(index, storage, (int)storageM);
                                break;
                            case 1f:
                                storage = HoneycombManager.Instance.changeNectarStorage(index, storage, (int)storageM);
                                break;
                            case 2f:
                                storage = HoneycombManager.Instance.changePollenStorage(index, storage, (int)storageM);
                                break;
                        }

                        HoneycombManager.Instance.drawTile(index);
                        
                        if(storage <= 0f)
                        {
                            Destroy(gameObject);
                        }

                        // storageM = (int)storage / 1000;
                        // storage /= (float)Math.Pow(1000, storageM);

                        string[] multipliers = new string[]{"ug", "mg", "g", "kg"};
                        valueText.text = storage.ToString() + multipliers[(int)storageM];
                    }
                }
            }
        }
        
        if(location.Equals(RoomManager.Instance.GetCurrentRoom()))
        {
            if(gameObject.GetComponent<Rigidbody2D>() == null)
            {
                gameObject.AddComponent<Rigidbody2D>();
                rgbody = gameObject.GetComponent<Rigidbody2D>();
            }

            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            valueText.gameObject.SetActive(true);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, -3.46f, 0);
            Destroy(rgbody);
            InputManager.Instance.couldBuy = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            valueText.gameObject.SetActive(false);
        }
    }
}

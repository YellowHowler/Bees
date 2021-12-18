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

    InputManager IPScript;
    RoomManager RMScript;
    HoneycombManager HCScript;

    Tilemap hiveGrid;

    Rigidbody2D rgbody;

    public float type{get; set;}
    public float storage{get; set;}
    public float storageM{get; set;}
    
    public  bool isMerge {get; set;}
    private bool canStore = false;

    private string location;

    private void OnMouseDrag()
    {
        rgbody.isKinematic = true;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.y > -3.46f) transform.position = mousePos;
        IPScript.couldBuy = false;
    }
    private void OnMouseExit()
    {
        rgbody.isKinematic = false;
        IPScript.couldBuy = true;
    }

    public void setItem(float[] setup, string loc)
    {
        isMerge = true;
        rgbody = gameObject.GetComponent<Rigidbody2D>();
        rgbody.isKinematic = false;

        location = loc;

        string[] multipliers = new string[]{"ug", "mg", "g", "kg"};
        type = setup[0];
        storage = setup[1];
        storageM = setup[2];

        gameObject.GetComponent<SpriteRenderer>().sprite = itemSprite[(int)type];
        valueText.text = storage.ToString() + multipliers[(int)storageM];

        IPScript = GameObject.FindWithTag("IN").GetComponent<InputManager>();
        RMScript = GameObject.FindWithTag("RM").GetComponent<RoomManager>();
        HCScript = GameObject.FindWithTag("HC").GetComponent<HoneycombManager>();
        hiveGrid = GameObject.FindWithTag("HCGrid").GetComponent<Tilemap>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        canStore = true;
        Debug.Log("collided");
        if(col.gameObject.CompareTag("Item") && col.gameObject.GetComponent<Item>().type == type)
        {
            Item ITScript = col.gameObject.GetComponent<Item>();
            float newStorage = ITScript.storage + storage * (float)Math.Pow(1000, -ITScript.storageM + storageM);
            float newStorageM = ITScript.storageM;

            if(newStorage >= 1000f)
            {
                newStorage /= 1000f;
                newStorageM++;
            }
            else if(newStorage < 1f)
            {
                newStorage *= 1000f;
                newStorageM--;
            }

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
        if(Input.GetMouseButtonUp(0))
        {
            rgbody.isKinematic = false;
            IPScript.couldBuy = true;

            if(canStore)
            {
                Vector3Int cellPos = hiveGrid.WorldToCell(transform.position);

                if(hiveGrid.HasTile(cellPos))
                {
                    int index = HCScript.findHcPos(cellPos.x, cellPos.y);
                    
                    if(HCScript.getStorageHC(index, false)[0] == type || HCScript.getStorageHC(index, false)[0] == -1)
                    {
                        switch(type)
                        {
                            case 0f:
                                storage = HCScript.changeHoneyStorage(index, storage, (int)storageM);
                                break;
                            case 1f:
                                storage = HCScript.changeNectarStorage(index, storage, (int)storageM);
                                break;
                            case 2f:
                                storage = HCScript.changePollenStorage(index, storage, (int)storageM);
                                break;
                        }

                        HCScript.drawTile(index);
                        
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
        if(location.Equals(RMScript.GetCurrentRoom()))
        {
            rgbody.isKinematic = false;
            gameObject.active = true;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, -3.46f, 0);
            rgbody.isKinematic = true;
            IPScript.couldBuy = true;
            gameObject.active = false;
        }
    }
}

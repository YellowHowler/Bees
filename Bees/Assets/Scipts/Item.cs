using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;
public class Item : MonoBehaviour
{
    [SerializeField] Text valueText;
    [SerializeField] Sprite[] itemSprite;

    public float type{get; set;}
    public float storage{get; set;}
    public float storageM{get; set;}
    

    public void setItem(float[] setup)
    {
        string[] multipliers = new string[]{"ug", "mg", "g", "kg"};
        type = setup[0];
        storage = setup[1];
        storageM = setup[2];

        gameObject.GetComponent<SpriteRenderer>().sprite = itemSprite[(int)type];
        valueText.text = storage.ToString() + multipliers[(int)storageM];
    }

    void OnCollisionEnter(Collision col)
    {
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

            ITScript.setItem(new float[]{type, newStorage, newStorageM});
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

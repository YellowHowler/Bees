using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyHoneycombText : MonoBehaviour
{
    [SerializeField] Text HCPriceTxt;
    public void setPriceText(string price)
    {
        HCPriceTxt.text = price;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

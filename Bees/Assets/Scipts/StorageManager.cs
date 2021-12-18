using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageManager : MonoBehaviour
{
    [SerializeField] Text honeyText;
    [SerializeField] Text nectarText;
    [SerializeField] Text pollenText;
    [SerializeField] Text waxText;

    private float honey;
    private float nectar;
    private float pollen;
    private float wax;

    private int honeyM;
    private int nectarM;
    private int pollenM;
    private int waxM;

    private string[] multipliers;

    public float getHoney(){return honey;}
    public float getNectar(){return nectar;} 
    public float getPollen(){return pollen;}
    public float getWax(){return wax;}

    public int getHoneyM(){return honeyM;}
    public int getNectarM(){return nectarM;} 
    public int getPollenM(){return pollenM;}
    public int getWaxM(){return waxM;}

    public void setHoney(float newValue, int newM) {honey = newValue;honeyM = newM;changeText();}
    public void setNectar(float newValue, int newM) {nectar = newValue;nectarM = newM;changeText();}
    public void setPollen(float newValue, int newM) {pollen = newValue;pollenM = newM;changeText();}
    public void setWax(float newValue, int newM) 
    {
        wax = newValue;
        waxM = newM;
        changeText();
    }
    
    public string[] getMultipliers() { return multipliers; }

    void Awake()
    {
        multipliers = new string[]{"ug", "mg", "g", "kg"};

        honey = 0f;
        nectar = 0f;
        pollen = 0f;
        wax = 1.3f;

        honeyM = 0;
        nectarM = 0;
        pollenM = 0;
        waxM = 1;

        changeText();
    }

    public void roundStorage()
    {
        honey = honeyM == 0 ? Mathf.RoundToInt(honey) : Mathf.Round(honey * 100.0f) * 0.01f;
        nectar = nectarM == 0 ? Mathf.RoundToInt(nectar) : Mathf.Round(nectar * 100.0f) * 0.01f;
        wax = waxM == 0 ? Mathf.RoundToInt(wax) : Mathf.Round(wax * 100.0f) * 0.01f;
        pollen = pollenM == 0 ? Mathf.RoundToInt(pollen) : Mathf.Round(pollen * 100.0f) * 0.01f;

        if(honey >= 1000)
        {
            honeyM++;
            honey /= 1000;
        }
        else if(honey < 1 && honeyM != 0)
        {
            honeyM--;
            honey *= 1000;
        }
        if(nectar >= 1000)
        {
            nectarM++;
            nectar /= 1000;
        }
        else if(nectar < 1 && nectarM != 0)
        {
            nectarM--;
            nectar *= 1000;
        }
        if(wax >= 1000)
        {
            waxM++;
            wax /= 1000;
        }
        else if(wax < 1 && waxM != 0)
        {
            waxM--;
            wax *= 1000;
        }
        if(pollen >= 1000)
        {
            pollenM++;
            pollen /= 1000;
        }
        else if(pollen < 1 && pollenM != 0)
        {
            pollenM--;
            pollen *= 1000;
        }
    }

    public void changeText()
    {   
        roundStorage();
        honeyText.text = (honey).ToString("F1") + multipliers[honeyM];
        nectarText.text = (nectar).ToString("F1") + multipliers[nectarM];
        pollenText.text = (pollen).ToString("F1") + multipliers[pollenM];
        waxText.text = (wax).ToString("F1") + multipliers[waxM];
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

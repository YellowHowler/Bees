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

    public void setHoney(float newValue) {honey = newValue;}
    public void setNectar(float newValue) {nectar = newValue;}
    public void setPollen(float newValue) {pollen = newValue;}
    public void setWax(float newValue) {wax = newValue;}
    
    public string[] getMultipliers() { return multipliers; }

    void Awake()
    {
        multipliers = new string[]{"", "A", "B", "C", "D"};

        honey = 5f;
        nectar = 5f;
        pollen = 5f;
        wax = 5.2f;

        honeyM = 0;
        nectarM = 0;
        pollenM = 0;
        waxM = 2;

        changeText();
    }

    public void changeText()
    {
        honeyText.text = (honeyM == 0 ? Mathf.RoundToInt(honey) : Mathf.Round(honey * 100.0f) * 0.01f).ToString() + multipliers[honeyM];
        nectarText.text = (nectarM == 0 ? Mathf.RoundToInt(nectar) : Mathf.Round(nectar * 100.0f) * 0.01f).ToString() + multipliers[nectarM];
        pollenText.text = (pollenM == 0 ? Mathf.RoundToInt(pollen) : Mathf.Round(pollen * 100.0f) * 0.01f).ToString() + multipliers[pollenM];
        waxText.text = (waxM == 0 ? Mathf.RoundToInt(wax) : Mathf.Round(wax * 100.0f) * 0.01f).ToString() + multipliers[waxM];
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

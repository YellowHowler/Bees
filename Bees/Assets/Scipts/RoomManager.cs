using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject StorageGrid;
    [SerializeField] GameObject GardenGrid;
    [SerializeField] GameObject HiveBGGrid;
    [SerializeField] GameObject Transition;

    private string currentRoom = "Storage";
    void Awake()
    {
        StorageGrid.SetActive(true);
        SetCurrentRoom("Storage");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetCurrentRoom() { return currentRoom; }

    public void SetCurrentRoom(string name) 
    {
        Instantiate(Transition);

        currentRoom = name; 

        if(name.Equals("Storage"))
        {
            StorageGrid.SetActive(true);
            HiveBGGrid.SetActive(true);
            GardenGrid.SetActive(false);
        }
        else if(name.Equals("Machinery"))
        {
            StorageGrid.SetActive(false);
            HiveBGGrid.SetActive(true);
            GardenGrid.SetActive(false);
        }
        else if(name.Equals("Garden"))
        {
            StorageGrid.SetActive(false);
            HiveBGGrid.SetActive(false);
            GardenGrid.SetActive(true);
        }
    }
}

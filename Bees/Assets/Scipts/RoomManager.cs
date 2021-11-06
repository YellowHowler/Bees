using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject StorageGrid;
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
        }
        else if(name.Equals("Machinery"))
        {
            StorageGrid.SetActive(false);
        }
    }
}

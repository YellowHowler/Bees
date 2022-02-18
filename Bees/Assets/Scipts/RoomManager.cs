using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    [SerializeField] GameObject StorageGrid;
    [SerializeField] GameObject GardenGrid;
    [SerializeField] GameObject HiveBGGrid;
    [SerializeField] GameObject Transition;


    Camera camera;

    private string currentRoom = "Storage";

    private float temp;
    void Awake()
    {
        camera = Camera.main;
        StorageGrid.SetActive(true);
        SetCurrentRoom("Storage");
    }

    // Update is called once per frame
    void Start()
    {
        temp = CameraManager.Instance.getTemp();
    }

    public string GetCurrentRoom() { return currentRoom; }

    public void SetCurrentRoom(string name) 
    {
        Instantiate(Transition);

        currentRoom = name; 

        CameraManager.Instance.RoomChange();

        if(name.Equals("Storage"))
        {
            StorageGrid.SetActive(true);
            HiveBGGrid.SetActive(true);
            GardenGrid.SetActive(false);
            camera.transform.position = new Vector3(1.8f, 0, -10);
            Debug.Log("storage");
        }
        else if(name.Equals("Machinery"))
        {
            StorageGrid.SetActive(false);
            HiveBGGrid.SetActive(true);
            GardenGrid.SetActive(false);
            camera.transform.position = new Vector3(1.8f, 0, -10);
        }
        else if(name.Equals("Garden"))
        {
            StorageGrid.SetActive(false);
            HiveBGGrid.SetActive(false);
            GardenGrid.SetActive(true);
            camera.transform.position = new Vector3(temp, 6, -10);
        }
    }

    public void SetCurrentRoom(string name, bool stopFollow) 
    {
        Instantiate(Transition);

        currentRoom = name; 

        if(stopFollow) CameraManager.Instance.RoomChange();

        if(name.Equals("Storage"))
        {
            StorageGrid.SetActive(true);
            HiveBGGrid.SetActive(true);
            GardenGrid.SetActive(false);
            camera.transform.position = new Vector3(1.8f, 0, -10);
            Debug.Log("storage");
        }
        else if(name.Equals("Machinery"))
        {
            StorageGrid.SetActive(false);
            HiveBGGrid.SetActive(true);
            GardenGrid.SetActive(false);
            camera.transform.position = new Vector3(1.8f, 0, -10);
        }
        else if(name.Equals("Garden"))
        {
            StorageGrid.SetActive(false);
            HiveBGGrid.SetActive(false);
            GardenGrid.SetActive(true);
            camera.transform.position = new Vector3(temp, 6, -10);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeManager : MonoBehaviour
{
    [SerializeField] GameObject RoomManager;
    [SerializeField] GameObject beeObj;
    RoomManager RMScript;
    
    Renderer rd;

    private float moveSpeed = 2f;

    private float[] storage;
    private int[] storageM;
    //honey, nectar, pollen, wax

    private string currentRoom;
    private string location = "Storage";

    void Awake()
    {
        rd = beeObj.GetComponent<Renderer>();
        RMScript = RoomManager.GetComponent<RoomManager>();

        location = "Storage";
    }

    // Update is called once per frame
    void Update()
    {
        move(new Vector3(3, 3, 0));

        currentRoom = RMScript.GetCurrentRoom();

        if(location.Equals(currentRoom))
        {
            rd.enabled = true;
        }
        else
        {
            rd.enabled = false;
        }
    }

    void move(Vector3 endPos)
    {
        if(endPos.x > transform.position.x) transform.localRotation = Quaternion.Euler(0, 180, 0);
        else transform.localRotation = Quaternion.Euler(0, 0, 0);

        transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
    }
}

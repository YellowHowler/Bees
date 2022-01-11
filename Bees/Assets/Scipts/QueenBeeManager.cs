using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class QueenBeeManager : MonoBehaviour
{
    [SerializeField] GameObject beeObj;
    [SerializeField] Slider eggSlider;

    GameObject RoomManager;
    GameObject HiveManager;
    GameObject HiveBGManager;
    GameObject FlowerManager;
    GameObject StorageManager;

    RoomManager RMScript;
    HiveBGManager HVBGScript;
    FlowerManager FLScript;
    HoneycombManager HCScript;
    StorageManager SMScript;

    Renderer rd;
    Animator ani;
    Tilemap BGTemp;

    private Vector3 destination;
    private Vector3 exitPos;

    private float moveSpeed = 2.6f;
    private float sliderSpeed = 2f;

    private float[] storage;
    private int[] storageM;
    //honey, nectar, pollen, wax

    private int flower = -1;
    private int honeycomb = -1;

    private string currentRoom;
    private string location = "Storage";

    private int currentDestination = 0;
    //going to exit, going to flower, waiting, going to hive, going to honeycomb

    private float onFlower = 0f;
    private float flowerTime = 5f;

    private float left;
    private float right;
    private float up;
    private float down;
    private float offset = 4f;

    void Awake()
    {
        RoomManager = GameObject.FindWithTag("RM");
        HiveBGManager = GameObject.FindWithTag("HVBG");
        FlowerManager = GameObject.FindWithTag("FL");
        HiveManager = GameObject.FindWithTag("HC");
        StorageManager = GameObject.FindWithTag("SM");

        BGTemp = GameObject.FindWithTag("BGTemp").GetComponent<Tilemap>();

        rd = beeObj.GetComponent<Renderer>();
        ani = beeObj.GetComponent<Animator>();

        RMScript = RoomManager.GetComponent<RoomManager>();
        HVBGScript = HiveBGManager.GetComponent<HiveBGManager>();
        FLScript = FlowerManager.GetComponent<FlowerManager>();
        HCScript = HiveManager.GetComponent<HoneycombManager>();
        SMScript = StorageManager.GetComponent<StorageManager>();

        location = "Storage";

        destination = transform.position;
    }

    void Start()
    {
        exitPos = HVBGScript.getExitPos();
        
        ani.SetBool("isStopped", false);
        
        left = BGTemp.GetCellCenterWorld(new Vector3Int(HCScript.getLeft(), 0, 0)).x + offset + 1f;
        right = BGTemp.GetCellCenterWorld(new Vector3Int(HCScript.getRight(), 0, 0)).x - offset - 1f;
        up = BGTemp.GetCellCenterWorld(new Vector3Int(0, HCScript.getUp(), 0)).y + offset;
        down = BGTemp.GetCellCenterWorld(new Vector3Int(0, HCScript.getDown(), 0)).y - offset;

        Debug.Log("left: " + left + "\n" + "right: " + right + "\n" + "up: " + up + "\n" + "down: " + down + "\n");

        setDestination(new Vector3(Random.Range(left, right), Random.Range(down, up), 0));
        startMoveRandom();
    }

    private void hide()
    {
        if(location.Equals(currentRoom))
        {
            rd.enabled = true;
        }
        else
        {
            rd.enabled = false;
        }
    }

    private void startMoveRandom()
    {
        StartCoroutine("MoveRandom");
    }

    void Update()
    {
        currentRoom = RMScript.GetCurrentRoom();
        hide();
    }
    void move(Vector3 endPos)
    {
        ani.SetBool("isStopped", false);

        Vector2 direction = new Vector2(
        transform.position.x - endPos.x,
        transform.position.y - endPos.y
        );

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, 100f);
        transform.rotation = rotation;
        
        transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
    }

    private void setDestination(Vector3 newDes)
    {
        destination = newDes;
    }

    IEnumerator MoveRandom()
    {
        while(true)
        {
            moveSpeed = 2f;

            if(transform.position == destination)
            {
                ani.SetBool("isStopped", true);
                yield return new WaitForSeconds(Random.Range(0.7f, 4f));
                setDestination(new Vector3(Random.Range(left, right), Random.Range(down, up), 0));
                Debug.Log("destination: " + destination);
                ani.SetBool("isStopped", false);
            }

            move(destination);
            Debug.Log("move");
            yield return new WaitForFixedUpdate();
        }
    }
}

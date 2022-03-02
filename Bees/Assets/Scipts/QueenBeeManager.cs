using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class QueenBeeManager : Singleton<QueenBeeManager>
{
    [SerializeField] GameObject beeObj;
    [SerializeField] Slider eggSlider;
    [SerializeField] GameObject eggReadySign;
    [SerializeField] Tilemap HCGrid;
    [SerializeField] Button guideQueenButton;
    [SerializeField] GameObject dottedLineObj;

    Renderer rd;
    Animator ani;
    Tilemap BGTemp;

    LineRenderer lr;

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
    private float offset = 3.8f;


    private bool guidingQueen = false;
    private bool movingRandom = true;
    private bool eggReady = false;
    private float eggPrepareTime = 3f; //seconds

    private Vector3 prevMousePos;

    void Awake()
    {
        BGTemp = GameObject.FindWithTag("BGTemp").GetComponent<Tilemap>();
        prevMousePos = new Vector3(0, 0, 0);

        rd = beeObj.GetComponent<Renderer>();
        ani = beeObj.GetComponent<Animator>();

        location = "Storage";

        destination = transform.position;
    }

    void Start()
    {
        lr = dottedLineObj.GetComponent<LineRenderer>();
        exitPos = HiveBGManager.Instance.getExitPos();
        
        ani.SetBool("isStopped", false);
        
        left = BGTemp.GetCellCenterWorld(new Vector3Int(HoneycombManager.Instance.getLeft(), 0, 0)).x + offset + 2.5f;
        right = BGTemp.GetCellCenterWorld(new Vector3Int(HoneycombManager.Instance.getRight(), 0, 0)).x - offset - 2.5f;
        up = BGTemp.GetCellCenterWorld(new Vector3Int(0, HoneycombManager.Instance.getUp(), 0)).y + offset + 1.5f;
        down = BGTemp.GetCellCenterWorld(new Vector3Int(0, HoneycombManager.Instance.getDown(), 0)).y - offset + 0.5f;

        guideQueenButton.gameObject.SetActive(false);

        setDestination(new Vector3(Random.Range(left, right), Random.Range(down, up), 0));
        startMoveRandom(); 

        StartCoroutine("PrepareEgg");
    }

    private void hide()
    {
        if(location.Equals(currentRoom))
        {
            rd.enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            if(eggReady)
            {
                eggReadySign.SetActive(true);
            }
            else{
                eggReadySign.SetActive(false);
            }
        }
        else
        {
            rd.enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            eggReadySign.SetActive(false);
        }
    }

    private void startMoveRandom()
    {
        StartCoroutine("MoveRandom");
    }

    void Update()
    {
        currentRoom = RoomManager.Instance.GetCurrentRoom();
        hide();

        if(guidingQueen)
        {
            Vector3Int goalTilePos = HCGrid.WorldToCell(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            if(Input.GetMouseButtonDown(0) && HCGrid.HasTile(goalTilePos))
            {
                setDestination(HCGrid.GetCellCenterWorld(goalTilePos));
            }
        
        }

        if(transform.position != destination)
        {
            ani.SetBool("isStopped", false);
            move(destination);
        }
        else
        {
            ani.SetBool("isStopped", true);
        }

        if(guidingQueen)
        {
            lr.enabled = true;
            lr.SetPosition(0, transform.position);
            lr.useWorldSpace = true;
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            lr.SetPosition(1, endPos);
        }
        else
        {
            lr.enabled = false;
        }
    }

    public void GuideQueen()
    {
        guideQueenButton.interactable = false;
        setDestination(transform.position);
        guidingQueen = true;

        ani.SetBool("isStopped", true);
        movingRandom = false;

        
        //LookMouse();
    }

    public void stopFollow()
    {
        guideQueenButton.gameObject.SetActive(false);
    }

    private void LookMouse()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        if(mousePos != prevMousePos)
        {
            ani.SetBool("isStopped", false);

            Vector2 direction = new Vector2(
            transform.position.x - mousePos.x,
            transform.position.y - mousePos.y
            );

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, 100f);
            beeObj.transform.rotation = rotation;

        }
        else{
            ani.SetBool("isStopped", true);
        }

        prevMousePos = mousePos;
    }

    private void OnMouseDown()
    {
        guideQueenButton.gameObject.SetActive(true);
        guideQueenButton.interactable = true;
        
        CameraManager.Instance.FollowBee(gameObject);
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
        beeObj.transform.rotation = rotation;
        
        transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
    }

    private void setDestination(Vector3 newDes)
    {
        destination = newDes;
    }

    private void setEggReady(bool newEgg)
    {
        eggReady = newEgg;
    }
    IEnumerator MoveRandom()
    {
        while(true)
        {
            moveSpeed = 2f;

            if(transform.position == destination && movingRandom)
            {
                //ani.SetBool("isStopped", true);
                if(movingRandom) 
                {
                    yield return new WaitForSeconds(Random.Range(0.7f, 4f));
                
                    setDestination(new Vector3(Random.Range(left, right), Random.Range(down, up), 0));
                    Debug.Log("destination: " + destination);
                    //ani.SetBool("isStopped", false);
                }
            }

            
            // if(movingRandom)
            // {
            //     move(destination);
            //     Debug.Log("move");
            // }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator PrepareEgg()
    {
        yield return new WaitForSeconds(eggPrepareTime);
        setEggReady(true);
        yield break; 
    }
}

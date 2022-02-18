using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class CameraManager : Singleton<CameraManager>
{
    Camera camera;

    [SerializeField] Tilemap BGGrid;
    [SerializeField] Tilemap FlowerGrid;
    [SerializeField] Tilemap FlowerGridTemp;
    [SerializeField] GameObject honeycombObj;

    private Vector3 bgZeroPos;

    public float horizontalResolution = 1920f;
    public float ScrollSpeed = 0.3f;
    private int hcNum;
    float currentAspect;
    float cameraSize;
    float offSetX;
    float offSetY;

    float leftBound;
    float rightBound;
    float upBound;
    float downBound;

    float leftBoundGarden;
    float rightBoundGarden;
    float upBoundGarden;
    float downBoundGarden;

    private int left;
    private int right;
    private int up;
    private int down;

    private int gardenLeft;
    private int gardenRight;

    private bool isFollowing = false;
    public GameObject followingObj{get;set;}

    float temp;

    void Awake()
    {
        camera = Camera.main;
        currentAspect = (float) Screen.width / (float) Screen.height;
        cameraSize = horizontalResolution / (350);
        camera.orthographicSize = cameraSize;

        temp = camera.ScreenToWorldPoint(new Vector3((Screen.width - 1400)/2, 0, 0)).x - camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
    }

    public void setBound()
    {
        left = HoneycombManager.Instance.getLeft();
        right = HoneycombManager.Instance.getRight();
        up = HoneycombManager.Instance.getUp();
        down = HoneycombManager.Instance.getDown();

        gardenRight = FlowerManager.Instance.getFlowerNum() * 2;

        leftBound = BGGrid.GetCellCenterWorld(new Vector3Int(left, 0, 0)).x;
        rightBound = BGGrid.GetCellCenterWorld(new Vector3Int(right, 0, 0)).x;
        upBound = BGGrid.GetCellCenterWorld(new Vector3Int(0, up, 0)).y;
        downBound = BGGrid.GetCellCenterWorld(new Vector3Int(0, 0, 0)).y;

        leftBoundGarden = FlowerGridTemp.GetCellCenterWorld(new Vector3Int(-4, 0, 0)).x;
        rightBoundGarden = FlowerGridTemp.GetCellCenterWorld(new Vector3Int(gardenRight, 0, 0)).x;
        upBoundGarden = FlowerGridTemp.GetCellCenterWorld(new Vector3Int(0, 5, 0)).y;
        downBoundGarden = FlowerGridTemp.GetCellCenterWorld(new Vector3Int(0, 0, 0)).y;
    }

    void Start()
    {
        hcNum = HoneycombManager.Instance.getHoneycombNum();
        
        setBound();

        Debug.Log(leftBoundGarden);

        bgZeroPos = BGGrid.GetCellCenterWorld(new Vector3Int(0, 0, 0));

        offSetX = -2f;
        offSetY = 0f;

        camera.transform.position = BGGrid.GetCellCenterWorld(new Vector3Int(2, 0, 0));
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, -10);
    }

    public float getTemp() {return leftBoundGarden - offSetX - 0.65f + temp;}
    void Update()
    {   
        if(isFollowing)
        {
            camera.transform.position = new Vector3(followingObj.transform.position.x, followingObj.transform.position.y, -10);
        }
        if(RoomManager.Instance.GetCurrentRoom().Equals("Storage"))
        {
            if ( Input.mousePosition.x >= Screen.width * 0.96 && camera.transform.position.x < rightBound + offSetX)
            {
                camera.transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
                isFollowing = false;
            }
            else if ( Input.mousePosition.x <= Screen.width * 0.04 && camera.transform.position.x > leftBound - offSetX)
            {
                camera.transform.Translate(Vector3.left * Time.deltaTime * ScrollSpeed, Space.World);
                isFollowing = false;
            }
            if ( Input.mousePosition.y >= Screen.height * 0.96 && camera.transform.position.y < upBound + offSetY)
            {
                camera.transform.Translate(Vector3.up * Time.deltaTime * ScrollSpeed, Space.World);
                isFollowing = false;
            }
            else if ( Input.mousePosition.y <= Screen.height * 0.04 && camera.transform.position.y > downBound - offSetY)
            {
                camera.transform.Translate(Vector3.down * Time.deltaTime * ScrollSpeed, Space.World);
                isFollowing = false;
            }
        }  
        else if(RoomManager.Instance.GetCurrentRoom().Equals("Garden"))
        {
            if ( Input.mousePosition.x >= Screen.width * 0.96 && camera.transform.position.x < rightBoundGarden + offSetX - 1f)
            {
                camera.transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
                isFollowing = false;
            }
            else if ( Input.mousePosition.x <= Screen.width * 0.04 && camera.transform.position.x > leftBoundGarden - offSetX - 0.65f + temp)
            {
                Debug.Log("left");
                camera.transform.Translate(Vector3.left * Time.deltaTime * ScrollSpeed, Space.World);
                isFollowing = false;
            }
            if ( Input.mousePosition.y >= Screen.height * 0.96 && camera.transform.position.y < upBoundGarden + offSetY - 0.4f)
            {
                camera.transform.Translate(Vector3.up * Time.deltaTime * ScrollSpeed, Space.World);
                isFollowing = false;
            }
            else if ( Input.mousePosition.y <= Screen.height * 0.04 && camera.transform.position.y > downBoundGarden - offSetY)
            {
                camera.transform.Translate(Vector3.down * Time.deltaTime * ScrollSpeed, Space.World);
                isFollowing = false;
            }
        }   
    } 

    
    public void FollowBee(GameObject follow)
    {
        Debug.Log("follow");
        followingObj = follow;
        isFollowing = true;
    }

    public void RoomChange()
    {
        isFollowing = false;
        followingObj = null;
    }
}

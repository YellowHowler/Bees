using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class CameraManager : MonoBehaviour
{
    Camera camera;

    [SerializeField] Tilemap BGGrid;
    [SerializeField] GameObject honeycombObj;

    HoneycombManager hiveSC;

    private Vector3 bgZeroPos;

    public float horizontalResolution = 1920f;
    public float ScrollSpeed = 1.5f;
    private int hcNum;
    float currentAspect;
    float cameraSize;
    float offSetX;
    float offSetY;

    float leftBound;
    float rightBound;
    float upBound;
    float downBound;

    private int left;
    private int right;
    private int up;
    private int down;

    void Awake()
    {
        camera = Camera.main;
        currentAspect = (float) Screen.width / (float) Screen.height;
        cameraSize = horizontalResolution / currentAspect / (350);
        camera.orthographicSize = cameraSize;

        hiveSC = honeycombObj.GetComponent<HoneycombManager>();
    }
    void Start()
    {
        hcNum = hiveSC.getHoneycombNum();
        left = hiveSC.getLeft();
        right = hiveSC.getRight();
        up = hiveSC.getUp();
        down = hiveSC.getDown();

        leftBound = BGGrid.GetCellCenterWorld(new Vector3Int(left, 0, 0)).x;
        rightBound = BGGrid.GetCellCenterWorld(new Vector3Int(right, 0, 0)).x;
        upBound = BGGrid.GetCellCenterWorld(new Vector3Int(0, up, 0)).y;
        upBound = BGGrid.GetCellCenterWorld(new Vector3Int(0, down, 0)).y;

        bgZeroPos = BGGrid.GetCellCenterWorld(new Vector3Int(0, 0, 0));

        offSetX = -1.5f;
        offSetY = -1f;

        camera.transform.position = BGGrid.GetCellCenterWorld(new Vector3Int(2, 0, 0));
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, -10);
    }

    // Update is called once per frame
    void Update()
    {                             
        if ( Input.mousePosition.x >= Screen.width * 0.98 && camera.transform.position.x < rightBound + offSetX)
        {
            camera.transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
        }
        else if ( Input.mousePosition.x <= Screen.width * 0.02 && camera.transform.position.x > leftBound * offSetX)
        {
            camera.transform.Translate(Vector3.left * Time.deltaTime * ScrollSpeed, Space.World);
        }
        if ( Input.mousePosition.y >= Screen.height * 0.98 && camera.transform.position.y < upBound + offSetY)
        {
            camera.transform.Translate(Vector3.up * Time.deltaTime * ScrollSpeed, Space.World);
        }
        else if ( Input.mousePosition.y <= Screen.height * 0.02 && camera.transform.position.y > downBound * offSetY)
        {
            camera.transform.Translate(Vector3.down * Time.deltaTime * ScrollSpeed, Space.World);
        }
    } 
}

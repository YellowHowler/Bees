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

    private Vector3 bgZeroPos;

    public float horizontalResolution = 1920f;
    public float ScrollSpeed = 1.5f;
    private int hcNum;
    float currentAspect;
    float cameraSize;
    float offSetX;
    float offSetY;

    float xBound;
    float yBound;

    int hiveCol;
    int hiveRow;

    void Awake()
    {
        camera = Camera.main;
        currentAspect = (float) Screen.width / (float) Screen.height;
        cameraSize = horizontalResolution / currentAspect / (350);
        camera.orthographicSize = cameraSize;

        HoneycombManager hiveSC = honeycombObj.GetComponent<HoneycombManager>();
        hiveCol = hiveSC.getHiveCol();
        hcNum = hiveSC.getHoneycombNum();
        hiveRow = hcNum / hiveCol;

        xBound = BGGrid.GetCellCenterWorld(new Vector3Int(hiveCol - 1, 0, 0)).x;
        yBound = BGGrid.GetCellCenterWorld(new Vector3Int(0, hcNum/hiveCol, 0)).y;

        bgZeroPos = BGGrid.GetCellCenterWorld(new Vector3Int(0, 0, 0));

        offSetX = -2.5f;
        offSetY = -1f;
    }
    void Start()
    {
        camera.transform.position = BGGrid.GetCellCenterWorld(new Vector3Int((hiveCol - 1) / 2, hiveRow / 2 - 1, 0));
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, -10);
    }

    // Update is called once per frame
    void Update()
    {                             
        if ( Input.mousePosition.x >= Screen.width * 0.98 && camera.transform.position.x < xBound + offSetX)
        {
            camera.transform.Translate(Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
        }
        else if ( Input.mousePosition.x <= Screen.width * 0.02 && camera.transform.position.x > bgZeroPos.x + -1 * offSetX)
        {
            camera.transform.Translate(Vector3.left * Time.deltaTime * ScrollSpeed, Space.World);
        }
        if ( Input.mousePosition.y >= Screen.height * 0.98 && camera.transform.position.y < yBound + offSetY)
        {
            camera.transform.Translate(Vector3.up * Time.deltaTime * ScrollSpeed, Space.World);
        }
        else if ( Input.mousePosition.y <= Screen.height * 0.02 && camera.transform.position.y > bgZeroPos.y + -1 * offSetY)
        {
            camera.transform.Translate(Vector3.down * Time.deltaTime * ScrollSpeed, Space.World);
        }
    } 
}

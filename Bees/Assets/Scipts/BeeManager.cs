using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeeManager : MonoBehaviour
{
    [SerializeField] GameObject RoomManager;
    [SerializeField] GameObject beeObj;
    [SerializeField] GameObject HiveBGManager;
    [SerializeField] GameObject FlowerManager;
    [SerializeField] GameObject flowerCollectSliderObj;
    [SerializeField] Slider flowerCollectSlider;
    RoomManager RMScript;
    HiveBGManager HVBGScript;
    FlowerManager FLScript;
    
    Renderer rd;

    private Vector3 destination;
    private Vector3 exitPos;

    private float moveSpeed = 2.5f;
    private float sliderSpeed = 2f;

    private float[] storage;
    private int[] storageM;
    //honey, nectar, pollen, wax

    private int flower = 0;

    private string currentRoom;
    private string location = "Storage";

    private int currentDestination = 0;
    //going to exit, going to flower, waiting, going to hive, going to honeycomb

    private float onFlower = 0f;
    private float flowerTime = 10f;

    private string job = "idle";

    void Awake()
    {
        rd = beeObj.GetComponent<Renderer>();
        RMScript = RoomManager.GetComponent<RoomManager>();
        HVBGScript = HiveBGManager.GetComponent<HiveBGManager>();
        FLScript = FlowerManager.GetComponent<FlowerManager>();

        storage = new float[]{0f, 0f, 0f, 0f};
        storageM = new int[]{0, 0, 0, 0};

        location = "Storage";
    }

    void Start()
    {
        exitPos = HVBGScript.getExitPos();
        
        changeJob("flower");
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

    void Update()
    {
        currentRoom = RMScript.GetCurrentRoom();

        hide();

        if(job.Equals("flower") && currentDestination == 0)
        {
            destination = HVBGScript.getExitPos();
        }

        move(destination);

        if(transform.position == destination && job.Equals("flower"))
        {
            if(currentDestination == 0)
            {
                transform.position = new Vector3(-3, 2, 0);

                currentDestination++;
                location = "Garden";

                hide();
                Debug.Log(location);

                if(flower != -1)
                {
                    destination = FLScript.getFlowerPos(flower);
                    Debug.Log(flower);
                    Debug.Log(destination);
                }  
            }
            else if(currentDestination == 1)
            {
                currentDestination++;
                onFlower = flowerTime;
                resetSlider();

                Debug.Log("collecting");
                
                if(RMScript.GetCurrentRoom().Equals("Garden")) flowerCollectSliderObj.SetActive(true);
            }
            else if(currentDestination == 2 && onFlower > 0)
            {
                onFlower -= sliderSpeed * Time.deltaTime;
                flowerCollectSlider.value = 1- (onFlower / flowerTime);
                
                if(RMScript.GetCurrentRoom().Equals("Garden")) flowerCollectSliderObj.SetActive(true);
                else flowerCollectSliderObj.SetActive(false);
            }
            else if(currentDestination == 2 && onFlower <= 0)
            {
                flowerCollectSliderObj.SetActive(false);
                currentDestination++;
                destination = new Vector3(-6.5f, 8f, 0);
                Debug.Log("done collecting");
            }
            else if(currentDestination == 3)
            {
                location = "Storage";
            }
        }
    }

    private void changeJob(string newJob)
    {
        job = newJob;
        
        if(newJob.Equals("flower"))
        {
            flower = FLScript.getIdleFlower();
            if(flower != -1)
            {
                FLScript.setFlowerFull(flower);
            }
        }
    }

    private void resetSlider()
    {
        flowerCollectSlider.value = 0;
    }

    void move(Vector3 endPos)
    {
        if(endPos.x > transform.position.x) transform.localRotation = Quaternion.Euler(0, 180, 0);
        else transform.localRotation = Quaternion.Euler(0, 0, 0);

        transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
    }
}

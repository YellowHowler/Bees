using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Tilemaps;
using Random=UnityEngine.Random;
 
public class BeeManager : MonoBehaviour
{
   [SerializeField] GameObject beeObj;
   [SerializeField] GameObject flowerCollectSliderObj;
   [SerializeField] Slider flowerCollectSlider;
  
   Renderer rd;
   Animator ani;
   Tilemap BGTemp;
 
   private Vector3 destination;
   private Vector3 exitPos;
 
   private float moveSpeed = 2.5f;
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
 
   private string job = "idle";

   private bool movingRandom = false;

   private float left;
    private float right;
    private float up;
    private float down;
    private float offset = 3.8f;
 
   void Awake()
   {
       rd = beeObj.GetComponent<Renderer>();
       ani = beeObj.GetComponent<Animator>();

       BGTemp = GameObject.FindWithTag("BGTemp").GetComponent<Tilemap>();

       left = BGTemp.GetCellCenterWorld(new Vector3Int(HoneycombManager.Instance.getLeft(), 0, 0)).x + offset + 2.5f;
        right = BGTemp.GetCellCenterWorld(new Vector3Int(HoneycombManager.Instance.getRight(), 0, 0)).x - offset - 2.5f;
        up = BGTemp.GetCellCenterWorld(new Vector3Int(0, HoneycombManager.Instance.getUp(), 0)).y + offset + 1.5f;
        down = BGTemp.GetCellCenterWorld(new Vector3Int(0, HoneycombManager.Instance.getDown(), 0)).y - offset + 0.5f;
 
       storage = new float[]{0f, 0f, 0f, 0f};
       storageM = new int[]{0, 0, 0, 0};
 
       //honey, nectar, pollen, wax
 
       location = "Storage";
   }

   private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
            CameraManager.Instance.FollowBee(gameObject);
        }
    }
 
   void Start()
   {
       exitPos = HiveBGManager.Instance.getExitPos();
      
       changeJob("flower");
       ani.SetBool("hasPollen", false);
       ani.SetBool("isStopped", false);
   }
 
   private void hide()
   {
       if(location.Equals(currentRoom))
       {
           rd.enabled = true;
           gameObject.GetComponent<BoxCollider2D>().enabled = true;
       }
       else
       {
           rd.enabled = false;
           gameObject.GetComponent<BoxCollider2D>().enabled = false;
       }
   }
 
   void Update()
   {
       currentRoom = RoomManager.Instance.GetCurrentRoom();
 
       hide();

       if(storage[2] != 0) ani.SetBool("hasPollen", true);
       else ani.SetBool("hasPollen", false);
 
       if(job.Equals("flower"))
       {
           if(currentDestination == 0)
           {
               destination = HiveBGManager.Instance.getExitPos();
           }
 
           move(destination);
 
           if(transform.position == destination)
           {
               if(currentDestination == 0)
               {
                   transform.position = new Vector3(-5, 7, 0);
 
                   currentDestination++;
                   location = "Garden";
                   if(CameraManager.Instance.followingObj == gameObject) RoomManager.Instance.SetCurrentRoom("Garden", false);
                   ani.SetBool("isWalking", false);
                    hide();
 
                   if(flower != -1)
                   {
                       destination = FlowerManager.Instance.getFlowerPos(flower);
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
                  
                   if(RoomManager.Instance.GetCurrentRoom().Equals("Garden")) flowerCollectSliderObj.SetActive(true);
               }
               else if(currentDestination == 2 && onFlower > 0)
               {
                   onFlower -= sliderSpeed * Time.deltaTime;
                   flowerCollectSlider.value = 1- (onFlower / flowerTime);
                  
                   if(RoomManager.Instance.GetCurrentRoom().Equals("Garden")) flowerCollectSliderObj.SetActive(true);
                   else flowerCollectSliderObj.SetActive(false);
               }
               else if(currentDestination == 2 && onFlower <= 0)
               {
                   flowerCollectSliderObj.SetActive(false);
                   currentDestination++;
                   destination = new Vector3(-6.5f, 8f, 0);
                   Debug.Log("done collecting");
 
                   storage[1] += FlowerManager.Instance.getFlowerNectar(flower) * (float)Math.Pow(10.0f, (storageM[1] - FlowerManager.Instance.getFlowerNectarM(flower)) * -3);
                   storage[2] += FlowerManager.Instance.getFlowerPollen(flower) * (float)Math.Pow(10.0f, (storageM[2] - FlowerManager.Instance.getFlowerPollenM(flower)) * -3);
                   
                   if(storage[1] >= 1000)
                   {
                       storageM[1]++;
                       storage[1] /= 1000;
                   }
               }
               else if(currentDestination == 3 && location == "Garden")
               {
                   honeycomb = HoneycombManager.Instance.getEmptyHCNectar();
 
                   if(honeycomb == -1)
                   {
                       job = "idle";
                       Debug.Log("idle");
                   }
                   else
                   {
                       Debug.Log("honeycomb: " + honeycomb);
                       location = "Storage";
                       if(CameraManager.Instance.followingObj == gameObject) RoomManager.Instance.SetCurrentRoom("Storage", false);
                       ani.SetBool("isWalking", true);
                       hide();
                       transform.position = HiveBGManager.Instance.getExitPos();
                       destination = HoneycombManager.Instance.getHCTilePos(honeycomb);
                       currentDestination++;
                   } 
               }
               else if(currentDestination == 3 && location == "Storage")
               {
                   honeycomb = HoneycombManager.Instance.getEmptyHCNectar();
 
                   if(honeycomb == -1)
                   {
                       job = "idle";
                       Debug.Log("idle");
                   }
                   else
                   {
                       Debug.Log("honeycomb: " + honeycomb);
                       destination = HoneycombManager.Instance.getHCTilePos(honeycomb);
                       currentDestination++;
                   } 
               }
               else if(currentDestination == 4 && location == "Storage")
               {
                   storage[1] = HoneycombManager.Instance.changeNectarStorage(honeycomb, storage[1], storageM[1]);
 
                   if(storage[1] > 0) 
                   {
                       currentDestination = 3;
                   }
                   else if(storage[2] > 0)
                   {
                       currentDestination = 5;
                   }
                   else
                   {
                       currentDestination = 0;
                   }
               }
               else if(currentDestination == 5 && location == "Storage")
               {
                   honeycomb = HoneycombManager.Instance.getEmptyHCPollen();
 
                   if(honeycomb == -1)
                   {
                       job = "idle";
                       Debug.Log("idle");
                   }
                   else
                   {
                       Debug.Log("honeycomb: " + honeycomb);
                       destination = HoneycombManager.Instance.getHCTilePos(honeycomb);
                       currentDestination++;
                   } 
               }
               else if(currentDestination == 6 && location == "Storage")
               {
                   storage[2] = HoneycombManager.Instance.changePollenStorage(honeycomb, storage[2], storageM[2]);
 
                   if(storage[2] > 0) 
                   {
                       currentDestination = 5;
                   }
                   else
                   {
                       currentDestination = 0;
                   }
               }
           }
       }
   }
 
   private void changeJob(string newJob)
   {
       job = newJob;
      
       if(newJob.Equals("flower"))
       {
           movingRandom = false;
           ani.SetBool("isStopped", false);

           flower = FlowerManager.Instance.getIdleFlower();
 
           if(flower != -1)
           {
               FlowerManager.Instance.setFlowerFull(flower);
           }
           else
           {
               changeJob("idle");
           }
       }
       if(newJob.Equals("idle"))
       {
           movingRandom = true;
           StartCoroutine("MoveRandom");
       }
   }
 
   private void resetSlider()
   {
       flowerCollectSlider.value = 0;
   }

   private void setDestination(Vector3 newDes)
    {
        destination = newDes;
    }
 
   void move(Vector3 endPos)
   {
       if(location == "Garden")
       {
           moveSpeed = 2.5f;
            ani.SetBool("isWalking", false);
            if(endPos.x > transform.position.x) transform.localRotation = Quaternion.Euler(0, 180, 0);
            else transform.localRotation = Quaternion.Euler(0, 0, 0);
       }
       else
       {
           moveSpeed = 2.6f;
            ani.SetBool("isWalking", true);

            Vector2 direction = new Vector2(
            transform.position.x - endPos.x,
            transform.position.y - endPos.y
            );

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, 100f);
            transform.rotation = rotation;
       }
       transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
   }

   IEnumerator MoveRandom()
    {
        while(movingRandom)
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

        yield break;
    }
}
 

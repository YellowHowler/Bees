using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class BeeManager : MonoBehaviour
{
   [SerializeField] GameObject beeObj;
   [SerializeField] GameObject flowerCollectSliderObj;
   [SerializeField] Slider flowerCollectSlider;

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
 
   void Awake()
   {
       RoomManager = GameObject.FindWithTag("RM");
       HiveBGManager = GameObject.FindWithTag("HVBG");
       FlowerManager = GameObject.FindWithTag("FL");
       HiveManager = GameObject.FindWithTag("HC");
       StorageManager = GameObject.FindWithTag("SM");

       rd = beeObj.GetComponent<Renderer>();
       ani = beeObj.GetComponent<Animator>();

       RMScript = RoomManager.GetComponent<RoomManager>();
       HVBGScript = HiveBGManager.GetComponent<HiveBGManager>();
       FLScript = FlowerManager.GetComponent<FlowerManager>();
       HCScript = HiveManager.GetComponent<HoneycombManager>();
       SMScript = StorageManager.GetComponent<StorageManager>();
 
       storage = new float[]{0f, 0f, 0f, 0f};
       storageM = new int[]{0, 0, 0, 0};
 
       //honey, nectar, pollen, wax
 
       location = "Storage";
   }
 
   void Start()
   {
       exitPos = HVBGScript.getExitPos();
      
       changeJob("flower");
       ani.SetBool("hasPollen", false);
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

       if(storage[2] != 0) ani.SetBool("hasPollen", true);
       else ani.SetBool("hasPollen", false);
 
       if(job.Equals("flower"))
       {
           if(currentDestination == 0)
           {
               destination = HVBGScript.getExitPos();
           }
 
           move(destination);
 
           if(transform.position == destination)
           {
               if(currentDestination == 0)
               {
                   transform.position = new Vector3(-5, 7, 0);
 
                   currentDestination++;
                   location = "Garden";
                   ani.SetBool("isWalking", false);
                    hide();
 
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
 
                   storage[1] += FLScript.getFlowerNectar(flower) * (float)Math.Pow(10.0f, (storageM[1] - FLScript.getFlowerNectarM(flower)) * -3);
                   storage[2] += FLScript.getFlowerPollen(flower) * (float)Math.Pow(10.0f, (storageM[2] - FLScript.getFlowerPollenM(flower)) * -3);
                   
                   if(storage[1] >= 1000)
                   {
                       storageM[1]++;
                       storage[1] /= 1000;
                   }
               }
               else if(currentDestination == 3 && location == "Garden")
               {
                   honeycomb = HCScript.getEmptyHCNectar();
 
                   if(honeycomb == -1)
                   {
                       job = "idle";
                       Debug.Log("idle");
                   }
                   else
                   {
                       Debug.Log("honeycomb: " + honeycomb);
                       location = "Storage";
                       ani.SetBool("isWalking", true);
                       hide();
                       transform.position = HVBGScript.getExitPos();
                       destination = HCScript.getHCTilePos(honeycomb);
                       currentDestination++;
                   } 
               }
               else if(currentDestination == 3 && location == "Storage")
               {
                   honeycomb = HCScript.getEmptyHCNectar();
 
                   if(honeycomb == -1)
                   {
                       job = "idle";
                       Debug.Log("idle");
                   }
                   else
                   {
                       Debug.Log("honeycomb: " + honeycomb);
                       destination = HCScript.getHCTilePos(honeycomb);
                       currentDestination++;
                   } 
               }
               else if(currentDestination == 4 && location == "Storage")
               {
                   storage[1] = HCScript.changeNectarStorage(honeycomb, storage[1], storageM[1]);
 
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
                   honeycomb = HCScript.getEmptyHCPollen();
 
                   if(honeycomb == -1)
                   {
                       job = "idle";
                       Debug.Log("idle");
                   }
                   else
                   {
                       Debug.Log("honeycomb: " + honeycomb);
                       destination = HCScript.getHCTilePos(honeycomb);
                       currentDestination++;
                   } 
               }
               else if(currentDestination == 6 && location == "Storage")
               {
                   storage[2] = HCScript.changePollenStorage(honeycomb, storage[2], storageM[2]);
 
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
           flower = FLScript.getIdleFlower();
 
           if(flower != -1)
           {
               FLScript.setFlowerFull(flower);
           }
           else
           {
               job = "idle";
           }
       }
   }
 
   private void resetSlider()
   {
       flowerCollectSlider.value = 0;
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
}
 

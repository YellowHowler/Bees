using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomButtomManager : MonoBehaviour
{
    [SerializeField] Text RoomText;
    [SerializeField] GameObject RoomManager;      
    [SerializeField] string room;

    RoomManager RMScript;

    void Awake()
    {
        RMScript = RoomManager.GetComponent<RoomManager>();
    }

    public void PointerEnter()
    {
        
        RoomText.text = room;
    }

    public void PointerExit()
    {
        RoomText.text = "";
    }

    public void PointerClick()
    {
        RMScript.SetCurrentRoom(room);
    }
}

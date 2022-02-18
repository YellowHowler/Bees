using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomButtomManager : MonoBehaviour
{
    [SerializeField] Text RoomText;   
    [SerializeField] string room;

    void Awake()
    {

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
        RoomManager.Instance.SetCurrentRoom(room);
    }
}

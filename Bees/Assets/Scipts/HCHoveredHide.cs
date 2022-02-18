using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HCHoveredHide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(RoomManager.Instance.GetCurrentRoom().Equals("Garden"))
        {
            if(gameObject.GetComponent<SpriteRenderer>()) gameObject.GetComponent<SpriteRenderer>().enabled = false;
            else gameObject.SetActive(false);
        }
    }
}

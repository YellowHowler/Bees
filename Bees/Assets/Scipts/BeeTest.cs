using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeTest : MonoBehaviour
{
    Vector3 targetPos;
    void Start()
    {
        targetPos = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0f;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 1.5f * Time.deltaTime);
    }
}

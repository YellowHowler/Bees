using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeManager : MonoBehaviour
{
    private float moveSpeed = 2f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move(new Vector3(5, 5, 0));
    }

    void move(Vector3 endPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
    }
}

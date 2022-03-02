using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIndicatorScript : MonoBehaviour
{
    //[SerializeField] AnimationCurve ac;

    Vector3 startPos;
    Vector3 endPos;
    Camera camera;
    LineRenderer lr;

    Vector3 camOffset = new Vector3(0, 0, 10);
    void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        camera = Camera.main;

        startPos = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(lr == null)
            {
                lr = gameObject.AddComponent<LineRenderer>();
            }

            lr.enabled = true;
            
            //lr.widthCurve = ac;
            lr.positionCount = 2;
            //startPos = camera.ScreenToWorldPoint(Input.mousePosition) + camOffset;
            lr.SetPosition(0, startPos);
            lr.useWorldSpace = true;
        }
        if(Input.GetMouseButton(0))
        {
            endPos = camera.ScreenToWorldPoint(Input.mousePosition) + camOffset;
            lr.SetPosition(1, endPos);
        }
        if(Input.GetMouseButtonUp(0))
        {
            lr.enabled = false;
        }
    }
}

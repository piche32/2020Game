using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSightScript : MonoBehaviour
{
    [SerializeField] float rotSpeed = 150.0f; //각도
    float rx, ry;
    [SerializeField] float maxX = 90.0f; //X축 회전시 범위
    [SerializeField] float minX = -90.0f; //X축 회전시 범위
   // [SerializeField] float rangeY = 90.0f; //Y축 회전시 범위

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rx = 0.0f;
        ry = 0.0f;

        if(maxX < minX)
        {
            float swap = maxX;
            maxX = minX;
            minX = swap;
        }

        //rangeY = Mathf.Abs(rangeY);

        rb = this.GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rx += mx * rotSpeed * Time.deltaTime;
        ry += my * rotSpeed * Time.deltaTime;

        if (ry >= maxX)
        {
            ry = maxX;
        }

        else if (ry <= minX)
        {
            ry = minX;
        }

        /*if(rx >= rangeY)
        {
            rx = rangeY;
        }

        else if(rx <= -rangeY)
        {
            rx = -rangeY;
        }*/

        transform.parent.eulerAngles = new Vector3(0, rx, 0);

        //this.GetComponentInParent<Transform>().eulerAngles = new Vector3(0, rx, 0);
        transform.eulerAngles = new Vector3(-ry, rx, 0);


    }
}

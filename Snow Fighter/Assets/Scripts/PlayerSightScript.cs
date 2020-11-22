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

    [SerializeField] LayerMask playerLM;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rx = 0.0f;
        ry = 0.0f;

        if (maxX < minX)
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

        transform.parent.eulerAngles = new Vector3(0, rx, 0);
        
        transform.eulerAngles = new Vector3(-ry, rx, 0);
    }

     bool isTargetInSight(Transform obj)
    {
        Vector3 dir = (obj.position - transform.position).normalized;
        float angle = Vector3.Angle(dir, new Vector3(transform.forward.x, obj.position.y, transform.forward.z)); //y가 같은 평면 상의 각도 구하기

        if (angle < 60)
        {
            if (isTarget(obj))
            {
                return true;
            }
        }
        return false;
    }

    bool isTarget(Transform obj)
    {
        RaycastHit ray;
        Vector3 dir = (obj.position - transform.position).normalized;
        float maxDist = Vector3.Distance(obj.position, transform.position);
        Debug.DrawRay(transform.position + transform.up * 0.5f, transform.forward * maxDist, Color.blue, 0.1f);
        if (Physics.Raycast(transform.position + transform.up * 0.5f, dir, out ray, maxDist, -1 -playerLM))
        {
            return ray.transform.CompareTag(obj.tag);
        }

        Debug.Log("Raycast error, obj.tag: " + obj.tag + "obj.name: " +obj.name);
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy") return;
        if (isTargetInSight(other.transform))
        {
            this.GetComponentInParent<PlayerScript>().SetTarget(other.transform);
        }
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTarget();
    }



}

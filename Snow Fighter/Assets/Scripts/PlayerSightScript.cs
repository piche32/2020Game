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

    [SerializeField] float targetingAngle = 15.0f;

    [SerializeField] LayerMask playerLM;
    Rigidbody rb;
    Transform target;
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
        target = null;
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

        CheckTarget();
    }

    void CheckTarget()
    {
        if (target == null) return; //Enemy가 여러명으로 늘어날 때는 우선 순위 큐로 만들어서 계산
        if (isTargetInSight(target))
        {
            this.GetComponentInParent<PlayerScript>().Target = target;
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTarget(true);
            return;
        }
        this.GetComponentInParent<PlayerScript>().Target = null;
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTarget(false);
    }

     bool isTargetInSight(Transform obj)
    {
        Vector3 dir = (obj.position - transform.position).normalized;
        float angle = Vector3.Angle(dir, new Vector3(transform.forward.x, obj.position.y, transform.forward.z)); //y가 같은 평면 상의 각도 구하기
        //float angleY = Vector3.Angle(dir, new Vector3(dir.x, 0, dir.z)); //y가 같은 평면 상의 각도 구하기

        float dist = Vector3.Distance(obj.position, transform.position);

        if (dist < 3) targetingAngle = 50.0f;
        else targetingAngle = 15.0f;

        if ( angle <targetingAngle)
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
        //RaycastHit ray;
        Vector3 dir = (obj.position - transform.position).normalized;
        float maxDist = Vector3.Distance(obj.position, transform.position);
        Debug.DrawRay(transform.position, transform.forward * maxDist, Color.blue, 0.1f);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, maxDist, -1 - playerLM);
        if(hits == null)
        {
            return false;
        }
        foreach (RaycastHit hit in hits)
        {
           if(hit.collider.gameObject.tag == "Enemy")
            {
                if (hit.collider.gameObject.name == "FollowColl" || hit.collider.gameObject.name == "AttackColl") continue;
                return true;
            }
        }
        //if (Physics.Raycast(transform.position + transform.up * 0.5f, dir, out ray, maxDist, -1 -playerLM))
        //{
        //   // if (ray.transform.name == "FollowColl" || ray.transform.name == "AttackColl") return false;
        //    return ray.transform.CompareTag(obj.tag);
        //}
        
        Debug.Log("Raycast error, obj.tag: " + obj.tag + "obj.name: " +obj.name);
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy" || other.name == "FollowColl" || other.name == "AttackColl") return;
        
        if (!isTargetInSight(other.transform))
        {
            return;
        }
        target = other.transform;
        this.GetComponentInParent<PlayerScript>().Target = other.transform;
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTarget(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Enemy" || other.name == "FollowColl" || other.name == "AttackColl") return;
       target = null;
            this.GetComponentInParent<PlayerScript>().Target =null;
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTarget(false);
    }


}

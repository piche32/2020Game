using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSightScript : MonoBehaviour
{

    private Vector2 dPos;
    float rx, ry;

    [SerializeField] float rotSpeed = 150.0f; //각도
    [SerializeField] float maxX = 90.0f; //X축 회전시 범위
    [SerializeField] float minX = -90.0f; //X축 회전시 범위

    [SerializeField] float targetingDist = 10f;
    Rigidbody rb;
    [SerializeField] float offset = 10;

    Transform target;
    float targetingAngle;

    bool isCameraRotating;
    public bool IsCameraRotating { get { return isCameraRotating; } set { isCameraRotating = value; } }

    UIManager UI;
    // Start is called before the first frame update
    void Start()
    {
        rx = 0.0f;
        ry = 0.0f;

        rx = transform.parent.eulerAngles.y;
        ry = -transform.eulerAngles.x;


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


        if (maxX < minX)
        {
            float swap = maxX;
            maxX = minX;
            minX = swap;
        }

        rb = this.GetComponentInParent<Rigidbody>();

        isCameraRotating = false;

        UI = GameObject.Find("UIManager").GetComponent<UIManager>();
        target = null;
    }

    // Update is called once per frame
    void Update()
    {

        //SetCameraRot(); //카메라 회전

        CheckTarget();
    }

    public void SetCameraRot(Touch tempTouch)
    {
        #region never used
        //if (Input.touchCount < 1) return;

        ////Touch tempTouch;

        //for (int i = 0; i < Input.touchCount; i++)
        //{
        //    tempTouch = Input.GetTouch(i);

        //    //UI 터치 시 작동 막기
        //    if (tempTouch.phase == TouchPhase.Began)
        //    {
        //        isCameraRotating = true;
        //        continue;
        //    }

        //    //UI 터치 후 손 뗄 때 작동 막기
        //    if (tempTouch.phase == TouchPhase.Ended)
        //    {
        //        isCameraRotating = false;
        //        continue;
        //    }

        //    //일단 왼쪽 드래그 시 작동 막아두기
        //    if (tempTouch.position.x < Screen.height / 2.0f)
        //        continue;


        //    ////조이스틱 터치 시 작동 막기
        //    //if (tempTouch.phase == TouchPhase.Moved && tempTouch.position)
        //    //{

        //    //}
        #endregion never used

        dPos = tempTouch.deltaPosition;

        rx += dPos.x * rotSpeed * Time.deltaTime;
        ry += dPos.y * rotSpeed * Time.deltaTime;

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

    //눈 발사 시 자동 타겟팅.
    public Vector3 GetTarget()
    {
        //Vector3 dir = transform.forward;
        //RaycastHit[] hits = Physics.BoxCastAll(this.transform.position, new Vector3(10.0f, 10.0f, 10.0f), transform.forward);
        //if (hits.Length > 0)
        //{
        //    foreach (var hit in hits)
        //    {
        //        if (hit.collider.tag == "Enemy")
        //        {
        //            target = hit.collider.transform;
        //            GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTarget(target);
        //            return hit.point + transform.forward * offset;
        //        }
        //    }
        //}

        //if (Physics.Raycast(transform.position, dir, out ray, targetingDist))
        //{
        //    target = ray.collider.transform;
        //    GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTarget(target);
        //    return ray.point + transform.forward * offset; 
        //}

        return Vector3.negativeInfinity;
    }


    void CheckTarget()
    {
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTarget(null);

        if (target == null) return; //(다시)Enemy가 여러명으로 늘어날 때는 우선 순위 큐로 만들어서 계산
        if (isTargetInSight(target))
        {
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTarget(target);

            return;
        }

    }
    bool isTargetInSight(Transform obj)
    {
        Vector3 dir = (obj.position - transform.position).normalized;
        float angle = Vector3.Angle(dir, new Vector3(transform.forward.x, dir.y, transform.forward.z)); //y가 같은 평면 상의 각도 구하기

        float dist = Vector3.Distance(obj.position, transform.position);

        if (dist < 3) targetingAngle = 50.0f;
        else targetingAngle = 15.0f;

        if (angle < targetingAngle)
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
        Vector3 dir = (obj.position - transform.position).normalized;
        float maxDist = Vector3.Distance(obj.position, transform.position);
        Debug.DrawRay(transform.position, transform.forward * maxDist, Color.blue, 0.1f);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, maxDist);
        if (hits == null)
        {
            return false;
        }
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                if (hit.collider.gameObject.name == "FollowColl" || hit.collider.gameObject.name == "AttackColl") continue;
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy" || other.name == "FollowColl" || other.name == "AttackColl") return;
        target = other.transform;
        if (!isTargetInSight(other.transform))
        {
            return;
        }
        UI.SetTarget(target.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Enemy" || other.name == "FollowColl" || other.name == "AttackColl") return;
        target = null;
        UI.SetTarget(null);
    }
}

#region check target not used
/* void CheckTarget()
{
   if (target == null) return; //(다시)Enemy가 여러명으로 늘어날 때는 우선 순위 큐로 만들어서 계산
   if (isTargetInSight(target))
   {
       this.GetComponentInParent<PlayerAttack>().Target = target;
       GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTarget(true);
       return;
   }
   this.GetComponentInParent<PlayerAttack>().Target = null;
   GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTarget(false);
}

bool isTargetInSight(Transform obj)
{
   Vector3 dir = (obj.position - transform.position).normalized;
   float angle = Vector3.Angle(dir, new Vector3(transform.forward.x, dir.y, transform.forward.z)); //y가 같은 평면 상의 각도 구하기
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

 //  Debug.Log("Raycast error, obj.tag: " + obj.tag + "obj.name: " +obj.name);
   return false;
}
private void OnTriggerEnter(Collider other)
 {
        if (other.tag != "Enemy" || other.name == "FollowColl" || other.name == "AttackColl") return;

        target = other.transform;
        if (!isTargetInSight(other.transform))
 return;
 }  
this.GetComponentInParent<PlayerAttack>().Target = other.transform;
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTarget(true);
    }

private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Enemy" || other.name == "FollowColl" || other.name == "AttackColl") return;
       target = null;
            this.GetComponentInParent<PlayerAttack>().Target =null;
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetTarget(false);
    }
   */
#endregion check target not used


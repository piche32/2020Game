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
    public float offset = 10;
    // Start is called before the first frame update
    void Start()
    {
        rx = 0.0f;
        ry = 0.0f;

        if (maxX < minX) {
            float swap = maxX;
            maxX = minX;
            minX = swap;
        }

        rb = this.GetComponentInParent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        SetCameraRot(); //카메라 회전

        CheckTarget();
    }

    void SetCameraRot()
    {
        if (Input.touchCount < 1) return;

        Touch tempTouch;

        for(int i = 0; i < Input.touchCount; i++)
        {
            tempTouch = Input.GetTouch(i);

            //드래그 영역 제어
            //if (tempTouch.position.x < Screen.width / 2) continue;

            //UI 터치 시 작동 막기
            if (EventSystem.current.IsPointerOverGameObject(i)) continue;

            //UI 터치 후 손 뗄 때 작동 막기
            if (tempTouch.phase == TouchPhase.Ended) continue;

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

            break;
        }

    }

    //눈 발사 시 자동 타겟팅.
    public Vector3 GetTarget()
    {
        Vector3 dir = transform.forward;
        RaycastHit ray;
        if(Physics.Raycast(transform.position, dir, out ray, targetingDist))
            return ray.point + transform.forward * offset;
        //target이 없을 때, default target에 플레이어 움직이는 속도 더해주기 => 방법 바꿔줌
        //Vector3 interpolation;
        //if (rb.velocity.z > 0) interpolation = new Vector3(-rb.velocity.x, rb.velocity.y, rb.velocity.z);
        //else interpolation = new Vector3(-rb.velocity.x, rb.velocity.y, rb.velocity.z); 
        // return transform.position + dir * defaultTarget + interpolation; //+ rb.velocity;// + interpolation;
        return Vector3.negativeInfinity;
    }


    void CheckTarget()
    {
        Vector3 dir = transform.forward;
        RaycastHit ray;
        if (Physics.Raycast(transform.position, dir, out ray, targetingDist))
        {
            if (!ray.transform.CompareTag("Enemy"))
                GameObject.Find("UIManager").GetComponent<UIManager>().SetTarget(false);
            else
                GameObject.Find("UIManager").GetComponent<UIManager>().SetTarget(true);
        }
        else
            GameObject.Find("UIManager").GetComponent<UIManager>().SetTarget(false);

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public GameObject snow;
    public Animator animator;
    public Transform snowStart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveVector = new Vector3(h, 0.0f, v);
        //animator.SetBool("isRunning", moveVector.magnitude > 0);

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);

        if (Input.GetMouseButtonDown(0))
        {
            snow.GetComponentInChildren<Collider>().enabled = false;
            ReadyToThrow();
        }
        if (Input.GetMouseButtonUp(0))
        {
            snow.GetComponentInChildren<Collider>().enabled = true;

            animator.SetTrigger("throw");
        }
    }

    void ReadyToThrow()
    {
        animator.SetTrigger("readyToThrow");
        snow.transform.SetParent(snowStart);
        snow.transform.position = snowStart.position;
        snow.transform.rotation = snowStart.rotation;
    }

    void ThrowSnow()
    {
        snow.GetComponent<Trajectory>().moving = true;
        snow.transform.position = snowStart.position;
        snow.transform.parent = null;
        snow.GetComponent<Trajectory>().PreCalcultate();
       
    }
}

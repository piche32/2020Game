using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject snowball = null;
    float time;
    GameObject enemy;

    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float jumpPower = 10.0f;

    LayerMask groundLM;

    bool isJumping;

    [SerializeField] Transform sightCamTrans;

    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;

        if(snowball == null)
        {
            Debug.Log("Can not find Snowball.");
        }

        enemy = GameObject.FindGameObjectWithTag("Enemy");
        if(enemy == null)
        {
            Debug.Log("Can not find Enemy.");
        }

        if(sightCamTrans == null)
        {
            Debug.Log("Can not find SightCamTrans");
        }

        isJumping = false;

        groundLM = LayerMask.NameToLayer("Ground");

        animator = GetComponent<Animator>();
        animator.SetBool("isJumping", false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }

        move();

        if (Input.GetMouseButtonDown(0))
        {
            attack();
        }
        
    }
    
    void move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveVector = new Vector3(h, 0.0f, v);
        animator.SetBool("isRunning", moveVector.magnitude > 0);

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);
    }
    
    void jump()
    {
        if (isJumping) return;
        animator.Play("Jumping Up");
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower);
        isJumping = true;
        animator.SetBool("isJumping", true);
    }

    void attack()
    {
        Instantiate(snowball, sightCamTrans.position + (sightCamTrans.forward*(transform.lossyScale.z/2+1)), sightCamTrans.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isJumping && collision.transform.tag == "Ground")
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
    }


}

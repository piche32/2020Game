using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    [SerializeField]
    private JoyStickScript mJoyStick;

    [SerializeField] private float defaultMoveSpeed = 10.0f;
    [SerializeField] private float defaultJumpPower = 10.0f;

    float moveSpeed;
    float jumpPower;
    PlayerScript playerSc;
    Rigidbody rb;

    [SerializeField] private float stunTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerSc = player.GetComponent<PlayerScript>();
        rb = player.GetComponent<Rigidbody>();

        moveSpeed = defaultMoveSpeed;
        jumpPower = defaultJumpPower;

        EventContainer.Instance.Events["OnPlayerIced"].AddListener(() =>
        {
            moveSpeed = 0.0f;
            jumpPower = 0.0f;
            Invoke("StunOff", stunTime);
        });
    }

    void StunOff()
    {
        moveSpeed = defaultMoveSpeed;
        jumpPower = defaultJumpPower;
    }
    // Update is called once per frame
    void Update()
    {
        move();
    }
    void move()
    {
        if (!playerSc.IsMoving) return;
        Vector3 dx = mJoyStick.MInputDir.y * transform.forward;
        Vector3 dy = mJoyStick.MInputDir.x * transform.right;

        Vector3 moveDir = (dx + dy).normalized;
        Vector3 getVel = moveDir * moveSpeed;
        getVel.y = rb.velocity.y;
        rb.velocity = getVel;

    }

    public void jump()
    {
        if (playerSc.SightCamTrans.GetComponent<PlayerSightScript>().IsCameraRotating) return;
        if (playerSc.IsJumping) return; //점프 중이면 점프 금지
        Vector3 getVel = transform.up * jumpPower;
        getVel.x = rb.velocity.x;
        getVel.z = rb.velocity.z;
        rb.velocity = getVel;
       // this.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower);
        playerSc.IsJumping = true;
        //animator.SetBool("isJumping", true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playerSc.IsJumping && collision.transform.tag == "Ground")
        {
            playerSc.IsJumping = false;
            //animator.SetBool("isJumping", false);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (playerSc.IsJumping && collision.transform.tag == "Ground")
        {
            playerSc.IsJumping = false;
            //animator.SetBool("isJumping", false);
        }
    }
}

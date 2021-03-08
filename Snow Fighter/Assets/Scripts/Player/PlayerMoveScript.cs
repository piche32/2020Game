using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    [SerializeField]
    private JoyStickScript mJoyStick;

    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float jumpPower = 10.0f;

    PlayerScript playerSc;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerSc = player.GetComponent<PlayerScript>();
        rb = player.GetComponent<Rigidbody>();
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

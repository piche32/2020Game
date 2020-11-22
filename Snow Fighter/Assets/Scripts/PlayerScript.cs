using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] GameObject snowball = null;
    [SerializeField] UIManager UI= null;
    float time;
    GameObject enemy;
    Transform snow;
    [SerializeField] Transform snowStart = null;

    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float jumpPower = 10.0f;
    [SerializeField] float maxHP = 100.0f;
    [SerializeField] float initPower = 200.0f;
    public float InitPower { get { return initPower; } set { initPower = value; } }
    [SerializeField] float powerIncrease = 1.0f;
    [SerializeField] float maxPower = 500.0f;
    public float MaxPower { get { return maxPower; } set { maxPower = value; } }
    public float MaxHP { get { return maxHP; } set { maxHP = value; } }
    float power;
    LayerMask groundLM;

    bool isJumping;

    [SerializeField] Transform sightCamTrans;

    Animator animator;

    private float hp;
    public float Hp { get { return hp; } set { hp = value; } }

    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

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

        hp = maxHP;
        power = initPower;
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
            ReadyToThrow();
        }
        if (Input.GetMouseButton(0))
        {
            if (power > maxPower) power = maxPower;
            else power += powerIncrease;
            UI.SetPlayerPowerSlider(power);
        }
        if (Input.GetMouseButtonUp(0)){
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

    void ReadyToThrow()
    {
        animator.SetTrigger("readyToThrow");
        snow = SnowBallPoolingScript.Instance.GetObject().transform;
        snow.SetParent(snowStart);
        snow.position = snowStart.position;
        snow.rotation = snowStart.rotation;
    }
    void ThrowSnow()
    {
        snow.SetParent(null);
        power = initPower;
        if (target != null) { 
            snow.GetComponent<SnowBallScript>().Initialize("Player", power, snowStart.position, snowStart.rotation, target.transform);
        }
       else snow.GetComponent<SnowBallScript>().Initialize("Player", power, snowStart.position, snowStart.rotation);
        
        UI.SetPlayerPowerSlider(power);
       // target = null;
        Debug.Log(snowStart.position);
    }
    void attack()
    {
        animator.SetTrigger("throw");
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isJumping && collision.transform.tag == "Ground")
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }
    }

    public void checkHp()
    {
        if(hp <= 0)
        {
            Debug.Log("GameOver");
            GameManagerScript.Instance.Gameover();
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        Debug.Log(target.name);
    }
}

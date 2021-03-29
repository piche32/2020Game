using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{

    [SerializeField] UIManager UI = null;

    GameObject enemy;

   
    [SerializeField] float maxHP = 100.0f;
    [SerializeField] float powerIncrease = 1.0f;
    [SerializeField] float maxPower = 500.0f;
    public float MaxPower { get { return maxPower; } set { maxPower = value; } }
    public float MaxHP { get { return maxHP; } set { maxHP = value; } }
    float power;
    LayerMask groundLM;


    [SerializeField] Transform sightCamTrans;
    public Transform SightCamTrans { get { return sightCamTrans; } }

    bool isThrowing;
    public bool IsThrowing { get { return isThrowing; } set { isThrowing = value; } }
    bool isReadyToThrowing;
    public bool IsReadyToThrowing { get { return isReadyToThrowing; } set { isReadyToThrowing = value; } }

    private bool isMoving;
    public bool IsMoving { get { return isMoving; } set { isMoving = value; } }

    private bool isJumping;
    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }

    private float hp;
    public float Hp { get { return hp; }}

    Rigidbody rb;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
      // Cursor.lockState = CursorLockMode.Locked;

        enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy == null)
        {
            Debug.Log("Can not find Enemy.");
        }

        if (sightCamTrans == null)
        {
            Debug.Log("Can not find SightCamTrans");
        }

        isJumping = false;
        isThrowing = false;
        isReadyToThrowing = false;
        isMoving = false;

        groundLM = LayerMask.NameToLayer("Ground");

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        hp = maxHP;
    }

    void checkHp()
    {
        if (hp <= 0)
        {
            hp = 0f;
            Debug.Log("GameOver");
            GameManagerScript.Instance.Gameover();
        }
    }

    public void setHP(float value)
    {
        hp += value;
        checkHp();
        UI.SetPlayerHPSlider(hp);
    }

}

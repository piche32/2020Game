using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerScript playerSc;

    //[SerializeField] GameObject snowball = null;
    [SerializeField] UIManager UI = null;

    Transform snow;
    [SerializeField] Transform snowStart = null;

    Animator animator;

    [SerializeField] float initPower = 10.0f;
    [SerializeField] float maxPower = 30.0f;
    [SerializeField] float addedPower = 0.5f;

    float power;

    Vector3 target;
    public Vector3 Target { get { return target; } set { target = value; } }

    [SerializeField]float throwingCoolTime = 0.5f;
    float throwingTime;

    // Start is called before the first frame update

    void Start()
    {
        playerSc = this.GetComponent<PlayerScript>();
        animator = this.GetComponent<Animator>();

        throwingTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSc.IsThrowing)
        {
            if (throwingCoolTime < throwingTime)
            {
                throwingTime = 0.0f;
                playerSc.IsThrowing = false;
                playerSc.IsReadyToThrowing = false;
            }
            else
                throwingTime += Time.deltaTime;
            // Debug.Log("throwingTime: " + throwingTime);
        }
    }

    public void ReadyToThrow()
    {
        //카메라 회전 시 버튼 작동 막기
        if (playerSc.SightCamTrans.GetComponent<PlayerSightScript>().IsCameraRotating) return;

        if (playerSc.IsThrowing == true || playerSc.IsReadyToThrowing == true) return;

        playerSc.IsReadyToThrowing = true;
        animator.SetTrigger("readyToThrow");
        snow = SnowBallPoolingScript.Instance.GetObject().transform;
        snow.SetParent(snowStart);
        snow.position = snowStart.position;
        snow.rotation = snowStart.rotation;
        power = initPower;
    }

    public void chargePower()
    {
        if (power > maxPower) return;
        power += addedPower;
    }

    void ThrowSnow()
    {
        if (playerSc.IsThrowing == true) return;
        playerSc.IsThrowing = true;
        snow.SetParent(null);
        snow.GetComponent<SnowBallScript>().IsFired = true;

       target = this.GetComponentInChildren<PlayerSightScript>().GetTarget();

       snow.GetComponent<SnowBallScript>().Initialize(power, snowStart.position, snowStart.rotation, transform, target);

        StageManager.Instance.setAttackedCount();
        /*
        if(target != null) //목표물 없을 때
        {
            snow.GetComponent<SnowBallScript>().Initialize(power, snowStart.position, snowStart.rotation, transform, target.transform.position);
        }
        else
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 7, -1 - this.gameObject.layer);
            if(hits == null)
            { //마땅한 목표물이 없을 때
                snow.GetComponent<SnowBallScript>().Initialize(power, snowStart.position, snowStart.rotation, transform);
            }

            foreach(RaycastHit hit in hits)
            {
                if(hit.collider.gameObject.tag == "enemy")
                { //적이 있으면 보정
                    if (hit.collider.gameObject.name == "FollowColl" || hit.collider.gameObject.name == "AttackColl") continue;
                    snow.GetComponent<SnowBallScript>().Initialize(power, snowStart.position, snowStart.rotation, transform, hit.collider.transform);

                    return;
                }
            }
            snow.GetComponent<SnowBallScript>().Initialize(power, snowStart.position, snowStart.rotation, transform);
        }
        UI.SetPlayerPowerSlider(power);
        */
    }

    public void Attack()
    {
        animator.SetTrigger("throw");
    }

    
}

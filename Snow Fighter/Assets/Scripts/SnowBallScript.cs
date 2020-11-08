using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallScript : MonoBehaviour
{

    [SerializeField] float destroyTime = 10.0f;
    float time;

    [SerializeField] float power = 10.0f;
    [SerializeField] float damage = 10.0f;

    
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;

        GetComponent<Rigidbody>().AddForce(transform.forward * power);
    }

    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime;

        if (time > destroyTime)
            Destroy(this.gameObject);


    }

    

    private void OnCollisionEnter(Collision collision)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(collision.transform.tag == "Player")
        {
            player.GetComponent<PlayerScript>().Hp -= damage;
            Debug.Log(player.GetComponent<PlayerScript>().Hp);
            player.GetComponent<PlayerScript>().checkPlayerState();


        }

        Destroy(this.gameObject);
    }
}

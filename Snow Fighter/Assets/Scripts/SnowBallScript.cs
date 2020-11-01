using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallScript : MonoBehaviour
{

    [SerializeField] float destroyTime = 10.0f;
    float time;

    [SerializeField] float power = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        time += Time.deltaTime;

        GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.Impulse);

        if (time > destroyTime)
            Destroy(this.gameObject);


    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}

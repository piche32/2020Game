using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject snowball;
    float time;
    GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        if(enemy == null)
        {
            Debug.Log("Can not find Enemy.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*time += Time.deltaTime;
        if (time > 3.0f)
        {
            gameObject.transform.LookAt(enemy.transform.position);
            Vector3 snowballPos = transform.position;
            snowballPos += (transform.rotation * Vector3.forward);
            Instantiate(snowball, snowballPos, transform.rotation, transform);
            time = 0.0f;
        }*/
    }
}

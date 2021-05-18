using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionTest : MonoBehaviour
{
    public Transform a;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 dir = Vector3.zero; //Enemy와 Player 간의 방향 벡터
        dir.x = a.transform.position.x - this.transform.position.x;
        dir.z = a.transform.position.z - this.transform.position.z;
        dir.y = transform.forward.y;
        dir = dir.normalized;
        Vector3 look = Vector3.Slerp(this.transform.forward, dir, Time.deltaTime);
        this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);
    }
}

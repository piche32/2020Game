using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{

    public Transform enemy;

    Vector3 destPos;
    Vector3 startPos;
    float time;

    float vx; //x축 속도
    float vy;//y축 속도
    float vz;
    float g;//중력가속도
    float dat;//도착점 도달 시간
    float mh;//도착점 높이
    float dh;//진행 시간
    float my = 4.0f;//최고점 높이
    float mht = .6f; //최고점 도달 시간

    public bool moving = false;
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        startPos = transform.position;
        destPos = enemy.position;
        PreCalcultate();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving == true)
        {
            time += Time.deltaTime;
            this.transform.position = Move();
        }
    }

    public void PreCalcultate()
    {
        time = 0.0f;
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        startPos = transform.position;
        destPos = enemy.position;

        dh = destPos.y - startPos.y;
        mh = my - startPos.y;

        g = 2 * mh / (mht * mht);

        vy = Mathf.Sqrt(2 * g * mh);

        float a = g;
        float b = -2 * vy;
        float c = 2 * dh;

        dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
        vx = -(startPos.x - destPos.x) / dat;
        vz = -(startPos.z - destPos.z) / dat;

    }

    Vector3 Move()
    {
        if (time > dat) {

            return this.transform.position;
        }

        float x = startPos.x + vx * time;
        float y = startPos.y + vy * time - 0.5f * g * time * time;
        float z = startPos.z + vz * time;
        return new Vector3(x, y, z);
    }
}

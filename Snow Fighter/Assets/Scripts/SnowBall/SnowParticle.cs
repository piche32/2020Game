using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowParticle : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
       if(transform.GetComponent<ParticleSystem>().isStopped && gameObject.activeSelf) //파티클 실행 끝
        {
            SnowParticlePooling.Instance.ReturnObject(this.gameObject);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SnowBallPoolingScript : SingletonWithoutDontDistroy<SnowBallPoolingScript>
{
    protected SnowBallPoolingScript() { }
    [SerializeField] GameObject snowball;
    [SerializeField] int amount = 30;
    Queue<SnowBallScript> snowballs = new Queue<SnowBallScript>();

    // Start is called before the first frame update
    void Start()
    {
       // snowball = GameObject.Find("SnowBall");
        for (int i = 0; i < amount; i++)
        {
            snowballs.Enqueue(CreateNewObject());
        }
    }

    SnowBallScript CreateNewObject()
    {
        var newObj = Instantiate(snowball).GetComponent<SnowBallScript>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        newObj.transform.GetComponentInChildren<Collider>().isTrigger = true;
        return newObj;
    }

    public SnowBallScript GetObject()
    {
        if(snowballs.Count > 0)
        //if (Instance.snowballs.Count > 0)
        {
            //var obj = snowballs.Dequeue();
             var obj = Instance.snowballs.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            //var newObj = CreateNewObject();
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(SnowBallScript obj)
    {
        obj.gameObject.SetActive(false);
        //obj.transform.SetParent(this.transform);
        obj.transform.SetParent(this.transform);
        //snowballs.Enqueue(obj);
        Instance.snowballs.Enqueue(obj);
    }

    
}
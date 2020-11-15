using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowParticlePooling : MonoBehaviour
{
    [SerializeField] GameObject particle;
    [SerializeField] int amount = 30;
    Queue<GameObject> particles = new Queue<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            particles.Enqueue(CreateNewObject());
        }
    }

    GameObject CreateNewObject()
    {
        var newObj = Instantiate(particle);
        newObj.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public GameObject GetObject()
    {
        if(particles.Count > 0)
        {
            var obj = particles.Dequeue();
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj;
        }

        else
        {
            var newObj = CreateNewObject();
            newObj.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        particles.Enqueue(obj);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSnowBallPooling : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int amount = 5;
    Queue<GameObject> snowballs = new Queue<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < amount; i++)
        {
            snowballs.Enqueue(CreateNewObject());
        }
    }

    GameObject CreateNewObject()
    {
        var newObj = Instantiate(prefab);
        newObj.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public GameObject GetObject()
    {
        if (snowballs.Count > 0)
        {
            var obj = snowballs.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(this.transform);
        snowballs.Enqueue(obj);
    }


}

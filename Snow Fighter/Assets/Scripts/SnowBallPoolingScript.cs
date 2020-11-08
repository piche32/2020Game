using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SnowBallPoolingScript : Singleton<SnowBallPoolingScript>
{
    protected SnowBallPoolingScript() { }
    [SerializeField] GameObject snowball;
    [SerializeField] int amount = 30;
    Queue<SnowBallScript> snowballs = new Queue<SnowBallScript>();

    // Start is called before the first frame update
    void Start()
    {
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
        return newObj;
    }

    public SnowBallScript GetObject()
    {
        if (Instance.snowballs.Count > 0)
        {
            var obj = Instance.snowballs.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnObject(SnowBallScript obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.snowballs.Enqueue(obj);
    }
}

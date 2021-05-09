using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingTest : MonoBehaviour
{
    float durability;
    [SerializeField] GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        durability = 100.0f;
        obj.SetActive(false);
    }

    public void Hit(float damage)
    {
        durability -= damage;
        if(durability <= 0)
        {
            Destroy(this.gameObject);
            GameObject.Find("UIManager").GetComponent<TutorialUI>().isObjDestroyed = true;
            obj.SetActive(true);
        }
    }
}

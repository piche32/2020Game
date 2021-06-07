using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackingTest : MonoBehaviour
{
    float durability;
    [SerializeField] GameObject obj;
    AttackingTestDurability durabilityUI;

    // Start is called before the first frame update
    void Start()
    {
        durability = 100.0f;
        durabilityUI = GetComponent<AttackingTestDurability>();
        durabilityUI.Init(durability);

        if(obj == null)
        {
            Debug.LogError(string.Format("[{0}: {1}]", this.gameObject.name.ToString(), this.name.ToString()) + "There's no hamburger.");
            return;
        }
        obj.SetActive(false);
    }
    public void Hit(float damage)
    {
        durability -= damage;
        durabilityUI.SetDurability(durability);
        

        if (durability <= 0)
        {
            Destroy(this.gameObject);
            GameObject.Find("UIManager").GetComponent<TutorialUI>().isObjDestroyed = true;
            obj.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackingTest : MonoBehaviour
{
    [SerializeField] float durability = 100.0f;
    [SerializeField] GameObject obj;
    AttackingTestDurability durabilityUI;

    // Start is called before the first frame update
    void Start()
    {
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
            TutorialUI tutorialUI = GameObject.Find("UIManager").GetComponent<TutorialUI>();
            if(tutorialUI != null) tutorialUI.isObjDestroyed = true;
            obj.SetActive(true);
        }
    }
}

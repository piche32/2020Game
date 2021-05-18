using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] GameObject sightObj = null;
    Transform sight;
    float attackingDist;
    float accuracy;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAIScript enemyAI = this.gameObject.GetComponent<EnemyAIScript>();
        sight = this.gameObject.transform.Find("Sight");
        attackingDist = enemyAI.AttackingDist;
        accuracy = enemyAI.AttackingAccuracy;
        player = GameObject.Find("Player");
    }

    public Vector3 GetTarget()
    {
        //Vector3 dir = sight.forward;
        // Debug.DrawRay(sight.position, sight.forward * 10.0f, Color.blue, 0.1f);

        //transform.LookAt(player.transform.position);
        // Vector3 dir = player.transform.position + transform.up * 3.0f;// + transform.forward * 5.0f;// + new Vector3(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy), 0);

        //sightObj.transform.position = dir;
        //return dir;

        //RaycastHit[] hits = Physics.RaycastAll(sight.position, sight.forward, attackingDist);
        //if(hits != null) { 
        //    foreach(RaycastHit hit in hits)
        //    {
        //        if(hit.collider.gameObject.name == "Player")
        //        {
        //            sightObj.transform.position = hit.point;
        //            return hit.point + transform.forward * 10.0f;
        //        }
        //    }
        //}
        //if (Physics.Raycast(sight.position, dir, out ray, attackingDist))
        //{
        //    Debug.Log(ray.collider.name.ToString());
        //    sightObj.transform.position = ray.point;
        //    return ray.point + transform.forward;
        //}

        Vector3 dir;
        RaycastHit hit;

        dir = sight.forward;// + new Vector3(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy), 0);

        //Collider[] hitColliders = Physics.OverlapBox(boxColl.center + dir * attackingDist, (boxColl.size / 2) * attackingDist);
        //if(hitColliders.Length > 0)
        //{
        //    for(int i = 0; i < hitColliders.Length; i++)
        //    {
        //        if (hitColliders[i].CompareTag("Player"))
        //        {
        //            if(hitColliders[i].name != "Player")
        //            {
        //                Transform tr = hitColliders[i].transform;
        //                while(tr.name != "Player")
        //                {
        //                    tr = tr.parent;
        //                }
        //                sightObj.transform.position = tr.position;
        //                return tr.position;
        //            }
        //        }
        //    }
    //}

        if (Physics.Raycast(
            sight.position,
            dir,
            out hit, attackingDist))
        {
            if (hit.collider.gameObject.name == "Player")
            {
                sightObj.transform.position = hit.point;
                return hit.point;// + transform.forward * 5.0f;
            }
        }

        //맞으면 데미지

        /*rb.velocity = (transform.forward + 
            new Vector3(Random.Range(-accuracy, accuracy),
            Random.Range(-accuracy, accuracy), 0)) * speed;*/

        return Vector3.negativeInfinity;
    }
}

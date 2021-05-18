using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollChanger : MonoBehaviour
{
    [SerializeField]GameObject originObj;
    [SerializeField]GameObject ragdollObj;

    public void ChangeRagdoll()
    {
        originObj.transform.parent.gameObject.SetActive(false);
        CopyCharacterTransformToRagdoll(originObj.transform, ragdollObj.transform);
        ragdollObj.transform.parent.gameObject.transform.parent.gameObject.SetActive(true);

    }

    void CopyCharacterTransformToRagdoll(Transform origin, Transform ragdoll)
    {
        for(int i = 0; i < origin.childCount; i++)
        {
            if(origin.childCount != 0)
            {
                CopyCharacterTransformToRagdoll(origin.GetChild(i), ragdoll.GetChild(i));
            }

            ragdoll.GetChild(i).localPosition = origin.GetChild(i).localPosition;
            ragdoll.GetChild(i).localRotation = origin.GetChild(i).localRotation;
        }
    }
}

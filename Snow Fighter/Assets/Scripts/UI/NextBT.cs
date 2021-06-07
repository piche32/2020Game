using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBT : MonoBehaviour
{
    public void GoToNext()
    {
        GameManagerScript.Instance.GoToNext();
    }
}

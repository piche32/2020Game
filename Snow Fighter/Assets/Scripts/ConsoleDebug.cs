using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleDebug : MonoBehaviour
{
    public static bool IsNull(string assetName, string objectName, Object obj)
    {
        if (obj == null)
        {
            Debug.LogError(string.Format("[{0}] Can not find \'{1}\'.", assetName, objectName));
            return true;
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleDebug : MonoBehaviour
{
    public static bool IsNull(string scriptName, string objectName, Object obj)
    {
        if (obj == null)
        {
            Debug.LogError(string.Format("[{0}] Can not find \'{1}\'.", scriptName, objectName));
            return true;
        }
        return false;
    }
}

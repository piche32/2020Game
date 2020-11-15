using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartBTScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void restart()
    {
        Debug.Log(GameManagerScript.Instance);
        GameManagerScript.Instance.Restart();
    }
}

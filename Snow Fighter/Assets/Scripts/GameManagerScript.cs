using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManagerScript : Singleton<GameManagerScript>
{
    protected GameManagerScript() { }


    // Start is called before the first frame update
    void Start()
    {
    }
    
    // Update is called once per frame
    public void Gameover()
    {
        SceneManager.LoadScene("GameOver");
        Cursor.lockState = CursorLockMode.None;
    }

    public void Success()
    {
        SceneManager.LoadScene("Success");
        Cursor.lockState = CursorLockMode.None;
    }
    public void Restart()
    {
        SceneManager.LoadScene("Main");
        Cursor.lockState = CursorLockMode.Locked;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Quit : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

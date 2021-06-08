using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Quit : MonoBehaviour
{
    public Image fadeBlack;
    public void PlayGame()
    {
        StartCoroutine(FadeToNormal());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator FadeToNormal(bool fadeToBlack = true, int fadeSpeed = 1)
    {
        Color objectColor = fadeBlack.color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (fadeBlack.color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeBlack.color = objectColor;
                yield return null;
            }
        }
        else
        {
            while (fadeBlack.color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeBlack.color = objectColor;
                yield return null;
            }
        }
        SceneManager.LoadScene("TestScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    static public int[] visualMessageBuffer = { 0, 0 , 0, 0};
    private float tensionValue = 4f;
    static public Tension currentTension = Tension.MEDIUM;
    public Image fadeBlack;
    public TMP_Text initialTxt;
    public Dialogue initialDialogue;
    public Dialogue Final1Dialogue;
    public Dialogue Final2Dialogue;
    private void Start()
    {
        //InitialGameScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            FinalGameScene(Final1Dialogue);
    }

    public void IncreaseTension(float value)
    {
        tensionValue += value;

        CheckTension();
    }

    public void DecreaseTension(float value)
    {
        tensionValue -= value;

        CheckTension();
    }

    private void CheckTension()
    {
        Tension lastTension = currentTension;
        if(tensionValue < 2f) 
        {
            currentTension = Tension.PEACEFUL;
        }
        else if(tensionValue < 4f)
        {
            currentTension = Tension.LOW;
        }
        else if (tensionValue < 6f)
        {
            currentTension = Tension.MEDIUM;
        }
        else if (tensionValue < 8f)
        {
            currentTension = Tension.DANGER;
        }
        else
        {
            currentTension = Tension.THREAT;
        }

        if (currentTension != lastTension)
            Debug.Log("TENSION CHANGED TO: " + currentTension);
    }

    public void InitialGameScene()
    {
        StartCoroutine(RevealInitialText(initialDialogue.dialogues));
       
    }

    public void FinalGameScene(Dialogue finalDialogue)
    {
        StartCoroutine(FadeToNormal());
        StartCoroutine(RevealFinalText(finalDialogue.dialogues, 1.2f));
    }

    IEnumerator FadeToNormal(bool fadeToBlack = true, int fadeSpeed = 1)
    {
        Color objectColor = fadeBlack.color;
        float fadeAmount;

        if(fadeToBlack)
        {
            while(fadeBlack.color.a < 1)
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
    }

    private IEnumerator RevealInitialText(string[] dialogue)
    {
        yield return new WaitForSecondsRealtime(1f);
        for (int i = 0; i < dialogue.Length; ++i) //PHRASES OF NODE
        {
            var originalString = dialogue[i];
            initialTxt.text = "";
            string[] words = originalString.Split();
            var numCharsRevealed = 0;
            for (int j = 0; j < words.Length; ++j) //WORD OF PHRASE
            {
                var charss = 0;
                while (charss < words[j].Length) //CHAR OF WORD
                {
                    charss++;
                    ++numCharsRevealed;
                    initialTxt.text = originalString.Substring(0, numCharsRevealed);

                    yield return new WaitForSecondsRealtime(0.15f);
                }
                ++numCharsRevealed;
            }
            yield return new WaitForSecondsRealtime(1f);
            while (!initialTxt.text.Equals(""))
            {
                initialTxt.text = originalString.Substring(0, numCharsRevealed-1);
                numCharsRevealed--;
                yield return new WaitForSecondsRealtime(0.02f);
            }
            
        }
        StartCoroutine(FadeToNormal(false));
        initialTxt.text = "";
    }

    private IEnumerator RevealFinalText(string[] dialogue, float waitTime = 0f)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        for (int i = 0; i < dialogue.Length; ++i) //PHRASES OF NODE
        {
            var originalString = dialogue[i];
            initialTxt.text = "";
            string[] words = originalString.Split();
            var numCharsRevealed = 0;
            for (int j = 0; j < words.Length; ++j) //WORD OF PHRASE
            {
                var charss = 0;
                while (charss < words[j].Length) //CHAR OF WORD
                {
                    charss++;
                    ++numCharsRevealed;
                    initialTxt.text = originalString.Substring(0, numCharsRevealed);

                    yield return new WaitForSecondsRealtime(0.15f);
                }
                ++numCharsRevealed;
            }
            yield return new WaitForSecondsRealtime(1f);
            while (!initialTxt.text.Equals(""))
            {
                initialTxt.text = originalString.Substring(0, numCharsRevealed - 1);
                numCharsRevealed--;
                yield return new WaitForSecondsRealtime(0.02f);
            }

        }
        initialTxt.text = "";
    }
}

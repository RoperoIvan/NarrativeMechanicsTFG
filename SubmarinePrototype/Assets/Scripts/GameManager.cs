using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public int[] visualMessageBuffer = { 0, 0 , 0, 0};
    private float tensionAllyValue = 1f;
    private float tensionEnemyValue = 3f;

    static public Tension currentAllyTension = Tension.PEACEFUL;
    static public Tension currentEnemyTension = Tension.MEDIUM;

    public Image fadeBlack;
    public TMP_Text initialTxt;
    public Dialogue initialDialogue;
    public Dialogue Final1Dialogue;
    public Dialogue Final2Dialogue;
    public Dialogue Final3Dialogue;
    public Dialogue Final4Dialogue;
    public Dialogue Final5Dialogue;
    public Dialogue Final6Dialogue;
    public RadarController radarController;
    public TimeLineController timeLineController;
    public AudioSource uiAS;
    public AudioSource ambienceAS;
    static public bool isAlly;
    static public bool isEnd = false;

    private AudioClip click1;
    private AudioClip click2;
    private AudioClip click3;
    private AudioClip endBellTypeWriter;
    private AudioClip launchingMissile;
    private AudioClip missileFinal;
    private AudioClip sinking;
    private void Awake()
    {
        click1 = Resources.Load<AudioClip>("Sound/click1");
        click2 = Resources.Load<AudioClip>("Sound/click2");
        click3 = Resources.Load<AudioClip>("Sound/click3");
        endBellTypeWriter = Resources.Load<AudioClip>("Sound/bellTypeWriter");
        launchingMissile = Resources.Load<AudioClip>("Sound/torpedoFinalPlayerLaunch");
        missileFinal = Resources.Load<AudioClip>("Sound/missileLaunch");
        sinking = Resources.Load<AudioClip>("Sound/sinking");
    }
    private void Start()
    {
        InitialGameScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            ExecuteFinal(2);

        if (Input.GetKeyDown(KeyCode.V))
            ExecuteFinal(4);
    }

    public void IncreaseTension(float value, bool isAlly)
    {
        if(isAlly)
        {
            if (tensionAllyValue <= 5)
                tensionAllyValue += value;
        }
            
        else
        {
            if (tensionEnemyValue <= 5)
                tensionEnemyValue += value;
        }
            

        CheckTension(isAlly);
    }

    public void DecreaseTension(float value, bool isAlly)
    {
        if (isAlly)
        {
            if(tensionAllyValue >= 1)
                tensionAllyValue -= value;
        }
          
        else
        {
            if (tensionEnemyValue >= 1)
                tensionEnemyValue -= value;
        }
            

        CheckTension(isAlly);
    }

    private void CheckTension(bool isAlly)
    {
        if(!isAlly) //ENEMY
        {
            Tension lastTension = currentEnemyTension;
            if (tensionEnemyValue < 2f)
            {
                currentEnemyTension = Tension.PEACEFUL;
            }
            else if (tensionEnemyValue < 3f)
            {
                currentEnemyTension = Tension.LOW;
            }
            else if (tensionEnemyValue < 4f)
            {
                currentEnemyTension = Tension.MEDIUM;
            }
            else if (tensionEnemyValue < 5f)
            {
                currentEnemyTension = Tension.DANGER;
            }
            else
            {
                currentEnemyTension = Tension.THREAT;
            }

            if (currentEnemyTension > lastTension)
            {
                radarController.GoToNextCheckpoint(isAlly, false);
            }
            if (currentEnemyTension < lastTension)
            {
                radarController.GoToNextCheckpoint(isAlly, true);
            }
        }
        else //ALLY
        {
            Tension lastTension = currentAllyTension;
            if (tensionAllyValue < 2f)
            {
                currentAllyTension = Tension.PEACEFUL;
            }
            else if (tensionAllyValue < 3f)
            {
                currentAllyTension = Tension.LOW;
            }
            else if (tensionAllyValue < 4f)
            {
                currentAllyTension = Tension.MEDIUM;
            }
            else if (tensionAllyValue < 5f)
            {
                currentAllyTension = Tension.DANGER;
            }
            else
            {
                currentAllyTension = Tension.THREAT;
            }

            if (currentAllyTension > lastTension)
            {
                radarController.GoToNextCheckpoint(isAlly, true);
            }
            if (currentAllyTension < lastTension)
            {
                radarController.GoToNextCheckpoint(isAlly, false);
            }
        }
    }

    public void InitialGameScene()
    {
        StartCoroutine(RevealInitialText(initialDialogue.dialogues));
       
    }

    public void FinalGameScene(int finalDialogue)
    {
        Dialogue d;
        switch (finalDialogue)
        {
            case 0://RETREAT SHIP
                d = Final1Dialogue;
                break;
            case 1: //MISSILE BY PLAYER
                d = Final2Dialogue;
                uiAS.PlayOneShot(launchingMissile, 0.5f);
                break;
            case 2: //MISSILES BY ENEMY
                d = Final3Dialogue;
                break;
            case 3: //MISSILES BY ALLIES
                d = Final4Dialogue;
                break;
            case 4: //LEAKS BAD
                d = Final5Dialogue;
                break;
            case 5://CALIBRATE BAD
                d = Final6Dialogue;
                break;
            default:
                d = Final1Dialogue;
                Debug.Log("ERROR FINAL");
                break;
        }
        StartCoroutine(FadeToNormal());
        StartCoroutine(RevealFinalText(d.dialogues, 1.2f));
    }

    public void ExecuteFinal(int final)
    {
        timeLineController.isIntro = true;
        timeLineController.doAttack = false;
        timeLineController.DeleteAllTimeLineEvents();

        switch (final)
        {
            case 0://RETREAT SHIP
                FinalGameScene(0);
                break;
            case 1: //MISSILE BY PLAYER
                FinalGameScene(1);
                break;
            case 2: //MISSILES BY ENEMY
                uiAS.PlayOneShot(missileFinal);
                FinalGameScene(2);
                break;
            case 3: //MISSILES BY ALLIES
                uiAS.PlayOneShot(missileFinal);
                FinalGameScene(3);
                break;
            case 4: //LEAKS BAD
                uiAS.PlayOneShot(sinking);
                FinalGameScene(4);
                break;
            case 5://CALIBRATE BAD
                uiAS.PlayOneShot(sinking);
                FinalGameScene(5);
                break;
        }
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
                    int aud = Random.Range(0, 2);
                    switch(aud)
                    {
                        case 0:
                            uiAS.PlayOneShot(click1,0.5f);
                            break;
                        case 1:
                            uiAS.PlayOneShot(click2, 0.5f);
                            break;
                        case 2:
                            uiAS.PlayOneShot(click3, 0.5f);
                            break;
                    }
                    
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
        uiAS.PlayOneShot(endBellTypeWriter);
        StartCoroutine(FadeToNormal(false));
        ambienceAS.Play();
        timeLineController.isIntro = false;
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
                    int aud = Random.Range(0, 2);
                    switch (aud)
                    {
                        case 0:
                            uiAS.PlayOneShot(click1, 0.5f);
                            break;
                        case 1:
                            uiAS.PlayOneShot(click2, 0.5f);
                            break;
                        case 2:
                            uiAS.PlayOneShot(click3, 0.5f);
                            break;
                    }

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
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene("MainScene");
    }
}

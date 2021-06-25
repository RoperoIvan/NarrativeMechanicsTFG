using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float playerResponseWaitingTime = 10f;
    public float playerEnterWaitingTime = 12f;
    public ShipEvent currentShipEvent = ShipEvent.NONE;
    public FillBar bar;
    public SpriteRenderer flag;
    public Sprite defaultSprite;
    public VisualCommunicationController visualController;
    public CalibrationController calibrationController;
    public FrequencyCommunicationController frequencyCommunicationController;
    public TimeLineController timeLineController;
    public RepairController repairController;
    public GameManager gameManager;
    public Animator shipFlagAnimator;
    public Animator shipPoleAnimator;
    public Animator notebookAnimator;
    public List<Sprite> flagSprites = new List<Sprite>();
    public AudioSource uiAS;

    public Coroutine showflags;
    [HideInInspector]
    public bool isInHeight = true;
    private float timerEnterRoom = 0f;
    private bool isFirstTime = true;
    private bool waitingForPlayer = false;
    private bool showNotebook = false;
    private VisualMessage lastSendedMessage;
    private Screens goalScreen = Screens.NONE;
    private AudioClip showNote;
    private AudioClip closeNote;
    private List<Screens> currentScreens = new List<Screens>();
    private void Awake()
    {
        showNote = Resources.Load<AudioClip>("Sound/openManual");
        closeNote = Resources.Load<AudioClip>("Sound/closeManual");
        
        
    }
    private void Update()
    {
        //if (waitingForPlayer)
        //{
            for(int s = currentScreens.Count -1; s >= 0; --s)
            {
                if (PlayerController.currentScreen == currentScreens[s])
                {
                    switch (currentScreens[s])
                    {
                        case Screens.GLASS:
                            if (isInHeight)
                            {
                                SendVisualMessage();
                                waitingForPlayer = false;
                            }
                            break;
                        case Screens.CALIBRATE:
                            calibrationController.SetDecalibrateValues();
                            waitingForPlayer = false;
                            break;
                        case Screens.REPAIR:
                            repairController.SetLeaks();
                            waitingForPlayer = false;
                            break;
                        case Screens.NONE:
                            break;
                    }
                    //bar.Stopbar();
                    currentScreens.Remove(currentScreens[s]);
                }
                else if (Time.realtimeSinceStartup - timerEnterRoom >= playerEnterWaitingTime) //BAD CONSEQUENCES
                {
                    switch (currentScreens[s])
                    {
                        case Screens.GLASS:
                            gameManager.IncreaseTension(0.5f, false);
                            timeLineController.isCurrentEventOver = true;
                            break;
                        case Screens.CALIBRATE: //END FOR INCOMPETENCE IN REPAIRING OR CALIBRATING
                            Debug.Log("DIDNT CALIBRATE IN TIME");
                            GameManager.isEnd = true;
                            gameManager.ExecuteFinal(4);
                            break;
                        case Screens.REPAIR: //END FOR INCOMPETENCE IN REPAIRING OR CALIBRATING
                            GameManager.isEnd = true;
                            Debug.Log("DIDNT REPAIR IN TIME");
                            gameManager.ExecuteFinal(5);
                            break;
                        case Screens.NONE:
                            break;
                    }
                    //bar.Stopbar();
                    waitingForPlayer = false;
                    currentScreens.RemoveAt(s);

                }
            }
        //}
    }

    public void LaunchEvent(ShipEvent incomingEvent)
    {
        switch (incomingEvent)
        {
            case ShipEvent.VISUAL:
                SendVisualMessage();
                currentScreens.Add(Screens.GLASS);
                break;
            case ShipEvent.BOMB:
                SendAttack();
                break;
            case ShipEvent.NONE:
                break;
        }
        currentShipEvent = incomingEvent;
    }

    public void InterpretVisualMessage(string code)
    {
        if(VisualCommunicationController.flagCodes.TryGetValue(code, out VisualMessage value))
        {
            int reaction = GetMessageReaction(code);

            if(reaction == 0) //Positive response to tension
            {
                GameManager.isAlly = false;
                gameManager.DecreaseTension(1f, false);
            }
            else if(reaction == 1) //Negative response to tension
            {
                GameManager.isAlly = false;
                timeLineController.AddNewEvent(5f,TimeLineController.TimeEventType.BOMB);
                timeLineController.doAttack = false;
                gameManager.IncreaseTension(1f, false);
            }
            else //Neutral response to tension
            {

            }
            visualController.ManageVisualFeedback();
        }
        else //Non-existent message
        {
            //Debug.Log("THIS MESSAGE IS NON-SENSE!");

        }
        visualController.playerMessage = "";
    }

    public void ShowNotebook()
    {
        showNotebook = !showNotebook;
        if (showNotebook)
            uiAS.PlayOneShot(showNote,0.3f);
        else
            uiAS.PlayOneShot(closeNote, 0.3f);
        notebookAnimator.SetBool("showPole", showNotebook);
    }

    private void SendVisualMessage()
    {
        if (PlayerController.currentScreen == Screens.GLASS)
        {
            if (isFirstTime) //TODO: HERE SHOULD BE THE TUTORIAL OR SOMETHING SIMILAR
            {
                ProcessMessage("3313");
                isFirstTime = false;
                VisualCommunicationController.flagCodes.TryGetValue("3313", out VisualMessage val);
                lastSendedMessage = val;
            }
            else
            {
                List<string> totalTypes = new List<string>();
                foreach (var d in VisualCommunicationController.flagCodes)
                {
                    if (d.Value.type == GameManager.currentEnemyTension || d.Value.type == GameManager.currentEnemyTension+1 || d.Value.type == GameManager.currentEnemyTension -1)
                        totalTypes.Add(d.Key);
                }

                Random.InitState(System.DateTime.Now.Millisecond);
                int randomMessage = Random.Range(0, totalTypes.Count - 1);
                ProcessMessage(totalTypes[randomMessage]);

                VisualCommunicationController.flagCodes.TryGetValue(totalTypes[randomMessage], out VisualMessage val);
                lastSendedMessage = val;
            }
        }
        else //Give the player a little bit of time to go to the screen of the communications
        {
            waitingForPlayer = true;
            timerEnterRoom = Time.realtimeSinceStartup;
            goalScreen = Screens.GLASS;
        }
    }

    private void SendAttack()
    {
        switch(Random.Range(0, 2))
        {
            case 0: //Repair
                currentScreens.Add(Screens.REPAIR);
                if (PlayerController.currentScreen == Screens.REPAIR)
                    repairController.SetLeaks();
                else //Give the player a little bit of time to go to the screen of the communications
                {
                    waitingForPlayer = true;
                    timerEnterRoom = Time.realtimeSinceStartup;
                    goalScreen = Screens.REPAIR;
                }
                break;
            case 1://Calibration
                currentScreens.Add(Screens.CALIBRATE);
                if (PlayerController.currentScreen == Screens.CALIBRATE)
                    calibrationController.SetDecalibrateValues();
                else //Give the player a little bit of time to go to the screen of the communications
                {
                    waitingForPlayer = true;
                    timerEnterRoom = Time.realtimeSinceStartup;
                    goalScreen = Screens.CALIBRATE;
                }
                break;
        }
        frequencyCommunicationController.SetFrequenciesValues();
    }

    private void ProcessMessage(string flagCode)
    {
        int[] bufferSprites = GetFlagsFromCode(flagCode);
        showflags = StartCoroutine(ShowVisualMessage(bufferSprites));
    }


    private int[] GetFlagsFromCode(string flagCode)
    {
        int[] flagCodes = { 0, 0, 0, 0};

        flagCodes[0] = int.Parse(flagCode.Substring(0, 1));
        flagCodes[1] = int.Parse(flagCode.Substring(1, 1));
        flagCodes[2] = int.Parse(flagCode.Substring(2, 1));
        flagCodes[3] = int.Parse(flagCode.Substring(3, 1));

        return flagCodes;
    }

    private int GetMessageReaction(string code)
    {

        foreach(string c in lastSendedMessage.positiveMessages)
        {
            if(c.Equals(code))
                return 0;
        }

        foreach(string c in lastSendedMessage.negativeMessages)
        {
            if (c.Equals(code))
                return 1;
        }

        return 3;
    }

    private IEnumerator ShowVisualMessage(int[] bufferSprites)
    {
        shipPoleAnimator.SetBool("showPole", true);
        yield return new WaitForSeconds(1.5f);

        shipFlagAnimator.SetBool("showFlag", true);
        flag.sprite = flagSprites[bufferSprites[0] - 1];
        yield return new WaitForSeconds(2.5f);

        shipFlagAnimator.SetBool("showFlag", false);
        yield return new WaitForSeconds(2f);

        shipFlagAnimator.SetBool("showFlag", true);
        flag.sprite = flagSprites[bufferSprites[1] - 1];
        yield return new WaitForSeconds(2.5f);

        shipFlagAnimator.SetBool("showFlag", false);
        yield return new WaitForSeconds(2f);

        shipFlagAnimator.SetBool("showFlag", true);
        flag.sprite = flagSprites[bufferSprites[2] - 1];
        yield return new WaitForSeconds(2.5f);

        shipFlagAnimator.SetBool("showFlag", false);
        yield return new WaitForSeconds(2f);

        shipFlagAnimator.SetBool("showFlag", true);
        flag.sprite = flagSprites[bufferSprites[3] - 1];
        yield return new WaitForSeconds(2.5f);

        shipFlagAnimator.SetBool("showFlag", false);
        yield return new WaitForSeconds(2f);

        shipPoleAnimator.SetBool("showPole", false);

        visualController.ActivateFlagsButtons();
        StartCoroutine(WaitingForResponse());
    }

    private IEnumerator WaitingForResponse()
    {
        bar.BeginFilling(playerResponseWaitingTime);
        for (float timer = playerResponseWaitingTime; timer >= 0; timer -= Time.deltaTime)
        {
            if (!string.IsNullOrEmpty(visualController.playerMessage))
            {
                timeLineController.totalTime += 20f;
                timeLineController.isCurrentEventOver = true;
                visualController.showshipflags = StartCoroutine(visualController.ShowVisualMessage(GetFlagsFromCode(visualController.GetCodeFromFlags()))); //Player response
                timeLineController.lastTimeEvent = Time.realtimeSinceStartup;
                yield break;
            }
            
            yield return null;
        }
        timeLineController.isCurrentEventOver = true;
        visualController.DeactivateFlagsButtons();
        visualController.ClearFlagImages();
    }

    public void StopFlags()
    {
        StopCoroutine(showflags);
        shipPoleAnimator.SetBool("showPole", false);
        shipPoleAnimator.gameObject.transform.localPosition = new Vector3(shipPoleAnimator.gameObject.transform.localPosition.x, -1.5f, shipPoleAnimator.gameObject.transform.localPosition.z);
        shipFlagAnimator.SetBool("showFlag", false);
        shipFlagAnimator.gameObject.transform.localPosition = new Vector3(shipFlagAnimator.gameObject.transform.localPosition.x, -1f, shipFlagAnimator.gameObject.transform.localPosition.z);
    }

    public enum ShipEvent
    {
        VISUAL,
        BOMB,
        NONE
    }
}

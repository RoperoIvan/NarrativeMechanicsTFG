using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float playerResponseWaitingTime = 10f;
    public float playerEnterWaitingTime = 10f;
    public ShipEvent currentShipEvent = ShipEvent.NONE;
    public SpriteRenderer flag;
    public Sprite defaultSprite;
    public VisualCommunicationController visualController;
    public CalibrationController calibrationController;
    public Repair repairController;
    public GameManager gameManager;
    public Animator shipFlagAnimator;
    public Animator shipPoleAnimator;
    public List<Sprite> flagSprites = new List<Sprite>();

    private float timerEnterRoom = 0f;
    private bool isFirstTime = true;
    private bool waitingForPlayer = false;
    private VisualMessage lastSendedMessage;
    private Screens goalScreen = Screens.NONE;
    private void Update()
    {
        if(waitingForPlayer)
        {
            if (PlayerController.currentScreen == goalScreen)
            {
                switch (goalScreen)
                {
                    case Screens.GLASS:
                        SendVisualMessage();
                        break;
                    case Screens.CALIBRATE:
                        calibrationController.SetDecalibrateValues();
                        break;
                    case Screens.REPAIR:
                        repairController.ResetRepair();
                        break;
                    case Screens.NONE:
                        break;
                }
                
                waitingForPlayer = false;
            }
            if (Time.realtimeSinceStartup - timerEnterRoom >= playerEnterWaitingTime) //BAD CONSEQUENCES
            {
                if (PlayerController.currentScreen != goalScreen)
                    

                switch (goalScreen)
                {
                    case Screens.GLASS:
                        gameManager.IncreaseTension(0.2f);
                        break;
                    case Screens.CALIBRATE:
                            Debug.Log("DIDNT CALIBRATE IN TIME");
                        break;
                        case Screens.REPAIR:
                            Debug.Log("DIDNT REPAIR IN TIME");
                            break;
                    case Screens.NONE:
                        break;
                }

                waitingForPlayer = false;
            }
        }
    }

    public void LaunchEvent(ShipEvent incomingEvent)
    {
        switch (incomingEvent)
        {
            case ShipEvent.VISUAL:
                SendVisualMessage();
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
                gameManager.DecreaseTension(1f);
            }
            else if(reaction == 1) //Negative response to tension
            {
                gameManager.IncreaseTension(1f);
            }
            else //Neutral response to tension
            {

            }

        }
        else //Non-existent message
        {
            //Debug.Log("THIS MESSAGE IS NON-SENSE!");

        }
        visualController.playerMessage = "";
    }

    private void SendVisualMessage()
    {
        if (PlayerController.currentScreen == Screens.GLASS)
        {
            if (isFirstTime) //TODO: HERE SHOULD BE THE TUTORIAL OR SOMETHING SIMILAR
            {
                ProcessMessage("1142");
                isFirstTime = false;
                VisualCommunicationController.flagCodes.TryGetValue("1142", out VisualMessage val);
                lastSendedMessage = val;
            }
            else
            {
                List<string> totalTypes = new List<string>();
                foreach (var d in VisualCommunicationController.flagCodes)
                {
                    if (d.Value.type == GameManager.currentTension)
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
        switch(Random.Range(0, 1))
        {
            case 0: //Repair
                if (PlayerController.currentScreen == Screens.REPAIR)
                    repairController.ResetRepair();
                else //Give the player a little bit of time to go to the screen of the communications
                {
                    waitingForPlayer = true;
                    timerEnterRoom = Time.realtimeSinceStartup;
                    goalScreen = Screens.REPAIR;
                }
                break;
            case 1://Calibration
                goto case 0;
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

    }

    private void ProcessMessage(string flagCode)
    {
        int[] bufferSprites = GetFlagsFromCode(flagCode);
        StartCoroutine(ShowVisualMessage(bufferSprites));
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

        for (float timer = playerResponseWaitingTime; timer >= 0; timer -= Time.deltaTime)
        {
            if (!string.IsNullOrEmpty(visualController.playerMessage))
            {
                StartCoroutine(visualController.ShowVisualMessage(GetFlagsFromCode(visualController.GetCodeFromFlags())));
                yield break;
            }
            yield return null;
        }

        visualController.DeactivateFlagsButtons();
        visualController.ClearFlagImages();
    }

    public enum ShipEvent
    {
        VISUAL,
        BOMB,
        NONE
    }
}

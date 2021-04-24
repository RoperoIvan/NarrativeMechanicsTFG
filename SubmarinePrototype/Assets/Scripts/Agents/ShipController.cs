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
    public GameManager gameManager;
    public List<Sprite> flagSprites = new List<Sprite>();

    private float timerEnterRoom = 0f;
    private bool isFirstTime = true;
    private bool waitingForPlayer = false;
    private VisualMessage lastSendedMessage;

    private void Update()
    {
        if(waitingForPlayer)
        {
            if(Time.realtimeSinceStartup - timerEnterRoom >= playerEnterWaitingTime)
            {
                if (PlayerController.currentScreen != Screens.GLASS)
                    gameManager.IncreaseTension(0.2f);
                else
                    SendVisualMessage();

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
            if (isFirstTime)
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
        else
        {
            waitingForPlayer = true;
            timerEnterRoom = Time.realtimeSinceStartup;
        }
    }

    private void SendAttack()
    {

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
        yield return new WaitForSeconds(1f);
        flag.sprite = flagSprites[bufferSprites[0] - 1];
        yield return new WaitForSeconds(1f);
        flag.sprite = defaultSprite;
        yield return new WaitForSeconds(1f);
        flag.sprite = flagSprites[bufferSprites[1] - 1];
        yield return new WaitForSeconds(1f);
        flag.sprite = defaultSprite;
        yield return new WaitForSeconds(1f);
        flag.sprite = flagSprites[bufferSprites[2] - 1];
        yield return new WaitForSeconds(1f);
        flag.sprite = defaultSprite;
        yield return new WaitForSeconds(1f);
        flag.sprite = flagSprites[bufferSprites[3] - 1];
        yield return new WaitForSeconds(1f);
        flag.sprite = defaultSprite;

        visualController.ActivateFlagsButtons();
        StartCoroutine(WaitingForResponse());
    }

    private IEnumerator WaitingForResponse()
    {
        yield return new WaitForSeconds(playerResponseWaitingTime);
        if (string.IsNullOrEmpty(visualController.playerMessage))
        {
            //Debug.Log("THIS MESSAGE IS NULL OR EMPTY!");
        }
        else
            InterpretVisualMessage(visualController.playerMessage);

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

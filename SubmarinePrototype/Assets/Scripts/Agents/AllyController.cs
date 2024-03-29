using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AllyController : MonoBehaviour
{
    public AllyEvent currentAllyEvent = AllyEvent.NONE;
    public float playerEnterWaitingTime = 12f;
    public GameManager gameManager;
    public FrequencyCommunicationController frequencyController;
    public TimeLineController timeLineController;
    private float timerEnterRoom = 0f;
    private bool waitingForPlayer = false;
    [HideInInspector]
    public bool isInHeight = true;
    private void Update()
    {
        if (waitingForPlayer)
        {
            if (PlayerController.currentScreen == Screens.RADIO)
            {
                if(isInHeight)
                {
                    SendFrequency();

                    waitingForPlayer = false;
                }
                
            }
            else if (Time.realtimeSinceStartup - timerEnterRoom >= playerEnterWaitingTime)
            {
                if (PlayerController.currentScreen != Screens.RADIO)
                {
                    GameManager.isAlly = true;
                    gameManager.IncreaseTension(0.5f, true);
                    timeLineController.isCurrentEventOver = true;
                }
                else
                    SendFrequency();

                waitingForPlayer = false;
            }
        }
    }

    public void LaunchEvent(AllyEvent incomingEvent)
    {
        switch (incomingEvent)
        {
            case AllyEvent.RADIO:
                break;
            case AllyEvent.FREQUENCY:
                SendFrequency();
                break;
            case AllyEvent.NONE:
                break;
        }
    }

    private void SendFrequency()
    {
        if(PlayerController.currentScreen == Screens.RADIO)
        {
            frequencyController.SetFrequenciesValues();
            DialogueManager.dialogueManager.RefreshDialogueContainer();
        }
        else //Give the player a little bit of time to go to the screen of the radio
        {
            waitingForPlayer = true;
            timerEnterRoom = Time.realtimeSinceStartup;
        }
    }

    public enum AllyEvent
    {
        RADIO,
        FREQUENCY,
        NONE
    }
}

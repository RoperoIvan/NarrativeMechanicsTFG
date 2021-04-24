using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Screens currentScreen = Screens.NONE;
    public TimeLineController timeLineController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            timeLineController.AddNewEvent(5f, TimeLineController.TimeEventType.BOMB);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            timeLineController.AddNewEvent(60f, TimeLineController.TimeEventType.FREQUENCY);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            timeLineController.AddNewEvent(20f, TimeLineController.TimeEventType.RADIO);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            timeLineController.AddNewEvent(10f, TimeLineController.TimeEventType.VISUAL);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            currentScreen = Screens.GLASS;
            Debug.Log("IN VISUAL ROOM");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.currentTension = Tension.PEACEFUL;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameManager.currentTension = Tension.LOW;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameManager.currentTension = Tension.MEDIUM;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.currentTension = Tension.DANGER;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameManager.currentTension = Tension.THREAT;
        }
    }

    public enum Screens
    {
        COMMAND,
        ENGINES,
        RADIO,
        GLASS,
        SHELVES,
        FREQUENCY,
        NONE
    }
}

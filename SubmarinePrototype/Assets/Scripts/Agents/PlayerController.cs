using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // VISUAL OBJECTS TO ACTIVATE/DEACTIVATE
    [InspectorName("VISUAL ROOM")]
    public GameObject enemyFlagContainer;
    public GameObject playerFlagContainer;
    public GameObject UIVisualContainer;

    [InspectorName("RADIO")]
    public GameObject frequencyWheelsContainer;
    public GameObject frequencyController;
    //public GameObject UIVisualContainer;


    static public Screens currentScreen = Screens.NONE;
    public TimeLineController timeLineController;
    public DialogueManager dialogueManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    timeLineController.AddNewEvent(5f, TimeLineController.TimeEventType.BOMB);
        //}
        if (Input.GetKeyDown(KeyCode.T))
        {
            timeLineController.AddNewEvent(6f, TimeLineController.TimeEventType.FREQUENCY);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            timeLineController.AddNewEvent(5f, TimeLineController.TimeEventType.RADIO);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            timeLineController.AddNewEvent(10f, TimeLineController.TimeEventType.VISUAL);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentScreen = Screens.GLASS;
            GoToVisualRoom(true);
            Debug.Log("IN VISUAL ROOM");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            currentScreen = Screens.RADIO;
            GoToRadio(true);
            Debug.Log("IN RADIO");
        }

        if (Input.GetKeyDown(KeyCode.Q))
            dialogueManager.RefreshDialogueContainer();
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    GameManager.currentTension = Tension.PEACEFUL;
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    GameManager.currentTension = Tension.LOW;
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    GameManager.currentTension = Tension.MEDIUM;
        //}
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    GameManager.currentTension = Tension.DANGER;
        //}
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    GameManager.currentTension = Tension.THREAT;
        //}
    }

    private void GoToVisualRoom(bool isEnter)
    {
        currentScreen = Screens.GLASS;
        enemyFlagContainer.SetActive(isEnter);
        playerFlagContainer.SetActive(isEnter);
        UIVisualContainer.SetActive(isEnter);
    }

    private void GoToRadio(bool isEnter)
    {
        frequencyController.SetActive(isEnter);
        frequencyWheelsContainer.SetActive(isEnter);
    }
}

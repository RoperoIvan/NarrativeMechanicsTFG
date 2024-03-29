using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreen : MonoBehaviour
{
    public static OpenScreen openScreenManager;
    //public Screens screen;
    [InspectorName("VISUAL ROOM")]
    public GameObject visualContainer;
    public GameObject UIVisualContainer;

    [InspectorName("RADIO")]
    public GameObject frequencyWheelsContainer;
    public GameObject frequencyController;
    //public GameObject UIVisualContainer;

    [InspectorName("CALIBRATE")]
    public GameObject calibrateUIContainer;
    public GameObject calibrateContainer;

    [InspectorName("REPAIR")]
    public GameObject repairContainer;

    public GameObject commandContainer;
    public GameObject shelvesContainer;
    public GameObject engineContainer;

    public GameObject movementButtons;
    public GameObject goBackButton;
    private Screens fromRoom;

    [InspectorName("RADAR")]
    public GameObject radarContainer;

    public GameObject missileContainer;

    public AudioSource ambienceAS;
    public TimeLineController timeLineController;
    private AudioClip ambienceSubmarine;
    private AudioClip ambienceRadio;
    private void Awake()
    {
        ambienceSubmarine = Resources.Load<AudioClip>("Sound/submarineAmbience");
        ambienceRadio = Resources.Load<AudioClip>("Sound/radioAmbience");
        if (openScreenManager == null)
            openScreenManager = this;
    }

    public void GoToScreen(Screens screen)
    {
        goBackButton.SetActive(true);
        movementButtons.SetActive(false);
        switch (screen)
        {
            case Screens.RADIO:
                ambienceAS.Stop();
                ambienceAS.PlayOneShot(ambienceRadio,0.3f);
                fromRoom = Screens.COMMAND;
                PlayerController.currentScreen = Screens.RADIO;
                commandContainer.SetActive(false);
                frequencyController.SetActive(true);
                frequencyWheelsContainer.SetActive(true);
                timeLineController.DeleteEventIcon(TimeLineController.TimeEventType.FREQUENCY);
                break;
            case Screens.GLASS:
                fromRoom = Screens.SHELVES;
                PlayerController.currentScreen = Screens.GLASS;
                visualContainer.SetActive(true);
                UIVisualContainer.SetActive(true);
                shelvesContainer.SetActive(false);
                timeLineController.DeleteEventIcon(TimeLineController.TimeEventType.VISUAL);
                break;
            case Screens.CALIBRATE:
                fromRoom = Screens.SHELVES;
                PlayerController.currentScreen = Screens.CALIBRATE;
                shelvesContainer.SetActive(false);
                calibrateContainer.SetActive(true);
                calibrateUIContainer.SetActive(true);
                timeLineController.DeleteEventIcon(TimeLineController.TimeEventType.BOMB);
                break;
            case Screens.REPAIR:
                fromRoom = Screens.ENGINES;
                PlayerController.currentScreen = Screens.REPAIR;
                timeLineController.DeleteEventIcon(TimeLineController.TimeEventType.BOMB);
                engineContainer.SetActive(false);
                repairContainer.SetActive(true);
                break;
            case Screens.RADAR:
                fromRoom = Screens.COMMAND;
                PlayerController.currentScreen = Screens.RADAR;
                commandContainer.SetActive(false);
                radarContainer.SetActive(true);
                break;
            case Screens.MISSILE:
                fromRoom = Screens.ENGINES;
                PlayerController.currentScreen = Screens.MISSILE;
                engineContainer.SetActive(false);
                missileContainer.SetActive(true);
                break;
            case Screens.NONE:
                break;
        }
        
    }

    public void GoBackToRoom()
    {
        goBackButton.SetActive(false);
        movementButtons.SetActive(true);
        ambienceAS.Stop();
        switch (fromRoom)
        {
            case Screens.COMMAND:
                commandContainer.SetActive(true);
                frequencyController.SetActive(false);
                frequencyWheelsContainer.SetActive(false);
                radarContainer.SetActive(false);
                break;
            case Screens.ENGINES:
                engineContainer.SetActive(true);
                repairContainer.SetActive(false);
                missileContainer.SetActive(false);
                break;
            case Screens.SHELVES:
                shelvesContainer.SetActive(true);
                calibrateContainer.SetActive(false);
                calibrateUIContainer.SetActive(false);
                visualContainer.SetActive(false);
                UIVisualContainer.SetActive(false);
                break;
            case Screens.NONE:
                break;
        }
        ambienceAS.Play();
    }
}

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

    private void Awake()
    {
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
                fromRoom = Screens.COMMAND;
                PlayerController.currentScreen = Screens.RADIO;
                commandContainer.SetActive(false);
                frequencyController.SetActive(true);
                frequencyWheelsContainer.SetActive(true);
                break;
            case Screens.GLASS:
                fromRoom = Screens.SHELVES;
                PlayerController.currentScreen = Screens.GLASS;
                visualContainer.SetActive(true);
                UIVisualContainer.SetActive(true);
                shelvesContainer.SetActive(false);
                break;
            case Screens.CALIBRATE:
                fromRoom = Screens.SHELVES;
                PlayerController.currentScreen = Screens.CALIBRATE;
                shelvesContainer.SetActive(false);
                calibrateContainer.SetActive(true);
                calibrateUIContainer.SetActive(true);
                break;
            case Screens.REPAIR:
                fromRoom = Screens.ENGINES;
                PlayerController.currentScreen = Screens.REPAIR;
                engineContainer.SetActive(false);
                repairContainer.SetActive(true);
                break;
            case Screens.NONE:
                break;
        }
    }

    public void GoBackToRoom()
    {
        goBackButton.SetActive(false);
        movementButtons.SetActive(true);

        switch (fromRoom)
        {
            case Screens.COMMAND:
                commandContainer.SetActive(true);
                frequencyController.SetActive(false);
                frequencyWheelsContainer.SetActive(false);
                break;
            case Screens.ENGINES:
                engineContainer.SetActive(true);
                repairContainer.SetActive(false);
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public ShipEvent currentShipEvent = ShipEvent.NONE;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void InterpretVisualMessage(string code)
    {

    }

    private void SendVisualMessage()
    {
        foreach (var d in VisualCommunicationController.flagCodes)
        {
            switch (GameManager.currentTension)
            {
                case GameManager.Tension.PEACEFUL:
                    break;
                case GameManager.Tension.LOW:
                    break;
                case GameManager.Tension.MEDIUM:
                    break;
                case GameManager.Tension.DANGER:
                    break;
                case GameManager.Tension.THREAT:
                    break;
                case GameManager.Tension.NONE:
                    break;
            }
        }
    }

    private void SendAttack()
    {

    }

    public enum ShipEvent
    {
        VISUAL,
        BOMB,
        NONE
    }
}

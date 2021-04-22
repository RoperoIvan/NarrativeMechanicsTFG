using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float playerWaitingTime = 10f;
    public ShipEvent currentShipEvent = ShipEvent.NONE;
    public SpriteRenderer flag;
    public Sprite defaultSprite;
    public VisualCommunicationController visualController;
    public List<Sprite> flagSprites = new List<Sprite>();

    private bool isFirstTime = true;

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
        if(VisualCommunicationController.flagCodes.TryGetValue(code, out VisualCommunicationController.VisualMessage value))
        {
            
        }
        else //Non-existent message
        {

        }
    }

    private void SendVisualMessage()
    {
        if (isFirstTime)
        {
            ProcessMessage("1142");
            isFirstTime = false;
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

        WaitingForResponse();
    }

    private IEnumerator WaitingForResponse()
    {
        string code = visualController.CodifyFlags();
        yield return new WaitForSeconds(playerWaitingTime);
        if (string.IsNullOrEmpty(code))
        {

        }
        else
            InterpretVisualMessage(code);
    }

    public enum ShipEvent
    {
        VISUAL,
        BOMB,
        NONE
    }
}

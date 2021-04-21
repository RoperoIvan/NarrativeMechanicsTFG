using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class VisualCommunicationController : MonoBehaviour
{
    public ShipController shipController;
    public List<Sprite> flagSprites = new List<Sprite>();
    public List<Button> flagButtons = new List<Button>();
    public List<Image> flagImage = new List<Image>();

    static public Dictionary<string, VisualMessage> flagCodes = new Dictionary<string,VisualMessage>();
    // Start is called before the first frame update
    void Start()
    {
        FillVisualCodes();
    }

    public void RecordFlag(int flag)
    {
        for(int i = 0; i < GameManager.visualMessageBuffer.Length; ++i)
        {
            if(GameManager.visualMessageBuffer[i] == 0)
            {
                GameManager.visualMessageBuffer[i] = flag;
                flagImage[i].sprite = flagSprites[flag-1];
                break;
            }
        }
    }

    public void CodifyFlags()
    {
        string mCode = GetCodeFromFlags();

        if(flagCodes.ContainsKey(mCode))
        {
            //TODO: Send message to ship Controller of tension etc...
            shipController.InterpretVisualMessage(mCode);
        }
        else
        {
            //Confuse message non-existent
        }
    }

    private void FillVisualCodes()
    {
        TextAsset codesJSON = Resources.Load<TextAsset>("FlagCodes");
        JSONNode codesFromJSON;
        codesFromJSON = JSON.Parse(codesJSON.ToString());
        for (int i = 0; i < codesFromJSON.Count; i++)
        {
            string nCode = codesFromJSON[i]["MESSAGE CODE"];
            string nDesc = codesFromJSON[i]["DESCRIPTION"];
            GameManager.Tension nTension = (GameManager.Tension)int.Parse(codesFromJSON[i]["TYPE"]);

            flagCodes.Add(nCode, new VisualMessage(nDesc, nTension));
        }
    }

    private string GetCodeFromFlags()
    {
        int nCode = 0;
        nCode += GameManager.visualMessageBuffer[0] * 1000;
        nCode += GameManager.visualMessageBuffer[1] * 100;
        nCode += GameManager.visualMessageBuffer[2] * 10;
        nCode += GameManager.visualMessageBuffer[3];

        return nCode.ToString();
    }

    public struct VisualMessage
    {
        public string description;
        public GameManager.Tension type;
        public VisualMessage(string description, GameManager.Tension type)
        {
            this.description = description;
            this.type = type;
        }
    }
}

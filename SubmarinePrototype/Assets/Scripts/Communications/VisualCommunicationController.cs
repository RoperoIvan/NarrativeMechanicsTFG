using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class VisualCommunicationController : MonoBehaviour
{
    public ShipController shipController;
    public Button sendMessageBtn;
    public Sprite defaultSprite;
    public List<Sprite> flagSprites = new List<Sprite>();
    public List<Button> flagButtons = new List<Button>();
    public List<Image> flagImage = new List<Image>();

    [HideInInspector]
    public string playerMessage;

    static public Dictionary<string, VisualMessage> flagCodes = new Dictionary<string,VisualMessage>();
    // Start is called before the first frame update
    void Awake()
    {
        FillVisualCodes();
        gameObject.SetActive(false);
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
        playerMessage = GetCodeFromFlags();
    }


    public void ActivateFlagsButtons()
    {
        foreach(Button bt in flagButtons)
        {
            bt.interactable = true;
        }
        sendMessageBtn.interactable = true;
    }

    public void DeactivateFlagsButtons()
    {
        foreach (Button bt in flagButtons)
        {
            bt.interactable = false;
        }
        sendMessageBtn.interactable = false;
    }

    public void ClearFlagImages()
    {
        foreach(Image img in flagImage)
        {
            img.sprite = defaultSprite;
        }
        int[] clean = { 0, 0, 0, 0 };
        GameManager.visualMessageBuffer = clean;
    }

    private void FillVisualCodes()
    {
        TextAsset codesJSON = Resources.Load<TextAsset>("FlagCodes (2)");
        JSONNode codesFromJSON;
        codesFromJSON = JSON.Parse(codesJSON.ToString());
        for (int i = 0; i < codesFromJSON.Count; i++)
        {
            string nCode = codesFromJSON[i]["MESSAGE CODE"];
            string nDesc = codesFromJSON[i]["DESCRIPTION"];
            var arrayJSON = codesFromJSON[i]["POSITIVE"].AsArray;
            List<string> nPosit = new List<string>();
            for(int j = 0; j < arrayJSON.Count; ++j)
            {
                nPosit.Add(arrayJSON[j]); 
            }
            var arrayNJSON = codesFromJSON[i]["NEGATIVE"].AsArray;
            List<string> nNeg = new List<string>();
            for (int k = 0; k < arrayNJSON.Count; ++k)
            {
                nNeg.Add(arrayNJSON[k]);
            }

            Tension nTension = (Tension)int.Parse(codesFromJSON[i]["TYPE"]);

            flagCodes.Add(nCode, new VisualMessage(nDesc, nPosit, nNeg, nTension));
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
}

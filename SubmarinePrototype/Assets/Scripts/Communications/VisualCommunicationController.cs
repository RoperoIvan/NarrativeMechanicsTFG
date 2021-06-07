using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class VisualCommunicationController : MonoBehaviour
{
    public ShipController shipController;
    public Button sendMessageBtn;
    public Button resetBtn;
    public Sprite defaultSprite;
    public SpriteRenderer subFlag;
    public Animator subFlagAnimator;
    public Animator subPoleAnimator;
    public Animator shipHoleAnimator;
    public Animator shipCannonAnimator;
    public Animator shipBaseAnimator;


    public List<Sprite> flagSprites = new List<Sprite>();
    public List<Button> flagButtons = new List<Button>();
    public List<Image> flagImage = new List<Image>();

    [HideInInspector]
    public string playerMessage;

    private Tension lastTension;
    private bool hasHole = false;
    private bool hasCannon = false;
    private bool hasBase = false;

    static public Dictionary<string, VisualMessage> flagCodes = new Dictionary<string,VisualMessage>();
    // Start is called before the first frame update
    void Awake()
    {
        FillVisualCodes();
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            hasHole = !hasHole;
            shipHoleAnimator.SetBool("hasHole", hasHole);
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            hasCannon = !hasCannon;
            shipCannonAnimator.SetBool("hasCannon", hasCannon);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            hasBase = !hasBase;
            shipBaseAnimator.SetBool("hasBase", hasBase);
        }
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
        resetBtn.interactable = true;
    }

    public void DeactivateFlagsButtons()
    {
        foreach (Button bt in flagButtons)
        {
            bt.interactable = false;
        }
        sendMessageBtn.interactable = false;
        resetBtn.interactable = false;
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

    public string GetCodeFromFlags()
    {
        int nCode = 0;
        nCode += GameManager.visualMessageBuffer[0] * 1000;
        nCode += GameManager.visualMessageBuffer[1] * 100;
        nCode += GameManager.visualMessageBuffer[2] * 10;
        nCode += GameManager.visualMessageBuffer[3];

        return nCode.ToString();
    }

    public IEnumerator ShowVisualMessage(int[] bufferSprites)
    {
        DeactivateFlagsButtons();
        subPoleAnimator.SetBool("showPole", true);
        yield return new WaitForSeconds(1.5f);

        subFlagAnimator.SetBool("showFlag", true);
        subFlag.sprite = flagSprites[bufferSprites[0] - 1];
        yield return new WaitForSeconds(2.5f);

        subFlagAnimator.SetBool("showFlag", false);
        yield return new WaitForSeconds(1.5f);

        subFlagAnimator.SetBool("showFlag", true);
        subFlag.sprite = flagSprites[bufferSprites[1] - 1];
        yield return new WaitForSeconds(2.5f);

        subFlagAnimator.SetBool("showFlag", false);
        yield return new WaitForSeconds(1.5f);

        subFlagAnimator.SetBool("showFlag", true);
        subFlag.sprite = flagSprites[bufferSprites[2] - 1];
        yield return new WaitForSeconds(2.5f);

        subFlagAnimator.SetBool("showFlag", false);
        yield return new WaitForSeconds(1.5f);

        subFlagAnimator.SetBool("showFlag", true);
        subFlag.sprite = flagSprites[bufferSprites[3] - 1];
        yield return new WaitForSeconds(2.5f);

        subFlagAnimator.SetBool("showFlag", false);
        yield return new WaitForSeconds(1.5f);

        subPoleAnimator.SetBool("showPole", false);

        shipController.InterpretVisualMessage(playerMessage);
        ClearFlagImages();
    }

    public void ManageVisualFeedback()
    {
        if(lastTension != GameManager.currentTension)
        {
            switch (GameManager.currentTension)
            {
                case Tension.PEACEFUL:
                    shipHoleAnimator.SetBool("hasHole", false);
                    break;
                case Tension.LOW:
                    shipHoleAnimator.SetBool("hasHole", true);
                    shipBaseAnimator.SetBool("hasBase", false);
                    break;
                case Tension.MEDIUM:
                    shipBaseAnimator.SetBool("hasBase", true);
                    shipCannonAnimator.SetBool("hasCannon", false);
                    break;
                case Tension.DANGER:
                    shipCannonAnimator.SetBool("hasCannon", true);
                    break;
                case Tension.THREAT:
                    break;
                case Tension.NONE:
                    break;
            }
            lastTension = GameManager.currentTension;
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
}

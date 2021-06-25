using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class VisualCommunicationController : MonoBehaviour
{
    public ShipController shipController;
    public GameManager gameManager;
    public Button sendMessageBtn;
    public Button resetBtn;
    public Sprite defaultSprite;
    public SpriteRenderer subFlag;
    public Animator subFlagAnimator;
    public Animator subPoleAnimator;
    public Animator shipFlagAnimator;
    public Animator shipPoleAnimator;
    public Animator shipHoleAnimator;
    public Animator shipCannonAnimator;
    public Animator shipBaseAnimator;
    public FillBar bar;

    public List<Sprite> flagSprites = new List<Sprite>();
    public List<Button> flagButtons = new List<Button>();
    public List<Image> flagImage = new List<Image>();

    [HideInInspector]
    public string playerMessage;

    private Tension lastTension;
    public bool hasHole = false;
    public bool hasCannon = false;
    public bool hasBase = false;
    static public Dictionary<string, VisualMessage> flagCodes = new Dictionary<string,VisualMessage>();

    public Coroutine showshipflags;
    // Start is called before the first frame update
    void Awake()
    {
        FillVisualCodes();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        shipHoleAnimator.SetBool("hasHole", hasHole);
        //shipBaseAnimator.SetBool("hasBase", hasBase);
        //shipCannonAnimator.SetBool("hasCannon", hasCannon);

    }
    
    private void OnDisable()
    {
        if(showshipflags != null)
            StopCoroutine(showshipflags);

        subPoleAnimator.SetBool("showPole", false);
        subPoleAnimator.gameObject.transform.localPosition = new Vector3(subPoleAnimator.gameObject.transform.localPosition.x, -1.5f, subPoleAnimator.gameObject.transform.localPosition.z);
        subFlagAnimator.SetBool("showFlag", false);
        subFlagAnimator.gameObject.transform.localPosition = new Vector3(subFlagAnimator.gameObject.transform.localPosition.x, -1f, subFlagAnimator.gameObject.transform.localPosition.z);

        shipController.InterpretVisualMessage(playerMessage);
        if (shipController != null && shipController.showflags != null)
            shipController.StopFlags();


        shipHoleAnimator.SetBool("hasHole", false);
        shipCannonAnimator.SetBool("hasCannon", false);
        shipBaseAnimator.SetBool("hasBase", false);

        shipBaseAnimator.gameObject.transform.localPosition = new Vector3(shipBaseAnimator.gameObject.transform.localPosition.x, -1.201f, shipBaseAnimator.gameObject.transform.localPosition.z);
        shipCannonAnimator.gameObject.transform.localPosition = new Vector3(shipCannonAnimator.gameObject.transform.localPosition.x, -1.5f, shipCannonAnimator.gameObject.transform.localPosition.z);
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
        bar.ResetBar();
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
        yield return new WaitForSeconds(2f);

        subFlagAnimator.SetBool("showFlag", true);
        subFlag.sprite = flagSprites[bufferSprites[1] - 1];
        yield return new WaitForSeconds(2.5f);

        subFlagAnimator.SetBool("showFlag", false);
        yield return new WaitForSeconds(2f);

        subFlagAnimator.SetBool("showFlag", true);
        subFlag.sprite = flagSprites[bufferSprites[2] - 1];
        yield return new WaitForSeconds(2.5f);

        subFlagAnimator.SetBool("showFlag", false);
        yield return new WaitForSeconds(2f);

        subFlagAnimator.SetBool("showFlag", true);
        subFlag.sprite = flagSprites[bufferSprites[3] - 1];
        yield return new WaitForSeconds(2.5f);

        subFlagAnimator.SetBool("showFlag", false);
        yield return new WaitForSeconds(2f);

        subPoleAnimator.SetBool("showPole", false);

        shipController.InterpretVisualMessage(playerMessage);
        ClearFlagImages();
    }

    public void ManageVisualFeedback()
    {
        if(lastTension != GameManager.currentEnemyTension)
        {
            switch (GameManager.currentEnemyTension)
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
                    gameManager.ExecuteFinal(2);
                    break;
                case Tension.NONE:
                    break;
            }
            lastTension = GameManager.currentEnemyTension;
        }
    }

    private void FillVisualCodes()
    {
        flagCodes.Clear();
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

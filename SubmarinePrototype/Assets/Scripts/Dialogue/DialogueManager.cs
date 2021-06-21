using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager dialogueManager;
    public static bool hasDialog = false;
    public FrequencyCommunicationController freqCommController;
    public RecieveMessageButton recieveMessageButton;
    public Dialogue currentDialogueNode;
    public GameObject responseContainer;
    public GameObject responsePrefab;
    public TMP_Text dialogueTxt;
    public SpriteRenderer screenShader;
    public SpriteRenderer screenDialogue;
    public AudioSource uiAS;
    private Dialogue[] dialogues;
    //private AudioClip click1;
    //private AudioClip click2;
    //private AudioClip click3;
    private AudioClip beep;

    private void Awake()
    {
        //click1 = Resources.Load<AudioClip>("Sound/click1");
        //click2 = Resources.Load<AudioClip>("Sound/click2");
        //click3 = Resources.Load<AudioClip>("Sound/click3");
        beep = Resources.Load<AudioClip>("Sound/morseBeep");
        if (dialogueManager != null)
        {
            Debug.LogError("There is more than one instance!");
            return;
        }
        dialogueTxt.text = "";
        dialogueManager = this;
        //gameObject.SetActive(false);
        dialogues = Resources.LoadAll<Dialogue>("Dialogue");
        currentDialogueNode = dialogues[0];

    }
    public void RefreshDialogueContainer()
    {
        StartCoroutine(RevealText(currentDialogueNode.dialogues));
    }

    public void GoToNextNode(Dialogue nexNode)
    {
        currentDialogueNode = nexNode;

        StartCoroutine(RevealText(currentDialogueNode.dialogues));
    }

    private IEnumerator RevealText(string[] dialogue)
    {
        freqCommController.graph.GetComponent<Renderer>().sharedMaterial.SetColor("_ForegroundColor", freqCommController.lightShader);
        screenShader.color = freqCommController.lightScreen;
        screenDialogue.color = freqCommController.lightScreen;

        for (int i = 0; i < dialogue.Length; ++i) //PHRASES OF NODE
        {
            var originalString = dialogue[i];
            dialogueTxt.text = "";
            string[] words = originalString.Split();
            var numCharsRevealed = 0;
            string modWords = freqCommController.GetModifiedPhrase(originalString);
            for (int j = 0; j < words.Length; ++j) //WORD OF PHRASE
            {
                var charss = 0;
                while (charss < words[j].Length) //CHAR OF WORD
                {
                    charss++;
                    ++numCharsRevealed;
                    dialogueTxt.text = modWords.Substring(0, numCharsRevealed);
                    uiAS.PlayOneShot(beep,0.1f);
                    yield return new WaitForSecondsRealtime(0.15f);
                }
                ++numCharsRevealed;
            }
            yield return new WaitForSecondsRealtime(1f);
        }

        freqCommController.graph.GetComponent<Renderer>().sharedMaterial.SetColor("_ForegroundColor", freqCommController.darkShader);
        screenShader.color = freqCommController.darkScreen;
        screenDialogue.color = freqCommController.darkScreen;

        dialogueTxt.text = "";
        recieveMessageButton.activateButton = true;
        recieveMessageButton.timer = Time.realtimeSinceStartup;
        recieveMessageButton.RecieveResponses(currentDialogueNode.responses);
    }
}

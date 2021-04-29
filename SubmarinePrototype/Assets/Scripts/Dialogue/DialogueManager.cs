using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager dialogueManager;
    public static bool hasDialog = false;
    public Dialogue currentDialogueNode;
    public GameObject responseContainer;
    public GameObject responsePrefab;
    public TMP_Text dialogueTxt;

    private int currentChar;
    private Dialogue testNode;
    private void Awake()
    {
        if (dialogueManager != null)
        {
            Debug.LogError("There is more than one instance!");
            return;
        }

        dialogueManager = this;
        //gameObject.SetActive(false);
        testNode = Resources.Load<Dialogue>("Test");
        //nodesJames = Resources.LoadAll<Dialogue>("DialogueNodes/James");
        //nodesGrace = Resources.LoadAll<Dialogue>("DialogueNodes/Grace");
        //nodesDiane = Resources.LoadAll<Dialogue>("DialogueNodes/Diane");
        RefreshDialogueContainer();
    }
    public void RefreshDialogueContainer()
    {
        currentDialogueNode = testNode;

        for (int i = 0; i <= currentDialogueNode.responses.Length - 1; ++i)
        {
            GameObject newResponse = Instantiate(responsePrefab, responseContainer.transform);
            Dialogue nDial = currentDialogueNode.responses[i].dialogueNode;
            if (nDial == null)
                newResponse.GetComponent<Button>().onClick.AddListener(() => { CloseDialogue(); });
            else
                newResponse.GetComponent<Button>().onClick.AddListener(() => { GoToNextNode(nDial); });

            newResponse.transform.GetChild(0).GetComponent<TMP_Text>().text = currentDialogueNode.responses[i].response;

        }
        StartCoroutine(RevealText(currentDialogueNode.dialogues));
    }

    public void GoToNextNode(Dialogue nexNode)
    {
        currentDialogueNode = nexNode;

        CleanResponses();
        CreateResponses();


        StartCoroutine(RevealText(currentDialogueNode.dialogues));
    }

    public void CloseDialogue()
    {
        CleanResponses();
        hasDialog = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
    }

    private void CleanResponses()
    {
        foreach (Transform child in responseContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateResponses()
    {
        for (int i = 0; i < currentDialogueNode.responses.Length; ++i)
        {
            GameObject newResponse = Instantiate(responsePrefab, responseContainer.transform);
            Dialogue nDial = currentDialogueNode.responses[i].dialogueNode;
            if (nDial == null)
                newResponse.GetComponent<Button>().onClick.AddListener(() => { CloseDialogue(); });
            else
                newResponse.GetComponent<Button>().onClick.AddListener(() => { GoToNextNode(nDial); });
            newResponse.transform.GetChild(0).GetComponent<TMP_Text>().text = currentDialogueNode.responses[i].response;
        }
    }

    private IEnumerator RevealText(string[] dialogue)
    {
        for (int i = 0; i < dialogue.Length; ++i)
        {
            dialogueTxt.text = dialogue[i];
            var originalString = dialogueTxt.text;
            dialogueTxt.text = "";

            var numCharsRevealed = 0;
            while (numCharsRevealed < originalString.Length)
            {
                ++numCharsRevealed;
                dialogueTxt.text = originalString.Substring(0, numCharsRevealed);

                yield return new WaitForSecondsRealtime(0.03f);
            }
            yield return new WaitForSecondsRealtime(1f);
        }

        dialogueTxt.text = "";
    }
}

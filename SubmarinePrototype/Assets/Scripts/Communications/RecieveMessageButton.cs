using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecieveMessageButton : MonoBehaviour
{
    public bool activateButton = false;

    private bool checkResponse = false;
    private int response = 0; // 1: yes , 2: no
    public float timer = 0f;
    public DialogueManager dialogueManager;
    private Dialogue.Responses positiveResponse;
    private Dialogue.Responses negativeResponse;

    private void Update()
    {
        if(checkResponse)
        {
            
            checkResponse = false;
            if (response == 1)
                DialogueManager.dialogueManager.GoToNextNode(positiveResponse.dialogueNode);
            else if(response >= 2)
                DialogueManager.dialogueManager.GoToNextNode(negativeResponse.dialogueNode);

            response = 0;
        }

    }

    public void RecieveResponses(Dialogue.Responses[] responses)
    {
        if(responses.Length > 1)
        {
            positiveResponse = responses[0];
            negativeResponse = responses[1];

            StartCoroutine(WaitResponse());
        }
        if(responses.Length == 1)
        {
            dialogueManager.currentDialogueNode = responses[0].dialogueNode;
        }
    }

    IEnumerator WaitResponse()
    {
        yield return new WaitForSecondsRealtime(5f);
        checkResponse = true;
    }

    private void OnMouseDown()
    {
        if (activateButton)
        {
            if (response < 2)
                response++;
            else
            {
                activateButton = false;
                checkResponse = true;
            }
            GetComponent<SpriteRenderer>().color = new Vector4(0.254717f, 0.05647027f, 0.05647027f, 1f); // HOVER
        }
            

        Debug.Log(response);
    }

    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().color = new Vector4(0.4339623f, 0.09620862f, 0.09620862f, 1f); // NOTHOVER
    }
}

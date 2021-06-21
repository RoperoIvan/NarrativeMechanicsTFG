using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecieveMessageButton : MonoBehaviour
{
    public GameObject bar;
    public bool activateButton = false;
    public float timer = 0f;
    public DialogueManager dialogueManager;
    public SpriteRenderer ledYes;
    public SpriteRenderer ledNo;
    public GameManager gameManager;
    public AudioSource uiAS;
    private bool checkResponse = false;
    private int response = 0; // 1: yes , 2: no
    private Dialogue.Responses positiveResponse;
    private Dialogue.Responses negativeResponse;
    private AudioClip clickResponse;

    private void Awake()
    {
        clickResponse = Resources.Load<AudioClip>("Sound/messageclick");
    }
    private void Update()
    {
        if(checkResponse)
        {
            checkResponse = false;
            GameManager.isAlly = true;
            if (response == 1)
            {
                DialogueManager.dialogueManager.GoToNextNode(positiveResponse.dialogueNode);
                gameManager.DecreaseTension(1f, true);
            }

            else if (response >= 2)
            {
                DialogueManager.dialogueManager.GoToNextNode(negativeResponse.dialogueNode);
                gameManager.IncreaseTension(1f, true);
            }
                

            
            RestartRecieve();
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
        bar.GetComponent<FillBar>().BeginFilling(5f);
        yield return new WaitForSecondsRealtime(5f);
        checkResponse = true;
    }

    private void OnMouseDown()
    {
        if (activateButton)
        {
            if (response < 2)
            {
                response++;
                if (response < 2)
                    ledYes.color = new Vector4(0.8784314f, 0.8627451f, 0.1568628f, 1f); // YELLOW
                else
                    ledNo.color = new Vector4(0.8784314f, 0.8627451f, 0.1568628f, 1f); // YELLOW

                uiAS.PlayOneShot(clickResponse, 0.3f);
            }
            else
            {
                activateButton = false;
                checkResponse = true;
            }
            GetComponent<SpriteRenderer>().color = new Vector4(0.254717f, 0.05647027f, 0.05647027f, 1f); // CLICK
        }
            

        //Debug.Log(response);
    }

    private void RestartRecieve()
    {
        ledYes.color = new Vector4(0.5188679f, 0.5188679f, 0.5188679f, 1f); // GREY
        ledNo.color = new Vector4(0.5188679f, 0.5188679f, 0.5188679f, 1f); // GREY
        bar.GetComponent<FillBar>().ResetBar();
    }

    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().color = new Vector4(0.4339623f, 0.09620862f, 0.09620862f, 1f); // 6F1919
    }

    private void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().color = new Vector4(0.254717f, 0.05647027f, 0.05647027f, 1f); // CLICK
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = new Vector4(0.4339623f, 0.09620862f, 0.09620862f, 1f); // 6F1919
    }
}

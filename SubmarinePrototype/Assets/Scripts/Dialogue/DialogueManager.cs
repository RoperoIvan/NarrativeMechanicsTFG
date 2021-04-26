using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static bool hasDialog = false;
    public Dialogue currentDialogueNode;
    public GameObject responseContainer;
    public GameObject responsePrefab;
    public GameObject letterPrefab;
    public TMP_Text dialogueTxt;
    public Texture2D charSheet;
    public SpriteRenderer testSprite;

    private Dialogue[] lastDialogueNodes = new Dialogue[3]; //0: James, 1: Grace, 2: Diane
    private Dialogue[] nodesJames;
    private Dialogue[] nodesGrace;
    private Dialogue[] nodesDiane;

    private int currentChar;
    private int spriteSize = 20;
    private char[] chars = "!\"#$%&'()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[\\]^_ABCDEFGHIJKLMNOPQRSTUVWXYZ(|)~çüéâäàaÇÊËÈÏÎÌÄAÉæÆÔÖÒÛÙ".ToCharArray();
    private Sprite[] charSprites;
    private Dictionary<char, CharData> charData;
    private void Awake()
    {
        //nodesJames = Resources.LoadAll<Dialogue>("DialogueNodes/James");
        //nodesGrace = Resources.LoadAll<Dialogue>("DialogueNodes/Grace");
        //nodesDiane = Resources.LoadAll<Dialogue>("DialogueNodes/Diane");
        GetSubsprites();
        GetSpriteWidths();
        //RefreshDialogueContainer(1);
        PrintText("lililililililiiillllliili");
    }
    public void RefreshDialogueContainer(int character)
    {
        Dialogue[] charNodes = { };
        hasDialog = true;
        Cursor.lockState = CursorLockMode.None;
        currentChar = character;

        if (lastDialogueNodes[character] == null)
        {
            switch (character)
            {
                case 0:
                    charNodes = nodesJames;
                    break;
                case 1:
                    charNodes = nodesGrace;
                    break;
                case 2:
                    charNodes = nodesDiane;
                    break;
                default:
                    break;
            }
            lastDialogueNodes[character] = charNodes[0];
        }

        currentDialogueNode = lastDialogueNodes[character];
        dialogueTxt.text = currentDialogueNode.dialogues[0];

        for (int i = 0; i <= currentDialogueNode.responses.Length -1; ++i)
        {
            GameObject newResponse = Instantiate(responsePrefab, responseContainer.transform);
            Dialogue nDial = currentDialogueNode.responses[i].dialogueNode;
            if(nDial == null)
                newResponse.GetComponent<Button>().onClick.AddListener(() => { CloseDialogue(); });
            else
                newResponse.GetComponent<Button>().onClick.AddListener(() => { GoToNextNode(nDial); });

            newResponse.transform.GetChild(0).GetComponent<TMP_Text>().text = currentDialogueNode.responses[i].response;
        }
    }

    public void GoToNextNode(Dialogue nexNode)
    {
        currentDialogueNode = nexNode;

        CleanResponses();
        CreateResponses();

        dialogueTxt.text = currentDialogueNode.dialogues[0];

    }

    public void CloseDialogue()
    {
        CleanResponses();
        lastDialogueNodes[currentChar] = currentDialogueNode;
        hasDialog = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.SetActive(false);
    }

    private void CleanResponses()
    {
        foreach(Transform child in responseContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateResponses()
    {
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
    }

    public void PrintText(string text)
    {
        float pos = 0f;
        int pastWidth = 0;
        char[] s = text.ToCharArray();
        Vector3 d = new Vector3(0f, 0f, 0f);
        foreach (char c in s)
        {
            GameObject gO = Instantiate(letterPrefab, transform);
            gO.GetComponent<SpriteRenderer>().sprite = charData[c].sprite;
            gO.transform.position = new Vector3(pos, 0, 0);
            pos = pos + charData[c].width / charData[c].sprite.pixelsPerUnit*gO.transform.localScale.x;
        }
    }

    public void GetSubsprites()
    {
        Sprite[] subsprites = Resources.LoadAll<Sprite>(charSheet.name);
        charSprites = subsprites;
    }

    public void GetSpriteWidths()
    {
        charData = new Dictionary<char, CharData>();
        for (int i = 0; i < charSprites.Length; i++)
        {
            Sprite sp = charSprites[i];
            if (charData.ContainsKey(chars[i]))
                continue;

                charData.Add(chars[i], new CharData((int)sp.rect.width, sp));
        }
        return;
        int height = charSheet.height; // We might need this if we ever use a text image that is on more than one line
        int width = charSheet.width;

        int charIndex = 0;



        //Y Texture Coordinate
        for (int texCoordY = height - spriteSize; texCoordY >= 0 && charIndex < chars.Length; texCoordY -= spriteSize)
        {
            int minY = texCoordY;
            int maxY = texCoordY + spriteSize;

            //X Texture Coordinate
            for (int texCoordX = 0; texCoordX < width && charIndex < chars.Length; texCoordX += spriteSize)
            {
                int minX = texCoordX;
                int maxX = texCoordX + (spriteSize - 1);
                bool edgeFound = false;

                //right edge
                int rightEdge = 0;
                for (int currentX = maxX; currentX >= minX; currentX--)
                {
                    for (int currentY = minY; currentY < maxY; currentY++)
                    {
                        edgeFound = charSheet.GetPixel(currentX, currentY).a != 0;
                        if (edgeFound) break;
                    }
                    if (edgeFound) break;
                    rightEdge++;
                }

                edgeFound = false;


                //left edge
                int leftEdge = 0;
                for (int currentX = minX; currentX <= maxX; currentX++)
                {
                    //X
                    for (int currentY = minY; currentY < maxY; currentY++)
                    {
                        edgeFound = charSheet.GetPixel(currentX, currentY).a != 0;
                        if (edgeFound) break;
                    }
                    if (edgeFound) break;
                    leftEdge++;
                }

                //Store current sprite width
                int currentSpriteWidth = spriteSize - (leftEdge + rightEdge);

                //Determine center offsets
                int halfWidth = spriteSize / 2;
                int leftOffset = halfWidth - leftEdge;
                int rightOffset = halfWidth - rightEdge;

                if(!charData.ContainsKey(chars[charIndex]))
                {
                    charData.Add(chars[charIndex], new CharData(currentSpriteWidth, charSprites[charIndex]));
                    charIndex++;
                }
                    

                
            }
        }
    }

    public struct CharData
    {
        public int width;
        public Sprite sprite;

        public CharData(int width, Sprite sprite)
        {
            this.width = width;
            this.sprite = sprite;
        }
    }
}

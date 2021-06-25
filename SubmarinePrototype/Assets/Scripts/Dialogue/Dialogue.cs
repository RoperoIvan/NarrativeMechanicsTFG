using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 51)]
public class Dialogue : ScriptableObject
{
    public string[] dialogues;
    public Responses[] responses;

    [System.Serializable]
    public class Responses
    {
        public int responseIntention;
        public Dialogue dialogueNode;
    }
}

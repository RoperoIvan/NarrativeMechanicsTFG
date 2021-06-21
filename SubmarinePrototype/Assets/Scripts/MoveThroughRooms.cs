using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveThroughRooms : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public GameObject commandContainer;
    public GameObject shelvesContainer;
    public GameObject engineContainer;
    public AudioSource uiAS;
    public float rotationSpeed = 1f;

    //public Transform[] looks;
    private int currentRoom = 0;
    private AudioClip walk;
    private Screens[] rooms = { Screens.COMMAND, Screens.SHELVES, Screens.ENGINES };

    public void Awake()
    {
        walk = Resources.Load<AudioClip>("Sound/walkRooms");
    }
    public void GoToRoom(bool isLeft)
    {
        if(isLeft && currentRoom > 0)
        {
            uiAS.PlayOneShot(walk,0.3f);
            currentRoom--;
            ActivateRoom(rooms[currentRoom]);

        }
        else if(!isLeft && currentRoom < 2)
        {
            uiAS.PlayOneShot(walk,0.3f);
            currentRoom++;
            ActivateRoom(rooms[currentRoom]);
        }
    }

    void ActivateRoom(Screens room)
    {
        switch(room)
        {
            case Screens.COMMAND:
                commandContainer.SetActive(true);
                shelvesContainer.SetActive(false); ;
                engineContainer.SetActive(false); ;
                break;
            case Screens.SHELVES:
                commandContainer.SetActive(false); ;
                shelvesContainer.SetActive(true);
                engineContainer.SetActive(false); ;
                break;
            case Screens.ENGINES:
                commandContainer.SetActive(false); ;
                shelvesContainer.SetActive(false); ;
                engineContainer.SetActive(true);
                break;
        }
    }
}

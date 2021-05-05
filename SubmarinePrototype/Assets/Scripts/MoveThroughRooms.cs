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

    public float rotationSpeed = 1f;

    public Transform[] looks;

    private int currentLook = 0;
    private int currentRoom = 0;
    private Screens[] rooms = { Screens.COMMAND, Screens.SHELVES, Screens.ENGINES };
    public void GoToRoom(bool isLeft)
    {
        if(isLeft && currentRoom > 0)
        {
            currentRoom--;
            ActivateRoom(rooms[currentRoom]);
            //if()
            //Camera.main.gameObject.transform.Rotate(Vector3.up, -90f);
            //StartCoroutine(ChangingRoom(isLeft));

        }
        else if(!isLeft && currentRoom < 2)
        {
            currentRoom++;
            ActivateRoom(rooms[currentRoom]);
            //Camera.main.gameObject.transform.Rotate(Vector3.up, 90f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), rotationSpeed * Time.deltaTime);
            //StartCoroutine(ChangingRoom(isLeft));
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

    // IEnumerator ChangingRoom(bool isLeft)
    //{
    //    if(isLeft)
    //    {
    //        leftButton.interactable = false;
    //    }
    //    else
    //    {
    //        rightButton.interactable = false;
    //    }
    //    float progress = 0.0f;
    //    while (progress < 1.0f)
    //    {
    //        progress += (Time.deltaTime * rotationSpeed);
    //        yield return 0;
    //        transform.rotation = Quaternion.Slerp(transform.rotation, looks[currentLook].rotation, progress);

    //    }
    //    if (isLeft)
    //    {
    //        leftButton.interactable = true;
    //    }
    //    else
    //    {
    //        rightButton.interactable = true;
    //    }
    //}
}

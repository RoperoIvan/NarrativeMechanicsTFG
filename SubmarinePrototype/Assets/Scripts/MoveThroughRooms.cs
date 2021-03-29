using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveThroughRooms : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;

    public float rotationSpeed = 1f;

    public Transform[] looks;

    private int currentLook = 0;

    public void GoToRoom(bool isLeft)
    {
        if(isLeft && currentLook > 0)
        {
            currentLook--;
            //if()
            //Camera.main.gameObject.transform.Rotate(Vector3.up, -90f);
            StartCoroutine(ChangingRoom(isLeft));

        }
        else if(!isLeft && currentLook < 2)
        {
            currentLook++;
            //Camera.main.gameObject.transform.Rotate(Vector3.up, 90f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), rotationSpeed * Time.deltaTime);
            StartCoroutine(ChangingRoom(isLeft));

        }
    }
     IEnumerator ChangingRoom(bool isLeft)
    {
        if(isLeft)
        {
            leftButton.interactable = false;
        }
        else
        {
            rightButton.interactable = false;
        }
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            progress += (Time.deltaTime * rotationSpeed);
            yield return 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, looks[currentLook].rotation, progress);

        }
        if (isLeft)
        {
            leftButton.interactable = true;
        }
        else
        {
            rightButton.interactable = true;
        }
    }
}

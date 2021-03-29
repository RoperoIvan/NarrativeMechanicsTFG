using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThroughRooms : MonoBehaviour
{
    public float rotationSpeed = 0.1f;
    public Transform[] looks;
    private int currentLook = 0;
    public void GoToRoom(bool isLeft)
    {
        if(isLeft)
        {
            //Camera.main.gameObject.transform.Rotate(Vector3.up, -90f);
            StartCoroutine(ChangingRoom(currentLook--));

        }
        else
        {
            //Camera.main.gameObject.transform.Rotate(Vector3.up, 90f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), rotationSpeed * Time.deltaTime);
            StartCoroutine(ChangingRoom(currentLook++));

        }
    }
     IEnumerator ChangingRoom(float angle)
    {
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, looks[currentLook].rotation, progress);
            yield return new WaitForEndOfFrame();
            progress += Time.deltaTime * rotationSpeed;
        }
    }
}

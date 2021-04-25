using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FrequencyCommunicationController : MonoBehaviour
{
    public bool buttonPressed;

    private void Update()
    {
        if(buttonPressed)
            RegulateWheel();
    }
    public void RegulateWheel()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(
        mousePosition.x - transform.position.x,
        mousePosition.y - transform.position.y
        );

        transform.up = direction;
    }

    private void OnMouseDown()
    {
        buttonPressed = true;

    }

    private void OnMouseUp()
    {
        buttonPressed = false;

    }
}

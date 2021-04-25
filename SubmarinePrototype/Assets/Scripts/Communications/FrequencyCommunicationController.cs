using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrequencyCommunicationController : MonoBehaviour
{
    public float currentValue = 0f;
    public float maxRotationValue = 340f;
    public float minRotationValue = 0f;
    public bool buttonPressed = false;
    public TMP_Text value;

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
        currentValue = GetValueFromWheelRotation();
        value.text = currentValue.ToString("F2");
    }

    public float GetValueFromWheelRotation()
    {
        Vector3 eA = transform.rotation.eulerAngles;
        float currentRotationAngle = eA.z;

        return currentRotationAngle/maxRotationValue;
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

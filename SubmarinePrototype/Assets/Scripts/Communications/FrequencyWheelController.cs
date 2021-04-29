using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyWheelController : MonoBehaviour
{
    [HideInInspector]
    public float range = 0f;

    public float currentValue = 0f;
    public float maxRotationValue = 340f;
    public float minRotationValue = 0f;

    private float currentRotationAngle = 0f;

    public bool buttonPressed = false;
    public bool isInRange = false;
    public bool detect = false;

    public SpriteRenderer LEDRenderer;


    private void Update()
    {
        if (buttonPressed)
            RegulateWheel();

        if (detect)
            DetectFrequency();

        //val+=0.01f;
        //graph.GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_ForegroundMask", new Vector2(val, -0.59f));
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
        //value.text = currentValue.ToString("F2") + "Hz";
    }

    public float GetValueFromWheelRotation()
    {
        Vector3 eA = transform.rotation.eulerAngles;
        currentRotationAngle = eA.z;

        return currentRotationAngle / maxRotationValue;
    }

    private void DetectFrequency()
    {
        if (currentRotationAngle > range - 10 && currentRotationAngle < range + 10)
        {
            LEDRenderer.color = new Vector4(0f, 0.6509804f, 0.07074188f, 1f); // GREEN
            isInRange = true;
        }
        else
        {
            LEDRenderer.color = new Vector4(0.6509434f, 0f, 0f, 1f); // RED
            isInRange = false;
        }
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
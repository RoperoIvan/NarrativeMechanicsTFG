using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrequencyWheelController : MonoBehaviour
{
    [HideInInspector]
    public float range = 0f;
    public float currentValue = 0f;
    public float maxRotationValue = 340f;
    public float minRotationValue = 0f;
    public bool buttonPressed = false;
    public bool isInRange = false;
    public bool detect = false;
    public SpriteRenderer LEDRenderer;
    public TMP_Text valueHz;
    public AudioSource uiAS;

    private float currentRotationAngle = 0f;
    private AudioClip correct;
    private bool hasplayed = false;
    private void Awake()
    {
        correct = Resources.Load<AudioClip>("Sound/WheelsLed");
    }
    private void OnEnable()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        valueHz.text = "0.00 Hz";
    }

    private void Update()
    {
        if (buttonPressed)
            RegulateWheel();

        if (detect)
            DetectFrequency();
        
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
        valueHz.text = currentValue.ToString("F2") + " Hz";
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
            if(!hasplayed)
            {
                uiAS.PlayOneShot(correct);
                hasplayed = true;
            }
            
        }
        else
        {
            LEDRenderer.color = new Vector4(0.6509434f, 0f, 0f, 1f); // RED
            isInRange = false;
            hasplayed = false;
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

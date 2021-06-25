using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PushLever : MonoBehaviour
{
    public LeverType type;
    public float maxValue = 4f;
    public float minValue = -4f;
    public GameObject pivotLeft;
    public GameObject pivotRight;
    public GameObject indicatorHeight;
    public GameObject windowSubmarine;
    public GameObject windowLeftGlasses;
    public GameObject windowRightGlasses;
    public AllyController ally;
    public ShipController enemy;
    public AudioSource buttonsAS;
    public AudioSource uiAS;
    private AudioClip elevating;
    private AudioClip start;
    private AudioClip stop;

    [HideInInspector]
    public bool canMove = false;
    public bool isPlayed = false;
    private float value = 0f;

    private void Awake()
    {
        elevating = Resources.Load<AudioClip>("Sound/leversubmarine");
        stop = Resources.Load<AudioClip>("Sound/stoplever");
        start = Resources.Load<AudioClip>("Sound/startlever");
        if (type == LeverType.UPDOWN)
            value = minValue;
    }
    private void OnMouseDrag()
    {
        if(type == LeverType.UPDOWN)
        {
            if(canMove)
                ChangeValues();

            if(!isPlayed)
            {
                buttonsAS.PlayOneShot(elevating);
                isPlayed = true;
            }
        }
        else
        {
            if (!isPlayed)
            {
                uiAS.PlayOneShot(start,0.2f);
                isPlayed = true;
            }
            ChangeValues();
        }
    }

    private void OnMouseUp()
    {
        buttonsAS.Stop();
        uiAS.PlayOneShot(stop, 0.2f);
        isPlayed = false;
    }

    private void ChangeValues()
    {
        if (Input.GetAxis("Mouse Y") > 0)
        {
            IncreaseValue();
        }

        if (Input.GetAxis("Mouse Y") < 0)
        {
            DecreaseValue();
        }
    }

    private void IncreaseValue()
    {
        if(value < maxValue)
        {
            value += 0.1f;
            Vector3 valVec = new Vector3(0f, 0.03f, 0f);
            transform.position += valVec;

            switch (type)
            {
                case LeverType.UPDOWN:
                    windowLeftGlasses.transform.position += new Vector3(0f, -0.8f, 0f);
                    windowRightGlasses.transform.position += new Vector3(0f, -0.8f, 0f);
                    windowSubmarine.transform.position += new Vector3(0f, -0.8f, 0f);
                    indicatorHeight.transform.position += new Vector3(0f, 0.035f, 0f);
                    break;
                case LeverType.LEFT:
                    pivotLeft.transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, 0.1f));
                    break;
                case LeverType.RIGHT:
                    pivotRight.transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, 0.1f));
                    break;
                case LeverType.NONE:
                    break;
            }
            enemy.isInHeight = false;
            ally.isInHeight = false;
        }
        else if (value >= maxValue)
        {
            enemy.isInHeight = true;
        }
    }

    private void DecreaseValue()
    {
        if (value > minValue)
        {
            value -= 0.1f;
            Vector3 valVec = new Vector3(0f, 0.03f, 0f);
            transform.position -= valVec;

            switch (type)
            {
                case LeverType.UPDOWN:
                    windowLeftGlasses.transform.position += new Vector3(0f, 0.8f, 0f);
                    windowRightGlasses.transform.position += new Vector3(0f, 0.8f, 0f);
                    windowSubmarine.transform.position += new Vector3(0f, 0.8f, 0f);
                    indicatorHeight.transform.position += new Vector3(0f, -0.035f, 0f);
                    break;
                case LeverType.LEFT:
                    pivotLeft.transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, -0.1f));
                    break;
                case LeverType.RIGHT:
                    pivotRight.transform.rotation *= Quaternion.Euler(new Vector3(0f, 0f, -0.1f));
                    break;
                case LeverType.NONE:
                    break;
            }
            enemy.isInHeight = false;
            ally.isInHeight = false;
        }
        else if(value <= minValue)
        {
            ally.isInHeight = true;
        }
    }

    public void ResetCalibration()
    {
        if(type != LeverType.UPDOWN)
        {
            value = 0f;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, -1.76f, gameObject.transform.position.z);
            //indicatorHeight.transform.localPosition = new Vector3(indicatorHeight.transform.localPosition.x, 0f, indicatorHeight.transform.localPosition.z);
        }
    }

    public enum LeverType
    {
        UPDOWN,
        LEFT,
        RIGHT,
        NONE
    }
}

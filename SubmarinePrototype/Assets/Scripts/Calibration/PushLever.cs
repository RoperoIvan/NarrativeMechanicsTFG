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

    [HideInInspector]
    public bool canMove = false;

    private float value = 0f;
    
    private void OnMouseDrag()
    {
        if(type == LeverType.UPDOWN)
        {
            if(canMove)
                ChangeValues();
        }
        else
        {
            ChangeValues();
        }
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
        }
            
    }

    public void ResetCalibration()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, -1.76f, gameObject.transform.position.z);
        indicatorHeight.transform.localPosition = new Vector3(indicatorHeight.transform.localPosition.x, 0f, indicatorHeight.transform.localPosition.z);
    }

    public enum LeverType
    {
        UPDOWN,
        LEFT,
        RIGHT,
        NONE
    }
}

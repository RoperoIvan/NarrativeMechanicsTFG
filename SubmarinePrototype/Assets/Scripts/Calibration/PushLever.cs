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

    public enum LeverType
    {
        UPDOWN,
        LEFT,
        RIGHT,
        NONE
    }
}

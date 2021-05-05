using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationController : MonoBehaviour
{
    public SpriteRenderer LED;
    public GameObject pivotLeft;
    public GameObject pivotRight;
    public PushLever left;
    public PushLever right;
    public PushLever middle;
    public float range = 0f;
    public float calibrationGoal = 0f;

    void Update()
    {
        if(IsCalibrated())
        {
            middle.canMove = true;
        }
        else
            middle.canMove = false;
    }


    public void SetDecalibrateValues()
    {
        int rLeft = Random.Range(-4, 4);
        int rRight = Random.Range(-4, 4);

        pivotLeft.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rLeft));
        pivotRight.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rRight));
    }

    private bool IsCalibrated()
    {
        float pivotLValue = pivotLeft.transform.rotation.eulerAngles.z;
        float pivotRValue = pivotRight.transform.rotation.eulerAngles.z;
        if(pivotLValue <= calibrationGoal + range || pivotLValue >= 360 - range +1)
        {
            if(pivotRValue <= calibrationGoal + range || pivotRValue >= 360 - range + 1)
            {
                LED.color = new Vector4(0f, 0.6509804f, 0.07074188f, 1f); //GREEN
                return true;
            }
            else
            {
                LED.color = new Vector4(0.6509434f, 0f, 0f, 1f); //RED
                return false;
            }
        }
        else
        {
            LED.color = new Vector4(0.6509434f, 0f, 0f, 1f); //RED
            return false;
        }
    }


}

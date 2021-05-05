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
        float rLeft1 = Random.Range(2f, 4f);
        float rLeft2 = Random.Range(-2f, -4f);
        float rRight1 = Random.Range(2f, 4f);
        float rRight2 = Random.Range(-2f, -4f);
        int r1 = Random.Range(0,1);
        int r2 = Random.Range(0, 1);
        float rLeft = 0f;
        float rRight = 0f;

        if (r1 == 0)
            rLeft = rLeft1;
        else
            rLeft = rLeft2;

        if (r2 == 0)
            rRight = rRight1;
        else
            rRight = rRight1;

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

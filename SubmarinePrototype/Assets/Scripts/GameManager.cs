using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public int[] visualMessageBuffer = { 0, 0 , 0, 0};
    private float tensionValue = 0f;
    static public Tension currentTension = Tension.PEACEFUL;

    public void IncreaseTension(float value)
    {
        tensionValue += value;

        CheckTension();
    }

    public void DecreaseTension(float value)
    {
        tensionValue -= value;

        CheckTension();
    }

    private void CheckTension()
    {
        if(tensionValue < 2f) 
        {
            currentTension = Tension.PEACEFUL;
        }
        else if(tensionValue < 4f)
        {
            currentTension = Tension.LOW;
        }
        else if (tensionValue < 6f)
        {
            currentTension = Tension.MEDIUM;
        }
        else if (tensionValue < 8f)
        {
            currentTension = Tension.DANGER;
        }
        else
        {
            currentTension = Tension.THREAT;
        }
    }

    public enum Tension
    {
        PEACEFUL,
        LOW,
        MEDIUM,
        DANGER,
        THREAT,
        NONE
    };
}

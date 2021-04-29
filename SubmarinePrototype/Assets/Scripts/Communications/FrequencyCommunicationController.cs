using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FrequencyCommunicationController : MonoBehaviour
{
    public GameObject buttonReciever;
    public List<FrequencyWheelController> frequencyWheels = new List<FrequencyWheelController>();
    private bool checkWheels = false;

    private void Update()
    {
        if (checkWheels)
            CheckAllWheels();
    }
    public void SetFrequenciesValues()
    {
        foreach(FrequencyWheelController wheel in frequencyWheels)
        {
            wheel.range = Random.Range(3, 300);// ELF Frequency range in submarines
            wheel.detect = true;
            Debug.Log(wheel.range);
        }
        checkWheels = true;
    }

    public void CheckAllWheels()
    {
        int inRange = 0;
        foreach (FrequencyWheelController wheel in frequencyWheels)
        {
            if (wheel.isInRange)
                inRange++;
        }
    }
}

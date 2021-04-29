using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FrequencyCommunicationController : MonoBehaviour
{
    public List<FrequencyWheelController> frequencyWheels = new List<FrequencyWheelController>();

    public void SetFrequenciesValues()
    {
        foreach(FrequencyWheelController wheel in frequencyWheels)
        {
            wheel.range = Random.Range(3, 300);// ELF Frequency range in submarines
            wheel.detect = true;
            Debug.Log(wheel.range);
        }
    }
}

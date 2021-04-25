using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AllyController : MonoBehaviour
{
    public AllyEvent currentAllyEvent = AllyEvent.NONE;
    public int frequencyRange = 0;
    public FrequencyCommunicationController frequencyController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchEvent(AllyEvent incomingEvent)
    {
        switch (incomingEvent)
        {
            case AllyEvent.RADIO:
                break;
            case AllyEvent.FREQUENCY:
                SendFrequency();
                break;
            case AllyEvent.NONE:
                break;
        }
    }

    private void SendFrequency()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        frequencyRange = Random.Range(3, 300);// ELF Frequency range in submarines
        frequencyController.detect = true;
    }

    public enum AllyEvent
    {
        RADIO,
        FREQUENCY,
        NONE
    }
}

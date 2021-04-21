using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public AllyEvent currentAllyEvent = AllyEvent.NONE;
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

    }

    public enum AllyEvent
    {
        RADIO,
        FREQUENCY,
        NONE
    }
}

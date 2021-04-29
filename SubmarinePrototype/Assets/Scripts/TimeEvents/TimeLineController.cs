using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineController : MonoBehaviour
{
    public float maxVisibleEventTime = 10f;
    public float baseOffsetTimeEvent = 120f;

    // EVENT ICONS
    public GameObject bombPrefab;
    public GameObject freqPrefab;
    public GameObject radioPrefab;
    public GameObject visualPrefab;

    // CONTROLLERS
    public ShipController shipController;
    public AllyController allyController;

    // TIME RELATED
    private float timerTimeLine = 0f;
    private float timerNextEvent = 0f;
    private List<TimeEvent> timeEvents = new List<TimeEvent>();

    // EVENT DECISION RELATED
    private bool islastAlly = false;
    private int lastAllyEvent = 0; // 0: RADIO / 1: FREQUENCY
    private int lastEnemyEvent = 0; // 0: VISUAL / 1: BOMB

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Manage the addition of new events based in current tension
        ManageEvents();

        //Manage Visual icons in timeline and the execution and deletion of the events
        RefreshTimeLine();

        timerTimeLine = Time.realtimeSinceStartup;
    }

    public void AddNewEvent(float time, TimeEventType type)
    {
        GameObject go;

        switch (type)
        {
            case TimeEventType.VISUAL:
                go = Instantiate(visualPrefab, gameObject.transform);
                break;
            case TimeEventType.RADIO:
                go = Instantiate(radioPrefab, gameObject.transform);
                break;
            case TimeEventType.BOMB:
                go = Instantiate(bombPrefab, gameObject.transform);
                break;
            case TimeEventType.FREQUENCY:
                go = Instantiate(freqPrefab, gameObject.transform);
                break;
            default:
                go = null;
                break;
        }
        RectTransform rt = (RectTransform)gameObject.transform;
        go.transform.localPosition = new Vector3(rt.rect.width*0.5f, 0, 0);
        TimeEvent newEvent = new TimeEvent(time, type, go);
        timeEvents.Add(newEvent);
        timerNextEvent = Time.realtimeSinceStartup;
        //Debug.Log(type + " EVENT CREATED");
    }

    public void DeleteEvent(TimeEvent eventToDestroy)
    {
        if(timeEvents.Contains(eventToDestroy))
        {
            timeEvents.Remove(eventToDestroy);
            Destroy(eventToDestroy.visualGO);
            //Debug.Log(eventToDestroy.type + " EVENT DELETED");
            return;
        }
        //Debug.LogWarning("THIS TIME EVENT DOES NOT EXIST IN TIMELINE!");
    }

    public void LaunchEvent(TimeEventType ev)
    {
        switch (ev)
        {
            case TimeEventType.VISUAL:
                shipController.LaunchEvent(ShipController.ShipEvent.VISUAL);
                break;
            case TimeEventType.BOMB:
                shipController.LaunchEvent(ShipController.ShipEvent.BOMB);
                break;
            case TimeEventType.RADIO:
                allyController.LaunchEvent(AllyController.AllyEvent.RADIO);
                break;
            case TimeEventType.FREQUENCY:
                allyController.LaunchEvent(AllyController.AllyEvent.FREQUENCY);
                break;
        }
    }

    private void VisualTimeLineManagement(TimeEvent icon)
    {
        RectTransform rt = (RectTransform)icon.visualGO.transform.parent.transform;
        float speed = rt.rect.width / icon.timeToExecute;
        Vector3 x = new Vector3(speed * Time.deltaTime, 0, 0);
        icon.visualGO.transform.localPosition -= x;
    }

    private void ManageEvents()
    {
        float totalTime = baseOffsetTimeEvent + CalculateTensionFactor();
        if (Time.realtimeSinceStartup - timerNextEvent >= totalTime) // Calculate when to launch next event
        {
            //Check which was the last event from
            if(islastAlly)
            {
                if(lastAllyEvent == 0)
                    AddNewEvent(20f, TimeEventType.RADIO);
                else
                    AddNewEvent(60f, TimeEventType.FREQUENCY);

                lastAllyEvent = lastAllyEvent == 0 ? 1 : 0;
            }
            else
            {
                if(lastEnemyEvent == 0)
                    AddNewEvent(10f, TimeEventType.VISUAL);
                else
                    AddNewEvent(5f, TimeEventType.BOMB);

                lastEnemyEvent = lastEnemyEvent == 0 ? 1 : 0;
            }
            islastAlly = !islastAlly;
        }
    }

    private void RefreshTimeLine()
    {
        for (int i = 0; i < timeEvents.Count; ++i)
        {
            VisualTimeLineManagement(timeEvents[i]); //Movement of the event through the time line
            if (timerTimeLine - timeEvents[i].timeStamp >= timeEvents[i].timeToExecute) //ExecuteEvent
            {
                LaunchEvent(timeEvents[i].type);
                //Debug.Log(timeEvents[i].type + " EVENT EXECUTED");
                DeleteEvent(timeEvents[i]);
            }
        }
    }

    private float CalculateTensionFactor() // Lower value when tension is more extreme in both levels
    {
        float tensionFactor = 0f;
        switch (GameManager.currentTension)
        {
            case Tension.PEACEFUL:
                tensionFactor = 0.5f;
                break;
            case Tension.LOW:
                tensionFactor = 1f;
                break;
            case Tension.MEDIUM:
                tensionFactor = 1.5f;
                break;
            case Tension.DANGER:
                tensionFactor = 1f;
                break;
            case Tension.THREAT:
                tensionFactor = 0.5f;
                break;
            case Tension.NONE:
                tensionFactor = 0f;
                break;
        }

        return tensionFactor;
    }

    public struct TimeEvent
    {
        public float timeStamp;
        public float timeToExecute;
        public TimeEventType type;
        public GameObject visualGO;

        public TimeEvent(float timeToExecute, TimeEventType type,GameObject visualGO)
        {
            this.timeToExecute = timeToExecute;
            this.type = type;
            this.visualGO = visualGO;
            timeStamp = Time.realtimeSinceStartup;
        }
    };

    public enum TimeEventType
    {
        VISUAL,
        RADIO,
        BOMB,
        FREQUENCY
    };
}


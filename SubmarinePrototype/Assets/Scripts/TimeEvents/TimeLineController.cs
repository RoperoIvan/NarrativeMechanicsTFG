using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineController : MonoBehaviour
{
    public float maxVisibleEventTime = 10f;
    public float baseOffsetAttackEvent = 80f;
    private float baseOffsetTimeEvent = 60f;
    // EVENT ICONS
    public GameObject bombPrefab;
    public GameObject freqPrefab;
    public GameObject radioPrefab;
    public GameObject visualPrefab;
    public GameObject iconRadEventPrefab;
    public GameObject iconAttEventPrefab;
    public GameObject iconVisEventPrefab;

    // CONTROLLERS
    public ShipController shipController;
    public AllyController allyController;

    //EVENT
    public List<TimeEvent> structuredEvents = new List<TimeEvent>();

    // TIME RELATED
    //private float timerTimeLine = 0f;
    public float timerNextEvent = 0f;
    float lastTimeEvent = 0f;
    private List<TimeEvent> timeEvents = new List<TimeEvent>();

    // EVENT DECISION RELATED
    public bool doAttack = true;
    public int currentEvent = 0;
    private bool isFirst = true;
    public bool isIntro = false;
    //private int lastAllyEvent = 0; // 0: RADIO / 1: FREQUENCY
    //private int lastEnemyEvent = 0; // 0: VISUAL / 1: BOMB

    void Awake()
    {
        structuredEvents = CreateAllEvents();
        lastTimeEvent = Time.realtimeSinceStartup;
    }

    void Update()
    {
        if(!isIntro)
        {
            //Manage the addition of new events based in current tension
            //ManageAttacks();
            //ManageEvents();
            //Manage Visual icons in timeline and the execution and deletion of the events
            
        }
        RefreshTimeLine();
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

    private void ManageAttacks()
    {
        if(doAttack)
        {
            float totalTime = baseOffsetAttackEvent + CalculateTensionFactor();
            if (isFirst)
            {
                totalTime += 120;
                timerNextEvent = Time.realtimeSinceStartup;
            }
               

            if (Time.realtimeSinceStartup - timerNextEvent >= totalTime) // Calculate when to launch next event
            {
                AddNewEvent(5f, TimeEventType.BOMB);
                doAttack = false;
            }
        }
    }

    private void ManageEvents()
    {
        float totalTime = baseOffsetTimeEvent + CalculateTensionFactor();
        if (Time.realtimeSinceStartup - lastTimeEvent >= totalTime || isFirst)
        {
            isFirst = false;
            lastTimeEvent = Time.realtimeSinceStartup;
            AddNewEvent(structuredEvents[currentEvent].timeToExecute, structuredEvents[currentEvent].type);
            currentEvent++;
            //CalculateNextBaseTimeEvent();
        }
    }

    private void RefreshTimeLine()
    {
        for (int i = 0; i < timeEvents.Count; ++i)
        {
            VisualTimeLineManagement(timeEvents[i]); //Movement of the event through the time line
            if (Time.realtimeSinceStartup - timeEvents[i].timeStamp >= timeEvents[i].timeToExecute) //ExecuteEvent
            {
                GameObject icoGO;
                switch (timeEvents[i].type) //Create icon info event
                {
                    case TimeEventType.VISUAL:
                        icoGO = Instantiate(iconVisEventPrefab, transform.parent);
                        break;
                    case TimeEventType.BOMB:
                        icoGO= Instantiate(iconAttEventPrefab, transform.parent); 
                        break;
                    case TimeEventType.FREQUENCY:
                        icoGO = Instantiate(iconRadEventPrefab, transform.parent);
                        break;
                }
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
                tensionFactor = 5f;
                break;
            case Tension.LOW:
                tensionFactor = 3f;
                break;
            case Tension.MEDIUM:
                tensionFactor = 2f;
                break;
            case Tension.DANGER:
                tensionFactor = 1f;
                break;
            case Tension.THREAT:
                tensionFactor = 0f;
                break;
            case Tension.NONE:
                tensionFactor = 0f;
                break;
        }

        return tensionFactor;
    }

    private List<TimeEvent> CreateAllEvents()
    {
        List<TimeEvent> _events = new List<TimeEvent>();
        _events.Add(new TimeEvent(8f, TimeEventType.FREQUENCY));
        _events.Add(new TimeEvent(12f, TimeEventType.VISUAL));
        _events.Add(new TimeEvent(12f, TimeEventType.FREQUENCY));
        _events.Add(new TimeEvent(12f, TimeEventType.VISUAL));
        _events.Add(new TimeEvent(12f, TimeEventType.FREQUENCY));
        _events.Add(new TimeEvent(12f, TimeEventType.VISUAL));
        _events.Add(new TimeEvent(12f, TimeEventType.FREQUENCY));
        _events.Add(new TimeEvent(12f, TimeEventType.VISUAL));
        _events.Add(new TimeEvent(12f, TimeEventType.FREQUENCY));
        _events.Add(new TimeEvent(12f, TimeEventType.VISUAL));
        _events.Add(new TimeEvent(12f, TimeEventType.FREQUENCY));
        return _events;
    }

    public struct TimeEvent
    {
        public float timeStamp;
        public float timeToExecute;
        public TimeEventType type;
        public GameObject visualGO;

        public TimeEvent(float timeToExecute, TimeEventType type)
        {
            this.timeToExecute = timeToExecute;
            this.type = type;
            timeStamp = Time.realtimeSinceStartup;
            visualGO = null;
        }

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


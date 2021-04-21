using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineController : MonoBehaviour
{
    public float maxVisibleEventTime = 10f;

    public GameObject bombPrefab;
    public GameObject freqPrefab;
    public GameObject radioPrefab;
    public GameObject visualPrefab;
    public ShipController shipController;
    public AllyController allyController;

    [SerializeField]
    private float timeUntilNextEvent = 0f;
    private float timerTimeLine = 0f;
    private List<TimeEvent> timeEvents = new List<TimeEvent>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        Debug.Log("EVENT " + type + " CREATED");
    }

    public void DeleteEvent(TimeEvent eventToDestroy)
    {
        if(timeEvents.Contains(eventToDestroy))
        {
            timeEvents.Remove(eventToDestroy);
            Destroy(eventToDestroy.visualGO);
            Debug.Log("EVENT DELETED");
            return;
        }
        Debug.LogWarning("THIS TIME EVENT DOES NOT EXIST IN TIMELINE!");
    }

    public void VisualTimeLineManagement(TimeEvent icon)
    {
        RectTransform rt = (RectTransform)icon.visualGO.transform.parent.transform;
        float speed = rt.rect.width/icon.timeToExecute;
        Vector3 x = new Vector3(speed * Time.deltaTime, 0, 0);
        icon.visualGO.transform.localPosition -= x;
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

    private void RefreshTimeLine()
    {
        for (int i = 0; i < timeEvents.Count; ++i)
        {
            VisualTimeLineManagement(timeEvents[i]);
            if (timerTimeLine - timeEvents[i].timeStamp >= timeEvents[i].timeToExecute) //ExecuteEvent
            {
                LaunchEvent(timeEvents[i].type);
                Debug.Log("EVENT " + timeEvents[i].type + " EXECUTED");
                DeleteEvent(timeEvents[i]);
            }
        }
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


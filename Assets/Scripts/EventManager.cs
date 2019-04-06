using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
                
                if (!eventManager)
                {
                    Debug.LogError("Missing EventManager script on a GameObject in the scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }


    //public delegate void PlayerMovementControlsCall();
    //public static event PlayerMovementControlsCall OnCalled;

    void Init ()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    /// <summary>
    /// Starts listening for events
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener">Unity Action: Act as a function pointer that will act as the listener. </param>
    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;    // when we are going to look at the dictionary, want to make sure a key value is paired
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        } // first time this is done, if just creating a new event to listen to, there will be no entry (empty dictionary)
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        // if destroyed or not found event manager, ignore
        if (eventManager == null) return;

        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
        
    }

    /// <summary>
    /// Calls all the functions on the listeners that are listening for the event
    /// </summary>
    /// <param name="eventName"></param>
    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
    
}

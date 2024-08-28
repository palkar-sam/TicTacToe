using Model;
using Props;
using System;
using System.Collections.Generic;
using UnityEngine;
using Aik.Utils;

public class EventManager
{
    private static Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();

    public static void StartListening(GameEvents gameEvent, Action listener)
    {
        Action thisEvent;
        string eventString = gameEvent.ToString();
        if (eventDictionary.TryGetValue(eventString, out thisEvent))
        {
            //Add more event to the existing one
            thisEvent += listener;

            //Update the Dictionary
            eventDictionary[eventString] = thisEvent;
        }
        else
        {
            //Add event to the Dictionary for the first time
            thisEvent += listener;
            eventDictionary.Add(eventString, thisEvent);
        }
    }

    public static void StopListening(GameEvents gameEvent, Action listener)
    {

        Action thisEvent;
        string eventString = gameEvent.ToString();
        if (eventDictionary.TryGetValue(eventString, out thisEvent))
        {
            //Remove event from the existing one
            thisEvent -= listener;

            //Update the Dictionary
            eventDictionary[eventString] = thisEvent;
        }
    }

    public static void TriggerEvent(GameEvents eventName)
    {
        Action thisEvent = null;
        if (eventDictionary.TryGetValue(eventName.ToString(), out thisEvent))
        {
            if (thisEvent != null)
            {
                thisEvent.Invoke();
            }
            if (LoggerUtil.logEnabled) LoggerUtil.Log("Triggered event " + eventName.ToString());
            // OR USE instance.eventDictionary[eventName]();
        }
        else
        {
            Debug.LogWarning("Event " + eventName + " has no listeners, but is being triggered.");
        }
    }
}

public class EventManager<T> : EventManager where T : BaseModel
{
    private static Dictionary<string, Action<T>> parameterizedEventDictionary = new Dictionary<string, Action<T>>();

    internal static void TriggerEvent(object oN_DEEP_LINK_INVITE_RECIEVED)
    {
        throw new NotImplementedException();
    }

    public static void StartListening(GameEvents gameEvent, Action<T> listener)
    {
        Action<T> thisEvent;
        string eventString = gameEvent.ToString();
        if (parameterizedEventDictionary.TryGetValue(eventString, out thisEvent))
        {
            //Add more event to the existing one
            thisEvent += listener;

            //Update the Dictionary
            parameterizedEventDictionary[eventString] = thisEvent;
        }
        else
        {
            //Add event to the Dictionary for the first time
            thisEvent += listener;
            parameterizedEventDictionary.Add(eventString, thisEvent);
        }
    }

    public static void StopListening(GameEvents gameEvent, Action<T> listener)
    {

        Action<T> thisEvent;
        string eventString = gameEvent.ToString();
        if (parameterizedEventDictionary.TryGetValue(eventString, out thisEvent))
        {
            //Remove event from the existing one
            thisEvent -= listener;

            //Update the Dictionary
            parameterizedEventDictionary[eventString] = thisEvent;
        }
    }

    public static void TriggerEvent(GameEvents eventName, T arguments)
    {
        Action<T> thisEvent = null;
        if (parameterizedEventDictionary.TryGetValue(eventName.ToString(), out thisEvent))
        {
            if (thisEvent != null)
            {
                thisEvent.Invoke(arguments);
            }
            if (LoggerUtil.logEnabled) LoggerUtil.Log("Triggered event " + eventName.ToString());
        }
        else
        {
            //Debug.LogWarning("Event " + eventName + " has no listeners, but is being triggered.");
        }
    }
}

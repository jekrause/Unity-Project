using System.Collections.Generic;
using System;
using UnityEngine;
public class EventAggregator : IEventAggregator
{
    private static EventAggregator EventAggregator_Singleton = null;
    private List<object> Subscribers = new List<object>(); // the subscribers that are listening to a specific Event type (eg. a list of subscribers listening for ItemPickedUpEvent type)
    private Dictionary<Type, List<object>> EventTypes = new Dictionary<Type, List<object>>(); // the Map of Event type to list of subscribers who are listening
    
    private EventAggregator() { }

    public static EventAggregator GetInstance()
    {
        if (EventAggregator_Singleton == null)
            EventAggregator_Singleton = new EventAggregator();

        return EventAggregator_Singleton;
    }

    public void Publish<T>(T EventData)
    {
        foreach(KeyValuePair<Type, List<object>> type in EventTypes)
        {
            if(type.Key == typeof(T))
            {
                foreach(var subscriber in type.Value)
                {
                    ((ISubscriber<T>)(subscriber)).OnEventHandler(EventData);
                }
                break;
            }
        }
    }

    public void Register<T>(ISubscriber<T> subscriber)
    {
        try
        {
            Subscribers = null;
            bool typeExist = EventTypes.TryGetValue(typeof(T), out Subscribers); 

            if (!typeExist)
            {
                Subscribers = new List<object>();
                Subscribers.Add(subscriber);
                EventTypes.Add(typeof(T), Subscribers);
            }
            else
            {
                Subscribers.Add(subscriber);
            }
        }
        catch
        {
            Debug.Log("Error on registering subscriber");
        }
        
    }

    public void Unregister<T>(ISubscriber<T> subscriber)
    {
        Subscribers = null;
        bool typeExist = EventTypes.TryGetValue(typeof(T), out Subscribers);

        if (typeExist)
        {
            Subscribers.Remove(subscriber);
        }
    }
}

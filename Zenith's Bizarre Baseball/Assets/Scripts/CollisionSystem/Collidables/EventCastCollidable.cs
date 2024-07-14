using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventCastCollidable : ICollidable, IGameObjectProcessor
{
    [SerializeField] EventCast[] eventCasts;
    public override void OnCollide(Collider2D collider) => Process(collider.gameObject);

    public void Process(GameObject gameObject)
    {
        EventCast.Cast(gameObject, eventCasts);
    }
}

[Serializable] public struct EventCast
{
    [SerializeField] ObjectProcessor condition;
    public readonly bool CheckConditions() => condition.ResultBool(1);

    [SerializeField] string[] _eventsNames;
    public readonly string[] EventsNames => _eventsNames;

    public static void Cast(GameObject gameObject, EventCast[] eventCasts)
    {
        if(gameObject.TryGetComponent(out BehaviourByTextInput eventByTextReceiver))
        {
            List<string> eventsNames = new List<string>();

            foreach (EventCast eventCast in eventCasts)
            {
                if(eventCast.CheckConditions())
                {
                    eventsNames.AddRange(eventCast.EventsNames);
                }
            }

            eventByTextReceiver.ExecuteEvents(eventsNames.ToArray());
        }
    }
} 
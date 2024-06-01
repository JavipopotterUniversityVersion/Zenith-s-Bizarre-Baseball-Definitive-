using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventCastCollidable : ICollidable
{
    [SerializeField] EventCast[] eventCasts;
    public override void OnCollide(Collider2D collider)
    {
        if(collider.TryGetComponent(out EventByTextReceiver eventByTextReceiver))
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

    [Serializable] struct EventCast
    {
        [SerializeField] ScriptableCondition[] conditions;
        public readonly bool CheckConditions() => ScriptableCondition.CheckAllConditions(conditions);

        [SerializeField] string[] _eventsNames;
        public readonly string[] EventsNames => _eventsNames;
    } 
}
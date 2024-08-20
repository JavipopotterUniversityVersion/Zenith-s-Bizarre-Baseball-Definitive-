using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSender : ICollidable
{
    [SerializeField] Identifiable[] _messages;
    public override void OnCollide(Collider2D collider)
    {
        if (collider.TryGetComponent(out MessageReceiver messageReceiver))
        {
            foreach (Identifiable message in _messages) messageReceiver.InvokeEvent(message);
        }
    }
}
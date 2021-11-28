using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventContainer : Singleton<EventContainer>
{
    protected EventContainer() {}

    Dictionary<string, UnityEvent> events;
    public Dictionary<string, UnityEvent> Events { get { return events; } }

    void AddEvent(string name)
    {
        events.Add(name, new UnityEvent());
    }

    private void OnEnable()
    {
        events = new Dictionary<string, UnityEvent>();

        AddEvent("OnPlayerAttack");
        AddEvent("OnPlayerDamaged");
        AddEvent("OnPlayerIced");
        AddEvent("OnEnemyAttacked");
    }
}

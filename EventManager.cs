using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameEventType
{
    EnemyDamaged,
    AllEnemiesAffected
}

public class EventManager : MonoBehaviour
{
    private Dictionary<GameEventType, Action<object>> eventListeners = new Dictionary<GameEventType, Action<object>>();

    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Subscribe(GameEventType eventType, Action<object> listener)
    {
        if (!eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType] = listener;
        }
        else
        {
            eventListeners[eventType] += listener;
        }
    }

    public void Unsubscribe(GameEventType eventType, Action<object> listener)
    {
        if (eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType] -= listener;
        }
    }

    public void TriggerEvent(GameEventType eventType, object parameter = null)
    {
        if (eventListeners.TryGetValue(eventType, out Action<object> listener))
        {
            listener?.Invoke(parameter);
        }
    }
}


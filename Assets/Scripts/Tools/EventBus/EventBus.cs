using System;
using System.Collections.Generic;

public static class EventBus
{
    private static Dictionary<Type, List<WeakReference<IEventBaseSubscriber>>> _subscribers = new();
    private static Dictionary<int, WeakReference<IEventBaseSubscriber>> _hashToReferences = new();

    public static void RegisterTo<T>(IEventSubscriber<T> subcriber) where T : struct, IEvent
    {
        Type eventType = typeof(T);

        if (!_subscribers.ContainsKey(eventType))
            _subscribers[eventType] = new List<WeakReference<IEventBaseSubscriber>>();

        WeakReference<IEventBaseSubscriber> reference = new WeakReference<IEventBaseSubscriber>(subcriber);

        _subscribers[eventType].Add(reference);
        _hashToReferences[subcriber.GetHashCode()] = reference;
    }

    public static void UnregisterTo<T>(IEventSubscriber<T> subcriber) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        int subscriberHash = subcriber.GetHashCode();

        if (!_subscribers.ContainsKey(eventType) || _hashToReferences.ContainsKey(subscriberHash))
            return;

        WeakReference<IEventBaseSubscriber> reference = _hashToReferences[subscriberHash];

        _subscribers[eventType].Remove(reference);
        _hashToReferences.Remove(subscriberHash);
    }

    public static void RaiseEvent<T>(T eventName) where T : struct, IEvent
    {
        Type eventType = typeof(T);

        if (!_subscribers.ContainsKey(eventType))
            return;

        foreach(WeakReference<IEventBaseSubscriber> reference in _subscribers[eventType])
        {
            if (reference.TryGetTarget(out var subscriber))
            {
                ((IEventSubscriber<T>)subscriber).OnEvent(eventName);
            }
        }
    }
}
